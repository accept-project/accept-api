using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Miscellaneous;

namespace AcceptFramework.Interfaces.Miscellaneous
{
    public interface IMiscellaneousManagerService
    {
        ExternalUsage CreateExternalUsage(string externalInstanceIdentifier, int usageCount, DateTime lastUpdate, string metadata);
        ExternalUsage UpdateExternalUsageCount(ExternalUsage externalUsage);
        ExternalUsage GetExternalUsageItem(string externalUsageIdentifier);
        ExternalUsage CreateExternalUsage(ExternalUsage newExternalUsage);
        List<ExternalUsage> GetAllExternalUsageItemByUsageExternalUsageIdentifier(string externalUsageIdentifier);
        List<ExternalUsage> GetAllExternalUsageItemByUsageExternalUsageIdentifierRange(string externalUsageIdentifier, DateTime start, DateTime end);     
    }
}
