using System;
using System.Collections.Generic;
namespace Emdat.InVision
{
    public interface IReportParametersView
    {
        string ReportId { get; set; }
        ReportingEnvironmentEnum ReportingEnvironmentId { get; set; }
        List<Emdat.InVision.SSRS.ParameterValue> Parameters { get; set; }        
    }
}
