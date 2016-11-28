using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.IO;
using Emdat.InVision.SSRS;
using System.Net;
using System.Globalization;

namespace Emdat.InVision
{
    /// <summary>
    /// Report class
    /// </summary>
    [Serializable]
    [DataObject(true)]
    public class Report
    {
		public static readonly string[] TimeFormats = { 
                    "t", 
                    "T",                     
                    "hh:mm tt",                     
                    "hh:mm t",
                    "h:mm tt", 
                    "h:mm t",                     
                    "HH:mm",                    
                    "H:mm",
                    "hh:mmtt", 
                    "hh:mmt", 
					"h:mmtt",
                    "h:mmt"
                };

        #region data methods

        /// <summary>
        /// Lists the reports.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<Report> ListReports(int userId, ReportingApplication application)
        {
            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                var reports =
                    from r in sql.ListReports(userId)
                    orderby r.CategorySortOrder, r.SortOrder
                    select new Report
                    {
                        Id = r.ReportID.ToString(),
                        Name = r.Name,
                        Description = r.Description,
                        ReportingServicesPath = r.ReportPath,
                        Category = r.CategoryName,
                        ReportParametersUrl = r.ReportParametersURL,
                        ReportingEnvironmentId = (ReportingEnvironmentEnum)r.EnvironmentID,
                        ReportingEnvironmentName = r.EnvironmentName
                    };
                return reports.ToList();
            }
        }

        /// <summary>
        /// Gets all report categories.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<Category> ListReportsByCategory(int userId, ReportingApplication application)
        {
            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                var reportCategories =
                    from r in sql.ListReports(userId)
                    orderby r.CategorySortOrder
                    group r by r.CategoryName into categoryGroup
                    select new Category
                    {
                        Name = !string.IsNullOrEmpty(categoryGroup.Key) ? categoryGroup.Key : "Miscellaneous Reports",
                        Reports =
                            from r in categoryGroup
                            orderby
                                r.SortOrder.HasValue ? r.SortOrder.Value : int.MaxValue,
                                r.Name
                            select new Report
                            {
                                Id = r.ReportID.ToString(),
                                Name = r.Name,
                                Description = r.Description,
                                ReportingServicesPath = r.ReportPath,
                                Category = categoryGroup.Key,
                                ReportParametersUrl = r.ReportParametersURL,
                                ReportingEnvironmentId = (ReportingEnvironmentEnum)r.EnvironmentID,
                                ReportingEnvironmentName = r.EnvironmentName
                            }
                    };

                return reportCategories.ToList();
            }
        }

        /// <summary>
        /// Loads the report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Report LoadReport(string reportId, int userId, int? clientId, ReportingApplication application, ReportingEnvironmentEnum environment)
        {
            #region input validation

            if (string.IsNullOrEmpty(reportId))
            {
                throw new ArgumentNullException("reportId");
            }

            int reportIdValue = int.Parse(reportId);

            #endregion

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                Report report =
                    (from r in sql.GetReport(reportIdValue, (int)environment)
                     select new Report
                     {
                         Id = r.ReportID.ToString(),
                         Name = r.Name,
                         Description = r.Description,
                         ReportingServicesPath = r.ReportPath,
                         Category = r.CategoryName,
                         ReportParametersUrl = r.ReportParametersURL,
                         Options = r.Options
                     })
                     .FirstOrDefault();
                if (report == null)
                {
                    throw new KeyNotFoundException(string.Format("The report could not be found: {0}", reportId));
                }
                else
                {
                    if (ReportingApplication.InQuiry == application && clientId.HasValue)
                    {
                        var clientLabels = ClientLabels.LoadClientLabels(clientId.Value, application);
                        clientLabels.ApplyToReport(report);
                    }
                }
                return report;
            }
        }

        public string GetSpecificReportPath(IEnumerable<ParameterValue> parameters)
        {
            return GetSpecificReportPath(this.Id, this.ReportingServicesPath, parameters);
        }

        public static string GetSpecificReportPath(string reportId, string reportPath, IEnumerable<ParameterValue> parameters)
        {
            string path = reportPath;
            if (parameters == null)
            {
                return path;
            }
            //TODO: uncomment the following section when No Detail report path thing is implemented
            //using (ReportingDataContext context = new ReportingDataContext())
            //{
            //    var prms = parameters.ToDictionary(n => n.Name);
            //    var result = context.GetReportOptions(int.Parse(reportId));
            //    if (string.IsNullOrEmpty(result.Options))
            //    {
            //        return path;
            //    }
            //    XElement ele = XElement.Parse(result.Options);
            //    var s = (from el in ele.Elements("SortingsAndGroupings")
            //            let noDetailName = (string)el.Attribute("noDetailName")
            //            let sortName = (string)el.Attribute("sortName")
            //            where prms.ContainsKey(sortName)
            //            from e in el.Elements()
            //            where (string)e.Attribute("value") == prms[sortName].Value
            //            let hideDetail = prms.ContainsKey(noDetailName) && bool.Parse(prms[noDetailName].Value)
            //            select hideDetail ? (string)e.Attribute("noDetailRDL") : (string)e.Attribute("detailRDL")
            //            ).FirstOrDefault();
            //    if (!string.IsNullOrEmpty(s))
            //    {
            //        path = s;
            //    }
            //}
            return path;
        }

        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <param name="reportPath">The report path.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<ReportParameterViewItem> GetReportParameters(
            string reportId,
            IEnumerable<ParameterValue> parameterValues,
            GetReportParameterOptions options,
            int userId,
            int? clientId,
            ReportingApplication application,
            ReportingEnvironmentEnum environment)
        {
            //load report info to get the SSRS path
            Report report = LoadReport(reportId, userId, clientId, application, environment);
            if (report == null)
            {
                throw new KeyNotFoundException(string.Format("The report was not found: {0}", reportId));
            }

            string path = report.GetSpecificReportPath(parameterValues);

            using (var ssrs = new ReportingService2005())
            {
                //HACK: To get around issues between DEV-DB02 and the DC, use a local user
                if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
                {
                    ssrs.UseDefaultCredentials = false;
                    ssrs.Credentials = new NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
                        System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"]);
                }
                else
                {
                    ssrs.UseDefaultCredentials = true;
                }

                //first, get default report parameters
                var prms = ssrs.GetReportParameters(
                    path,
                    null,
                    true,
                    null,
                    null);

                //build a list of dependent parameters
                var dependentPrms =
                    from p in prms
                    where p.Dependencies != null
                    from d in p.Dependencies
                    where (from q in prms
                           where q.Name == d
                           where q.PromptUser
                           select q).Any() //Do not include internal parameters as dependencies as the UI does not need to refresh.
                    select new
                    {
                        ParameterName = d,
                        Dependent = p.Name
                    };

                //if parameter values were specified, calculate "actual" values and validate again
                if (parameterValues != null)
                {
                    var actualParameterValues =
                        from p in prms
                        join v in parameterValues on p.Name equals v.Name
                        select new ParameterValue
                        {
                            Name = p.Name,
                            Value = _GetActualParameterValue(p, v)
                        };

                    prms = ssrs.GetReportParameters(
                        path,
                        null,
                        true,
                        actualParameterValues.ToArray(),
                        null);
                }

                //build view item list
                var prmViewItems =
                    from p in prms
                    where p.PromptUser //don't return internal (read-only) parameters
                    join v in parameterValues ?? new List<ParameterValue>() on p.Name equals v.Name into pvGroup
                    from pv in pvGroup.DefaultIfEmpty(GetDefaultParameterValue(p))
                    let dependents = from d in dependentPrms where d.ParameterName == p.Name select d.Dependent
                    select new ReportParameterViewItem(p, pv, dependents.ToArray());

                //return view items depending on the options specified
                return
                    from p in prmViewItems
                    where (GetReportParameterOptions.IncludeAllParameters == (options & GetReportParameterOptions.IncludeAllParameters)) ||
                        p.IsVisible == (options == GetReportParameterOptions.IncludeVisibleParameters)
                    select p;

            }
        }

        internal static string _GetActualParameterValue(ReportParameter p, ParameterValue v)
        {
            if (v.Value == null)
            {
                return null;
            }
            else if (p.Type == ParameterTypeEnum.DateTime)
            {
                //extract the date part of the value 
                DateTime? date;
                try
                {
                    date = DateTimeExpression.Evaluate(v.Value, DateTime.Now);
                }
                catch
                {
                    //this exception will be thrown later for invalid expressions
                    return null;
                }
                return date.HasValue ? date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null;
            }
            else if (p.Type == ParameterTypeEnum.String && p.Name.EndsWith("Time", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(v.Value))
                {
                    return v.Value;
                }

                DateTime dt;
                if (DateTime.TryParseExact(v.Value, Report.TimeFormats, null, DateTimeStyles.None, out dt))
                {
                    return dt.ToString("HH:mm");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return v.Value;
            }
        }

        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public static IEnumerable<ReportParameterViewItem> GetReportParameters(
            string reportId,
            IEnumerable<ParameterValue> parameterValues,
            int userId,
            int? clientId,
            ReportingApplication application,
            ReportingEnvironmentEnum environment)
        {
            return GetReportParameters(reportId, parameterValues, GetReportParameterOptions.IncludeAllParameters, userId, clientId, application, environment);
        }

        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <param name="reportPath">The report path.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<ReportParameterViewItem> GetReportParameters(string reportId, int userId, int? clientId, ReportingApplication application, ReportingEnvironmentEnum environment)
        {
            return GetReportParameters(reportId, null, userId, clientId, application, environment);
        }

        /// <summary>
        /// Gets the default parameter value.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private static ParameterValue GetDefaultParameterValue(ReportParameter p)
        {
            if (p.DefaultValues != null && p.DefaultValues.Length > 0)
            {
                return new ParameterValue
                {
                    Name = p.Name,
                    Value = p.DefaultValues[0]
                };
            }
            return null;
        }

        /// <summary>
        /// Gets the time zone of the Base Transcription Company for this specified report parameters.
        /// </summary>
        /// <returns></returns>
        public static string GetBaseCompanyTimeZoneIdentifier(string reportPath, List<ParameterValue> parameters, string defaultTimeZone)
        {
            if (string.IsNullOrEmpty(reportPath))
            {
                throw new ArgumentNullException("reportPath");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            string baseCoTimeZone = defaultTimeZone;

            //when needed, BaseCo time zone is an internal parameter to the RDL, 
            //if it is not defined, we need to use a default (server time zone).
            using (var ssrs = new ReportingService2005())
            {
                //HACK: To get around issues between DEV-DB02 and the DC, use a local user
                if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
                {
                    ssrs.UseDefaultCredentials = false;
                    ssrs.Credentials = new NetworkCredential(
                        System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
                        System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"]);
                }
                else
                {
                    ssrs.UseDefaultCredentials = true;
                }

                //first, get default report parameters
                var prms = ssrs.GetReportParameters(
                    reportPath,
                    null,
                    true,
                    null,
                    null);

                //use those values to calculate actual report parameters
                var actualParameterValues =
                    from p in prms
                    where p.PromptUser == true //just skip internal parameters
                    join v in parameters on p.Name equals v.Name
                    select new ParameterValue
                    {
                        Name = p.Name,
                        Value = Report._GetActualParameterValue(p, v)
                    };

                //finally, retrieve parameters again, which will evaluate all defaults
                prms = ssrs.GetReportParameters(
                    reportPath,
                    null,
                    true,
                    actualParameterValues.ToArray(),
                    null);

                //extract base tranco time zone
                baseCoTimeZone =
                    (from p in prms
                     where p.Name == "BaseTranCo_TZ"
                     where p.DefaultValues != null
                     where p.DefaultValues.Length > 0
                     select p.DefaultValues[0])
                     .DefaultIfEmpty(defaultTimeZone)
                     .FirstOrDefault();
            }
            return baseCoTimeZone;
        }

        #endregion

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataObjectField(true, true, false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        [DataObjectField(false, false, false)]
        public string ReportingServicesPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataObjectField(false, false, false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataObjectField(false, false, true)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [DataObjectField(false, false, true)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the report parameters URL.
        /// </summary>
        /// <value>The report parameters URL.</value>
        [DataObjectField(false, false, false)]
        public string ReportParametersUrl
        {
            get
            {
                return _reportParametersUrl;
            }
            set
            {
                _reportParametersUrl = value;
            }
        }
        private string _reportParametersUrl;

        [DataObjectField(false, false, true)]
        public string Options { get; set; }

        [DataObjectField(true, false, false)]
        public ReportingEnvironmentEnum ReportingEnvironmentId { get; set; }

        [DataObjectField(false, false, false)]
        public string ReportingEnvironmentName { get; set; }
    }
}
