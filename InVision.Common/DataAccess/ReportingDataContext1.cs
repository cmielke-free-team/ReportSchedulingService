using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Emdat.InVision.Sql
{
    partial class ReportingDataContext : IReportingDataContext
    {
		public IEnumerable<GetAssociate2Row> GetAssociate2(System.Int32? associateID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetAssociate2");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Associate2";
				
					
					SqlParameter associateIDParameter = new SqlParameter("@Associate_ID", (object)associateID ?? DBNull.Value); 
					associateIDParameter.Size = 4;
					associateIDParameter.Direction = ParameterDirection.Input;
					associateIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(associateIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Associate2", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordAssociateID = reader.GetOrdinal("Associate_ID"); 
						int ordClientID = reader.GetOrdinal("Client_ID"); 
						int ordAssociateClientCode = reader.GetOrdinal("Associate_Client_Code"); 
						int ordAssociatePrefix = reader.GetOrdinal("Associate_Prefix"); 
						int ordAssociateNameFirst = reader.GetOrdinal("Associate_Name_First"); 
						int ordAssociateNameMiddle = reader.GetOrdinal("Associate_Name_Middle"); 
						int ordAssociateNameLast = reader.GetOrdinal("Associate_Name_Last"); 
						int ordAssociateSuffix = reader.GetOrdinal("Associate_Suffix"); 
						int ordAssociateBusinessName = reader.GetOrdinal("Associate_Business_Name"); 
						int ordAssociateSpecialty = reader.GetOrdinal("Associate_Specialty"); 
						int ordAssociateGreeting = reader.GetOrdinal("Associate_Greeting"); 
						int ordAssociateAddress1 = reader.GetOrdinal("Associate_Address_1"); 
						int ordAssociateAddress2 = reader.GetOrdinal("Associate_Address_2"); 
						int ordAssociateAddress3 = reader.GetOrdinal("Associate_Address_3"); 
						int ordAssociateCity = reader.GetOrdinal("Associate_City"); 
						int ordAssociateState = reader.GetOrdinal("Associate_State"); 
						int ordAssociateZipCode = reader.GetOrdinal("Associate_Zip_Code"); 
						int ordAssociateCompany = reader.GetOrdinal("Associate_Company"); 
						int ordAssociatePhone = reader.GetOrdinal("Associate_Phone"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordReferralUserID = reader.GetOrdinal("Referral_User_ID"); 
						int ordAssociateFax = reader.GetOrdinal("Associate_Fax"); 
						int ordAssociateEMail = reader.GetOrdinal("Associate_EMail"); 
						int ordReferralLocationID = reader.GetOrdinal("Referral_Location_ID"); 
						int ordRegionMask = reader.GetOrdinal("Region_Mask"); 
						int ordAssociateUsername = reader.GetOrdinal("Associate_Username"); 
						int ordFilePath = reader.GetOrdinal("File_Path"); 
						int ordExportType = reader.GetOrdinal("Export_Type"); 
						int ordAssociateActive = reader.GetOrdinal("Associate_Active"); 
						int ordAssociateSuspended = reader.GetOrdinal("Associate_Suspended"); 
						int ordHCNUserID = reader.GetOrdinal("HCN_User_ID"); 
						int ordHCNAccountID = reader.GetOrdinal("HCN_Account_ID"); 
						int ordAssociateAutoFax = reader.GetOrdinal("Associate_AutoFax");
						do
						{
							GetAssociate2Row row = new GetAssociate2Row();
							row.AssociateID = (!reader.IsDBNull(ordAssociateID) ? reader.GetInt32(ordAssociateID) : default(int));
							row.ClientID = (!reader.IsDBNull(ordClientID) ? reader.GetInt32(ordClientID) : default(int));
							row.AssociateClientCode = (!reader.IsDBNull(ordAssociateClientCode) ? reader.GetString(ordAssociateClientCode).Trim() : default(string));
							row.AssociatePrefix = (!reader.IsDBNull(ordAssociatePrefix) ? reader.GetString(ordAssociatePrefix).Trim() : default(string));
							row.AssociateNameFirst = (!reader.IsDBNull(ordAssociateNameFirst) ? reader.GetString(ordAssociateNameFirst).Trim() : default(string));
							row.AssociateNameMiddle = (!reader.IsDBNull(ordAssociateNameMiddle) ? reader.GetString(ordAssociateNameMiddle).Trim() : default(string));
							row.AssociateNameLast = (!reader.IsDBNull(ordAssociateNameLast) ? reader.GetString(ordAssociateNameLast).Trim() : default(string));
							row.AssociateSuffix = (!reader.IsDBNull(ordAssociateSuffix) ? reader.GetString(ordAssociateSuffix).Trim() : default(string));
							row.AssociateBusinessName = (!reader.IsDBNull(ordAssociateBusinessName) ? reader.GetString(ordAssociateBusinessName).Trim() : default(string));
							row.AssociateSpecialty = (!reader.IsDBNull(ordAssociateSpecialty) ? reader.GetString(ordAssociateSpecialty).Trim() : default(string));
							row.AssociateGreeting = (!reader.IsDBNull(ordAssociateGreeting) ? reader.GetString(ordAssociateGreeting).Trim() : default(string));
							row.AssociateAddress1 = (!reader.IsDBNull(ordAssociateAddress1) ? reader.GetString(ordAssociateAddress1).Trim() : default(string));
							row.AssociateAddress2 = (!reader.IsDBNull(ordAssociateAddress2) ? reader.GetString(ordAssociateAddress2).Trim() : default(string));
							row.AssociateAddress3 = (!reader.IsDBNull(ordAssociateAddress3) ? reader.GetString(ordAssociateAddress3).Trim() : default(string));
							row.AssociateCity = (!reader.IsDBNull(ordAssociateCity) ? reader.GetString(ordAssociateCity).Trim() : default(string));
							row.AssociateState = (!reader.IsDBNull(ordAssociateState) ? reader.GetString(ordAssociateState).Trim() : default(string));
							row.AssociateZipCode = (!reader.IsDBNull(ordAssociateZipCode) ? reader.GetString(ordAssociateZipCode).Trim() : default(string));
							row.AssociateCompany = (!reader.IsDBNull(ordAssociateCompany) ? reader.GetString(ordAssociateCompany).Trim() : default(string));
							row.AssociatePhone = (!reader.IsDBNull(ordAssociatePhone) ? reader.GetString(ordAssociatePhone).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime?));
							row.ReferralUserID = (!reader.IsDBNull(ordReferralUserID) ? reader.GetInt32(ordReferralUserID) : default(int?));
							row.AssociateFax = (!reader.IsDBNull(ordAssociateFax) ? reader.GetString(ordAssociateFax).Trim() : default(string));
							row.AssociateEMail = (!reader.IsDBNull(ordAssociateEMail) ? reader.GetString(ordAssociateEMail).Trim() : default(string));
							row.ReferralLocationID = (!reader.IsDBNull(ordReferralLocationID) ? reader.GetInt32(ordReferralLocationID) : default(int?));
							row.RegionMask = (!reader.IsDBNull(ordRegionMask) ? reader.GetInt32(ordRegionMask) : default(int));
							row.AssociateUsername = (!reader.IsDBNull(ordAssociateUsername) ? reader.GetString(ordAssociateUsername).Trim() : default(string));
							row.FilePath = (!reader.IsDBNull(ordFilePath) ? reader.GetString(ordFilePath).Trim() : default(string));
							row.ExportType = (!reader.IsDBNull(ordExportType) ? reader.GetInt32(ordExportType) : default(int));
							row.AssociateActive = (!reader.IsDBNull(ordAssociateActive) ? reader.GetBoolean(ordAssociateActive) : default(bool));
							row.AssociateSuspended = (!reader.IsDBNull(ordAssociateSuspended) ? reader.GetBoolean(ordAssociateSuspended) : default(bool));
							row.HCNUserID = (!reader.IsDBNull(ordHCNUserID) ? reader.GetString(ordHCNUserID).Trim() : default(string));
							row.HCNAccountID = (!reader.IsDBNull(ordHCNAccountID) ? reader.GetString(ordHCNAccountID).Trim() : default(string));
							row.AssociateAutoFax = (!reader.IsDBNull(ordAssociateAutoFax) ? reader.GetBoolean(ordAssociateAutoFax) : default(bool));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<SearchAssociates2Row> SearchAssociates2(System.Int32? clientID, System.String associateFirstName, System.String associateLastName, System.String associateClientCode, System.Boolean? checkfax)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "SearchAssociates2");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Search_Associates2";
				
					
					SqlParameter clientIDParameter = new SqlParameter("@Client_ID", (object)clientID ?? DBNull.Value); 
					clientIDParameter.Size = 4;
					clientIDParameter.Direction = ParameterDirection.Input;
					clientIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(clientIDParameter);
					
					SqlParameter associateFirstNameParameter = new SqlParameter("@Associate_First_Name", (object)associateFirstName ?? DBNull.Value); 
					associateFirstNameParameter.Size = 50;
					associateFirstNameParameter.Direction = ParameterDirection.Input;
					associateFirstNameParameter.SqlDbType = SqlDbType.Char;
					cmd.Parameters.Add(associateFirstNameParameter);
					
					SqlParameter associateLastNameParameter = new SqlParameter("@Associate_Last_Name", (object)associateLastName ?? DBNull.Value); 
					associateLastNameParameter.Size = 50;
					associateLastNameParameter.Direction = ParameterDirection.Input;
					associateLastNameParameter.SqlDbType = SqlDbType.Char;
					cmd.Parameters.Add(associateLastNameParameter);
					
					SqlParameter associateClientCodeParameter = new SqlParameter("@Associate_Client_Code", (object)associateClientCode ?? DBNull.Value); 
					associateClientCodeParameter.Size = 20;
					associateClientCodeParameter.Direction = ParameterDirection.Input;
					associateClientCodeParameter.SqlDbType = SqlDbType.Char;
					cmd.Parameters.Add(associateClientCodeParameter);
					
					SqlParameter checkfaxParameter = new SqlParameter("@CheckFax", (object)checkfax ?? DBNull.Value); 
					checkfaxParameter.Size = 1;
					checkfaxParameter.Direction = ParameterDirection.Input;
					checkfaxParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(checkfaxParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Search_Associates2", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordAssociateID = reader.GetOrdinal("Associate_ID"); 
						int ordClientID = reader.GetOrdinal("Client_ID"); 
						int ordAssociateClientCode = reader.GetOrdinal("Associate_Client_Code"); 
						int ordAssociatePrefix = reader.GetOrdinal("Associate_Prefix"); 
						int ordAssociateNameFirst = reader.GetOrdinal("Associate_Name_First"); 
						int ordAssociateNameMiddle = reader.GetOrdinal("Associate_Name_Middle"); 
						int ordAssociateNameLast = reader.GetOrdinal("Associate_Name_Last"); 
						int ordAssociateSuffix = reader.GetOrdinal("Associate_Suffix"); 
						int ordAssociateBusinessName = reader.GetOrdinal("Associate_Business_Name"); 
						int ordAssociateSpecialty = reader.GetOrdinal("Associate_Specialty"); 
						int ordAssociateGreeting = reader.GetOrdinal("Associate_Greeting"); 
						int ordAssociateAddress1 = reader.GetOrdinal("Associate_Address_1"); 
						int ordAssociateAddress2 = reader.GetOrdinal("Associate_Address_2"); 
						int ordAssociateAddress3 = reader.GetOrdinal("Associate_Address_3"); 
						int ordAssociateCity = reader.GetOrdinal("Associate_City"); 
						int ordAssociateState = reader.GetOrdinal("Associate_State"); 
						int ordAssociateZipCode = reader.GetOrdinal("Associate_Zip_Code"); 
						int ordAssociateCompany = reader.GetOrdinal("Associate_Company"); 
						int ordAssociatePhone = reader.GetOrdinal("Associate_Phone"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordReferralUserID = reader.GetOrdinal("Referral_User_ID"); 
						int ordAssociateFax = reader.GetOrdinal("Associate_Fax"); 
						int ordAssociateEMail = reader.GetOrdinal("Associate_EMail"); 
						int ordReferralLocationID = reader.GetOrdinal("Referral_Location_ID"); 
						int ordRegionMask = reader.GetOrdinal("Region_Mask"); 
						int ordAssociateUsername = reader.GetOrdinal("Associate_Username"); 
						int ordFilePath = reader.GetOrdinal("File_Path"); 
						int ordExportType = reader.GetOrdinal("Export_Type"); 
						int ordAssociateActive = reader.GetOrdinal("Associate_Active"); 
						int ordAssociateSuspended = reader.GetOrdinal("Associate_Suspended"); 
						int ordHCNUserID = reader.GetOrdinal("HCN_User_ID"); 
						int ordHCNAccountID = reader.GetOrdinal("HCN_Account_ID"); 
						int ordAssociateAutoFax = reader.GetOrdinal("Associate_AutoFax");
						do
						{
							SearchAssociates2Row row = new SearchAssociates2Row();
							row.AssociateID = (!reader.IsDBNull(ordAssociateID) ? reader.GetInt32(ordAssociateID) : default(int));
							row.ClientID = (!reader.IsDBNull(ordClientID) ? reader.GetInt32(ordClientID) : default(int));
							row.AssociateClientCode = (!reader.IsDBNull(ordAssociateClientCode) ? reader.GetString(ordAssociateClientCode).Trim() : default(string));
							row.AssociatePrefix = (!reader.IsDBNull(ordAssociatePrefix) ? reader.GetString(ordAssociatePrefix).Trim() : default(string));
							row.AssociateNameFirst = (!reader.IsDBNull(ordAssociateNameFirst) ? reader.GetString(ordAssociateNameFirst).Trim() : default(string));
							row.AssociateNameMiddle = (!reader.IsDBNull(ordAssociateNameMiddle) ? reader.GetString(ordAssociateNameMiddle).Trim() : default(string));
							row.AssociateNameLast = (!reader.IsDBNull(ordAssociateNameLast) ? reader.GetString(ordAssociateNameLast).Trim() : default(string));
							row.AssociateSuffix = (!reader.IsDBNull(ordAssociateSuffix) ? reader.GetString(ordAssociateSuffix).Trim() : default(string));
							row.AssociateBusinessName = (!reader.IsDBNull(ordAssociateBusinessName) ? reader.GetString(ordAssociateBusinessName).Trim() : default(string));
							row.AssociateSpecialty = (!reader.IsDBNull(ordAssociateSpecialty) ? reader.GetString(ordAssociateSpecialty).Trim() : default(string));
							row.AssociateGreeting = (!reader.IsDBNull(ordAssociateGreeting) ? reader.GetString(ordAssociateGreeting).Trim() : default(string));
							row.AssociateAddress1 = (!reader.IsDBNull(ordAssociateAddress1) ? reader.GetString(ordAssociateAddress1).Trim() : default(string));
							row.AssociateAddress2 = (!reader.IsDBNull(ordAssociateAddress2) ? reader.GetString(ordAssociateAddress2).Trim() : default(string));
							row.AssociateAddress3 = (!reader.IsDBNull(ordAssociateAddress3) ? reader.GetString(ordAssociateAddress3).Trim() : default(string));
							row.AssociateCity = (!reader.IsDBNull(ordAssociateCity) ? reader.GetString(ordAssociateCity).Trim() : default(string));
							row.AssociateState = (!reader.IsDBNull(ordAssociateState) ? reader.GetString(ordAssociateState).Trim() : default(string));
							row.AssociateZipCode = (!reader.IsDBNull(ordAssociateZipCode) ? reader.GetString(ordAssociateZipCode).Trim() : default(string));
							row.AssociateCompany = (!reader.IsDBNull(ordAssociateCompany) ? reader.GetString(ordAssociateCompany).Trim() : default(string));
							row.AssociatePhone = (!reader.IsDBNull(ordAssociatePhone) ? reader.GetString(ordAssociatePhone).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime?));
							row.ReferralUserID = (!reader.IsDBNull(ordReferralUserID) ? reader.GetInt32(ordReferralUserID) : default(int?));
							row.AssociateFax = (!reader.IsDBNull(ordAssociateFax) ? reader.GetString(ordAssociateFax).Trim() : default(string));
							row.AssociateEMail = (!reader.IsDBNull(ordAssociateEMail) ? reader.GetString(ordAssociateEMail).Trim() : default(string));
							row.ReferralLocationID = (!reader.IsDBNull(ordReferralLocationID) ? reader.GetInt32(ordReferralLocationID) : default(int?));
							row.RegionMask = (!reader.IsDBNull(ordRegionMask) ? reader.GetInt32(ordRegionMask) : default(int));
							row.AssociateUsername = (!reader.IsDBNull(ordAssociateUsername) ? reader.GetString(ordAssociateUsername).Trim() : default(string));
							row.FilePath = (!reader.IsDBNull(ordFilePath) ? reader.GetString(ordFilePath).Trim() : default(string));
							row.ExportType = (!reader.IsDBNull(ordExportType) ? reader.GetInt32(ordExportType) : default(int));
							row.AssociateActive = (!reader.IsDBNull(ordAssociateActive) ? reader.GetBoolean(ordAssociateActive) : default(bool));
							row.AssociateSuspended = (!reader.IsDBNull(ordAssociateSuspended) ? reader.GetBoolean(ordAssociateSuspended) : default(bool));
							row.HCNUserID = (!reader.IsDBNull(ordHCNUserID) ? reader.GetString(ordHCNUserID).Trim() : default(string));
							row.HCNAccountID = (!reader.IsDBNull(ordHCNAccountID) ? reader.GetString(ordHCNAccountID).Trim() : default(string));
							row.AssociateAutoFax = (!reader.IsDBNull(ordAssociateAutoFax) ? reader.GetBoolean(ordAssociateAutoFax) : default(bool));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListReportsByASPUserRow> ListReportsByASPUser(System.Int32? reportingUserID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListReportsByASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Reports_ByASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Reports_ByASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordCategoryID = reader.GetOrdinal("Category_ID"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordCategorySortOrder = reader.GetOrdinal("Category_SortOrder"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordSortOrder = reader.GetOrdinal("Sort_Order"); 
						int ordReportPath = reader.GetOrdinal("Report_Path"); 
						int ordReportParametersURL = reader.GetOrdinal("Report_Parameters_URL"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListReportsByASPUserRow row = new ListReportsByASPUserRow();
							row.CategoryID = (!reader.IsDBNull(ordCategoryID) ? reader.GetInt32(ordCategoryID) : default(int));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.CategorySortOrder = (!reader.IsDBNull(ordCategorySortOrder) ? reader.GetInt32(ordCategorySortOrder) : default(int?));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.SortOrder = (!reader.IsDBNull(ordSortOrder) ? reader.GetInt32(ordSortOrder) : default(int?));
							row.ReportPath = (!reader.IsDBNull(ordReportPath) ? reader.GetString(ordReportPath).Trim() : default(string));
							row.ReportParametersURL = (!reader.IsDBNull(ordReportParametersURL) ? reader.GetString(ordReportParametersURL).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListReportsByMntUserRow> ListReportsByMntUser(System.Int32? reportingUserID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListReportsByMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Reports_ByMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Reports_ByMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordCategoryID = reader.GetOrdinal("Category_ID"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordCategorySortOrder = reader.GetOrdinal("Category_SortOrder"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordSortOrder = reader.GetOrdinal("Sort_Order"); 
						int ordReportPath = reader.GetOrdinal("Report_Path"); 
						int ordReportParametersURL = reader.GetOrdinal("Report_Parameters_URL"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListReportsByMntUserRow row = new ListReportsByMntUserRow();
							row.CategoryID = (!reader.IsDBNull(ordCategoryID) ? reader.GetInt32(ordCategoryID) : default(int));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.CategorySortOrder = (!reader.IsDBNull(ordCategorySortOrder) ? reader.GetInt32(ordCategorySortOrder) : default(int?));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.SortOrder = (!reader.IsDBNull(ordSortOrder) ? reader.GetInt32(ordSortOrder) : default(int?));
							row.ReportPath = (!reader.IsDBNull(ordReportPath) ? reader.GetString(ordReportPath).Trim() : default(string));
							row.ReportParametersURL = (!reader.IsDBNull(ordReportParametersURL) ? reader.GetString(ordReportParametersURL).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListReportsByInqUserRow> ListReportsByInqUser(System.Int32? reportingUserID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListReportsByInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Reports_ByInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Reports_ByInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordCategoryID = reader.GetOrdinal("Category_ID"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordCategorySortOrder = reader.GetOrdinal("Category_SortOrder"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordSortOrder = reader.GetOrdinal("Sort_Order"); 
						int ordReportPath = reader.GetOrdinal("Report_Path"); 
						int ordReportParametersURL = reader.GetOrdinal("Report_Parameters_URL"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListReportsByInqUserRow row = new ListReportsByInqUserRow();
							row.CategoryID = (!reader.IsDBNull(ordCategoryID) ? reader.GetInt32(ordCategoryID) : default(int));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.CategorySortOrder = (!reader.IsDBNull(ordCategorySortOrder) ? reader.GetInt32(ordCategorySortOrder) : default(int?));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.SortOrder = (!reader.IsDBNull(ordSortOrder) ? reader.GetInt32(ordSortOrder) : default(int?));
							row.ReportPath = (!reader.IsDBNull(ordReportPath) ? reader.GetString(ordReportPath).Trim() : default(string));
							row.ReportParametersURL = (!reader.IsDBNull(ordReportParametersURL) ? reader.GetString(ordReportParametersURL).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListExecutionStatusesRow> ListExecutionStatuses()
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListExecutionStatuses");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Execution_Statuses";
				

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Execution_Statuses", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID");
						do
						{
							ListExecutionStatusesRow row = new ListExecutionStatusesRow();
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListFormatsRow> ListFormats()
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListFormats");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Formats";
				

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Formats", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordSSRSFormat = reader.GetOrdinal("SSRS_Format"); 
						int ordSSRSDeviceInfo = reader.GetOrdinal("SSRS_DeviceInfo"); 
						int ordExtension = reader.GetOrdinal("Extension"); 
						int ordHTTPContentType = reader.GetOrdinal("HTTP_Content_Type");
						do
						{
							ListFormatsRow row = new ListFormatsRow();
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.SSRSFormat = (!reader.IsDBNull(ordSSRSFormat) ? reader.GetString(ordSSRSFormat).Trim() : default(string));
							row.SSRSDeviceInfo = (!reader.IsDBNull(ordSSRSDeviceInfo) ? reader.GetString(ordSSRSDeviceInfo).Trim() : default(string));
							row.Extension = (!reader.IsDBNull(ordExtension) ? reader.GetString(ordExtension).Trim() : default(string));
							row.HTTPContentType = (!reader.IsDBNull(ordHTTPContentType) ? reader.GetString(ordHTTPContentType).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetFormatRow> GetFormat(System.Int32? reportFormatID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetFormat");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Format";
				
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Format", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordSSRSFormat = reader.GetOrdinal("SSRS_Format"); 
						int ordSSRSDeviceInfo = reader.GetOrdinal("SSRS_DeviceInfo"); 
						int ordExtension = reader.GetOrdinal("Extension"); 
						int ordHTTPContentType = reader.GetOrdinal("HTTP_Content_Type");
						do
						{
							GetFormatRow row = new GetFormatRow();
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.SSRSFormat = (!reader.IsDBNull(ordSSRSFormat) ? reader.GetString(ordSSRSFormat).Trim() : default(string));
							row.SSRSDeviceInfo = (!reader.IsDBNull(ordSSRSDeviceInfo) ? reader.GetString(ordSSRSDeviceInfo).Trim() : default(string));
							row.Extension = (!reader.IsDBNull(ordExtension) ? reader.GetString(ordExtension).Trim() : default(string));
							row.HTTPContentType = (!reader.IsDBNull(ordHTTPContentType) ? reader.GetString(ordHTTPContentType).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListExecutionsByASPUserRow> ListExecutionsByASPUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListExecutionsByASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Executions_ByASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Executions_ByASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListExecutionsByASPUserRow row = new ListExecutionsByASPUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListExecutionsByMntUserRow> ListExecutionsByMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListExecutionsByMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Executions_ByMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Executions_ByMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListExecutionsByMntUserRow row = new ListExecutionsByMntUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListExecutionsByInqUserRow> ListExecutionsByInqUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListExecutionsByInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Executions_ByInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Executions_ByInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							ListExecutionsByInqUserRow row = new ListExecutionsByInqUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListSubscriptionsByASPUserRow> ListSubscriptionsByASPUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListSubscriptionsByASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Subscriptions_ByASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Subscriptions_ByASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordScheduleFrequencyID = reader.GetOrdinal("Schedule_Frequency_ID"); 
						int ordFrequencyInterval = reader.GetOrdinal("Frequency_Interval"); 
						int ordFrequencyRecurrenceFactor = reader.GetOrdinal("Frequency_Recurrence_Factor"); 
						int ordFrequencyRelativeInterval = reader.GetOrdinal("Frequency_Relative_Interval"); 
						int ordStartTime = reader.GetOrdinal("Start_Time"); 
						int ordEndTime = reader.GetOrdinal("End_Time"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordOptions = reader.GetOrdinal("Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordPrevStartDate = reader.GetOrdinal("Prev_Start_Date"); 
						int ordPrevScheduledStartDate = reader.GetOrdinal("Prev_Scheduled_Start_Date"); 
						int ordPrevEndDate = reader.GetOrdinal("Prev_End_Date"); 
						int ordPrevReportExecutionStatusID = reader.GetOrdinal("Prev_Report_Execution_Status_ID"); 
						int ordPrevReportExecutionErrorDescription = reader.GetOrdinal("Prev_Report_Execution_Error_Description"); 
						int ordNextScheduledStartDate = reader.GetOrdinal("Next_Scheduled_Start_Date"); 
						int ordNextReportExecutionStatusID = reader.GetOrdinal("Next_Report_Execution_Status_ID");
						do
						{
							ListSubscriptionsByASPUserRow row = new ListSubscriptionsByASPUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ScheduleFrequencyID = (!reader.IsDBNull(ordScheduleFrequencyID) ? reader.GetInt32(ordScheduleFrequencyID) : default(int));
							row.FrequencyInterval = (!reader.IsDBNull(ordFrequencyInterval) ? reader.GetInt32(ordFrequencyInterval) : default(int));
							row.FrequencyRecurrenceFactor = (!reader.IsDBNull(ordFrequencyRecurrenceFactor) ? reader.GetInt32(ordFrequencyRecurrenceFactor) : default(int?));
							row.FrequencyRelativeInterval = (!reader.IsDBNull(ordFrequencyRelativeInterval) ? reader.GetString(ordFrequencyRelativeInterval).Trim() : default(string));
							row.StartTime = (!reader.IsDBNull(ordStartTime) ? reader.GetString(ordStartTime).Trim() : default(string));
							row.EndTime = (!reader.IsDBNull(ordEndTime) ? reader.GetString(ordEndTime).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.Options = (!reader.IsDBNull(ordOptions) ? reader.GetString(ordOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.PrevStartDate = (!reader.IsDBNull(ordPrevStartDate) ? reader.GetDateTime(ordPrevStartDate) : default(DateTime?));
							row.PrevScheduledStartDate = (!reader.IsDBNull(ordPrevScheduledStartDate) ? reader.GetDateTime(ordPrevScheduledStartDate) : default(DateTime?));
							row.PrevEndDate = (!reader.IsDBNull(ordPrevEndDate) ? reader.GetDateTime(ordPrevEndDate) : default(DateTime?));
							row.PrevReportExecutionStatusID = (!reader.IsDBNull(ordPrevReportExecutionStatusID) ? reader.GetInt32(ordPrevReportExecutionStatusID) : default(int?));
							row.PrevReportExecutionErrorDescription = (!reader.IsDBNull(ordPrevReportExecutionErrorDescription) ? reader.GetString(ordPrevReportExecutionErrorDescription).Trim() : default(string));
							row.NextScheduledStartDate = (!reader.IsDBNull(ordNextScheduledStartDate) ? reader.GetDateTime(ordNextScheduledStartDate) : default(DateTime?));
							row.NextReportExecutionStatusID = (!reader.IsDBNull(ordNextReportExecutionStatusID) ? reader.GetInt32(ordNextReportExecutionStatusID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListSubscriptionsByMntUserRow> ListSubscriptionsByMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListSubscriptionsByMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Subscriptions_ByMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Subscriptions_ByMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordScheduleFrequencyID = reader.GetOrdinal("Schedule_Frequency_ID"); 
						int ordFrequencyInterval = reader.GetOrdinal("Frequency_Interval"); 
						int ordFrequencyRecurrenceFactor = reader.GetOrdinal("Frequency_Recurrence_Factor"); 
						int ordFrequencyRelativeInterval = reader.GetOrdinal("Frequency_Relative_Interval"); 
						int ordStartTime = reader.GetOrdinal("Start_Time"); 
						int ordEndTime = reader.GetOrdinal("End_Time"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordOptions = reader.GetOrdinal("Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordPrevStartDate = reader.GetOrdinal("Prev_Start_Date"); 
						int ordPrevScheduledStartDate = reader.GetOrdinal("Prev_Scheduled_Start_Date"); 
						int ordPrevEndDate = reader.GetOrdinal("Prev_End_Date"); 
						int ordPrevReportExecutionStatusID = reader.GetOrdinal("Prev_Report_Execution_Status_ID"); 
						int ordPrevReportExecutionErrorDescription = reader.GetOrdinal("Prev_Report_Execution_Error_Description"); 
						int ordNextScheduledStartDate = reader.GetOrdinal("Next_Scheduled_Start_Date"); 
						int ordNextReportExecutionStatusID = reader.GetOrdinal("Next_Report_Execution_Status_ID");
						do
						{
							ListSubscriptionsByMntUserRow row = new ListSubscriptionsByMntUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ScheduleFrequencyID = (!reader.IsDBNull(ordScheduleFrequencyID) ? reader.GetInt32(ordScheduleFrequencyID) : default(int));
							row.FrequencyInterval = (!reader.IsDBNull(ordFrequencyInterval) ? reader.GetInt32(ordFrequencyInterval) : default(int));
							row.FrequencyRecurrenceFactor = (!reader.IsDBNull(ordFrequencyRecurrenceFactor) ? reader.GetInt32(ordFrequencyRecurrenceFactor) : default(int?));
							row.FrequencyRelativeInterval = (!reader.IsDBNull(ordFrequencyRelativeInterval) ? reader.GetString(ordFrequencyRelativeInterval).Trim() : default(string));
							row.StartTime = (!reader.IsDBNull(ordStartTime) ? reader.GetString(ordStartTime).Trim() : default(string));
							row.EndTime = (!reader.IsDBNull(ordEndTime) ? reader.GetString(ordEndTime).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.Options = (!reader.IsDBNull(ordOptions) ? reader.GetString(ordOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.PrevStartDate = (!reader.IsDBNull(ordPrevStartDate) ? reader.GetDateTime(ordPrevStartDate) : default(DateTime?));
							row.PrevScheduledStartDate = (!reader.IsDBNull(ordPrevScheduledStartDate) ? reader.GetDateTime(ordPrevScheduledStartDate) : default(DateTime?));
							row.PrevEndDate = (!reader.IsDBNull(ordPrevEndDate) ? reader.GetDateTime(ordPrevEndDate) : default(DateTime?));
							row.PrevReportExecutionStatusID = (!reader.IsDBNull(ordPrevReportExecutionStatusID) ? reader.GetInt32(ordPrevReportExecutionStatusID) : default(int?));
							row.PrevReportExecutionErrorDescription = (!reader.IsDBNull(ordPrevReportExecutionErrorDescription) ? reader.GetString(ordPrevReportExecutionErrorDescription).Trim() : default(string));
							row.NextScheduledStartDate = (!reader.IsDBNull(ordNextScheduledStartDate) ? reader.GetDateTime(ordNextScheduledStartDate) : default(DateTime?));
							row.NextReportExecutionStatusID = (!reader.IsDBNull(ordNextReportExecutionStatusID) ? reader.GetInt32(ordNextReportExecutionStatusID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<ListSubscriptionsByInqUserRow> ListSubscriptionsByInqUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ListSubscriptionsByInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.List_Subscriptions_ByInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.List_Subscriptions_ByInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordScheduleFrequencyID = reader.GetOrdinal("Schedule_Frequency_ID"); 
						int ordFrequencyInterval = reader.GetOrdinal("Frequency_Interval"); 
						int ordFrequencyRecurrenceFactor = reader.GetOrdinal("Frequency_Recurrence_Factor"); 
						int ordFrequencyRelativeInterval = reader.GetOrdinal("Frequency_Relative_Interval"); 
						int ordStartTime = reader.GetOrdinal("Start_Time"); 
						int ordEndTime = reader.GetOrdinal("End_Time"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordOptions = reader.GetOrdinal("Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordPrevStartDate = reader.GetOrdinal("Prev_Start_Date"); 
						int ordPrevScheduledStartDate = reader.GetOrdinal("Prev_Scheduled_Start_Date"); 
						int ordPrevEndDate = reader.GetOrdinal("Prev_End_Date"); 
						int ordPrevReportExecutionStatusID = reader.GetOrdinal("Prev_Report_Execution_Status_ID"); 
						int ordPrevReportExecutionErrorDescription = reader.GetOrdinal("Prev_Report_Execution_Error_Description"); 
						int ordNextScheduledStartDate = reader.GetOrdinal("Next_Scheduled_Start_Date"); 
						int ordNextReportExecutionStatusID = reader.GetOrdinal("Next_Report_Execution_Status_ID");
						do
						{
							ListSubscriptionsByInqUserRow row = new ListSubscriptionsByInqUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ScheduleFrequencyID = (!reader.IsDBNull(ordScheduleFrequencyID) ? reader.GetInt32(ordScheduleFrequencyID) : default(int));
							row.FrequencyInterval = (!reader.IsDBNull(ordFrequencyInterval) ? reader.GetInt32(ordFrequencyInterval) : default(int));
							row.FrequencyRecurrenceFactor = (!reader.IsDBNull(ordFrequencyRecurrenceFactor) ? reader.GetInt32(ordFrequencyRecurrenceFactor) : default(int?));
							row.FrequencyRelativeInterval = (!reader.IsDBNull(ordFrequencyRelativeInterval) ? reader.GetString(ordFrequencyRelativeInterval).Trim() : default(string));
							row.StartTime = (!reader.IsDBNull(ordStartTime) ? reader.GetString(ordStartTime).Trim() : default(string));
							row.EndTime = (!reader.IsDBNull(ordEndTime) ? reader.GetString(ordEndTime).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.Options = (!reader.IsDBNull(ordOptions) ? reader.GetString(ordOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.PrevStartDate = (!reader.IsDBNull(ordPrevStartDate) ? reader.GetDateTime(ordPrevStartDate) : default(DateTime?));
							row.PrevScheduledStartDate = (!reader.IsDBNull(ordPrevScheduledStartDate) ? reader.GetDateTime(ordPrevScheduledStartDate) : default(DateTime?));
							row.PrevEndDate = (!reader.IsDBNull(ordPrevEndDate) ? reader.GetDateTime(ordPrevEndDate) : default(DateTime?));
							row.PrevReportExecutionStatusID = (!reader.IsDBNull(ordPrevReportExecutionStatusID) ? reader.GetInt32(ordPrevReportExecutionStatusID) : default(int?));
							row.PrevReportExecutionErrorDescription = (!reader.IsDBNull(ordPrevReportExecutionErrorDescription) ? reader.GetString(ordPrevReportExecutionErrorDescription).Trim() : default(string));
							row.NextScheduledStartDate = (!reader.IsDBNull(ordNextScheduledStartDate) ? reader.GetDateTime(ordNextScheduledStartDate) : default(DateTime?));
							row.NextReportExecutionStatusID = (!reader.IsDBNull(ordNextReportExecutionStatusID) ? reader.GetInt32(ordNextReportExecutionStatusID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetSubscriptionNextExecutionRow> GetSubscriptionNextExecution(System.Int32? reportSubscriptionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetSubscriptionNextExecution");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Subscription_NextExecution";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Subscription_NextExecution", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID");
						do
						{
							GetSubscriptionNextExecutionRow row = new GetSubscriptionNextExecutionRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int?));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetSubscriptionPreviousExecutionRow> GetSubscriptionPreviousExecution(System.Int32? reportSubscriptionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetSubscriptionPreviousExecution");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Subscription_PreviousExecution";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Subscription_PreviousExecution", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordReportFormatDescription = reader.GetOrdinal("Report_Format_Description"); 
						int ordReportFormatExtension = reader.GetOrdinal("Report_Format_Extension"); 
						int ordReportFormatIsActive = reader.GetOrdinal("Report_Format_IsActive"); 
						int ordReportFormatName = reader.GetOrdinal("Report_Format_Name"); 
						int ordReportFormatSSRSDeviceInfo = reader.GetOrdinal("Report_Format_SSRSDeviceInfo"); 
						int ordReportFormatSSRSFormat = reader.GetOrdinal("Report_Format_SSRS_Format"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionStatusDescription = reader.GetOrdinal("Report_Execution_Status_Description"); 
						int ordReportExecutionStatusName = reader.GetOrdinal("Report_Execution_Status_Name"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordReportExecutionErrorName = reader.GetOrdinal("Report_Execution_Error_Name"); 
						int ordReportExecutionErrorDescription = reader.GetOrdinal("Report_Execution_Error_Description"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID");
						do
						{
							GetSubscriptionPreviousExecutionRow row = new GetSubscriptionPreviousExecutionRow();
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ReportFormatDescription = (!reader.IsDBNull(ordReportFormatDescription) ? reader.GetString(ordReportFormatDescription).Trim() : default(string));
							row.ReportFormatExtension = (!reader.IsDBNull(ordReportFormatExtension) ? reader.GetString(ordReportFormatExtension).Trim() : default(string));
							row.ReportFormatIsActive = (!reader.IsDBNull(ordReportFormatIsActive) ? reader.GetBoolean(ordReportFormatIsActive) : default(bool));
							row.ReportFormatName = (!reader.IsDBNull(ordReportFormatName) ? reader.GetString(ordReportFormatName).Trim() : default(string));
							row.ReportFormatSSRSDeviceInfo = (!reader.IsDBNull(ordReportFormatSSRSDeviceInfo) ? reader.GetString(ordReportFormatSSRSDeviceInfo).Trim() : default(string));
							row.ReportFormatSSRSFormat = (!reader.IsDBNull(ordReportFormatSSRSFormat) ? reader.GetString(ordReportFormatSSRSFormat).Trim() : default(string));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionStatusDescription = (!reader.IsDBNull(ordReportExecutionStatusDescription) ? reader.GetString(ordReportExecutionStatusDescription).Trim() : default(string));
							row.ReportExecutionStatusName = (!reader.IsDBNull(ordReportExecutionStatusName) ? reader.GetString(ordReportExecutionStatusName).Trim() : default(string));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ReportExecutionErrorName = (!reader.IsDBNull(ordReportExecutionErrorName) ? reader.GetString(ordReportExecutionErrorName).Trim() : default(string));
							row.ReportExecutionErrorDescription = (!reader.IsDBNull(ordReportExecutionErrorDescription) ? reader.GetString(ordReportExecutionErrorDescription).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetReportRow> GetReport(System.Int32? reportID, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetReport");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Report";
				
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Report", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordDescription = reader.GetOrdinal("Description"); 
						int ordSortOrder = reader.GetOrdinal("Sort_Order"); 
						int ordReportPath = reader.GetOrdinal("Report_Path"); 
						int ordCategoryID = reader.GetOrdinal("Category_ID"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordCategorySortOrder = reader.GetOrdinal("Category_SortOrder"); 
						int ordReportParametersURL = reader.GetOrdinal("Report_Parameters_URL"); 
						int ordOptions = reader.GetOrdinal("Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							GetReportRow row = new GetReportRow();
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.Description = (!reader.IsDBNull(ordDescription) ? reader.GetString(ordDescription).Trim() : default(string));
							row.SortOrder = (!reader.IsDBNull(ordSortOrder) ? reader.GetInt32(ordSortOrder) : default(int?));
							row.ReportPath = (!reader.IsDBNull(ordReportPath) ? reader.GetString(ordReportPath).Trim() : default(string));
							row.CategoryID = (!reader.IsDBNull(ordCategoryID) ? reader.GetInt32(ordCategoryID) : default(int));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.CategorySortOrder = (!reader.IsDBNull(ordCategorySortOrder) ? reader.GetInt32(ordCategorySortOrder) : default(int?));
							row.ReportParametersURL = (!reader.IsDBNull(ordReportParametersURL) ? reader.GetString(ordReportParametersURL).Trim() : default(string));
							row.Options = (!reader.IsDBNull(ordOptions) ? reader.GetString(ordOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int AddSubscriptionExecution(System.Int32? reportSubscriptionID, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddSubscriptionExecution");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Subscription_Execution";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetExecutionForASPUserRow> GetExecutionForASPUser(System.Int32? reportingUserID, System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionForASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Execution_ForASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Execution_ForASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordRenderOptions = reader.GetOrdinal("Render_Options"); 
						int ordSubscriptionOptions = reader.GetOrdinal("Subscription_Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							GetExecutionForASPUserRow row = new GetExecutionForASPUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.RenderOptions = (!reader.IsDBNull(ordRenderOptions) ? reader.GetString(ordRenderOptions).Trim() : default(string));
							row.SubscriptionOptions = (!reader.IsDBNull(ordSubscriptionOptions) ? reader.GetString(ordSubscriptionOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetExecutionForMntUserRow> GetExecutionForMntUser(System.Int32? reportingUserID, System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionForMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Execution_ForMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Execution_ForMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordRenderOptions = reader.GetOrdinal("Render_Options"); 
						int ordSubscriptionOptions = reader.GetOrdinal("Subscription_Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							GetExecutionForMntUserRow row = new GetExecutionForMntUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.RenderOptions = (!reader.IsDBNull(ordRenderOptions) ? reader.GetString(ordRenderOptions).Trim() : default(string));
							row.SubscriptionOptions = (!reader.IsDBNull(ordSubscriptionOptions) ? reader.GetString(ordSubscriptionOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetExecutionForInqUserRow> GetExecutionForInqUser(System.Int32? reportingUserID, System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionForInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Execution_ForInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Execution_ForInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordExpirationDate = reader.GetOrdinal("Expiration_Date"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorName = reader.GetOrdinal("Error_Name"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordOwnerID = reader.GetOrdinal("Owner_ID"); 
						int ordOwnerNameFirst = reader.GetOrdinal("Owner_Name_First"); 
						int ordOwnerNameLast = reader.GetOrdinal("Owner_Name_Last"); 
						int ordOwnerUsername = reader.GetOrdinal("Owner_Username"); 
						int ordRenderOptions = reader.GetOrdinal("Render_Options"); 
						int ordSubscriptionOptions = reader.GetOrdinal("Subscription_Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name");
						do
						{
							GetExecutionForInqUserRow row = new GetExecutionForInqUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ExpirationDate = (!reader.IsDBNull(ordExpirationDate) ? reader.GetDateTime(ordExpirationDate) : default(DateTime?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorName = (!reader.IsDBNull(ordErrorName) ? reader.GetString(ordErrorName).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.OwnerID = (!reader.IsDBNull(ordOwnerID) ? reader.GetInt32(ordOwnerID) : default(int));
							row.OwnerNameFirst = (!reader.IsDBNull(ordOwnerNameFirst) ? reader.GetString(ordOwnerNameFirst).Trim() : default(string));
							row.OwnerNameLast = (!reader.IsDBNull(ordOwnerNameLast) ? reader.GetString(ordOwnerNameLast).Trim() : default(string));
							row.OwnerUsername = (!reader.IsDBNull(ordOwnerUsername) ? reader.GetString(ordOwnerUsername).Trim() : default(string));
							row.RenderOptions = (!reader.IsDBNull(ordRenderOptions) ? reader.GetString(ordRenderOptions).Trim() : default(string));
							row.SubscriptionOptions = (!reader.IsDBNull(ordSubscriptionOptions) ? reader.GetString(ordSubscriptionOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetSubscriptionRow> GetSubscription(System.Int32? reportSubscriptionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetSubscription");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Subscription";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Subscription", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordIsActive = reader.GetOrdinal("Is_Active"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordScheduleFrequencyID = reader.GetOrdinal("Schedule_Frequency_ID"); 
						int ordFrequencyInterval = reader.GetOrdinal("Frequency_Interval"); 
						int ordFrequencyRecurrenceFactor = reader.GetOrdinal("Frequency_Recurrence_Factor"); 
						int ordFrequencyRelativeInterval = reader.GetOrdinal("Frequency_Relative_Interval"); 
						int ordStartTime = reader.GetOrdinal("Start_Time"); 
						int ordEndTime = reader.GetOrdinal("End_Time"); 
						int ordOptions = reader.GetOrdinal("Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID"); 
						int ordEnvironmentName = reader.GetOrdinal("Environment_Name"); 
						int ordReportName = reader.GetOrdinal("Report_Name"); 
						int ordReportDescription = reader.GetOrdinal("Report_Description"); 
						int ordCategoryName = reader.GetOrdinal("Category_Name"); 
						int ordPrevStartDate = reader.GetOrdinal("Prev_Start_Date"); 
						int ordPrevScheduledStartDate = reader.GetOrdinal("Prev_Scheduled_Start_Date"); 
						int ordPrevEndDate = reader.GetOrdinal("Prev_End_Date"); 
						int ordPrevReportExecutionStatusID = reader.GetOrdinal("Prev_Report_Execution_Status_ID"); 
						int ordPrevReportExecutionErrorDescription = reader.GetOrdinal("Prev_Report_Execution_Error_Description"); 
						int ordNextScheduledStartDate = reader.GetOrdinal("Next_Scheduled_Start_Date"); 
						int ordNextReportExecutionStatusID = reader.GetOrdinal("Next_Report_Execution_Status_ID");
						do
						{
							GetSubscriptionRow row = new GetSubscriptionRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.IsActive = (!reader.IsDBNull(ordIsActive) ? reader.GetBoolean(ordIsActive) : default(bool));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime));
							row.ScheduleFrequencyID = (!reader.IsDBNull(ordScheduleFrequencyID) ? reader.GetInt32(ordScheduleFrequencyID) : default(int));
							row.FrequencyInterval = (!reader.IsDBNull(ordFrequencyInterval) ? reader.GetInt32(ordFrequencyInterval) : default(int));
							row.FrequencyRecurrenceFactor = (!reader.IsDBNull(ordFrequencyRecurrenceFactor) ? reader.GetInt32(ordFrequencyRecurrenceFactor) : default(int?));
							row.FrequencyRelativeInterval = (!reader.IsDBNull(ordFrequencyRelativeInterval) ? reader.GetString(ordFrequencyRelativeInterval).Trim() : default(string));
							row.StartTime = (!reader.IsDBNull(ordStartTime) ? reader.GetString(ordStartTime).Trim() : default(string));
							row.EndTime = (!reader.IsDBNull(ordEndTime) ? reader.GetString(ordEndTime).Trim() : default(string));
							row.Options = (!reader.IsDBNull(ordOptions) ? reader.GetString(ordOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int));
							row.EnvironmentName = (!reader.IsDBNull(ordEnvironmentName) ? reader.GetString(ordEnvironmentName).Trim() : default(string));
							row.ReportName = (!reader.IsDBNull(ordReportName) ? reader.GetString(ordReportName).Trim() : default(string));
							row.ReportDescription = (!reader.IsDBNull(ordReportDescription) ? reader.GetString(ordReportDescription).Trim() : default(string));
							row.CategoryName = (!reader.IsDBNull(ordCategoryName) ? reader.GetString(ordCategoryName).Trim() : default(string));
							row.PrevStartDate = (!reader.IsDBNull(ordPrevStartDate) ? reader.GetDateTime(ordPrevStartDate) : default(DateTime?));
							row.PrevScheduledStartDate = (!reader.IsDBNull(ordPrevScheduledStartDate) ? reader.GetDateTime(ordPrevScheduledStartDate) : default(DateTime?));
							row.PrevEndDate = (!reader.IsDBNull(ordPrevEndDate) ? reader.GetDateTime(ordPrevEndDate) : default(DateTime?));
							row.PrevReportExecutionStatusID = (!reader.IsDBNull(ordPrevReportExecutionStatusID) ? reader.GetInt32(ordPrevReportExecutionStatusID) : default(int?));
							row.PrevReportExecutionErrorDescription = (!reader.IsDBNull(ordPrevReportExecutionErrorDescription) ? reader.GetString(ordPrevReportExecutionErrorDescription).Trim() : default(string));
							row.NextScheduledStartDate = (!reader.IsDBNull(ordNextScheduledStartDate) ? reader.GetDateTime(ordNextScheduledStartDate) : default(DateTime?));
							row.NextReportExecutionStatusID = (!reader.IsDBNull(ordNextReportExecutionStatusID) ? reader.GetInt32(ordNextReportExecutionStatusID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetSubscriptionNotificationsRow> GetSubscriptionNotifications(System.Int32? reportSubscriptionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetSubscriptionNotifications");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Subscription_Notifications";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Subscription_Notifications", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportNotificationTypeID = reader.GetOrdinal("Report_Notification_Type_ID"); 
						int ordReportNotificationOptions = reader.GetOrdinal("Report_Notification_Options");
						do
						{
							GetSubscriptionNotificationsRow row = new GetSubscriptionNotificationsRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int));
							row.ReportNotificationTypeID = (!reader.IsDBNull(ordReportNotificationTypeID) ? reader.GetInt32(ordReportNotificationTypeID) : default(int));
							row.ReportNotificationOptions = (!reader.IsDBNull(ordReportNotificationOptions) ? reader.GetString(ordReportNotificationOptions).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetExecutionDataRow> GetExecutionData(System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionData");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_Reports.Reporting.Get_Execution_Data";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_Reports.Reporting.Get_Execution_Data", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordFileType = reader.GetOrdinal("File_Type"); 
						int ordData = reader.GetOrdinal("Data");
						do
						{
							GetExecutionDataRow row = new GetExecutionDataRow();
							row.FileType = (!reader.IsDBNull(ordFileType) ? reader.GetString(ordFileType).Trim() : default(string));
							row.Data = (!reader.IsDBNull(ordData) ? reader.GetSqlBinary(ordData).Value : null);
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddSubscriptionForASPUserRow> AddSubscriptionForASPUser(System.Int32? reportingUserID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddSubscriptionForASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Subscription_ForASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter firstExecutionDateParameter = new SqlParameter("@First_Execution_Date", (object)firstExecutionDate ?? DBNull.Value); 
					firstExecutionDateParameter.Size = 8;
					firstExecutionDateParameter.Direction = ParameterDirection.Input;
					firstExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(firstExecutionDateParameter);
					
					SqlParameter firstExecutionParametersParameter = new SqlParameter("@First_Execution_Parameters", (object)firstExecutionParameters ?? DBNull.Value); 
					firstExecutionParametersParameter.Size = 4000;
					firstExecutionParametersParameter.Direction = ParameterDirection.Input;
					firstExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(firstExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Subscription_ForASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID");
						do
						{
							AddSubscriptionForASPUserRow row = new AddSubscriptionForASPUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddSubscriptionForInqUserRow> AddSubscriptionForInqUser(System.Int32? reportingUserID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddSubscriptionForInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Subscription_ForInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter firstExecutionDateParameter = new SqlParameter("@First_Execution_Date", (object)firstExecutionDate ?? DBNull.Value); 
					firstExecutionDateParameter.Size = 8;
					firstExecutionDateParameter.Direction = ParameterDirection.Input;
					firstExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(firstExecutionDateParameter);
					
					SqlParameter firstExecutionParametersParameter = new SqlParameter("@First_Execution_Parameters", (object)firstExecutionParameters ?? DBNull.Value); 
					firstExecutionParametersParameter.Size = 4000;
					firstExecutionParametersParameter.Direction = ParameterDirection.Input;
					firstExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(firstExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Subscription_ForInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID");
						do
						{
							AddSubscriptionForInqUserRow row = new AddSubscriptionForInqUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddSubscriptionForMntUserRow> AddSubscriptionForMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddSubscriptionForMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Subscription_ForMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter firstExecutionDateParameter = new SqlParameter("@First_Execution_Date", (object)firstExecutionDate ?? DBNull.Value); 
					firstExecutionDateParameter.Size = 8;
					firstExecutionDateParameter.Direction = ParameterDirection.Input;
					firstExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(firstExecutionDateParameter);
					
					SqlParameter firstExecutionParametersParameter = new SqlParameter("@First_Execution_Parameters", (object)firstExecutionParameters ?? DBNull.Value); 
					firstExecutionParametersParameter.Size = 4000;
					firstExecutionParametersParameter.Direction = ParameterDirection.Input;
					firstExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(firstExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Subscription_ForMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID");
						do
						{
							AddSubscriptionForMntUserRow row = new AddSubscriptionForMntUserRow();
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddExecutionForASPUserRow> AddExecutionForASPUser(System.Int32? reportingUserID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddExecutionForASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Execution_ForASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter executionDateParameter = new SqlParameter("@Execution_Date", (object)executionDate ?? DBNull.Value); 
					executionDateParameter.Size = 8;
					executionDateParameter.Direction = ParameterDirection.Input;
					executionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(executionDateParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter usedHistoryParameter = new SqlParameter("@Used_History", (object)usedHistory ?? DBNull.Value); 
					usedHistoryParameter.Size = 1;
					usedHistoryParameter.Direction = ParameterDirection.Input;
					usedHistoryParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(usedHistoryParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Execution_ForASPUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID");
						do
						{
							AddExecutionForASPUserRow row = new AddExecutionForASPUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddExecutionForInqUserRow> AddExecutionForInqUser(System.Int32? reportingUserID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddExecutionForInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Execution_ForInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter executionDateParameter = new SqlParameter("@Execution_Date", (object)executionDate ?? DBNull.Value); 
					executionDateParameter.Size = 8;
					executionDateParameter.Direction = ParameterDirection.Input;
					executionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(executionDateParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter usedHistoryParameter = new SqlParameter("@Used_History", (object)usedHistory ?? DBNull.Value); 
					usedHistoryParameter.Size = 1;
					usedHistoryParameter.Direction = ParameterDirection.Input;
					usedHistoryParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(usedHistoryParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Execution_ForInqUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID");
						do
						{
							AddExecutionForInqUserRow row = new AddExecutionForInqUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<AddExecutionForMntUserRow> AddExecutionForMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "AddExecutionForMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Add_Execution_ForMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportingCompanyIDParameter = new SqlParameter("@Reporting_Company_ID", (object)reportingCompanyID ?? DBNull.Value); 
					reportingCompanyIDParameter.Size = 4;
					reportingCompanyIDParameter.Direction = ParameterDirection.Input;
					reportingCompanyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingCompanyIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter executionDateParameter = new SqlParameter("@Execution_Date", (object)executionDate ?? DBNull.Value); 
					executionDateParameter.Size = 8;
					executionDateParameter.Direction = ParameterDirection.Input;
					executionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(executionDateParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter usedHistoryParameter = new SqlParameter("@Used_History", (object)usedHistory ?? DBNull.Value); 
					usedHistoryParameter.Size = 1;
					usedHistoryParameter.Direction = ParameterDirection.Input;
					usedHistoryParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(usedHistoryParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Add_Execution_ForMntUser", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID");
						do
						{
							AddExecutionForMntUserRow row = new AddExecutionForMntUserRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int EditSubscriptionForASPUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "EditSubscriptionForASPUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Edit_Subscription_ForASPUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter nextExecutionDateParameter = new SqlParameter("@Next_Execution_Date", (object)nextExecutionDate ?? DBNull.Value); 
					nextExecutionDateParameter.Size = 8;
					nextExecutionDateParameter.Direction = ParameterDirection.Input;
					nextExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(nextExecutionDateParameter);
					
					SqlParameter nextExecutionParametersParameter = new SqlParameter("@Next_Execution_Parameters", (object)nextExecutionParameters ?? DBNull.Value); 
					nextExecutionParametersParameter.Size = 4000;
					nextExecutionParametersParameter.Direction = ParameterDirection.Input;
					nextExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nextExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int EditSubscriptionForInqUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "EditSubscriptionForInqUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Edit_Subscription_ForInqUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter nextExecutionDateParameter = new SqlParameter("@Next_Execution_Date", (object)nextExecutionDate ?? DBNull.Value); 
					nextExecutionDateParameter.Size = 8;
					nextExecutionDateParameter.Direction = ParameterDirection.Input;
					nextExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(nextExecutionDateParameter);
					
					SqlParameter nextExecutionParametersParameter = new SqlParameter("@Next_Execution_Parameters", (object)nextExecutionParameters ?? DBNull.Value); 
					nextExecutionParametersParameter.Size = 4000;
					nextExecutionParametersParameter.Direction = ParameterDirection.Input;
					nextExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nextExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int EditSubscriptionForMntUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "EditSubscriptionForMntUser");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Edit_Subscription_ForMntUser";
				
					
					SqlParameter reportingUserIDParameter = new SqlParameter("@Reporting_User_ID", (object)reportingUserID ?? DBNull.Value); 
					reportingUserIDParameter.Size = 4;
					reportingUserIDParameter.Direction = ParameterDirection.Input;
					reportingUserIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportingUserIDParameter);
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);
					
					SqlParameter reportIDParameter = new SqlParameter("@Report_ID", (object)reportID ?? DBNull.Value); 
					reportIDParameter.Size = 4;
					reportIDParameter.Direction = ParameterDirection.Input;
					reportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter reportFormatIDParameter = new SqlParameter("@Report_Format_ID", (object)reportFormatID ?? DBNull.Value); 
					reportFormatIDParameter.Size = 4;
					reportFormatIDParameter.Direction = ParameterDirection.Input;
					reportFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportFormatIDParameter);
					
					SqlParameter isActiveParameter = new SqlParameter("@Is_Active", (object)isActive ?? DBNull.Value); 
					isActiveParameter.Size = 1;
					isActiveParameter.Direction = ParameterDirection.Input;
					isActiveParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(isActiveParameter);
					
					SqlParameter parametersParameter = new SqlParameter("@Parameters", (object)parameters ?? DBNull.Value); 
					parametersParameter.Size = 4000;
					parametersParameter.Direction = ParameterDirection.Input;
					parametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(parametersParameter);
					
					SqlParameter scheduleFrequencyIDParameter = new SqlParameter("@Schedule_Frequency_ID", (object)scheduleFrequencyID ?? DBNull.Value); 
					scheduleFrequencyIDParameter.Size = 4;
					scheduleFrequencyIDParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIDParameter);
					
					SqlParameter scheduleFrequencyIntervalParameter = new SqlParameter("@Schedule_Frequency_Interval", (object)scheduleFrequencyInterval ?? DBNull.Value); 
					scheduleFrequencyIntervalParameter.Size = 4;
					scheduleFrequencyIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyIntervalParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyIntervalParameter);
					
					SqlParameter scheduleFrequencyRecurrenceFactorParameter = new SqlParameter("@Schedule_Frequency_Recurrence_Factor", (object)scheduleFrequencyRecurrenceFactor ?? DBNull.Value); 
					scheduleFrequencyRecurrenceFactorParameter.Size = 4;
					scheduleFrequencyRecurrenceFactorParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRecurrenceFactorParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(scheduleFrequencyRecurrenceFactorParameter);
					
					SqlParameter scheduleFrequencyRelativeIntervalParameter = new SqlParameter("@Schedule_Frequency_Relative_Interval", (object)scheduleFrequencyRelativeInterval ?? DBNull.Value); 
					scheduleFrequencyRelativeIntervalParameter.Size = 100;
					scheduleFrequencyRelativeIntervalParameter.Direction = ParameterDirection.Input;
					scheduleFrequencyRelativeIntervalParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleFrequencyRelativeIntervalParameter);
					
					SqlParameter scheduleStartTimeParameter = new SqlParameter("@Schedule_Start_Time", (object)scheduleStartTime ?? DBNull.Value); 
					scheduleStartTimeParameter.Size = 6;
					scheduleStartTimeParameter.Direction = ParameterDirection.Input;
					scheduleStartTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleStartTimeParameter);
					
					SqlParameter scheduleEndTimeParameter = new SqlParameter("@Schedule_End_Time", (object)scheduleEndTime ?? DBNull.Value); 
					scheduleEndTimeParameter.Size = 6;
					scheduleEndTimeParameter.Direction = ParameterDirection.Input;
					scheduleEndTimeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(scheduleEndTimeParameter);
					
					SqlParameter nextExecutionDateParameter = new SqlParameter("@Next_Execution_Date", (object)nextExecutionDate ?? DBNull.Value); 
					nextExecutionDateParameter.Size = 8;
					nextExecutionDateParameter.Direction = ParameterDirection.Input;
					nextExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(nextExecutionDateParameter);
					
					SqlParameter nextExecutionParametersParameter = new SqlParameter("@Next_Execution_Parameters", (object)nextExecutionParameters ?? DBNull.Value); 
					nextExecutionParametersParameter.Size = 4000;
					nextExecutionParametersParameter.Direction = ParameterDirection.Input;
					nextExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nextExecutionParametersParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);
					
					SqlParameter useOnScreenNotificationParameter = new SqlParameter("@Use_On_Screen_Notification", (object)useOnScreenNotification ?? DBNull.Value); 
					useOnScreenNotificationParameter.Size = 1;
					useOnScreenNotificationParameter.Direction = ParameterDirection.Input;
					useOnScreenNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useOnScreenNotificationParameter);
					
					SqlParameter useEmailNotificationParameter = new SqlParameter("@Use_Email_Notification", (object)useEmailNotification ?? DBNull.Value); 
					useEmailNotificationParameter.Size = 1;
					useEmailNotificationParameter.Direction = ParameterDirection.Input;
					useEmailNotificationParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(useEmailNotificationParameter);
					
					SqlParameter emailNotificationAddressParameter = new SqlParameter("@Email_Notification_Address", (object)emailNotificationAddress ?? DBNull.Value); 
					emailNotificationAddressParameter.Size = 100;
					emailNotificationAddressParameter.Direction = ParameterDirection.Input;
					emailNotificationAddressParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(emailNotificationAddressParameter);
					
					SqlParameter optionsParameter = new SqlParameter("@Options", (object)options ?? DBNull.Value); 
					optionsParameter.Size = -1;
					optionsParameter.Direction = ParameterDirection.Input;
					optionsParameter.SqlDbType = SqlDbType.Xml;
					cmd.Parameters.Add(optionsParameter);
					
					SqlParameter environmentIDParameter = new SqlParameter("@Environment_ID", (object)environmentID ?? DBNull.Value); 
					environmentIDParameter.Size = 4;
					environmentIDParameter.Direction = ParameterDirection.Input;
					environmentIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(environmentIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int SetExecutionData(System.Int32? reportExecutionID, System.String fileType, System.Byte[] data)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "SetExecutionData");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_Reports.Reporting.Set_Execution_Data";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);
					
					SqlParameter fileTypeParameter = new SqlParameter("@File_Type", (object)fileType ?? DBNull.Value); 
					fileTypeParameter.Size = 10;
					fileTypeParameter.Direction = ParameterDirection.Input;
					fileTypeParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(fileTypeParameter);
					
					SqlParameter dataParameter = new SqlParameter("@Data", (object)data ?? DBNull.Value); 
					dataParameter.Size = -1;
					dataParameter.Direction = ParameterDirection.Input;
					dataParameter.SqlDbType = SqlDbType.VarBinary;
					cmd.Parameters.Add(dataParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int DeleteSubscription(System.Int32? reportSubscriptionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "DeleteSubscription");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Delete_Subscription";
				
					
					SqlParameter reportSubscriptionIDParameter = new SqlParameter("@Report_Subscription_ID", (object)reportSubscriptionID ?? DBNull.Value); 
					reportSubscriptionIDParameter.Size = 4;
					reportSubscriptionIDParameter.Direction = ParameterDirection.Input;
					reportSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportSubscriptionIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int DeleteExecution(System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "DeleteExecution");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Delete_Execution";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int DeleteExecutionData(System.Int32? reportExecutionID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "DeleteExecutionData");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_Reports.Reporting.Delete_Execution_Data";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetExecutionFromQueueRow> GetExecutionFromQueue(System.Int32? maxFailures)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetExecutionFromQueue");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Execution_FromQueue";
				
					
					SqlParameter maxFailuresParameter = new SqlParameter("@Max_Failures", (object)maxFailures ?? DBNull.Value); 
					maxFailuresParameter.Size = 4;
					maxFailuresParameter.Direction = ParameterDirection.Input;
					maxFailuresParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(maxFailuresParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Execution_FromQueue", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordReportExecutionID = reader.GetOrdinal("Report_Execution_ID"); 
						int ordName = reader.GetOrdinal("Name"); 
						int ordScheduledStartDate = reader.GetOrdinal("Scheduled_Start_Date"); 
						int ordStartDate = reader.GetOrdinal("Start_Date"); 
						int ordEndDate = reader.GetOrdinal("End_Date"); 
						int ordReportID = reader.GetOrdinal("Report_ID"); 
						int ordReportSubscriptionID = reader.GetOrdinal("Report_Subscription_ID"); 
						int ordReportFormatID = reader.GetOrdinal("Report_Format_ID"); 
						int ordReportExecutionStatusID = reader.GetOrdinal("Report_Execution_Status_ID"); 
						int ordParameters = reader.GetOrdinal("Parameters"); 
						int ordErrorDescription = reader.GetOrdinal("Error_Description"); 
						int ordReportExecutionErrorID = reader.GetOrdinal("Report_Execution_Error_ID"); 
						int ordErrorCount = reader.GetOrdinal("Error_Count"); 
						int ordUsedHistory = reader.GetOrdinal("Used_History"); 
						int ordModifiedUser = reader.GetOrdinal("Modified_User"); 
						int ordModifiedDate = reader.GetOrdinal("Modified_Date"); 
						int ordSubscriptionOptions = reader.GetOrdinal("Subscription_Options"); 
						int ordEnvironmentID = reader.GetOrdinal("Environment_ID");
						do
						{
							GetExecutionFromQueueRow row = new GetExecutionFromQueueRow();
							row.ReportExecutionID = (!reader.IsDBNull(ordReportExecutionID) ? reader.GetInt32(ordReportExecutionID) : default(int?));
							row.Name = (!reader.IsDBNull(ordName) ? reader.GetString(ordName).Trim() : default(string));
							row.ScheduledStartDate = (!reader.IsDBNull(ordScheduledStartDate) ? reader.GetDateTime(ordScheduledStartDate) : default(DateTime?));
							row.StartDate = (!reader.IsDBNull(ordStartDate) ? reader.GetDateTime(ordStartDate) : default(DateTime?));
							row.EndDate = (!reader.IsDBNull(ordEndDate) ? reader.GetDateTime(ordEndDate) : default(DateTime?));
							row.ReportID = (!reader.IsDBNull(ordReportID) ? reader.GetInt32(ordReportID) : default(int?));
							row.ReportSubscriptionID = (!reader.IsDBNull(ordReportSubscriptionID) ? reader.GetInt32(ordReportSubscriptionID) : default(int?));
							row.ReportFormatID = (!reader.IsDBNull(ordReportFormatID) ? reader.GetInt32(ordReportFormatID) : default(int?));
							row.ReportExecutionStatusID = (!reader.IsDBNull(ordReportExecutionStatusID) ? reader.GetInt32(ordReportExecutionStatusID) : default(int));
							row.Parameters = (!reader.IsDBNull(ordParameters) ? reader.GetString(ordParameters).Trim() : default(string));
							row.ErrorDescription = (!reader.IsDBNull(ordErrorDescription) ? reader.GetString(ordErrorDescription).Trim() : default(string));
							row.ReportExecutionErrorID = (!reader.IsDBNull(ordReportExecutionErrorID) ? reader.GetInt32(ordReportExecutionErrorID) : default(int?));
							row.ErrorCount = (!reader.IsDBNull(ordErrorCount) ? reader.GetInt32(ordErrorCount) : default(int?));
							row.UsedHistory = (!reader.IsDBNull(ordUsedHistory) ? reader.GetBoolean(ordUsedHistory) : default(bool?));
							row.ModifiedUser = (!reader.IsDBNull(ordModifiedUser) ? reader.GetString(ordModifiedUser).Trim() : default(string));
							row.ModifiedDate = (!reader.IsDBNull(ordModifiedDate) ? reader.GetDateTime(ordModifiedDate) : default(DateTime?));
							row.SubscriptionOptions = (!reader.IsDBNull(ordSubscriptionOptions) ? reader.GetString(ordSubscriptionOptions).Trim() : default(string));
							row.EnvironmentID = (!reader.IsDBNull(ordEnvironmentID) ? reader.GetInt32(ordEnvironmentID) : default(int?));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int SetExecutionRetry(System.Int32? reportExecutionID, System.String name, System.DateTime? startDate, System.DateTime? endDate, System.String errorDescription, System.Int32? reportExecutionErrorID, System.Int32? errorCount, System.DateTime? retryDate, System.String modifiedUser, System.DateTime? modifiedDate)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "SetExecutionRetry");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Set_Execution_Retry";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter startDateParameter = new SqlParameter("@Start_Date", (object)startDate ?? DBNull.Value); 
					startDateParameter.Size = 8;
					startDateParameter.Direction = ParameterDirection.Input;
					startDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(startDateParameter);
					
					SqlParameter endDateParameter = new SqlParameter("@End_Date", (object)endDate ?? DBNull.Value); 
					endDateParameter.Size = 8;
					endDateParameter.Direction = ParameterDirection.Input;
					endDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(endDateParameter);
					
					SqlParameter errorDescriptionParameter = new SqlParameter("@Error_Description", (object)errorDescription ?? DBNull.Value); 
					errorDescriptionParameter.Size = 1000;
					errorDescriptionParameter.Direction = ParameterDirection.Input;
					errorDescriptionParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(errorDescriptionParameter);
					
					SqlParameter reportExecutionErrorIDParameter = new SqlParameter("@Report_Execution_Error_ID", (object)reportExecutionErrorID ?? DBNull.Value); 
					reportExecutionErrorIDParameter.Size = 4;
					reportExecutionErrorIDParameter.Direction = ParameterDirection.Input;
					reportExecutionErrorIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionErrorIDParameter);
					
					SqlParameter errorCountParameter = new SqlParameter("@Error_Count", (object)errorCount ?? DBNull.Value); 
					errorCountParameter.Size = 4;
					errorCountParameter.Direction = ParameterDirection.Input;
					errorCountParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(errorCountParameter);
					
					SqlParameter retryDateParameter = new SqlParameter("@Retry_Date", (object)retryDate ?? DBNull.Value); 
					retryDateParameter.Size = 8;
					retryDateParameter.Direction = ParameterDirection.Input;
					retryDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(retryDateParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int ResetExecutionQueue()
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "ResetExecutionQueue");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Reset_Execution_Queue";
				

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public int SetExecutionStatus(System.Int32? reportExecutionID, System.String name, System.DateTime? startDate, System.DateTime? endDate, System.Int32? reportExecutionStatusID, System.String errorDescription, System.Int32? errorCount, System.Int32? reportExecutionErrorID, System.Boolean? usedHistory, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.Int32? nextExecutionReportID, System.Int32? nextExecutionSubscriptionID, System.Int32? nextExecutionFormatID, System.String modifiedUser, System.DateTime? modifiedDate)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "SetExecutionStatus");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Set_Execution_Status";
				
					
					SqlParameter reportExecutionIDParameter = new SqlParameter("@Report_Execution_ID", (object)reportExecutionID ?? DBNull.Value); 
					reportExecutionIDParameter.Size = 4;
					reportExecutionIDParameter.Direction = ParameterDirection.Input;
					reportExecutionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionIDParameter);
					
					SqlParameter nameParameter = new SqlParameter("@Name", (object)name ?? DBNull.Value); 
					nameParameter.Size = 200;
					nameParameter.Direction = ParameterDirection.Input;
					nameParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nameParameter);
					
					SqlParameter startDateParameter = new SqlParameter("@Start_Date", (object)startDate ?? DBNull.Value); 
					startDateParameter.Size = 8;
					startDateParameter.Direction = ParameterDirection.Input;
					startDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(startDateParameter);
					
					SqlParameter endDateParameter = new SqlParameter("@End_Date", (object)endDate ?? DBNull.Value); 
					endDateParameter.Size = 8;
					endDateParameter.Direction = ParameterDirection.Input;
					endDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(endDateParameter);
					
					SqlParameter reportExecutionStatusIDParameter = new SqlParameter("@Report_Execution_Status_ID", (object)reportExecutionStatusID ?? DBNull.Value); 
					reportExecutionStatusIDParameter.Size = 4;
					reportExecutionStatusIDParameter.Direction = ParameterDirection.Input;
					reportExecutionStatusIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionStatusIDParameter);
					
					SqlParameter errorDescriptionParameter = new SqlParameter("@Error_Description", (object)errorDescription ?? DBNull.Value); 
					errorDescriptionParameter.Size = 1000;
					errorDescriptionParameter.Direction = ParameterDirection.Input;
					errorDescriptionParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(errorDescriptionParameter);
					
					SqlParameter errorCountParameter = new SqlParameter("@Error_Count", (object)errorCount ?? DBNull.Value); 
					errorCountParameter.Size = 4;
					errorCountParameter.Direction = ParameterDirection.Input;
					errorCountParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(errorCountParameter);
					
					SqlParameter reportExecutionErrorIDParameter = new SqlParameter("@Report_Execution_Error_ID", (object)reportExecutionErrorID ?? DBNull.Value); 
					reportExecutionErrorIDParameter.Size = 4;
					reportExecutionErrorIDParameter.Direction = ParameterDirection.Input;
					reportExecutionErrorIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(reportExecutionErrorIDParameter);
					
					SqlParameter usedHistoryParameter = new SqlParameter("@Used_History", (object)usedHistory ?? DBNull.Value); 
					usedHistoryParameter.Size = 1;
					usedHistoryParameter.Direction = ParameterDirection.Input;
					usedHistoryParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(usedHistoryParameter);
					
					SqlParameter nextExecutionDateParameter = new SqlParameter("@Next_Execution_Date", (object)nextExecutionDate ?? DBNull.Value); 
					nextExecutionDateParameter.Size = 8;
					nextExecutionDateParameter.Direction = ParameterDirection.Input;
					nextExecutionDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(nextExecutionDateParameter);
					
					SqlParameter nextExecutionParametersParameter = new SqlParameter("@Next_Execution_Parameters", (object)nextExecutionParameters ?? DBNull.Value); 
					nextExecutionParametersParameter.Size = 4000;
					nextExecutionParametersParameter.Direction = ParameterDirection.Input;
					nextExecutionParametersParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(nextExecutionParametersParameter);
					
					SqlParameter nextExecutionReportIDParameter = new SqlParameter("@Next_Execution_Report_ID", (object)nextExecutionReportID ?? DBNull.Value); 
					nextExecutionReportIDParameter.Size = 4;
					nextExecutionReportIDParameter.Direction = ParameterDirection.Input;
					nextExecutionReportIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(nextExecutionReportIDParameter);
					
					SqlParameter nextExecutionSubscriptionIDParameter = new SqlParameter("@Next_Execution_Subscription_ID", (object)nextExecutionSubscriptionID ?? DBNull.Value); 
					nextExecutionSubscriptionIDParameter.Size = 4;
					nextExecutionSubscriptionIDParameter.Direction = ParameterDirection.Input;
					nextExecutionSubscriptionIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(nextExecutionSubscriptionIDParameter);
					
					SqlParameter nextExecutionFormatIDParameter = new SqlParameter("@Next_Execution_Format_ID", (object)nextExecutionFormatID ?? DBNull.Value); 
					nextExecutionFormatIDParameter.Size = 4;
					nextExecutionFormatIDParameter.Direction = ParameterDirection.Input;
					nextExecutionFormatIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(nextExecutionFormatIDParameter);
					
					SqlParameter modifiedUserParameter = new SqlParameter("@Modified_User", (object)modifiedUser ?? DBNull.Value); 
					modifiedUserParameter.Size = 26;
					modifiedUserParameter.Direction = ParameterDirection.Input;
					modifiedUserParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(modifiedUserParameter);
					
					SqlParameter modifiedDateParameter = new SqlParameter("@Modified_Date", (object)modifiedDate ?? DBNull.Value); 
					modifiedDateParameter.Size = 8;
					modifiedDateParameter.Direction = ParameterDirection.Input;
					modifiedDateParameter.SqlDbType = SqlDbType.DateTime;
					cmd.Parameters.Add(modifiedDateParameter);

					var executeNonQueryResult = cmd.ExecuteNonQuery();
					return executeNonQueryResult;
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<GetClientLabelsRow> GetClientLabels(System.Int32? clientID)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "GetClientLabels");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.Get_Client_Labels";
				
					
					SqlParameter clientIDParameter = new SqlParameter("@Client_ID", (object)clientID ?? DBNull.Value); 
					clientIDParameter.Size = 4;
					clientIDParameter.Direction = ParameterDirection.Input;
					clientIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(clientIDParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.Get_Client_Labels", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordLabelPatientID = reader.GetOrdinal("Label_Patient_ID"); 
						int ordLabelPatientName = reader.GetOrdinal("Label_Patient_Name"); 
						int ordLabelGender = reader.GetOrdinal("Label_Gender"); 
						int ordLabelBirthdate = reader.GetOrdinal("Label_Birthdate"); 
						int ordLabelAppointmentDate = reader.GetOrdinal("Label_Appointment_Date"); 
						int ordLabelOrderNumber = reader.GetOrdinal("Label_Order_Number"); 
						int ordLabelPatientLetter = reader.GetOrdinal("Label_Patient_Letter"); 
						int ordLabelUserField1 = reader.GetOrdinal("Label_User_Field_1"); 
						int ordLabelUserField2 = reader.GetOrdinal("Label_User_Field_2"); 
						int ordLabelUserField3 = reader.GetOrdinal("Label_User_Field_3"); 
						int ordLabelUserField4 = reader.GetOrdinal("Label_User_Field_4"); 
						int ordLabelUserField5 = reader.GetOrdinal("Label_User_Field_5"); 
						int ordLabelBillingUnit = reader.GetOrdinal("Label_Billing_Unit"); 
						int ordLabelDictator = reader.GetOrdinal("Label_Dictator");
						do
						{
							GetClientLabelsRow row = new GetClientLabelsRow();
							row.LabelPatientID = (!reader.IsDBNull(ordLabelPatientID) ? reader.GetString(ordLabelPatientID).Trim() : default(string));
							row.LabelPatientName = (!reader.IsDBNull(ordLabelPatientName) ? reader.GetString(ordLabelPatientName).Trim() : default(string));
							row.LabelGender = (!reader.IsDBNull(ordLabelGender) ? reader.GetString(ordLabelGender).Trim() : default(string));
							row.LabelBirthdate = (!reader.IsDBNull(ordLabelBirthdate) ? reader.GetString(ordLabelBirthdate).Trim() : default(string));
							row.LabelAppointmentDate = (!reader.IsDBNull(ordLabelAppointmentDate) ? reader.GetString(ordLabelAppointmentDate).Trim() : default(string));
							row.LabelOrderNumber = (!reader.IsDBNull(ordLabelOrderNumber) ? reader.GetString(ordLabelOrderNumber).Trim() : default(string));
							row.LabelPatientLetter = (!reader.IsDBNull(ordLabelPatientLetter) ? reader.GetString(ordLabelPatientLetter).Trim() : default(string));
							row.LabelUserField1 = (!reader.IsDBNull(ordLabelUserField1) ? reader.GetString(ordLabelUserField1).Trim() : default(string));
							row.LabelUserField2 = (!reader.IsDBNull(ordLabelUserField2) ? reader.GetString(ordLabelUserField2).Trim() : default(string));
							row.LabelUserField3 = (!reader.IsDBNull(ordLabelUserField3) ? reader.GetString(ordLabelUserField3).Trim() : default(string));
							row.LabelUserField4 = (!reader.IsDBNull(ordLabelUserField4) ? reader.GetString(ordLabelUserField4).Trim() : default(string));
							row.LabelUserField5 = (!reader.IsDBNull(ordLabelUserField5) ? reader.GetString(ordLabelUserField5).Trim() : default(string));
							row.LabelBillingUnit = (!reader.IsDBNull(ordLabelBillingUnit) ? reader.GetString(ordLabelBillingUnit).Trim() : default(string));
							row.LabelDictator = (!reader.IsDBNull(ordLabelDictator) ? reader.GetString(ordLabelDictator).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
		public IEnumerable<SSRSInqGetUsersRow> SSRSInqGetUsers(System.Int32? clientID, System.Int32? userID, System.Boolean? includeselectalloption, System.String selectalllabel, System.Boolean? filterbyproxy, System.Boolean? dictatingOnly)
		{
			this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Start, 0, "SSRSInqGetUsers");
			try
			{
				using (SqlConnection conn = new SqlConnection(this.ConnectionString))
				using (SqlCommand cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "DATA_001.Reporting.SSRS_Inq_Get_Users";
				
					
					SqlParameter clientIDParameter = new SqlParameter("@Client_ID", (object)clientID ?? DBNull.Value); 
					clientIDParameter.Size = 4;
					clientIDParameter.Direction = ParameterDirection.Input;
					clientIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(clientIDParameter);
					
					SqlParameter userIDParameter = new SqlParameter("@User_ID", (object)userID ?? DBNull.Value); 
					userIDParameter.Size = 4;
					userIDParameter.Direction = ParameterDirection.Input;
					userIDParameter.SqlDbType = SqlDbType.Int;
					cmd.Parameters.Add(userIDParameter);
					
					SqlParameter includeselectalloptionParameter = new SqlParameter("@IncludeSelectAllOption", (object)includeselectalloption ?? DBNull.Value); 
					includeselectalloptionParameter.Size = 1;
					includeselectalloptionParameter.Direction = ParameterDirection.Input;
					includeselectalloptionParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(includeselectalloptionParameter);
					
					SqlParameter selectalllabelParameter = new SqlParameter("@SelectAllLabel", (object)selectalllabel ?? DBNull.Value); 
					selectalllabelParameter.Size = 50;
					selectalllabelParameter.Direction = ParameterDirection.Input;
					selectalllabelParameter.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(selectalllabelParameter);
					
					SqlParameter filterbyproxyParameter = new SqlParameter("@FilterByProxy", (object)filterbyproxy ?? DBNull.Value); 
					filterbyproxyParameter.Size = 1;
					filterbyproxyParameter.Direction = ParameterDirection.Input;
					filterbyproxyParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(filterbyproxyParameter);
					
					SqlParameter dictatingOnlyParameter = new SqlParameter("@Dictating_Only", (object)dictatingOnly ?? DBNull.Value); 
					dictatingOnlyParameter.Size = 1;
					dictatingOnlyParameter.Direction = ParameterDirection.Input;
					dictatingOnlyParameter.SqlDbType = SqlDbType.Bit;
					cmd.Parameters.Add(dictatingOnlyParameter);

					SqlDataReader reader;
					try
					{
						reader = cmd.ExecuteReader();
					}
					catch (SqlException e)
					{
						if (String.IsNullOrEmpty(e.Procedure))
						{
							throw new System.Data.DataException(String.Format("Unable to execute data reader on {0}: {1}", "DATA_001.Reporting.SSRS_Inq_Get_Users", e.Message), e);
						}
						else
						{
							throw;
						}
					}

					using (reader)
					{
						if (!reader.Read())
						{							
							yield break;
						}
 
						int ordUserID = reader.GetOrdinal("User_ID"); 
						int ordDisplayName = reader.GetOrdinal("Display_Name"); 
						int ordUserNameLast = reader.GetOrdinal("User_Name_Last"); 
						int ordUserNameFirst = reader.GetOrdinal("User_Name_First"); 
						int ordUserUsername = reader.GetOrdinal("User_Username"); 
						int ordIsDictating = reader.GetOrdinal("Is_Dictating"); 
						int ordDictationID = reader.GetOrdinal("Dictation_ID"); 
						int ordUserClientCode = reader.GetOrdinal("User_Client_Code");
						do
						{
							SSRSInqGetUsersRow row = new SSRSInqGetUsersRow();
							row.UserID = (!reader.IsDBNull(ordUserID) ? reader.GetInt32(ordUserID) : default(int?));
							row.DisplayName = (!reader.IsDBNull(ordDisplayName) ? reader.GetString(ordDisplayName).Trim() : default(string));
							row.UserNameLast = (!reader.IsDBNull(ordUserNameLast) ? reader.GetString(ordUserNameLast).Trim() : default(string));
							row.UserNameFirst = (!reader.IsDBNull(ordUserNameFirst) ? reader.GetString(ordUserNameFirst).Trim() : default(string));
							row.UserUsername = (!reader.IsDBNull(ordUserUsername) ? reader.GetString(ordUserUsername).Trim() : default(string));
							row.IsDictating = (!reader.IsDBNull(ordIsDictating) ? reader.GetInt32(ordIsDictating) : default(int));
							row.DictationID = (!reader.IsDBNull(ordDictationID) ? reader.GetString(ordDictationID).Trim() : default(string));
							row.UserClientCode = (!reader.IsDBNull(ordUserClientCode) ? reader.GetString(ordUserClientCode).Trim() : default(string));
							yield return row;
						}
						while (reader.Read());
					}				
									
				}
			}
			finally
			{
				this.traceSource.TraceEvent(System.Diagnostics.TraceEventType.Stop, 0);
			}
		}
					
	}

	public partial interface IReportingDataContext : IDisposable
	{
		String ConnectionString { get; }
				IEnumerable<GetAssociate2Row> GetAssociate2(System.Int32? associateID);
				IEnumerable<SearchAssociates2Row> SearchAssociates2(System.Int32? clientID, System.String associateFirstName, System.String associateLastName, System.String associateClientCode, System.Boolean? checkfax);
				IEnumerable<ListReportsByASPUserRow> ListReportsByASPUser(System.Int32? reportingUserID);
				IEnumerable<ListReportsByMntUserRow> ListReportsByMntUser(System.Int32? reportingUserID);
				IEnumerable<ListReportsByInqUserRow> ListReportsByInqUser(System.Int32? reportingUserID);
				IEnumerable<ListExecutionStatusesRow> ListExecutionStatuses();
				IEnumerable<ListFormatsRow> ListFormats();
				IEnumerable<GetFormatRow> GetFormat(System.Int32? reportFormatID);
				IEnumerable<ListExecutionsByASPUserRow> ListExecutionsByASPUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<ListExecutionsByMntUserRow> ListExecutionsByMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<ListExecutionsByInqUserRow> ListExecutionsByInqUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<ListSubscriptionsByASPUserRow> ListSubscriptionsByASPUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<ListSubscriptionsByMntUserRow> ListSubscriptionsByMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<ListSubscriptionsByInqUserRow> ListSubscriptionsByInqUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID);
				IEnumerable<GetSubscriptionNextExecutionRow> GetSubscriptionNextExecution(System.Int32? reportSubscriptionID);
				IEnumerable<GetSubscriptionPreviousExecutionRow> GetSubscriptionPreviousExecution(System.Int32? reportSubscriptionID);
				IEnumerable<GetReportRow> GetReport(System.Int32? reportID, System.Int32? environmentID);
				int AddSubscriptionExecution(System.Int32? reportSubscriptionID, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters);
				IEnumerable<GetExecutionForASPUserRow> GetExecutionForASPUser(System.Int32? reportingUserID, System.Int32? reportExecutionID);
				IEnumerable<GetExecutionForMntUserRow> GetExecutionForMntUser(System.Int32? reportingUserID, System.Int32? reportExecutionID);
				IEnumerable<GetExecutionForInqUserRow> GetExecutionForInqUser(System.Int32? reportingUserID, System.Int32? reportExecutionID);
				IEnumerable<GetSubscriptionRow> GetSubscription(System.Int32? reportSubscriptionID);
				IEnumerable<GetSubscriptionNotificationsRow> GetSubscriptionNotifications(System.Int32? reportSubscriptionID);
				IEnumerable<GetExecutionDataRow> GetExecutionData(System.Int32? reportExecutionID);
				IEnumerable<AddSubscriptionForASPUserRow> AddSubscriptionForASPUser(System.Int32? reportingUserID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				IEnumerable<AddSubscriptionForInqUserRow> AddSubscriptionForInqUser(System.Int32? reportingUserID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				IEnumerable<AddSubscriptionForMntUserRow> AddSubscriptionForMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? firstExecutionDate, System.String firstExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				IEnumerable<AddExecutionForASPUserRow> AddExecutionForASPUser(System.Int32? reportingUserID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Int32? environmentID);
				IEnumerable<AddExecutionForInqUserRow> AddExecutionForInqUser(System.Int32? reportingUserID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? reportingCompanyID, System.Int32? environmentID);
				IEnumerable<AddExecutionForMntUserRow> AddExecutionForMntUser(System.Int32? reportingUserID, System.Int32? reportingCompanyID, System.String name, System.DateTime? executionDate, System.Int32? reportID, System.Int32? reportFormatID, System.String parameters, System.Boolean? usedHistory, System.String modifiedUser, System.DateTime? modifiedDate, System.Int32? environmentID);
				int EditSubscriptionForASPUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				int EditSubscriptionForInqUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				int EditSubscriptionForMntUser(System.Int32? reportingUserID, System.Int32? reportSubscriptionID, System.Int32? reportID, System.String name, System.Int32? reportFormatID, System.Boolean? isActive, System.String parameters, System.Int32? scheduleFrequencyID, System.Int32? scheduleFrequencyInterval, System.Int32? scheduleFrequencyRecurrenceFactor, System.String scheduleFrequencyRelativeInterval, System.String scheduleStartTime, System.String scheduleEndTime, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.String modifiedUser, System.DateTime? modifiedDate, System.Boolean? useOnScreenNotification, System.Boolean? useEmailNotification, System.String emailNotificationAddress, System.String options, System.Int32? environmentID);
				int SetExecutionData(System.Int32? reportExecutionID, System.String fileType, System.Byte[] data);
				int DeleteSubscription(System.Int32? reportSubscriptionID);
				int DeleteExecution(System.Int32? reportExecutionID);
				int DeleteExecutionData(System.Int32? reportExecutionID);
				IEnumerable<GetExecutionFromQueueRow> GetExecutionFromQueue(System.Int32? maxFailures);
				int SetExecutionRetry(System.Int32? reportExecutionID, System.String name, System.DateTime? startDate, System.DateTime? endDate, System.String errorDescription, System.Int32? reportExecutionErrorID, System.Int32? errorCount, System.DateTime? retryDate, System.String modifiedUser, System.DateTime? modifiedDate);
				int ResetExecutionQueue();
				int SetExecutionStatus(System.Int32? reportExecutionID, System.String name, System.DateTime? startDate, System.DateTime? endDate, System.Int32? reportExecutionStatusID, System.String errorDescription, System.Int32? errorCount, System.Int32? reportExecutionErrorID, System.Boolean? usedHistory, System.DateTime? nextExecutionDate, System.String nextExecutionParameters, System.Int32? nextExecutionReportID, System.Int32? nextExecutionSubscriptionID, System.Int32? nextExecutionFormatID, System.String modifiedUser, System.DateTime? modifiedDate);
				IEnumerable<GetClientLabelsRow> GetClientLabels(System.Int32? clientID);
				IEnumerable<SSRSInqGetUsersRow> SSRSInqGetUsers(System.Int32? clientID, System.Int32? userID, System.Boolean? includeselectalloption, System.String selectalllabel, System.Boolean? filterbyproxy, System.Boolean? dictatingOnly);
			}
	
	public partial class GetAssociate2Row
	{
		public System.Int32 AssociateID { get; set; }
		public System.Int32 ClientID { get; set; }
		public System.String AssociateClientCode { get; set; }
		public System.String AssociatePrefix { get; set; }
		public System.String AssociateNameFirst { get; set; }
		public System.String AssociateNameMiddle { get; set; }
		public System.String AssociateNameLast { get; set; }
		public System.String AssociateSuffix { get; set; }
		public System.String AssociateBusinessName { get; set; }
		public System.String AssociateSpecialty { get; set; }
		public System.String AssociateGreeting { get; set; }
		public System.String AssociateAddress1 { get; set; }
		public System.String AssociateAddress2 { get; set; }
		public System.String AssociateAddress3 { get; set; }
		public System.String AssociateCity { get; set; }
		public System.String AssociateState { get; set; }
		public System.String AssociateZipCode { get; set; }
		public System.String AssociateCompany { get; set; }
		public System.String AssociatePhone { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime? ModifiedDate { get; set; }
		public System.Int32? ReferralUserID { get; set; }
		public System.String AssociateFax { get; set; }
		public System.String AssociateEMail { get; set; }
		public System.Int32? ReferralLocationID { get; set; }
		public System.Int32 RegionMask { get; set; }
		public System.String AssociateUsername { get; set; }
		public System.String FilePath { get; set; }
		public System.Int32 ExportType { get; set; }
		public System.Boolean AssociateActive { get; set; }
		public System.Boolean AssociateSuspended { get; set; }
		public System.String HCNUserID { get; set; }
		public System.String HCNAccountID { get; set; }
		public System.Boolean AssociateAutoFax { get; set; }
	}
	public partial class SearchAssociates2Row
	{
		public System.Int32 AssociateID { get; set; }
		public System.Int32 ClientID { get; set; }
		public System.String AssociateClientCode { get; set; }
		public System.String AssociatePrefix { get; set; }
		public System.String AssociateNameFirst { get; set; }
		public System.String AssociateNameMiddle { get; set; }
		public System.String AssociateNameLast { get; set; }
		public System.String AssociateSuffix { get; set; }
		public System.String AssociateBusinessName { get; set; }
		public System.String AssociateSpecialty { get; set; }
		public System.String AssociateGreeting { get; set; }
		public System.String AssociateAddress1 { get; set; }
		public System.String AssociateAddress2 { get; set; }
		public System.String AssociateAddress3 { get; set; }
		public System.String AssociateCity { get; set; }
		public System.String AssociateState { get; set; }
		public System.String AssociateZipCode { get; set; }
		public System.String AssociateCompany { get; set; }
		public System.String AssociatePhone { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime? ModifiedDate { get; set; }
		public System.Int32? ReferralUserID { get; set; }
		public System.String AssociateFax { get; set; }
		public System.String AssociateEMail { get; set; }
		public System.Int32? ReferralLocationID { get; set; }
		public System.Int32 RegionMask { get; set; }
		public System.String AssociateUsername { get; set; }
		public System.String FilePath { get; set; }
		public System.Int32 ExportType { get; set; }
		public System.Boolean AssociateActive { get; set; }
		public System.Boolean AssociateSuspended { get; set; }
		public System.String HCNUserID { get; set; }
		public System.String HCNAccountID { get; set; }
		public System.Boolean AssociateAutoFax { get; set; }
	}
	public partial class ListReportsByASPUserRow
	{
		public System.Int32 CategoryID { get; set; }
		public System.String CategoryName { get; set; }
		public System.Int32? CategorySortOrder { get; set; }
		public System.String Description { get; set; }
		public System.String Name { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.Int32? SortOrder { get; set; }
		public System.String ReportPath { get; set; }
		public System.String ReportParametersURL { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListReportsByMntUserRow
	{
		public System.Int32 CategoryID { get; set; }
		public System.String CategoryName { get; set; }
		public System.Int32? CategorySortOrder { get; set; }
		public System.String Description { get; set; }
		public System.String Name { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.Int32? SortOrder { get; set; }
		public System.String ReportPath { get; set; }
		public System.String ReportParametersURL { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListReportsByInqUserRow
	{
		public System.Int32 CategoryID { get; set; }
		public System.String CategoryName { get; set; }
		public System.Int32? CategorySortOrder { get; set; }
		public System.String Description { get; set; }
		public System.String Name { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.Int32? SortOrder { get; set; }
		public System.String ReportPath { get; set; }
		public System.String ReportParametersURL { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListExecutionStatusesRow
	{
		public System.String Description { get; set; }
		public System.String Name { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
	}
	public partial class ListFormatsRow
	{
		public System.Int32 ReportFormatID { get; set; }
		public System.String Name { get; set; }
		public System.String Description { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.String SSRSFormat { get; set; }
		public System.String SSRSDeviceInfo { get; set; }
		public System.String Extension { get; set; }
		public System.String HTTPContentType { get; set; }
	}
	public partial class GetFormatRow
	{
		public System.Int32 ReportFormatID { get; set; }
		public System.String Name { get; set; }
		public System.String Description { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.String SSRSFormat { get; set; }
		public System.String SSRSDeviceInfo { get; set; }
		public System.String Extension { get; set; }
		public System.String HTTPContentType { get; set; }
	}
	public partial class ListExecutionsByASPUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListExecutionsByMntUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListExecutionsByInqUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class ListSubscriptionsByASPUserRow
	{
		public System.Int32 ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String Name { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.String Parameters { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.Int32 ScheduleFrequencyID { get; set; }
		public System.Int32 FrequencyInterval { get; set; }
		public System.Int32? FrequencyRecurrenceFactor { get; set; }
		public System.String FrequencyRelativeInterval { get; set; }
		public System.String StartTime { get; set; }
		public System.String EndTime { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String Options { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.DateTime? PrevStartDate { get; set; }
		public System.DateTime? PrevScheduledStartDate { get; set; }
		public System.DateTime? PrevEndDate { get; set; }
		public System.Int32? PrevReportExecutionStatusID { get; set; }
		public System.String PrevReportExecutionErrorDescription { get; set; }
		public System.DateTime? NextScheduledStartDate { get; set; }
		public System.Int32? NextReportExecutionStatusID { get; set; }
	}
	public partial class ListSubscriptionsByMntUserRow
	{
		public System.Int32 ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String Name { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.String Parameters { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.Int32 ScheduleFrequencyID { get; set; }
		public System.Int32 FrequencyInterval { get; set; }
		public System.Int32? FrequencyRecurrenceFactor { get; set; }
		public System.String FrequencyRelativeInterval { get; set; }
		public System.String StartTime { get; set; }
		public System.String EndTime { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String Options { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.DateTime? PrevStartDate { get; set; }
		public System.DateTime? PrevScheduledStartDate { get; set; }
		public System.DateTime? PrevEndDate { get; set; }
		public System.Int32? PrevReportExecutionStatusID { get; set; }
		public System.String PrevReportExecutionErrorDescription { get; set; }
		public System.DateTime? NextScheduledStartDate { get; set; }
		public System.Int32? NextReportExecutionStatusID { get; set; }
	}
	public partial class ListSubscriptionsByInqUserRow
	{
		public System.Int32 ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String Name { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.String Parameters { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.Int32 ScheduleFrequencyID { get; set; }
		public System.Int32 FrequencyInterval { get; set; }
		public System.Int32? FrequencyRecurrenceFactor { get; set; }
		public System.String FrequencyRelativeInterval { get; set; }
		public System.String StartTime { get; set; }
		public System.String EndTime { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String Options { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.DateTime? PrevStartDate { get; set; }
		public System.DateTime? PrevScheduledStartDate { get; set; }
		public System.DateTime? PrevEndDate { get; set; }
		public System.Int32? PrevReportExecutionStatusID { get; set; }
		public System.String PrevReportExecutionErrorDescription { get; set; }
		public System.DateTime? NextScheduledStartDate { get; set; }
		public System.Int32? NextReportExecutionStatusID { get; set; }
	}
	public partial class GetSubscriptionNextExecutionRow
	{
		public System.Int32? ReportExecutionID { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? EnvironmentID { get; set; }
	}
	public partial class GetSubscriptionPreviousExecutionRow
	{
		public System.DateTime? EndDate { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.String Name { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportExecutionID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.String ReportFormatDescription { get; set; }
		public System.String ReportFormatExtension { get; set; }
		public System.Boolean ReportFormatIsActive { get; set; }
		public System.String ReportFormatName { get; set; }
		public System.String ReportFormatSSRSDeviceInfo { get; set; }
		public System.String ReportFormatSSRSFormat { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.String ReportExecutionStatusDescription { get; set; }
		public System.String ReportExecutionStatusName { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ReportExecutionErrorName { get; set; }
		public System.String ReportExecutionErrorDescription { get; set; }
		public System.Int32? EnvironmentID { get; set; }
	}
	public partial class GetReportRow
	{
		public System.Int32 ReportID { get; set; }
		public System.String Name { get; set; }
		public System.String Description { get; set; }
		public System.Int32? SortOrder { get; set; }
		public System.String ReportPath { get; set; }
		public System.Int32 CategoryID { get; set; }
		public System.String CategoryName { get; set; }
		public System.Int32? CategorySortOrder { get; set; }
		public System.String ReportParametersURL { get; set; }
		public System.String Options { get; set; }
		public System.Int32? EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class GetExecutionForASPUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String RenderOptions { get; set; }
		public System.String SubscriptionOptions { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class GetExecutionForMntUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String RenderOptions { get; set; }
		public System.String SubscriptionOptions { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class GetExecutionForInqUserRow
	{
		public System.Int32 ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.String Parameters { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.DateTime? ExpirationDate { get; set; }
		public System.Boolean UsedHistory { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.String ErrorName { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.Int32 OwnerID { get; set; }
		public System.String OwnerNameFirst { get; set; }
		public System.String OwnerNameLast { get; set; }
		public System.String OwnerUsername { get; set; }
		public System.String RenderOptions { get; set; }
		public System.String SubscriptionOptions { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
	}
	public partial class GetSubscriptionRow
	{
		public System.Int32 ReportSubscriptionID { get; set; }
		public System.Int32 ReportID { get; set; }
		public System.String Name { get; set; }
		public System.Boolean IsActive { get; set; }
		public System.Int32 ReportFormatID { get; set; }
		public System.String Parameters { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime ModifiedDate { get; set; }
		public System.Int32 ScheduleFrequencyID { get; set; }
		public System.Int32 FrequencyInterval { get; set; }
		public System.Int32? FrequencyRecurrenceFactor { get; set; }
		public System.String FrequencyRelativeInterval { get; set; }
		public System.String StartTime { get; set; }
		public System.String EndTime { get; set; }
		public System.String Options { get; set; }
		public System.Int32 EnvironmentID { get; set; }
		public System.String EnvironmentName { get; set; }
		public System.String ReportName { get; set; }
		public System.String ReportDescription { get; set; }
		public System.String CategoryName { get; set; }
		public System.DateTime? PrevStartDate { get; set; }
		public System.DateTime? PrevScheduledStartDate { get; set; }
		public System.DateTime? PrevEndDate { get; set; }
		public System.Int32? PrevReportExecutionStatusID { get; set; }
		public System.String PrevReportExecutionErrorDescription { get; set; }
		public System.DateTime? NextScheduledStartDate { get; set; }
		public System.Int32? NextReportExecutionStatusID { get; set; }
	}
	public partial class GetSubscriptionNotificationsRow
	{
		public System.Int32 ReportSubscriptionID { get; set; }
		public System.Int32 ReportNotificationTypeID { get; set; }
		public System.String ReportNotificationOptions { get; set; }
	}
	public partial class GetExecutionDataRow
	{
		public System.String FileType { get; set; }
		public System.Byte[] Data { get; set; }
	}
	public partial class AddSubscriptionForASPUserRow
	{
		public System.Int32? ReportSubscriptionID { get; set; }
	}
	public partial class AddSubscriptionForInqUserRow
	{
		public System.Int32? ReportSubscriptionID { get; set; }
	}
	public partial class AddSubscriptionForMntUserRow
	{
		public System.Int32? ReportSubscriptionID { get; set; }
	}
	public partial class AddExecutionForASPUserRow
	{
		public System.Int32? ReportExecutionID { get; set; }
	}
	public partial class AddExecutionForInqUserRow
	{
		public System.Int32? ReportExecutionID { get; set; }
	}
	public partial class AddExecutionForMntUserRow
	{
		public System.Int32? ReportExecutionID { get; set; }
	}
	public partial class GetExecutionFromQueueRow
	{
		public System.Int32? ReportExecutionID { get; set; }
		public System.String Name { get; set; }
		public System.DateTime? ScheduledStartDate { get; set; }
		public System.DateTime? StartDate { get; set; }
		public System.DateTime? EndDate { get; set; }
		public System.Int32? ReportID { get; set; }
		public System.Int32? ReportSubscriptionID { get; set; }
		public System.Int32? ReportFormatID { get; set; }
		public System.Int32 ReportExecutionStatusID { get; set; }
		public System.String Parameters { get; set; }
		public System.String ErrorDescription { get; set; }
		public System.Int32? ReportExecutionErrorID { get; set; }
		public System.Int32? ErrorCount { get; set; }
		public System.Boolean? UsedHistory { get; set; }
		public System.String ModifiedUser { get; set; }
		public System.DateTime? ModifiedDate { get; set; }
		public System.String SubscriptionOptions { get; set; }
		public System.Int32? EnvironmentID { get; set; }
	}
	public partial class GetClientLabelsRow
	{
		public System.String LabelPatientID { get; set; }
		public System.String LabelPatientName { get; set; }
		public System.String LabelGender { get; set; }
		public System.String LabelBirthdate { get; set; }
		public System.String LabelAppointmentDate { get; set; }
		public System.String LabelOrderNumber { get; set; }
		public System.String LabelPatientLetter { get; set; }
		public System.String LabelUserField1 { get; set; }
		public System.String LabelUserField2 { get; set; }
		public System.String LabelUserField3 { get; set; }
		public System.String LabelUserField4 { get; set; }
		public System.String LabelUserField5 { get; set; }
		public System.String LabelBillingUnit { get; set; }
		public System.String LabelDictator { get; set; }
	}
	public partial class SSRSInqGetUsersRow
	{
		public System.Int32? UserID { get; set; }
		public System.String DisplayName { get; set; }
		public System.String UserNameLast { get; set; }
		public System.String UserNameFirst { get; set; }
		public System.String UserUsername { get; set; }
		public System.Int32 IsDictating { get; set; }
		public System.String DictationID { get; set; }
		public System.String UserClientCode { get; set; }
	}
	
}

