using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Emdat.InVision
{
    public partial class Schedule : ISchedule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class.
        /// </summary>
        public Schedule()
        {
            this.RecurrenceOption = ScheduleRecurrence.Once;
            this.OnceNow = true;
            this.OnceDateTime = DateTime.Now;
        }

        public string Description
        {
            get
            {
                switch (this.RecurrenceOption)
                {
                    case ScheduleRecurrence.Once:
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "{0} once on {1:D} at {2:t}.",
                            this.OnceDateTime > DateTime.Now ? "Will run" : "Ran",
                            this.OnceDateTime,
                            this.OnceDateTime);
                    case ScheduleRecurrence.Daily:
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "Runs every {0}day{1} at {2:t}.",
                            this.DailyWeekdaysOnly ? "week" : this.DailyInterval > 1 ? string.Format("{0} ", this.DailyInterval) : string.Empty,
                            this.DailyInterval > 1 ? "s" : string.Empty,
                            new DateTime(2009, 1, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds));
                    case ScheduleRecurrence.Weekly:
                        List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
                        if (this.WeeklyOnSunday) daysOfWeek.Add(DayOfWeek.Sunday);
                        if (this.WeeklyOnMonday) daysOfWeek.Add(DayOfWeek.Monday);
                        if (this.WeeklyOnTuesday) daysOfWeek.Add(DayOfWeek.Tuesday);
                        if (this.WeeklyOnWednesday) daysOfWeek.Add(DayOfWeek.Wednesday);
                        if (this.WeeklyOnThursday) daysOfWeek.Add(DayOfWeek.Thursday);
                        if (this.WeeklyOnFriday) daysOfWeek.Add(DayOfWeek.Friday);
                        if (this.WeeklyOnSaturday) daysOfWeek.Add(DayOfWeek.Saturday);
                        string[] daysOfWeekArray =
                            (from d in daysOfWeek
                             orderby d
                             select CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(d))
                            .ToArray();
                        string daysOfWeekStr = string.Join(", ", daysOfWeekArray);
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "Runs every {0}week{1} on {2} at {3:t}.",
                            this.WeeklyInterval > 1 ? string.Format("{0} ", this.WeeklyInterval) : string.Empty,
                            this.WeeklyInterval > 1 ? "s" : string.Empty,
                            daysOfWeekStr,
                            new DateTime(2009, 1, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds));
                    case ScheduleRecurrence.SemiMonthly:
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "Runs on the 1st and 16th day of every month at {0:t}.",
                            new DateTime(2009, 1, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds));                        
                    case ScheduleRecurrence.Monthly:
                        string daysOfMonthStr = string.Empty;
                        if (this.MonthlyOnSpecificDays)
                        {
                            daysOfMonthStr = string.Format(
                                CultureInfo.CurrentCulture,
                                "day(s) {0}",
                                this.MonthlyDaysExpression);
                        }
                        else
                        {
                            daysOfMonthStr = string.Format(
                                CultureInfo.CurrentCulture,
                                "the {0} {1}",
                                this.MonthlyWeekNumber.ToString().ToLower(CultureInfo.CurrentCulture),
                                this.MonthlyDayOption == DayOfWeekFlags.Any ?
                                    "day" :
                                    this.MonthlyDayOption == DayOfWeekFlags.WeekDays ? "weekday" :
                                    this.MonthlyDayOption == DayOfWeekFlags.WeekendDays ? "weekend day" :
                                    this.MonthlyDayOption == DayOfWeekFlags.None ? string.Empty :
                                    CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(((DayOfWeek)Enum.Parse(typeof(DayOfWeek), this.MonthlyDayOption.ToString()))));
                        }
                        return string.Format(
                            CultureInfo.CurrentCulture,
                            "Runs on {0} of every {1}month{2} at {3:t}.",
                            daysOfMonthStr,
                            this.MonthlyInterval > 1 ? string.Format("{0} ", this.MonthlyInterval) : string.Empty,
                            this.MonthlyInterval > 1 ? "s" : string.Empty,                            
                            new DateTime(2009, 1, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds));
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the recurrence option.
        /// </summary>
        /// <value>The recurrence option.</value>
        public ScheduleRecurrence RecurrenceOption { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public TimeSpan EndTime { get; set; }

        #region once options

        public bool OnceNow { get; set; }

        public DateTime OnceDateTime { get; set; }

        #endregion

        #region hourly options

        public int HourlyInterval { get; set; }

        #endregion

        #region daily options

        public int DailyInterval { get; set; }

        public bool DailyWeekdaysOnly { get; set; }

        #endregion

        #region weekly options

        public int WeeklyInterval { get; set; }

        public bool WeeklyOnMonday { get; set; }

        public bool WeeklyOnTuesday { get; set; }

        public bool WeeklyOnWednesday { get; set; }

        public bool WeeklyOnThursday { get; set; }

        public bool WeeklyOnFriday { get; set; }

        public bool WeeklyOnSaturday { get; set; }

        public bool WeeklyOnSunday { get; set; }

        #endregion

        #region monthly options

        public int MonthlyInterval { get; set; }

        public bool MonthlyOnSpecificDays { get; set; }

        public string MonthlyDaysExpression { get; set; }

        public WeekOfMonth MonthlyWeekNumber { get; set; }

        public DayOfWeekFlags MonthlyDayOption { get; set; }

        #endregion
    }
}
