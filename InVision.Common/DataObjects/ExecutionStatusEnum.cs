using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Emdat.InVision
{
    /// <summary>
    /// 
    /// </summary>    
    public enum ExecutionStatusEnum
    {
        None = 0,
        New = 1,
        Inactive = 2,
        Idle = 3,
        Queued = 4,
        Running = 5,
        Failed = 6,
        Succeeded = 7
    }
}
