using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
	public class ClientLabels
	{
		public string LabelPatientID { get; set; }
		public string LabelPatientName { get; set; }
		public string LabelGender { get; set; }
		public string LabelBirthdate { get; set; }
		public string LabelAppointmentDate { get; set; }
		public string LabelOrderNumber { get; set; }
		public string LabelPatientLetter { get; set; }
		public string LabelUserField1 { get; set; }
		public string LabelUserField2 { get; set; }
		public string LabelUserField3 { get; set; }
		public string LabelUserField4 { get; set; }
		public string LabelUserField5 { get; set; }
		public string LabelBillingUnit { get; set; }
		public string LabelDictator { get; set; }

		public static ClientLabels LoadClientLabels(int clientId, ReportingApplication application)
		{
			using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
			{
				return (from l in sql.GetClientLabels(clientId)
						select new ClientLabels
						{
							LabelPatientID = l.LabelPatientID,
							LabelPatientName = l.LabelPatientName,
							LabelGender = l.LabelGender,
							LabelBirthdate = l.LabelBirthdate,
							LabelAppointmentDate = l.LabelAppointmentDate,
							LabelOrderNumber = l.LabelOrderNumber,
							LabelPatientLetter = l.LabelPatientLetter,
							LabelUserField1 = l.LabelUserField1,
							LabelUserField2 = l.LabelUserField2,
							LabelUserField3 = l.LabelUserField3,
							LabelUserField4 = l.LabelUserField4,
							LabelUserField5 = l.LabelUserField5,
							LabelBillingUnit = l.LabelBillingUnit,
							LabelDictator = l.LabelDictator
						}).FirstOrDefault();
			}
		}

		public void ApplyToReport(Report report)
		{
			if (null != report.Options)
			{
				report.Options = report.Options
									.Replace("[Patient_ID]", this.LabelPatientID)
									.Replace("[Patient_Name]", this.LabelPatientName)
									.Replace("[Gender]", this.LabelGender)
									.Replace("[Birthdate]", this.LabelBirthdate)
									.Replace("[Appointment_Date]", this.LabelAppointmentDate)
									.Replace("[Order_Number]", this.LabelOrderNumber)
									.Replace("[Patient_Letter]", this.LabelPatientLetter)
									.Replace("[User_Field_1]", this.LabelUserField1)
									.Replace("[User_Field_2]", this.LabelUserField1)
									.Replace("[User_Field_3]", this.LabelUserField1)
									.Replace("[User_Field_4]", this.LabelUserField1)
									.Replace("[User_Field_5]", this.LabelUserField1)
									.Replace("[Billing_Unit]", this.LabelBillingUnit)
									.Replace("[Clinician]", this.LabelDictator);
			}
		}
	}
}
