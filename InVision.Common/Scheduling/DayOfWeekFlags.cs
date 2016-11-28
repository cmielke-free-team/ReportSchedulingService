using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    /// <summary>
    /// DayOfWeekFlags enum
    /// </summary>
    [Flags]
    public enum DayOfWeekFlags : int
    {
        None = 0,
        Sunday = 2,
        Monday = 4,
        Tuesday = 8,
        Wednesday = 16,
        Thursday = 32,
        Friday = 64,
        Saturday = 128,
        Any = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday,
        WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
        WeekendDays = Sunday | Saturday
    }
}
