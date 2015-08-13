using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InVision.SchedulingService
{
    public class ReportExecutionException : InvalidOperationException
    {
        public ReportExecutionErrorEnum ErrorCode {get; private set;}

        public ReportExecutionException(ReportExecutionErrorEnum errorCode, string message) 
            : base(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}
