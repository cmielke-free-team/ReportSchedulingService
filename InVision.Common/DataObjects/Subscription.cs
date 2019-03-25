using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using Emdat.InVision.SSRS;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using InVisionMvc.Infrastructure;

namespace Emdat.InVision
{
    /// <summary>
    /// Subscription class
    /// </summary>
    [Serializable]
    [DataObject(true)]
    public partial class Subscription
    {
        private static XmlSerializer _subscriptionSerializer = new XmlSerializer(typeof(Subscription));
        private Report _report = null;
        private ReportFormat _format = null;
        private ExecutionStatus _status = null;
        private ScheduleOption _schedule = null;
        private string _baseCoTimeZone;
        private string _baseCoTimeZoneFromReport;
        private List<ParameterValue> _parameters;

        public static string ServerTimeZoneIdentifier
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ServerTimeZoneIdentifier"]) ? 
                    ConfigurationManager.AppSettings["ServerTimeZoneIdentifier"] : 
                    "Central Standard Time";
            }
        }

        /// <summary>
        /// Gets the subscription from file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private static Subscription GetSubscriptionFromFile(string file)
        {
            Subscription retVal = null;
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    retVal = _subscriptionSerializer.Deserialize(fs) as Subscription;
                    fs.Close();
                }
                return retVal;
            }
            catch
            {
            }
            return retVal;
        }

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

        #region data methods

        /// <summary>
        /// Lists the subscriptions.
        /// </summary>
        /// <param name="ownerId">The owner id.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<Subscription> ListSubscriptions(int userId, int? companyId, ReportingApplication application)
        {
            IEnumerable<Subscription> subscriptions = null;

            if (subscriptions == null)
            {
                using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
                {
                    var subscriptionsQuery =
                        from s in sql.ListSubscriptions(userId, companyId)
                        let next = sql.GetSubscriptionNextExecution(s.ReportSubscriptionID).FirstOrDefault()
                        let prev = sql.GetSubscriptionPreviousExecution(s.ReportSubscriptionID).FirstOrDefault()
                        select new Subscription
                        {
                            Application = application,
                            OwnerId = s.OwnerID.ToString(),
                            OwnerName = string.Format("{0}, {1}", s.OwnerNameLast, s.OwnerNameFirst),
                            OwnerUsername = s.OwnerUsername,
                            Description = s.Name,
                            FormatId = s.ReportFormatID.ToString(),
                            Id = s.ReportSubscriptionID.ToString(),
                            IsActive = s.IsActive,
                            ErrorDescription = prev != null ? prev.ReportExecutionErrorDescription : null,
                            LastRunEndTime = prev != null ? prev.EndDate : null,
                            LastRunScheduledTime = prev != null ? prev.ScheduledStartDate : null,
                            LastRunStartTime = prev != null ? prev.StartDate : null,
                            ModifiedDate = s.ModifiedDate,
                            ModifiedUser = s.ModifiedUser,
                            NextRunTime = next != null ? next.ScheduledStartDate : null,
                            Parameters = ReportParametersHelper.GetParameterValuesFromXml(s.Parameters),
                            ReportingEnvironmentId = (ReportingEnvironmentEnum)s.EnvironmentID.GetValueOrDefault(1),
                            ReportingEnvironmentName = s.EnvironmentName,
                            ReportId = s.ReportID.ToString(),
                            //								ScheduleOption = ???,
                            Schedule = new Schedule(
                                    s.ScheduleFrequencyID,
                                    s.FrequencyInterval,
                                    s.FrequencyRecurrenceFactor,
                                    s.FrequencyRelativeInterval,
                                    next != null ? next.ScheduledStartDate : null,
                                    s.StartTime,
                                    s.EndTime),
                            StatusId = s.IsActive ? GetSubscriptionStatus(prev, next) : ExecutionStatusEnum.Inactive,
                            Options = s.Options
                        };
                    subscriptions = subscriptionsQuery.ToList();

                }
            }
            return subscriptions;
        }

        public static void AddExecution(Subscription subscription, ReportingApplication application)
        {
            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                sql.AddSubscriptionExecution(int.Parse(subscription.Id), int.Parse(subscription.ReportId), int.Parse(subscription.FormatId), subscription.GetParameterValuesXml());
            }
        }

        /// <summary>
        /// Loads the subscription.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Subscription LoadSubscription(string id, int userId, ReportingApplication application)
        {
            #region validation

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            int subscriptionId = int.Parse(id);

            if (subscriptionId < 1)
            {
                throw new ArgumentOutOfRangeException("id");
            }

            #endregion

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                var subscription =
                    (from s in sql.GetSubscription(subscriptionId)
                     let next = sql.GetSubscriptionNextExecution(s.ReportSubscriptionID).FirstOrDefault()
                     let prev = sql.GetSubscriptionPreviousExecution(s.ReportSubscriptionID).FirstOrDefault()
                     let notifications = sql.GetSubscriptionNotifications(s.ReportSubscriptionID)
                     let notificationEmail =
                        (notifications.Any(n => n.ReportNotificationTypeID == 2) && !String.IsNullOrEmpty(notifications.FirstOrDefault(n => n.ReportNotificationTypeID == 2).ReportNotificationOptions)) ?
                            XDocument.Parse(notifications.FirstOrDefault(n => n.ReportNotificationTypeID == 2).ReportNotificationOptions).Element("NotificationOptions").Element("EmailAddress").Value : string.Empty
                     let schedule = new Schedule(
                         s.ScheduleFrequencyID,
                         s.FrequencyInterval,
                         s.FrequencyRecurrenceFactor,
                         s.FrequencyRelativeInterval,
                         next != null ? next.ScheduledStartDate : prev != null ? prev.ScheduledStartDate : null,
                         s.StartTime,
                         s.EndTime)
                     select new Subscription
                     {
                         Application = application,
                         OwnerId = userId.ToString(),
                         Description = s.Name,
                         FormatId = s.ReportFormatID.ToString(),
                         Id = s.ReportSubscriptionID.ToString(),
                         IsActive = s.IsActive,
                         ErrorDescription = prev != null ? prev.ReportExecutionErrorDescription : null,
                         LastRunEndTime = prev != null ? prev.EndDate : null,
                         LastRunScheduledTime = prev != null ? prev.ScheduledStartDate : null,
                         LastRunStartTime = prev != null ? prev.StartDate : null,
                         ModifiedDate = s.ModifiedDate,
                         ModifiedUser = s.ModifiedUser,
                         NextRunTime = next != null ? next.ScheduledStartDate : null,
                         Parameters = ReportParametersHelper.GetParameterValuesFromXml(s.Parameters),
                         ReportingEnvironmentId = (ReportingEnvironmentEnum)s.EnvironmentID,
                         ReportingEnvironmentName = s.EnvironmentName,
                         ReportId = s.ReportID.ToString(),
                         NotifyOnScreen = notifications.Any(n => n.ReportNotificationTypeID == 1),
                         NotifyEmail = notifications.Any(n => n.ReportNotificationTypeID == 2),
                         NotificationEmail = notificationEmail,
                         ScheduleOptionId = next != null && next.ScheduledStartDate.HasValue ? ScheduleOptionEnum.Schedule :
                            schedule.RecurrenceOption == ScheduleRecurrence.Once ? ScheduleOptionEnum.QueueNow :
                            false == s.IsActive ? ScheduleOptionEnum.Schedule :
                            ScheduleOptionEnum.QueueNow,
                         Schedule = schedule,
                         StatusId = s.IsActive ? GetSubscriptionStatus(prev, next) : ExecutionStatusEnum.Inactive,
                         Options = s.Options
                     }).FirstOrDefault();
                if (subscription == null)
                {
                    throw new KeyNotFoundException(string.Format("The report subscription could not be found: {0}", id));
                }
                return subscription;
            }
        }

        /// <summary>
        /// Adds the subscription.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public static void AddSubscription(Subscription subscription)
        {
            #region validation

            if (!subscription.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            //for InCommand, require a company ID
            if (subscription.Application.Value == ReportingApplication.InCommand &&
                    !subscription.CompanyId.HasValue)
            {
                throw new InvalidOperationException("CompanyId is required for InCommand reports");
            }

            if (string.IsNullOrEmpty(subscription.FormatId))
            {
                throw new InvalidOperationException("Format must be set");
            }

            if (string.IsNullOrEmpty(subscription.ReportId))
            {
                throw new InvalidOperationException("ReportId must be set");
            }

            if (subscription.Parameters == null)
            {
                throw new InvalidOperationException("Parameters must be set");
            }

            //TODO: validate the parameters

            if (subscription.Schedule == null)
            {
                throw new InvalidOperationException("Schedule must be set");
            }

            //TODO: validate the schedule

            #endregion

            //set input values for the insert operation
            int reportingUserId = int.Parse(subscription.OwnerId);
            int reportId = int.Parse(subscription.ReportId);
            int formatId = int.Parse(subscription.FormatId);

            //get next run info
            NextRunInfo nextRunInfo = subscription.GetNextRunInfo(DateTime.Now);

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(subscription.Application.Value))
            {
                var result = sql.AddSubscription(
                        reportingUserId,
                        subscription.CompanyId,
                        reportId,
                        subscription.Description,
                        formatId,
                        subscription.IsActive,
                        subscription.GetParameterValuesXml(),
                        subscription.Schedule.GetFrequencyId(),
                        subscription.Schedule.GetFrequencyInterval(),
                        subscription.Schedule.GetFrequencyRecurrenceFactor(),
                        subscription.Schedule.GetFrequencyRelativeInterval(),
                        string.Format("{0:00}{1:00}{2:00}",
                                subscription.Schedule.StartTime.Hours,
                                subscription.Schedule.StartTime.Minutes,
                                subscription.Schedule.StartTime.Seconds),
                        string.Format("{0:00}{1:00}{2:00}",
                                subscription.Schedule.EndTime.Hours,
                                subscription.Schedule.EndTime.Minutes,
                                subscription.Schedule.EndTime.Seconds),
                        nextRunInfo.Date,
                        nextRunInfo.ParametersAsXml,
                        subscription.ModifiedUser,
                        DateTime.Now,
                        subscription.NotifyOnScreen,
                        subscription.NotifyEmail,
                        subscription.NotificationEmail,
                        subscription.Options,
                        (int)subscription.ReportingEnvironmentId).FirstOrDefault();
                if (result == null || !result.ReportSubscriptionID.HasValue)
                {
                    throw new ApplicationException("The subscription could not be added. The procedure did not return a result.");
                }
            }
        }

        /// <summary>
        /// Updates subscription in the updated report form where run now is removed and notifications added.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static void UpdateSubscription(Subscription subscription)
        {
            #region validation

            if (!subscription.Application.HasValue)
            {
                throw new InvalidOperationException("Application must be set");
            }

            if (string.IsNullOrEmpty(subscription.FormatId))
            {
                throw new InvalidOperationException("Format must be set");
            }

            if (string.IsNullOrEmpty(subscription.ReportId))
            {
                throw new InvalidOperationException("ReportId must be set");
            }

            if (subscription.Parameters == null)
            {
                throw new InvalidOperationException("Parameters must be set");
            }

            if (subscription.Schedule == null)
            {
                throw new InvalidOperationException("Schedule must be set");
            }

            #endregion

            //set input values for the insert operation
            int reportingUserId = int.Parse(subscription.OwnerId);
            int subscriptionId = int.Parse(subscription.Id);
            int reportId = int.Parse(subscription.ReportId);
            int formatId = int.Parse(subscription.FormatId);

            //get next run info
            NextRunInfo nextRunInfo = subscription.GetNextRunInfo(DateTime.Now);

            //load old values
            Subscription oldSubscription = LoadSubscription(
                    subscription.Id,
                    reportingUserId,
                    subscription.Application.Value);

            //only change the properties that are not "read-only"
            oldSubscription.Application = subscription.Application;
            oldSubscription.FormatId = subscription.FormatId;
            oldSubscription.IsActive = subscription.IsActive;
            oldSubscription.ModifiedDate = DateTime.Now;
            oldSubscription.ModifiedUser = subscription.ModifiedUser;
            oldSubscription.Description = subscription.Description;
            oldSubscription.Parameters = subscription.Parameters;
            oldSubscription.Schedule = subscription.Schedule;

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(subscription.Application.Value))
            {
                int result = sql.EditSubscription(
                        reportingUserId,
                        subscriptionId,
                        reportId,
                        subscription.Description,
                        formatId,
                        subscription.IsActive,
                        subscription.GetParameterValuesXml(),
                        subscription.Schedule.GetFrequencyId(),
                        subscription.Schedule.GetFrequencyInterval(),
                        subscription.Schedule.GetFrequencyRecurrenceFactor(),
                        subscription.Schedule.GetFrequencyRelativeInterval(),
                        string.Format("{0:00}{1:00}{2:00}",
                                subscription.Schedule.StartTime.Hours,
                                subscription.Schedule.StartTime.Minutes,
                                subscription.Schedule.StartTime.Seconds),
                        string.Format("{0:00}{1:00}{2:00}",
                                subscription.Schedule.EndTime.Hours,
                                subscription.Schedule.EndTime.Minutes,
                                subscription.Schedule.EndTime.Seconds),
                        nextRunInfo.Date,
                        nextRunInfo.ParametersAsXml,
                        subscription.ModifiedUser,
                        DateTime.Now,
                        subscription.NotifyOnScreen,
                        subscription.NotifyEmail,
                        subscription.NotificationEmail,
                        subscription.Options,
                        (int)subscription.ReportingEnvironmentId);
                if (result < 1)
                {
                    throw new ApplicationException("The subscription was not updated. No rows were modified.");
                }
            }
        }

        /// <summary>
        /// Deletes the subscription.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static void DeleteSubscription(Subscription subscription)
        {
            //TODO: invalidate cached objects that are related to the specified subscription

            int subscriptionId = int.Parse(subscription.Id);

#if MOCKUP

						//build the subscriptions path
						string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["AppDataPathForMockup"], "Subscriptions");

						//find the file
						string file =
								(from f in Directory.GetFiles(path, string.Format("{0}.xml", subscription.Id), SearchOption.AllDirectories)
								 select f)
								 .FirstOrDefault();

						if (file != null)
						{
								File.Delete(file);
						}        
#else
            using (var sql = new Emdat.InVision.Sql.ReportingDataContext())
            {
                sql.DeleteSubscription(subscriptionId);
            }
#endif
        }

        /// <summary>
        /// Determines whether [is subscription name unique] [the specified subscription].
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns>
        /// 	<c>true</c> if [is subscription name unique] [the specified subscription]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSubscriptionNameUnique(Subscription subscription)
        {
            var existingSubscriptions =
                    from s in Subscription.ListSubscriptions(int.Parse(subscription.OwnerId), subscription.CompanyId, subscription.Application.Value)
                    where s.Id != subscription.Id
                    where s.OwnerId == subscription.OwnerId
                    where 0 == string.Compare(s.Description, subscription.Description, StringComparison.OrdinalIgnoreCase)
                    select s;
            return existingSubscriptions.Any() == false;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="ownerId">The owner id.</param>
        public Subscription(string reportId, ReportingEnvironmentEnum environment, int? ownerId, int? clientId, ReportingApplication? application)
        {
            this.Application = application;
            this.OwnerId = ownerId.ToString();
            this.ClientId = clientId;
            this.ReportingEnvironmentId = environment;
            this.ReportId = reportId;
            this.Description = this.ReportName; //default to report name
            this.ErrorDescription = null;
            this.FormatId = "7"; //default to PDF
            this.Id = null;
            this.IsActive = true;
            this.NotifyOnScreen = true; // default to on screen notification
            this.NotifyEmail = false;
            this.NotificationEmail = string.Empty; // todo: default to user/mt/asp email
            this.LastRunEndTime = null;
            this.LastRunScheduledTime = null;
            this.LastRunStartTime = null;
            this.ModifiedDate = null;
            this.ModifiedUser = string.Empty;
            this.NextRunTime = null;
            this.Parameters = null;
            this.Schedule = new Schedule();
            this.ScheduleOptionId = ScheduleOptionEnum.QueueNow;
            this.StatusId = ExecutionStatusEnum.New;
            this.Options = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        public Subscription()
            : this(null, ReportingEnvironmentEnum.Production, null, null, null)
        {
        }

        /// <summary>
        /// Gets the NextRunInfo
        /// </summary>
        /// <returns></returns>        
        public NextRunInfo GetNextRunInfo(DateTime sourceDate)
        {
            //TFS #3449: Calculate next run parameter values using the Base 
            //Company Time Zone; otherwise the date variables will be off            

            NextRunInfo nextRunInfo = new NextRunInfo();            
            nextRunInfo.Date = this.IsActive ? this.Schedule.GetNextRunDate(sourceDate) : null;
            DateTime? nextRunDateInBaseCoTimeZone = null;
            if (nextRunInfo.Date.HasValue && false == string.IsNullOrEmpty(this.BaseCompanyTimeZoneIdentifier))
            {
                nextRunDateInBaseCoTimeZone = TimeZoneInfoExtensions.SafeConvertTimeBySystemTimeZoneId(nextRunInfo.Date.Value, Subscription.ServerTimeZoneIdentifier, this.BaseCompanyTimeZoneIdentifier);
            }   
                nextRunInfo.ParametersAsXml = nextRunDateInBaseCoTimeZone.HasValue ? this.GetNextRunParameterValuesXml(nextRunDateInBaseCoTimeZone.Value) : null;
            return nextRunInfo;
        }        

        public DateTime? GetNextRunTime()
        {
            DateTime? result = null;
            if (this.IsActive)
            {
                if (this.ScheduleOptionId.GetValueOrDefault(ScheduleOptionEnum.QueueNow) != ScheduleOptionEnum.QueueNow)
                {
                    result = this.Schedule.GetNextRunDate(DateTime.Now);
                }
                else
                {
                    result = DateTime.Now;
                }
            }
            return result;
        }

        #region data properties

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        [DataObjectField(false, false, false)]
        public ReportingApplication? Application { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the base company time zone identifier.
        /// </summary>
        /// <value>The base company time zone identifier.</value>
        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string BaseCompanyTimeZoneIdentifier
        {
            get
            {
                if (string.IsNullOrEmpty(_baseCoTimeZone) &&
                        string.IsNullOrEmpty(_baseCoTimeZoneFromReport))
                {
                    _baseCoTimeZoneFromReport = Report.GetBaseCompanyTimeZoneIdentifier(_report.ReportingServicesPath, this.Parameters, ServerTimeZoneIdentifier);
                }
                return !string.IsNullOrEmpty(_baseCoTimeZone) ? _baseCoTimeZone : _baseCoTimeZoneFromReport;
            }
            set
            {
                _baseCoTimeZone = value;
            }
        }

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
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        [DataObjectField(false, false, false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user should be notified on screen when report is ready.
        /// </summary>
        /// <value><c>true</c> if user wantes on screen notification; otherwise, <c>false</c>.</value>
        [DataObjectField(false, false, false)]
        public bool NotifyOnScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user should be notified over email when report is ready.
        /// </summary>
        /// <value><c>true</c> if user wantes email notification; otherwise, <c>false</c>.</value>
        [DataObjectField(false, false, false)]
        public bool NotifyEmail { get; set; }

        /// <summary>
        /// Gets or sets email user should be notified at when report is ready.
        /// </summary>
        /// <value>The notification email.</value>
        [DataObjectField(false, false, false)]
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>The schedule.</value>
        [DataObjectField(false, false, false)]
        public Schedule Schedule { get; set; }

        /// <summary>
        /// Gets or sets the schedule option (queue now or schedule).
        /// </summary>
        /// <value>The schedule option.</value>
        public ScheduleOptionEnum? ScheduleOptionId
        {
            get
            {
                if (_schedule == null)
                {
                    return null;
                }
                if (String.IsNullOrEmpty(_schedule.Id))
                {
                    return null;
                }
                return (ScheduleOptionEnum)int.Parse(_schedule.Id);
            }
            set
            {
                _schedule = Emdat.InVision.ScheduleOption.ListScheduleOptions()
                    .FirstOrDefault(f => f.Id == string.Format("{0:d}", value));
            }
        }

        /// <summary>
        /// Gets the schedule description.
        /// </summary>
        /// <value>The schedule description.</value>
        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string ScheduleDescription
        {
            get
            {
                return this.Schedule != null ? this.Schedule.Description : "Not scheduled";
            }
        }

        /// <summary>
        /// Gets or sets the parameters XML.
        /// </summary>
        /// <value>The parameters XML.</value>
        [DataObjectField(false, false, false)]
        public List<ParameterValue> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
                _baseCoTimeZoneFromReport = null;
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
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>The owner id.</value>
        [DataObjectField(false, false, false)]
        public string OwnerId { get; set; }

        public int? ClientId { get; set; }

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

        [DataObjectField(false, false, true)]
        public string Options { get; set; }

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

        #endregion

        #region report properties

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
                    if (this.ReportingEnvironmentId == 0)
                    {
                        throw new InvalidOperationException("ReportId cannot be set without a ReportEnvironmentId");
                    }
                    _report = Report.LoadReport(value, int.Parse(this.OwnerId), this.ClientId, this.Application.Value, this.ReportingEnvironmentId);
                    _baseCoTimeZoneFromReport = null;
                }
            }
        }

        /// <summary>
        /// Gets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        [XmlIgnore]
        [DataObjectField(false, false, false)]
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
        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string ReportDescription
        {
            get
            {
                return _report != null ? _report.Description : null;
            }
        }

        /// <summary>
        /// Gets the report category.
        /// </summary>
        /// <value>The report category.</value>
        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string ReportCategory
        {
            get
            {
                return _report != null ? _report.Category : "Miscellaneous Reports";
            }
        }

        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string ReportParametersUrl
        {
            get
            {
                return _report != null ? _report.ReportParametersUrl : null;
            }
        }

        [XmlIgnore]
        [DataObjectField(false, false, false)]
        public string ReportOptions
        {
            get
            {
                return _report != null ? _report.Options : null;
            }
        }

        #endregion

        #region status properties

        /// <summary>
        /// Gets or sets the last run scheduled time.
        /// </summary>
        /// <value>The last run scheduled time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? LastRunScheduledTime { get; set; }

        /// <summary>
        /// Gets or sets the last run start time.
        /// </summary>
        /// <value>The last run start time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? LastRunStartTime { get; set; }

        /// <summary>
        /// Gets or sets the last run end time.
        /// </summary>
        /// <value>The last run end time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? LastRunEndTime { get; set; }

        /// <summary>
        /// Gets or sets the next run time.
        /// </summary>
        /// <value>The next run time.</value>
        [DataObjectField(false, false, true)]
        public DateTime? NextRunTime
        {
            get;
            set;
        }

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

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>The error description.</value>
        [DataObjectField(false, false, true)]
        public string ErrorDescription { get; set; }

        #endregion

        public ReportingEnvironmentEnum ReportingEnvironmentId { get; set; }

        public string ReportingEnvironmentName { get; set; }
    }
}
