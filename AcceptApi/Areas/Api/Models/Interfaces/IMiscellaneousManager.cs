using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.Interfaces
{
    public interface IMiscellaneousManager
    {
        CoreApiResponse UpdateExternalUsageItem(string externalUsageId, int usageCount, string metadata);
        CoreApiResponse CreateExternalUsageItem(string externalUsageId, int usageCount, string metadata);
        CoreApiResponse GetExternalUsageCount(string externalUsageId);
        CoreApiResponse GetExternalUsageReport(string externalUsageId, DateTime start, DateTime end, string format);
    }
}