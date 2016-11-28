using System;
namespace Emdat.InVision
{
    public interface ISchedule
    {
        int DailyInterval { get; set; }
        bool DailyWeekdaysOnly { get; set; }
        TimeSpan EndTime { get; set; }
        int HourlyInterval { get; set; }
        DayOfWeekFlags MonthlyDayOption { get; set; }
        string MonthlyDaysExpression { get; set; }
        int MonthlyInterval { get; set; }
        bool MonthlyOnSpecificDays { get; set; }
        WeekOfMonth MonthlyWeekNumber { get; set; }
        DateTime OnceDateTime { get; set; }
        bool OnceNow { get; set; }
        ScheduleRecurrence RecurrenceOption { get; set; }
        TimeSpan StartTime { get; set; }
        int WeeklyInterval { get; set; }
        bool WeeklyOnFriday { get; set; }
        bool WeeklyOnMonday { get; set; }
        bool WeeklyOnSaturday { get; set; }
        bool WeeklyOnSunday { get; set; }
        bool WeeklyOnThursday { get; set; }
        bool WeeklyOnTuesday { get; set; }
        bool WeeklyOnWednesday { get; set; }
    }
}
