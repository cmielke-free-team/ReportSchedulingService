using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    public partial class Schedule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class.
        /// </summary>
        /// <param name="frequencyId">The frequency id.</param>
        /// <param name="frequencyInterval">The frequency interval.</param>
        /// <param name="frequencyRecurrenceFactor">The frequency recurrence factor.</param>
        /// <param name="frequencyRelativeInterval">The frequency relative interval.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        public Schedule(
                int frequencyId,
                int frequencyInterval,
                int? frequencyRecurrenceFactor,
                string frequencyRelativeInterval,
                DateTime? startDate,
                string startTime,
                string endTime)
            : this()
        {
            if (!string.IsNullOrEmpty(startTime))
            {
                int hour = int.Parse(startTime.Substring(0, 2));
                int min = int.Parse(startTime.Substring(2, 2));
                int sec = int.Parse(startTime.Substring(4, 2));
                this.StartTime = new TimeSpan(hour, min, sec);
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                int hour = int.Parse(endTime.Substring(0, 2));
                int min = int.Parse(endTime.Substring(2, 2));
                int sec = int.Parse(endTime.Substring(4, 2));
                this.EndTime = new TimeSpan(hour, min, sec);
            }

            switch (frequencyId)
            {
                case 1:
                {
                    this.RecurrenceOption = ScheduleRecurrence.Daily;
                    this.DailyInterval = (frequencyRecurrenceFactor.HasValue ? frequencyRecurrenceFactor.Value : 1);
                    this.DailyWeekdaysOnly = (frequencyInterval == (int)DayOfWeekFlags.WeekDays);
                    break;
                }
                case 2:
                {
                    this.RecurrenceOption = ScheduleRecurrence.Weekly;
                    this.WeeklyInterval = (frequencyRecurrenceFactor.HasValue ? frequencyRecurrenceFactor.Value : 1);
                    this.WeeklyOnMonday = ((frequencyInterval & (int)DayOfWeekFlags.Monday) == (int)DayOfWeekFlags.Monday);
                    this.WeeklyOnTuesday = ((frequencyInterval & (int)DayOfWeekFlags.Tuesday) == (int)DayOfWeekFlags.Tuesday);
                    this.WeeklyOnWednesday = ((frequencyInterval & (int)DayOfWeekFlags.Wednesday) == (int)DayOfWeekFlags.Wednesday);
                    this.WeeklyOnThursday = ((frequencyInterval & (int)DayOfWeekFlags.Thursday) == (int)DayOfWeekFlags.Thursday);
                    this.WeeklyOnFriday = ((frequencyInterval & (int)DayOfWeekFlags.Friday) == (int)DayOfWeekFlags.Friday);
                    this.WeeklyOnSaturday = ((frequencyInterval & (int)DayOfWeekFlags.Saturday) == (int)DayOfWeekFlags.Saturday);
                    this.WeeklyOnSunday = ((frequencyInterval & (int)DayOfWeekFlags.Sunday) == (int)DayOfWeekFlags.Sunday);
                    break;
                }
                case 3:
                {
                    this.RecurrenceOption = ScheduleRecurrence.Monthly;
                    this.MonthlyDayOption = (DayOfWeekFlags)frequencyInterval;
                    this.MonthlyOnSpecificDays = (frequencyInterval == (int)DayOfWeekFlags.None);
                    if (this.MonthlyOnSpecificDays)
                    {
                        this.MonthlyDaysExpression = frequencyRelativeInterval;
                        this.MonthlyWeekNumber = WeekOfMonth.First;
                        this.MonthlyDayOption = DayOfWeekFlags.Any;
                    }
                    else
                    {
                        this.MonthlyWeekNumber = (WeekOfMonth)(int.Parse(frequencyRelativeInterval));
                    }
                    this.MonthlyInterval = (frequencyRecurrenceFactor.HasValue ? frequencyRecurrenceFactor.Value : 1);
                    if (this.MonthlyOnSpecificDays && this.MonthlyDaysExpression == "1,16" && this.MonthlyInterval == 1)
                    {
                        this.RecurrenceOption = ScheduleRecurrence.SemiMonthly;
                    }
                    break;
                }
                case 4:
                {
                    this.RecurrenceOption = ScheduleRecurrence.Once;
                    this.OnceDateTime = startDate.GetValueOrDefault(DateTime.Now);
                    this.OnceNow = !startDate.HasValue;
                    break;
                }
            }
        }

        public DateTime? GetNextRunDate(DateTime source)
        {
            switch (this.RecurrenceOption)
            {
                case ScheduleRecurrence.Once:
                {
                    if (this.OnceNow)
                    {
                        return DateTime.Now;
                    }
                    else
                    {
                        return this.OnceDateTime;
                    }
                }
                case ScheduleRecurrence.Daily:
                {
                    return GetNextRunForDailySchedule(source);
                }
                case ScheduleRecurrence.Monthly:
                {
                    return GetNextRunForMonthlySchedule(source);
                }
                case ScheduleRecurrence.SemiMonthly:
                {
                    var nextMonth = new DateTime(source.Year, source.Month, 1).AddMonths(1);
                    var possibleDates = new DateTime?[] 
					{
						new DateTime(source.Year, source.Month, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds),
						new DateTime(source.Year, source.Month, 16, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds),
						new DateTime(nextMonth.Year, nextMonth.Month, 1, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds),
						new DateTime(nextMonth.Year, nextMonth.Month, 16, this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds)
					};
                    return (from d in possibleDates where d > source orderby d select d).FirstOrDefault();
                }
                case ScheduleRecurrence.Weekly:
                {
                    return GetNextRunForWeeklySchedule(source);
                }
                default:
                {
                    return null;
                }
            }
        }        

        private DateTime? GetNextRunForWeeklySchedule(DateTime source)
        {
            //get all possible run times for the current week
            DateTime firstDayOfSourceWeek = source.Date;
            while (firstDayOfSourceWeek.DayOfWeek != DayOfWeek.Sunday)
            {
                firstDayOfSourceWeek = firstDayOfSourceWeek.AddDays(-1);
            }
            DateTime[] daysInSchedule = new DateTime[7];
            DateTime currentDayOfWeek = firstDayOfSourceWeek;
            for (int i = 0; i < daysInSchedule.Length; i++)
            {
                daysInSchedule[i] = new DateTime(
                        currentDayOfWeek.Year,
                        currentDayOfWeek.Month,
                        currentDayOfWeek.Day,
                        this.StartTime.Hours,
                        this.StartTime.Minutes,
                        this.StartTime.Seconds);
                currentDayOfWeek = currentDayOfWeek.AddDays(1);
            }

            //get the next run
            DateTime? nextRun =
                    (from d in daysInSchedule.Cast<DateTime?>()
                     orderby d
                     where d.HasValue
                     where d.Value > source
                     where IsDayIncluded(d.Value)
                     select d)
                    .FirstOrDefault();

            if (!nextRun.HasValue)
            {
                //increment all days to the next scheduled week (according to interval)
                nextRun =
                        (from d in daysInSchedule.Cast<DateTime?>()
                         orderby d
                         where d.HasValue
                         let nextIntervalDay = d.Value.AddDays(7 * Math.Max(this.WeeklyInterval, 1))
                         where IsDayIncluded(nextIntervalDay)
                         select nextIntervalDay)
                         .FirstOrDefault();
            }
            return nextRun;
        }

        private bool IsDayIncluded(DateTime dateTime)
        {
            int? dayMask = this.GetFrequencyInterval();
            if (!dayMask.HasValue)
            {
                dayMask = (int)DayOfWeekFlags.None;
            }
            DayOfWeekFlags flags = (DayOfWeekFlags)Enum.Parse(typeof(DayOfWeekFlags), dateTime.DayOfWeek.ToString());
            return (flags & (DayOfWeekFlags)dayMask.Value) != 0;
        }

        private DateTime? GetNextRunForMonthlySchedule(DateTime source)
        {
            List<int> daysOfMonth = new List<int>();
            DateTime nextMonth = new DateTime(source.Year, source.Month, 1).AddMonths(Math.Max(this.MonthlyInterval, 1));
            if (this.MonthlyOnSpecificDays)
            {
                #region handle the "Day(s) X of every Y months" option

                //parse the days expression                
                string[] parts = this.MonthlyDaysExpression.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length; i++)
                {
                    string[] range = parts[i].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (range.Length == 1)
                    {
                        daysOfMonth.Add(int.Parse(range[0].Trim()));
                    }
                    else if (range.Length == 2)
                    {
                        int start = int.Parse(range[0].Trim());
                        int end = int.Parse(range[1].Trim());
                        for (int j = start; j < end + 1; j++)
                        {
                            daysOfMonth.Add(j);
                        }
                    }
                }

                //get next run of source month
                var possibleNextRun =
                        (from d in daysOfMonth
                         where d <= DateTime.DaysInMonth(source.Year, source.Month)
                         select new DateTime(
                                 source.Year,
                                 source.Month,
                                 d,
                                 this.StartTime.Hours,
                                 this.StartTime.Minutes,
                                 this.StartTime.Seconds));
                DateTime? nextRun =
                        (from d in possibleNextRun.Cast<DateTime?>()
                         orderby d
                         where d > source
                         select d)
                         .FirstOrDefault();


                //the following loop handle a case where the user enters a day 
                //that may break the schedule (e.g. the 31st of every month)
                if (!nextRun.HasValue && nextMonth.Month != source.Month)
                {
                    return GetNextRunForMonthlyScheduleFromNextMonth(source, daysOfMonth, nextMonth);
                }
                return nextRun;

                #endregion
            }
            else
            {
                #region handle the "First/Second/... Day/Weekday/Monday... of every X months" option

                //get a list of all days of the source month, grouped by day
                var daysInMonth = new DateTime[DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)];
                for (int i = 0; i < daysInMonth.Length; i++)
                {
                    daysInMonth[i] = new DateTime(
                            nextMonth.Year,
                            nextMonth.Month,
                            i + 1,
                            this.StartTime.Hours,
                            this.StartTime.Minutes,
                            this.StartTime.Seconds);
                }

                if (this.MonthlyWeekNumber == WeekOfMonth.Last)
                {
                    //if searching for last, need to order descending
                    DateTime? nextRun =
                            (from d in daysInMonth
                             orderby d descending
                             where IsDayIncluded(d)
                             select d)
                             .FirstOrDefault();
                    return nextRun;
                }
                else
                {
                    //otherwise, we need to sort ascending, skip X number of records, 
                    //and return the first one
                    DateTime? nextRun =
                            (from d in daysInMonth
                             orderby d ascending
                             where IsDayIncluded(d)
                             select d)
                             .Skip((int)this.MonthlyWeekNumber - 1)
                             .FirstOrDefault();
                    return nextRun;
                }

                //var possibleNextRunByWeek =
                //    from d in daysInMonth
                //    where IsDayIncluded(d)
                //    group d by (int)(d.Day / 7) + 1 into groupByWeek
                //    select groupByWeek;

                //DateTime? nextRun =
                //    (from week in possibleNextRunByWeek
                //     orderby week.Key descending
                //     where week.Key <= (int)this.MonthlyWeekNumber
                //     from day in week                     
                //     select day)
                //     .FirstOrDefault();

                //return nextRun;

                #endregion
            }
        }

        private DateTime? GetNextRunForMonthlyScheduleFromNextMonth(DateTime source, List<int> daysOfMonth, DateTime nextMonth)
        {
            DateTime nextNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1).AddMonths(1);

            //get next run of source month
            var possibleNextRun =
                    (from d in daysOfMonth
                     where d <= DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month)
                     select new DateTime(
                             nextMonth.Year,
                             nextMonth.Month,
                             d,
                             this.StartTime.Hours,
                             this.StartTime.Minutes,
                             this.StartTime.Seconds));
            DateTime? nextRun =
                    (from d in possibleNextRun.Cast<DateTime?>()
                     orderby d
                     where d > source
                     select d)
                     .FirstOrDefault();

            if (!nextRun.HasValue && nextMonth.Month != source.Month)
            {
                return GetNextRunForMonthlyScheduleFromNextMonth(source, daysOfMonth, nextNextMonth);
            }
            return nextRun;
        }

        private DateTime? GetNextRunForDailySchedule(DateTime source)
        {
            DateTime nextRun = new DateTime(
                    source.Year,
                    source.Month,
                    source.Day,
                    this.StartTime.Hours,
                    this.StartTime.Minutes,
                    this.StartTime.Seconds)
                    .AddDays(this.DailyInterval);
            while (!this.IsDayIncluded(nextRun))
            {
                //just increment one day at a time
                nextRun = nextRun.AddDays(1);
            }
            return nextRun;
        }

        public string GetFrequencyRelativeInterval()
        {
            switch (this.RecurrenceOption)
            {
                case ScheduleRecurrence.Monthly: return this.MonthlyOnSpecificDays ? this.MonthlyDaysExpression : ((int)this.MonthlyWeekNumber).ToString();
                case ScheduleRecurrence.SemiMonthly: return "1,16";
                default: return null;
            }
        }

        public int? GetFrequencyRecurrenceFactor()
        {
            switch (this.RecurrenceOption)
            {
                case ScheduleRecurrence.Daily: return this.DailyInterval;
                case ScheduleRecurrence.Hourly: return this.HourlyInterval;
                case ScheduleRecurrence.Monthly: return this.MonthlyInterval;
                case ScheduleRecurrence.SemiMonthly: return 1;
                case ScheduleRecurrence.Weekly: return this.WeeklyInterval;
                default: return 0;
            }
        }

        public int? GetFrequencyInterval()
        {
            switch (this.RecurrenceOption)
            {
                case ScheduleRecurrence.Daily:
                return (int)(this.DailyWeekdaysOnly ? DayOfWeekFlags.WeekDays : DayOfWeekFlags.Any);
                case ScheduleRecurrence.SemiMonthly:
                return (int)DayOfWeekFlags.None;
                case ScheduleRecurrence.Monthly:
                DayOfWeekFlags monthlyDayOfWeek = DayOfWeekFlags.None;
                if (!this.MonthlyOnSpecificDays)
                {
                    monthlyDayOfWeek = this.MonthlyDayOption;
                }
                return (int)monthlyDayOfWeek;
                case ScheduleRecurrence.Weekly:
                DayOfWeekFlags weeklyDayOfWeek = DayOfWeekFlags.None;
                if (this.WeeklyOnMonday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Monday;
                }
                if (this.WeeklyOnTuesday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Tuesday;
                }
                if (this.WeeklyOnWednesday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Wednesday;
                }
                if (this.WeeklyOnThursday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Thursday;
                }
                if (this.WeeklyOnFriday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Friday;
                }
                if (this.WeeklyOnSaturday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Saturday;
                }
                if (this.WeeklyOnSunday)
                {
                    weeklyDayOfWeek = weeklyDayOfWeek | DayOfWeekFlags.Sunday;
                }
                return (int)weeklyDayOfWeek;
                default:
                return (int)DayOfWeekFlags.Any;
            }
        }

        public int? GetFrequencyId(bool once = false)
        {
            if (once)
                return 4;
            else
                switch (this.RecurrenceOption)
                {
                    case ScheduleRecurrence.Daily: return 1;
                    case ScheduleRecurrence.Weekly: return 2;
                    case ScheduleRecurrence.Monthly:
                    case ScheduleRecurrence.SemiMonthly: return 3;
                    case ScheduleRecurrence.Once: return 4;
                    default:
                    return null;
                }
        }
    }
}
