using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Miscellaneous;

namespace AcceptFramework.Repository.Miscellaneous
{
    internal static class ExternalUsageRepository
    {


        public static IEnumerable<ExternalUsage> GetAll()
        {
            return new RepositoryBase<ExternalUsage>().Select();
        }

        public static ExternalUsage Insert(ExternalUsage record)
        {
            return new RepositoryBase<ExternalUsage>().Create(record);
        }

        public static ExternalUsage UpdateExternalUsage(ExternalUsage externalUsage)
        {
            return new RepositoryBase<ExternalUsage>().Update(externalUsage);
        }

        public static ExternalUsage GetExternalUsage(string instanceIdentifier)
        {
            return new RepositoryBase<ExternalUsage>().Select(a => a.InstanceIdentifier == instanceIdentifier).FirstOrDefault();
        }

        public static IEnumerable<ExternalUsage> GetAllExternalUsagesByItemId(string usageIdentifier)
        {
            return new RepositoryBase<ExternalUsage>().Select(a => a.InstanceIdentifier == usageIdentifier);
        }

        public static IEnumerable<ExternalUsage> GetAllExternalUsagesByItemIdLastUpdateRange(string usageIdentifier, DateTime start, DateTime end)
        {
            return new RepositoryBase<ExternalUsage>().Select(a => a.InstanceIdentifier == usageIdentifier && a.LastUpdate >= start && a.LastUpdate <= end);            
        }

    }
}
