using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.Miscellaneous;
using AcceptFramework.Domain.Miscellaneous;
using AcceptFramework.Repository.Miscellaneous;

namespace AcceptFramework.Business.Miscellaneous
{
    internal class MiscellaneousManager : IMiscellaneousManagerService
    {
        public ExternalUsage CreateExternalUsage(string externalInstanceIdentifier, int usageCount, DateTime lastUpdate, string metadata)
        {
            ExternalUsage newExternalUsage = new ExternalUsage();
            newExternalUsage.InstanceIdentifier = externalInstanceIdentifier;
            newExternalUsage.UsageCount = usageCount;
            newExternalUsage.LastUpdate = lastUpdate;
            newExternalUsage.Metadata = metadata;
            return ExternalUsageRepository.Insert(newExternalUsage);
        }

        public ExternalUsage CreateExternalUsage(ExternalUsage newExternalUsage)
        {

          return  ExternalUsageRepository.Insert(newExternalUsage);
        }


        public ExternalUsage UpdateExternalUsageCount(ExternalUsage externalUsage)
        {

            return ExternalUsageRepository.UpdateExternalUsage(externalUsage);
        }

        public ExternalUsage GetExternalUsageItem(string externalUsageIdentifier)
        {
            return ExternalUsageRepository.GetExternalUsage(externalUsageIdentifier);
        }

        public List<ExternalUsage> GetAllExternalUsageItemByUsageExternalUsageIdentifier(string externalUsageIdentifier)
        {
            return ExternalUsageRepository.GetAllExternalUsagesByItemId(externalUsageIdentifier).ToList<ExternalUsage>();
        }

        public List<ExternalUsage> GetAllExternalUsageItemByUsageExternalUsageIdentifierRange(string externalUsageIdentifier, DateTime start, DateTime end)
        {
            return ExternalUsageRepository.GetAllExternalUsagesByItemIdLastUpdateRange(externalUsageIdentifier, start, end).ToList<ExternalUsage>();
        }

    

    }
}

