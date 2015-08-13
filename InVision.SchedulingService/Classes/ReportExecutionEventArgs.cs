using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emdat.InVision;
using Emdat.Diagnostics;
using System.Diagnostics;
using System.Net;
using Emdat;

namespace InVision.SchedulingService
{
    public class ReportExecutionEventArgs : EventArgs
    {
        public ReportExecution Job { get; private set; }

        public ReportExecutionEventArgs(ReportExecution job)
            : base()
        {
            this.Job = job;
        }
    }
}