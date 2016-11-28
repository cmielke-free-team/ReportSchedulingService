using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    /// <summary>
    /// RecurrenceOption enum
    /// </summary>
    public enum ScheduleRecurrence : int
    {
        /// <summary>
        /// Once
        /// </summary>
        Once = 0,
        /// <summary>
        /// Hourly
        /// </summary>
        Hourly = 1,
        /// <summary>
        /// Daily
        /// </summary>
        Daily = 2,
        /// <summary>
        /// Weekly
        /// </summary>
        Weekly = 3,
        /// <summary>
        /// Semi-monthly
        /// </summary>
        SemiMonthly = 4,
        /// <summary>
        /// Monthly
        /// </summary>
        Monthly = 5
    }    
}
