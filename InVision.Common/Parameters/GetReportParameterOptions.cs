using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    [Flags]
    public enum GetReportParameterOptions
    {
        IncludeVisibleParameters = 1,
        IncludeHiddenParameters = 2,
        IncludeAllParameters = IncludeVisibleParameters | IncludeHiddenParameters
    }
}
