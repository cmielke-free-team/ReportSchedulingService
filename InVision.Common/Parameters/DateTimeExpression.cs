using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Emdat.InVision
{
    public class DateTimeExpression
    {
        public const string DateTimeVariableRegex = @"^\s*(?<expr>(?<var>@Today|@FirstOfMonth|@FirstOfPreviousMonth|@LastOfPreviousMonth|@FirstOfQuarter|@FirstOfPreviousQuarter|@LastOfPreviousQuarter)(?<extra>(\s*(?<op>[+-])\s*(?<num>\d+)))?\s*$)";
        //public const string DateTimeVariableRegex = @"^\s*(?<expr>(?<var>@Today|@FirstOfMonth|@FirstOfPreviousMonth|@LastOfPreviousMonth|@FirstOfQuarter|@FirstOfPreviousQuarter|@LastOfPreviousQuarter)(?<extra>(\s*(?<op>[+-])\s*(?<num>\d+)))?\s*$)|((?<static>\d?\d/\d?\d/\d\d\d\d( \d?\d:\d\d:\d\d (AM|PM))?)\s*$)";
        public static string InvariantDateTimeFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        public static string[] ValidDateTimeFormats = new string[] 
        {
            InvariantDateTimeFormat,
            "M/d/yyyy",
            "M/d/yyyy H:mm:ss tt"
        };

        /// <summary>
        /// Evaluates the specified expression.
        /// </summary>
        /// <param name="expression">The expression. This is either a static 
        /// date in M/d/yyyy format or a custom date expression in the following 
        /// format: 
        /// 
        /// @Today|@FirstOfMonth|@FirstOfPreviousMonth|@LastOfPreviousMonth +|- X 
        /// where X is a number of days.</param>
        /// <param name="sourceDate">The source date used for calculations.</param>
        /// <returns></returns>
        public static DateTime? Evaluate(string expression, DateTime sourceDate)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            //parse the expression into parts
            Match m = Regex.Match(expression, DateTimeVariableRegex, RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                DateTime tmp;
                if (!DateTime.TryParse(expression.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
                {
                    throw new NotSupportedException(string.Format("The specified expression is not supported: '{0}'", expression));
                }
                return tmp;
            }
            //if (m.Groups["static"].Success)
            //{
            //    //if the expression is a static date time, parse and return
            //    return DateTime.Parse(m.Groups["static"].Value, CultureInfo.InvariantCulture, DateTimeStyles.None);
            //    //return DateTime.ParseExact(m.Groups["static"].Value, new string[] { "M/d/yyyy", "M/d/yyyy h:mm:ss tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
            //}
            else if (m.Groups["expr"].Success)
            {
                //otherwise, if it is a variable, calculate actual value and return
                string var = m.Groups["var"].Value;
                string extra = m.Groups["extra"].Success ? m.Groups["extra"].Value.Trim() : null;
                string op = m.Groups["op"].Success ? m.Groups["op"].Value : null;
                string num = m.Groups["num"].Success ? m.Groups["num"].Value : null;

                DateTime dt = EvaluateVariable(var, sourceDate);
                if (!string.IsNullOrEmpty(extra))
                {
                    if (op == null || num == null)
                    {
                        throw new NotSupportedException(string.Format("The specified expression is not supported: '{0}'", expression));
                    }

                    int days = int.Parse(num);
                    dt = dt.AddDays(op == "+" ? days : 0 - days);
                }
                return dt;
            }
            else
            {
                throw new NotSupportedException(string.Format("The specified expression is not supported: '{0}'", expression));
            }
        }

        private static DateTime EvaluateVariable(string var, DateTime source)
        {
            switch (var.ToUpperInvariant())
            {
                case "@TODAY":
                    return source;
                case "@FIRSTOFMONTH":
                    return new DateTime(source.Year, source.Month, 1);
                case "@FIRSTOFPREVIOUSMONTH":
                    DateTime firstOfPreviousMonth = source.AddMonths(-1);
                    return new DateTime(firstOfPreviousMonth.Year, firstOfPreviousMonth.Month, 1);
                case "@LASTOFPREVIOUSMONTH":
                    DateTime lastOfPreviousMonth = source.AddMonths(-1);
                    return new DateTime(lastOfPreviousMonth.Year, lastOfPreviousMonth.Month, DateTime.DaysInMonth(lastOfPreviousMonth.Year, lastOfPreviousMonth.Month));
                case "@FIRSTOFQUARTER":
                    int currentQuarter = (int)Math.Ceiling((double)source.Month / 3);
                    int firstMonthOfCurrentQuarter = 3 * (currentQuarter - 1) + 1;
                    return new DateTime(source.Year, firstMonthOfCurrentQuarter, 1);
                case "@FIRSTOFPREVIOUSQUARTER":
                    DateTime previousQuarterDate = source.AddMonths(-3);
                    int previousQuarter = (int)Math.Ceiling((double)previousQuarterDate.Month / 3);
                    int firstMonthOfPreviousQuarter = 3 * (previousQuarter - 1) + 1;
                    return new DateTime(previousQuarterDate.Year, firstMonthOfPreviousQuarter, 1);
                case "@LASTOFPREVIOUSQUARTER":
                    int currentQuarterForLast = (int)Math.Ceiling((double)source.Month / 3);
                    int firstMonth = 3 * (currentQuarterForLast - 1) + 1;
                    return new DateTime(source.Year, firstMonth, 1).AddDays(-1);
                default:
                    throw new NotSupportedException(string.Format("The specified variable is not supported: {0}", var));
            }
        }
    }
}
