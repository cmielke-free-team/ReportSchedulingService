using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Transactions;
using Emdat.InVision.SSRS;

namespace Emdat.InVision
{
    /// <summary>
    /// Execution class
    /// </summary>
    [DataObject(true)]
    public class Execution
    {
        private static XmlSerializer _xsExecution = new XmlSerializer(typeof(Execution));
        private Subscription _subscription;
        private Report _report;
        private ReportFormat _format;
        private ExecutionStatus _status;        

        public static TimeSpan CacheDuration
        {
            get
            {
                TimeSpan d;
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CacheDuration"]) &&
                    TimeSpan.TryParse(System.Configuration.ConfigurationManager.AppSettings["CacheDuration"], out d))
                {
                    return d;
                }
                return TimeSpan.FromMinutes(10);
            }
        }

        /// <summary>
        /// Gets the execution from file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private static Execution GetExecutionFromFile(string file)
        {
            Execution retVal = null;
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    retVal = _xsExecution.Deserialize(fs) as Execution;
                    fs.Close();
                }
                return retVal;
            }
            catch
            {
            }
            return retVal;
        }        

        /// <summary>
        /// Lists the executions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<Execution> ListExecutions(int userId, int? companyId, ReportingApplication application)
        {
            IEnumerable<Execution> executions = null;

            if (executions == null)
            {
                using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
                {
                    var executionsQuery =
                        from e in sql.ListExecutions(userId, companyId)
                        let prms = ReportParametersHelper.GetParameterValuesFromXml(e.Parameters)
                        //let startDatePrm = (from p in prms where p.Name == "Start_Date" select p.Value).FirstOrDefault()
                        //let endDatePrm = (from p in prms where p.Name == "End_Date" select p.Value).FirstOrDefault()
                        where e.ReportExecutionStatusID != (int)ExecutionStatusEnum.Running
                        select new Execution
                        {
                            Application = application,
                            OwnerId = e.OwnerID.ToString(),
                            OwnerName = string.Format("{0}, {1}", e.OwnerNameLast, e.OwnerNameFirst),
                            OwnerUsername = e.OwnerUsername,
                            Id = e.ReportExecutionID.ToString(),
                            ActualCompletionTime = e.EndDate,
                            ActualStartTime = e.StartDate,
                            Description = e.Name, //string.Format("{0} ({1} - {2})", e.Name, startDatePrm, endDatePrm),
                            ExpirationDate = e.ExpirationDate,
                            FormatId = e.ReportFormatID.ToString(),
                            Parameters = prms,
                            ReportingEnvironmentId = (ReportingEnvironmentEnum)e.EnvironmentID,
                            ReportingEnvironmentName = e.EnvironmentName,
                            ReportId = e.ReportID.ToString(),
                            ScheduledRunTime = e.ScheduledStartDate,
                            SubscriptionId = e.ReportSubscriptionID.ToString(),
                            StatusId = (ExecutionStatusEnum)e.ReportExecutionStatusID,
                            ErrorName = e.ErrorName,
                            ErrorDescription = e.ErrorDescription
                        };
                    executions = executionsQuery.ToList();

                }
            }
            return executions;
        }        

        /// <summary>
        /// Loads the execution.
        /// </summary>
        /// <param name="executionId">The execution id.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The report execution could not be found.</exception>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Execution LoadExecution(string id, int userId, ReportingApplication application)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            int executionId = int.Parse(id);

            if (executionId < 1)
            {
                throw new ArgumentOutOfRangeException("id");
            }

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                var result =
                    (from e in sql.GetExecution(userId, executionId)
                     select new Execution
                     {
                         Application = application,
                         OwnerId = userId.ToString(),
                         Id = e.ReportExecutionID.ToString(),
                         Description = e.Name,
                         SubscriptionId = e.ReportSubscriptionID.ToString(),
                         ReportingEnvironmentId = (ReportingEnvironmentEnum)e.EnvironmentID,
                         ReportingEnvironmentName = e.EnvironmentName,
                         ReportId = e.ReportID.ToString(),
                         StatusId = (ExecutionStatusEnum)e.ReportExecutionStatusID,
                         FormatId = e.ReportFormatID.ToString(),
                         Parameters = ReportParametersHelper.GetParameterValuesFromXml(e.Parameters),
                         ActualCompletionTime = e.EndDate,
                         ActualStartTime = e.StartDate,
                         ExpirationDate = e.ExpirationDate,
                         ScheduledRunTime = e.ScheduledStartDate,
                         ModifiedDate = e.ModifiedDate,
                         ModifiedUser = e.ModifiedUser,
                         ErrorName = e.ErrorName,
                         ErrorDescription = e.ErrorDescription                         
                     })
                    .FirstOrDefault();
                if (result == null)
                {
                    throw new KeyNotFoundException(string.Format("The report execution could not be found for user {0}: {1}", userId, executionId));
                }
                return result;
            }
        }

        /// <summary>
        /// Loads the execution data into the specified stream.
        /// </summary>
        /// <param name="execution">The execution.</param>
        public static System.Data.IDataReader LoadExecutionDataReader(Execution execution)
        {
            if (execution == null)
            {
                throw new ArgumentNullException("execution");
            }

            if (!execution.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (string.IsNullOrEmpty(execution.Id))
            {
                throw new InvalidOperationException("Id must be set");
            }

            int executionId = int.Parse(execution.Id);

            using (var data = new Emdat.InVision.Sql.ReportingDataContext(execution.Application.Value, "Emdat.InVision.ReportContent"))
            {
                return data.GetExecutionDataReader(executionId);
            }
        }

        /// <summary>
        /// Loads the execution data into the specified stream.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <param name="stream">The stream.</param>
        public static void LoadExecutionData(Execution execution, Stream stream)
        {
            if (execution == null)
            {
                throw new ArgumentNullException("execution");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanWrite)
            {
                throw new InvalidOperationException("Stream is not writable");
            }

            if (!execution.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (string.IsNullOrEmpty(execution.Id))
            {
                throw new InvalidOperationException("Id must be set");
            }

            int executionId = int.Parse(execution.Id);

            using (var data = new Emdat.InVision.Sql.ReportingDataContext(execution.Application.Value, "Emdat.InVision.ReportContent"))
            {
                var result = data.GetExecutionData(executionId, stream);
                if (result == null)
                {
                    throw new KeyNotFoundException(string.Format("Could not find data for report: {0}", executionId));
                }
            }
        }

        /// <summary>
        /// Loads the execution data into the specified file.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <param name="filepath">The filepath.</param>
        public static void LoadExecutionData(Execution execution, string file)
        {
            if (execution == null)
            {
                throw new ArgumentNullException("execution");
            }

            if (!execution.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (string.IsNullOrEmpty(execution.Id))
            {
                throw new InvalidOperationException("Id must be set");
            }

            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                LoadExecutionData(execution, fs);
                fs.Close();
            }
        }

        /// <summary>
        /// Loads the execution data.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns></returns>
        public static byte[] LoadExecutionData(Execution execution)
        {
            if (execution == null)
            {
                throw new ArgumentNullException("execution");
            }

            if (!execution.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (string.IsNullOrEmpty(execution.Id))
            {
                throw new InvalidOperationException("Id must be set");
            }

            int executionId = int.Parse(execution.Id);

            using (var data = new Emdat.InVision.Sql.ReportingDataContext(execution.Application.Value, "Emdat.InVision.ReportContent"))
            {
                var result = data.GetExecutionData(executionId).FirstOrDefault();
                if (result == null || result.Data == null)
                {
                    throw new KeyNotFoundException(string.Format("Could not find data for report: {0}", executionId));
                }
                return result.Data;
            }
        }

        /// <summary>
        /// Adds the execution.
        /// </summary>
        /// <param name="execution">The execution.</param>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public static void AddExecution(Execution execution, byte[] reportData)
        {
            #region validation

            if (execution == null)
            {
                throw new ArgumentNullException("execution");
            }

            if (reportData == null)
            {
                throw new ArgumentNullException("reportData");
            }

            if (!execution.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (execution.Application.Value == ReportingApplication.InCommand &&
                !execution.CompanyId.HasValue)
            {
                throw new InvalidOperationException("CompanyId must be set for InCommand reports");
            }

            if (string.IsNullOrEmpty(execution.OwnerId))
            {
                throw new InvalidOperationException("OwnerId must be set");
            }

            if (string.IsNullOrEmpty(execution.ReportId))
            {
                throw new InvalidOperationException("ReportId must be set");
            }

            if (string.IsNullOrEmpty(execution.FormatId))
            {
                throw new InvalidOperationException("FormatId must be set");
            }

            if (execution.Parameters == null)
            {

            }

            #endregion

            int reportingUserId = int.Parse(execution.OwnerId);
            int reportId = int.Parse(execution.ReportId);
            int reportFormatId = int.Parse(execution.FormatId);

            using (var data = new Emdat.InVision.Sql.ReportingDataContext(execution.Application.Value, "Emdat.InVision.ReportContent"))
            using (var info = new Emdat.InVision.Sql.ReportingDataContext(execution.Application.Value))
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //add the new execution record
                    var result = (info.AddExecution(
                        reportingUserId,
                        execution.CompanyId,
                        execution.Description,
                        execution.ActualCompletionTime,
                        reportId,
                        reportFormatId,
                        ReportParametersHelper.GetParameterValuesXml(execution.Parameters),
                        false,
                        execution.ModifiedUser,
                        execution.ModifiedDate,
                        (int)execution.ReportingEnvironmentId)).FirstOrDefault();
                    if (result == null || !result.ReportExecutionID.HasValue)
                    {
                        throw new ApplicationException(string.Format("Unable to add execution '{0}'. The procedure did not return a new ID.", execution.Description));
                    }

                    //avoid using a distributed transaction 
                    //if the following write fails, the outer transaction will get rolled back
                    using (TransactionScope noTs = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        //add the report content
                        var rowsAffected = data.SetExecutionData(result.ReportExecutionID.Value, execution.FormatFileType, reportData);
                        if (rowsAffected < 1)
                        {
                            throw new ApplicationException("SetExecutionData failed. No rows were affected");
                        }
                        noTs.Complete();
                    }

                    //complete the outer transaction
                    ts.Complete();
                }
            }

        }

        /// <summary>
        /// Adds the execution.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <param name="reportData">The report data.</param>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public static void AddExecution(Execution execution, Stream reportData)
        {
            //TODO: find a way to stream the data directly into the database without buffering it here            

            //read the data into a memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead = reportData.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                    bytesRead = reportData.Read(buffer, 0, buffer.Length);
                }

                //extract the full byte[] from the MemoryStream
                byte[] data = new byte[ms.Position];
                ms.Seek(0, SeekOrigin.Begin);
                bytesRead = ms.Read(data, 0, data.Length);
                if (bytesRead != data.Length)
                {
                    throw new ApplicationException("There was an error reading from the data stream");
                }

                //add the execution data
                AddExecution(execution, data);
            }
        }

        /// <summary>
        /// Deletes the execution.
        /// </summary>
        /// <param name="execution">The execution.</param>
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static void DeleteExecution(Execution execution)
        {
            int executionId = int.Parse(execution.Id);

#if MOCKUP
            //build the subscriptions path
            string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["AppDataPathForMockup"], "Executions");

            //find the file
            string file =
                (from f in Directory.GetFiles(path, string.Format("{0}.xml", execution.Id), SearchOption.AllDirectories)
                 select f)
                 .FirstOrDefault();

            if (file != null)
            {
                File.Delete(file);
            }

            //find the data file
            string dataPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["AppDataPathForMockup"], "ExecutionData");

            string dataFile =
                (from f in Directory.GetFiles(path, string.Format("{0}.*", execution.Id), SearchOption.AllDirectories)
                 select f)
                 .FirstOrDefault();

            if (dataFile != null)
            {
                File.Delete(file);
            }
#else

            using (var data = new Emdat.InVision.Sql.ReportingDataContext("Emdat.InVision.ReportContent"))
            using (var info = new Emdat.InVision.Sql.ReportingDataContext())
            {
                //transaction here would be distributed (not worth it)
                //TODO: need a back-end process cleaning up "orphaned" executions
                info.DeleteExecution(executionId);
                data.DeleteExecutionData(executionId);
            }

#endif
        }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        [DataObjectField(false, false, false)]
        public ReportingApplication? Application
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        [DataObjectField(false, false, false)]
        public int? CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>The owner id.</value>
        [DataObjectField(false, false, false)]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataObjectField(true, true, false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataObjectField(false, false, false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the scheduled run time.
        /// </summary>
        /// <value>The scheduled run time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? ScheduledRunTime { get; set; }

        /// <summary>
        /// Gets or sets the actual start time.
        /// </summary>
        /// <value>The actual start time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// Gets or sets the actual completion time.
        /// </summary>
        /// <value>The actual completion time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? ActualCompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        [DataObjectField(false, false, true)]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [DataObjectField(false, false, false)]
        public List<ParameterValue> Parameters { get; set; }

        /// <summary>
        /// Gets the parameter language.
        /// </summary>
        /// <value>The parameter language.</value>
        public string ParameterLanguage
        {
            get
            {
                return "en-us";
            }
        }

        /// <summary>
        /// Gets or sets the modified user.
        /// </summary>
        /// <value>The modified user.</value>
        [DataObjectField(false, false, false)]
        public string ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>The modified date.</value>
        [DataObjectField(false, false, false)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the owner username.
        /// </summary>
        /// <value>The owner username.</value>
        [DataObjectField(false, false, false)]
        public string OwnerUsername { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>The name of the owner.</value>
        [DataObjectField(false, false, false)]
        public string OwnerName { get; set; }

        #region status properties

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>        
        public string Status
        {
            get
            {
                return _status != null ? _status.Name : null;
            }
        }

        /// <summary>
        /// Gets or sets the status id.
        /// </summary>
        /// <value>The status id.</value>
        [DataObjectField(false, false, false)]
        public ExecutionStatusEnum StatusId
        {
            get
            {
                return _status != null ? _status.Id : ExecutionStatusEnum.None;
            }
            set
            {
                _status =
                    (from s in ExecutionStatus.ListExecutionStatuses()
                     where s.Id == value
                     select s)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the execution status.
        /// </summary>
        /// <returns></returns>
        public ExecutionStatus ExecutionStatus
        {
            get
            {
                return _status;
            }
        }

        #endregion

        #region error properties

        /// <summary>
        /// Gets or sets the name of the error.
        /// </summary>
        /// <value>The name of the error.</value>
        public string ErrorName { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>The error description.</value>
        public string ErrorDescription { get; set; }

        #endregion

        #region subscription properties

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        /// <value>The subscription id.</value>
        [DataObjectField(false, false, true)]
        public string SubscriptionId
        {
            get
            {
                return _subscription != null ? _subscription.Id : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _subscription = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(this.OwnerId))
                    {
                        throw new InvalidOperationException("ReportId cannot be set without an OwnerId");
                    }
                    if (!this.Application.HasValue)
                    {
                        throw new InvalidOperationException("ReportId cannot be set without an Application");
                    }
                    _subscription = Subscription.LoadSubscription(value, int.Parse(this.OwnerId), this.Application.Value);
                }
            }
        }

        /// <summary>
        /// Gets the subscription description.
        /// </summary>
        /// <value>The subscription description.</value>
        [System.Xml.Serialization.XmlIgnore]
        public string SubscriptionDescription
        {
            get
            {
                return _subscription != null ? _subscription.Description : null;
            }
        }

        #endregion

        #region format properties

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        [DataObjectField(false, false, false)]
        public string Format
        {
            get
            {
                return _format != null ? _format.Name : null;
            }
        }

        /// <summary>
        /// Gets the type of the format file.
        /// </summary>
        /// <value>The type of the format file.</value>
        public string FormatFileType
        {
            get
            {
                return _format != null ? _format.FileExtension : null;
            }
        }

        /// <summary>
        /// Gets the HTTP ContentType to be used when downloading the execution.
        /// </summary>
        /// <value>The type of the format HTTP content.</value>
        public string FormatHttpContentType
        {
            get
            {
                return _format != null ? _format.HttpContentType : null;
            }
        }

        /// <summary>
        /// Gets the format rendering extension.
        /// </summary>
        /// <value>The format rendering extension.</value>
        [DataObjectField(false, false, false)]
        public string FormatRenderingExtension
        {
            get
            {
                return _format != null ? _format.Value : null;
            }
        }

        [DataObjectField(false, false, true)]
        public string FormatId
        {
            get
            {
                return _format != null ? _format.Id : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _format = null;
                }
                _format =
                    (from f in ReportFormat.ListReportFormats()
                     where f.Id == value
                     select f)
                     .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets or sets the report format.
        /// </summary>
        /// <value>The report format.</value>
        [XmlIgnore]
        public ReportFormat ReportFormat
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        #endregion

        #region report properties

        /// <summary>
        /// Gets or sets the report.
        /// </summary>
        /// <value>The report.</value>
        [XmlIgnore]
        public Report Report
        {
            get
            {
                return _report;
            }
            set
            {
                _report = value;
            }
        }

        /// <summary>
        /// Gets or sets the report id.
        /// </summary>
        /// <value>The report id.</value>
        [DataObjectField(false, false, false)]
        public string ReportId
        {
            get
            {
                return _report != null ? _report.Id : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _report = null;
                }
                if (string.IsNullOrEmpty(value))
                {
                    _report = null;
                }
                if (string.IsNullOrEmpty(this.OwnerId))
                {
                    throw new InvalidOperationException("ReportId cannot be set without an OwnerId");
                }
                if (!this.Application.HasValue)
                {
                    throw new InvalidOperationException("ReportId cannot be set without an Application");
                }
                if (this.ReportingEnvironmentId == 0)
                {
                    throw new InvalidOperationException("ReportId cannot be set without a ReportEnvironmentId");
                }
                _report = Report.LoadReport(value, int.Parse(this.OwnerId), null, this.Application.Value, this.ReportingEnvironmentId);
            }
        }

        /// <summary>
        /// Gets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        [System.Xml.Serialization.XmlIgnore]
        public string ReportName
        {
            get
            {
                return _report != null ? _report.Name : null;
            }
        }

        /// <summary>
        /// Gets the report description.
        /// </summary>
        /// <value>The report description.</value>
        [System.Xml.Serialization.XmlIgnore]
        public string ReportDescription
        {
            get
            {
                return _report != null ? _report.Description : null;
            }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        [System.Xml.Serialization.XmlIgnore]
        public string Category
        {
            get
            {
                return _report != null ? _report.Category : null;
            }
        }

        #endregion        
    
        public ReportingEnvironmentEnum ReportingEnvironmentId { get; set; }

        public string ReportingEnvironmentName { get; set; }
    }
}
