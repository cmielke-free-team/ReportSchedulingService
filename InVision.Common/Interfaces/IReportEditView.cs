using System;
using System.Collections.Generic;
namespace Emdat.InVision
{
	public interface IReportEditView
	{
		string CurrentMode { get; set; }
		IReportParametersView ParametersView { get; set; }
		string ReportDescription { get; set; }
		string ReportId { get; set; }
		string ReportTitle { get; set; }
		IScheduleView ScheduleView { get; set; }
		string ScheduleFormatId { get; set; }
		string ScheduleOptionId { get; set; }
		bool ScheduleIsActive { get; set; }
		bool ScheduleOnScreenNotification { get; set; }
		bool ScheduleEmailNotification { get; set; }
		string ScheduleNotificationEmail { get; set; }
		string ScheduleName { get; set; }
		string SubscriptionId { get; set; }
		string OwnerId { get; set; }
        ReportingEnvironmentEnum ReportingEnvironmentId { get; set; }
	}
}
