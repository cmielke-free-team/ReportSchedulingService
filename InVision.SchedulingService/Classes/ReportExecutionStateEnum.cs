using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InVision.SchedulingService
{
    public enum ReportExecutionStateEnum
    {
        New = 1,
        Inactive = 2,
        Idle = 3,
        Queued = 4,
        Running = 5, 
        Failed = 6,
        Succeeded = 7
    }
}
