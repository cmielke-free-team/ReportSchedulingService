using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InVisionMvc.Infrastructure
{
    public static class TimeZoneInfoExtensions
    {
        /// <summary>
        /// Converts the time from one time zone to another.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="sourceTimeZoneId">The source time zone id.</param>
        /// <param name="destinationTimeZoneId">The destination time zone id.</param>        
        /// <returns></returns>
        /// <remarks>This method handles invalid dates by adjusting them to the approriate time.</remarks>
        public static DateTime SafeConvertTimeBySystemTimeZoneId(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            //find the source and destination time zones
            TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId);

            //make sure the specified date is valid in the source time zone
            //An invalid time falls within a range of times for the current time zone that cannot be mapped to Coordinated Universal Time (UTC) 
            //due to the application of an adjustment rule. Typically, invalid times occur when the time moves ahead for daylight saving time.            
            DateTime inDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            if (sourceTimeZone.IsInvalidTime(inDateTime))
            {
                //apply the correct adjustment rule to the datetime
                TimeZoneInfo.AdjustmentRule adjustmentRule =
                    (from rule in sourceTimeZone.GetAdjustmentRules()
                     where inDateTime >= rule.DateStart &&
                         inDateTime <= rule.DateEnd
                     select rule).First();
                inDateTime = inDateTime.Add(adjustmentRule.DaylightDelta);
            }

            //convert datetime to destination time zone                    
            return TimeZoneInfo.ConvertTime(inDateTime, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        /// Determines whether [is time zone id valid] [the specified time zone id].
        /// </summary>
        /// <param name="timeZoneId">The time zone id.</param>
        /// <returns>
        /// 	<c>true</c> if [is time zone id valid] [the specified time zone id]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTimeZoneIdValid(string timeZoneId)
        {
            try
            {
                TimeZoneInfo.ClearCachedData();
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return (0 == string.Compare(tzi.Id, timeZoneId, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}