using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision
{
    public interface IDefaultView
    {
        ReportingApplication CurrentReportingApplication { get; }
        int CurrentUserId { get; }
        int? CurrentCompanyId { get; }
		int? CurrentClientId { get; }
        string CurrentUserName { get; }
        string LocalTimeZoneIdentifier { get; }
        string ServerTimeZoneIdentifier { get; }        
    }
}
