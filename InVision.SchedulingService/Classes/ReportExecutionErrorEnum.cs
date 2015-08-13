using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InVision.SchedulingService
{
    public enum ReportExecutionErrorEnum
    {
        None = 0,
        InvalidReportParameter = 1,
        ExecutionTimeout = 2,
        TooManyRowsForExcel = 3
    }
}
