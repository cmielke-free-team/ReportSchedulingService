using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Emdat.InVision.Sql
{
    public partial class ReportingDataContext : IDisposable
    {
        public ReportingDataContext()
            : this(0)
        { }

        public ReportingDataContext(ReportingApplication application, string connectionStringName)
        {
            this.Application = application;
            this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            this.traceSource = new TraceSource("Emdat.InVision.Sql");
        }

        public ReportingDataContext(string connectionStringName)
            : this(0, connectionStringName)
        { }

        public ReportingDataContext(ReportingApplication application)
            : this(application, "Emdat.InVision.ReportInfo")
        {
        }

        public void Dispose()
        {
        }

        public string ConnectionString
        {
            get { return _connectionStringBuilder.ConnectionString; }
            set { _connectionStringBuilder.ConnectionString = value; }
        }
        private SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder();
        private TraceSource traceSource;

        //The text template may not be able to generate wrappers for every proc.
        //Use this file to implement them by hand

        public ReportingApplication Application { get; set; }

        public IEnumerable<IListReportsRow> ListReports(Int32? reportingUserID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.ListReportsByASPUser(reportingUserID).OfType<IListReportsRow>();
                case ReportingApplication.InCommand: return this.ListReportsByMntUser(reportingUserID).OfType<IListReportsRow>();
                case ReportingApplication.InQuiry: return this.ListReportsByInqUser(reportingUserID).OfType<IListReportsRow>();
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<IListExecutionsRow> ListExecutions(Int32? reportingUserID, Int32? reportingCompanyID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.ListExecutionsByASPUser(reportingUserID, reportingCompanyID).OfType<IListExecutionsRow>();
                case ReportingApplication.InCommand: return this.ListExecutionsByMntUser(reportingUserID, reportingCompanyID).OfType<IListExecutionsRow>();
                case ReportingApplication.InQuiry: return this.ListExecutionsByInqUser(reportingUserID, reportingCompanyID).OfType<IListExecutionsRow>();
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<IListSubscriptionsRow> ListSubscriptions(Int32? reportingUserID, Int32? reportingCompanyID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.ListSubscriptionsByASPUser(reportingUserID, reportingCompanyID).OfType<IListSubscriptionsRow>();
                case ReportingApplication.InCommand: return this.ListSubscriptionsByMntUser(reportingUserID, reportingCompanyID).OfType<IListSubscriptionsRow>();
                case ReportingApplication.InQuiry: return this.ListSubscriptionsByInqUser(reportingUserID, reportingCompanyID).OfType<IListSubscriptionsRow>();
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<IGetExecutionRow> GetExecution(Int32? reportingUserID, Int32? reportExecutionID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.GetExecutionForASPUser(reportingUserID, reportExecutionID).OfType<IGetExecutionRow>();
                case ReportingApplication.InCommand: return this.GetExecutionForMntUser(reportingUserID, reportExecutionID).OfType<IGetExecutionRow>();
                case ReportingApplication.InQuiry: return this.GetExecutionForInqUser(reportingUserID, reportExecutionID).OfType<IGetExecutionRow>();
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<IAddSubscriptionRow> AddSubscription(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, int environmentID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.AddSubscriptionForASPUser(reportingUserID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, firstExecutionDate, firstExecutionParameters, modifiedUser, modifiedDate, reportingCompanyID, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID).OfType<IAddSubscriptionRow>();
                case ReportingApplication.InCommand: return this.AddSubscriptionForMntUser(reportingUserID, reportingCompanyID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, firstExecutionDate, firstExecutionParameters, modifiedUser, modifiedDate, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID).OfType<IAddSubscriptionRow>();
                case ReportingApplication.InQuiry: return this.AddSubscriptionForInqUser(reportingUserID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, firstExecutionDate, firstExecutionParameters, modifiedUser, modifiedDate, reportingCompanyID, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID).OfType<IAddSubscriptionRow>();
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<IAddExecutionRow> AddExecution(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, int environmentID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.AddExecutionForASPUser(reportingUserID, name, executionDate, reportID, reportFormatID, parameters, usedHistory, modifiedUser, modifiedDate, reportingCompanyID, environmentID).OfType<IAddExecutionRow>();
                case ReportingApplication.InCommand: return this.AddExecutionForMntUser(reportingUserID, reportingCompanyID, name, executionDate, reportID, reportFormatID, parameters, usedHistory, modifiedUser, modifiedDate, environmentID).OfType<IAddExecutionRow>();
                case ReportingApplication.InQuiry: return this.AddExecutionForInqUser(reportingUserID, name, executionDate, reportID, reportFormatID, parameters, usedHistory, modifiedUser, modifiedDate, reportingCompanyID, environmentID).OfType<IAddExecutionRow>();
                default: throw new NotImplementedException();
            }
        }

        public int EditSubscription(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, int environmentID)
        {
            switch (this.Application)
            {
                case ReportingApplication.ASP: return this.EditSubscriptionForASPUser(reportingUserID, reportSubscriptionID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, nextExecutionDate, nextExecutionParameters, modifiedUser, modifiedDate, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID);
                case ReportingApplication.InCommand: return this.EditSubscriptionForMntUser(reportingUserID, reportSubscriptionID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, nextExecutionDate, nextExecutionParameters, modifiedUser, modifiedDate, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID);
                case ReportingApplication.InQuiry: return this.EditSubscriptionForInqUser(reportingUserID, reportSubscriptionID, reportID, name, reportFormatID, isActive, parameters, scheduleFrequencyID, scheduleFrequencyInterval, scheduleFrequencyRecurrenceFactor, scheduleFrequencyRelativeInterval, scheduleStartTime, scheduleEndTime, nextExecutionDate, nextExecutionParameters, modifiedUser, modifiedDate, useOnScreenNotification, useEmailNotification, emailNotificationAddress, options, environmentID);
                default: throw new NotImplementedException();
            }
        }

        public IDataReader GetExecutionDataReader(int? reportExecutionID)
        {
            this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionData");
            try
            {
                SqlConnection conn = new SqlConnection(this.ConnectionString);
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "DATA_Reports.Reporting.Get_Execution_Data";


                SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value);
                reportExecutionIDParameter.Size = 4;
                reportExecutionIDParameter.Direction = ParameterDirection.Input;
                reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(reportExecutionIDParameter);

                return cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);

            }
            finally
            {
                this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
            }
        }

        public GetExecutionDataStream GetExecutionData(int? reportExecutionID, Stream stream)
        {
            this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionData");
            try
            {
                SqlConnection conn = new SqlConnection(this.ConnectionString);
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "DATA_Reports.Reporting.Get_Execution_Data";


                SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value);
                reportExecutionIDParameter.Size = 4;
                reportExecutionIDParameter.Direction = ParameterDirection.Input;
                reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
                cmd.Parameters.Add(reportExecutionIDParameter);

                using(var reader = cmd.ExecuteReader())
                {
                int ordFileType = reader.GetOrdinal("File_Type");
                    int ordData = reader.GetOrdinal("Data");
                    byte[] buffer = new byte[8192];
                    long dataIndex = 0;
                    int bytesRead = 0;
                    bytesRead = (int)((SqlDataReader)reader).GetBytes(ordData, dataIndex, buffer, 0, buffer.Length);
                    while (bytesRead > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                        dataIndex += bytesRead;
                        bytesRead = (int)((SqlDataReader)reader).GetBytes(ordData, dataIndex, buffer, 0, buffer.Length);
                    }
                    return new GetExecutionDataStream
                    {
                        FileType = (!reader.IsDBNull(ordFileType) ? reader.GetString(ordFileType).Trim() : default(String)),
                    };
                }

            }
            finally
            {
                this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
            }
        }        
    }

    partial class ListReportsByASPUserRow : IListReportsRow { }
    partial class ListReportsByMntUserRow : IListReportsRow { }
    partial class ListReportsByInqUserRow : IListReportsRow { }
    partial class ListExecutionsByASPUserRow : IListExecutionsRow { }
    partial class ListExecutionsByInqUserRow : IListExecutionsRow { }
    partial class ListExecutionsByMntUserRow : IListExecutionsRow { }
    partial class ListSubscriptionsByASPUserRow : IListSubscriptionsRow { }
    partial class ListSubscriptionsByInqUserRow : IListSubscriptionsRow { }
    partial class ListSubscriptionsByMntUserRow : IListSubscriptionsRow { }
    partial class GetExecutionForASPUserRow : IGetExecutionRow { }
    partial class GetExecutionForInqUserRow : IGetExecutionRow { }
    partial class GetExecutionForMntUserRow : IGetExecutionRow { }
    partial class AddSubscriptionForASPUserRow : IAddSubscriptionRow { }
    partial class AddSubscriptionForInqUserRow : IAddSubscriptionRow { }
    partial class AddSubscriptionForMntUserRow : IAddSubscriptionRow { }
    partial class AddExecutionForASPUserRow : IAddExecutionRow { }
    partial class AddExecutionForInqUserRow : IAddExecutionRow { }
    partial class AddExecutionForMntUserRow : IAddExecutionRow { }

    public interface IListReportsRow
    {
        Int32 CategoryID
        {
            get;
            set;
        }
        String CategoryName
        {
            get;
            set;
        }
        Int32? CategorySortOrder
        {
            get;
            set;
        }
        String Description
        {
            get;
            set;
        }
        String Name
        {
            get;
            set;
        }
        Int32 ReportID
        {
            get;
            set;
        }
        Int32? SortOrder
        {
            get;
            set;
        }
        String ReportPath
        {
            get;
            set;
        }
        string ReportParametersURL { get; set; }
        System.Int32? EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
    }

    public interface IListExecutionsRow
    {
        DateTime? EndDate
        {
            get;
            set;
        }
        DateTime? ExpirationDate
        {
            get;
            set;
        }
        String ErrorName
        {
            get;
            set;
        }
        String ErrorDescription
        {
            get;
            set;
        }
        DateTime ModifiedDate
        {
            get;
            set;
        }
        String ModifiedUser
        {
            get;
            set;
        }
        String Name
        {
            get;
            set;
        }
        String Parameters
        {
            get;
            set;
        }
        Int32? ReportExecutionErrorID
        {
            get;
            set;
        }
        Int32 ReportExecutionID
        {
            get;
            set;
        }
        Int32 ReportExecutionStatusID
        {
            get;
            set;
        }
        Int32 ReportFormatID
        {
            get;
            set;
        }
        Int32? ReportSubscriptionID
        {
            get;
            set;
        }
        DateTime? ScheduledStartDate
        {
            get;
            set;
        }
        Int32 ReportID
        {
            get;
            set;
        }
        DateTime? StartDate
        {
            get;
            set;
        }
        Boolean UsedHistory
        {
            get;
            set;
        }
        String CategoryName
        {
            get;
            set;
        }
        String ReportName
        {
            get;
            set;
        }
        String ReportDescription
        {
            get;
            set;
        }
        Int32 OwnerID { get; set; }
        string OwnerNameFirst { get; set; }
        string OwnerNameLast { get; set; }
        string OwnerUsername { get; set; }
        System.Int32 EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
    }

    public interface IListSubscriptionsRow
    {
        System.Int32 ReportSubscriptionID { get; set; }
        System.Int32 ReportID { get; set; }
        System.String Name { get; set; }
        System.Boolean IsActive { get; set; }
        System.Int32 ReportFormatID { get; set; }
        System.String Parameters { get; set; }
        System.String ModifiedUser { get; set; }
        System.DateTime ModifiedDate { get; set; }
        System.Int32 ScheduleFrequencyID { get; set; }
        System.Int32 FrequencyInterval { get; set; }
        System.Int32? FrequencyRecurrenceFactor { get; set; }
        System.String FrequencyRelativeInterval { get; set; }
        System.String StartTime { get; set; }
        System.String EndTime { get; set; }
        System.Int32 OwnerID { get; set; }
        System.String OwnerNameFirst { get; set; }
        System.String OwnerNameLast { get; set; }
        System.String OwnerUsername { get; set; }
        System.String Options { get; set; }
        System.Int32? EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
    }

    public interface IGetExecutionRow
    {
        System.Int32 ReportExecutionID { get; set; }
        System.String Name { get; set; }
        System.Int32? ReportSubscriptionID { get; set; }
        System.Int32 ReportID { get; set; }
        System.String ReportName { get; set; }
        System.String ReportDescription { get; set; }
        System.String CategoryName { get; set; }
        System.String Parameters { get; set; }
        System.Int32 ReportFormatID { get; set; }
        System.DateTime? ScheduledStartDate { get; set; }
        System.DateTime? StartDate { get; set; }
        System.DateTime? EndDate { get; set; }
        System.DateTime? ExpirationDate { get; set; }
        System.Boolean UsedHistory { get; set; }
        System.Int32 ReportExecutionStatusID { get; set; }
        System.Int32? ReportExecutionErrorID { get; set; }
        System.String ErrorName { get; set; }
        System.String ErrorDescription { get; set; }
        System.DateTime ModifiedDate { get; set; }
        System.String ModifiedUser { get; set; }
        System.Int32 OwnerID { get; set; }
        System.String OwnerNameFirst { get; set; }
        System.String OwnerNameLast { get; set; }
        System.String OwnerUsername { get; set; }
        System.String RenderOptions { get; set; }
        System.Int32 EnvironmentID { get; set; }
        string EnvironmentName { get; set; }
    }

    public interface IAddSubscriptionRow
    {
        System.Int32? ReportSubscriptionID { get; set; }
    }

    public interface IAddExecutionRow
    {
        System.Int32? ReportExecutionID { get; set; }
    }

    public partial class GetExecutionDataStream
    {
        public String FileType
        {
            get;
            set;
        }
    }
}
