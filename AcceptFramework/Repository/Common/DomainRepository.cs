using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.Common
{
     internal class DomainRepository
    {

        public static IEnumerable<AcceptFramework.Domain.Common.Domain> GetAll()
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Domain>().Select();
        }

        public static AcceptFramework.Domain.Common.Domain Insert(AcceptFramework.Domain.Common.Domain record)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Domain>().Create(record);
        }


        public static AcceptFramework.Domain.Common.Domain GetDomain(int domainId)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Domain>().Select(a => a.Id == domainId).FirstOrDefault();

        }

        public static AcceptFramework.Domain.Common.Domain UpdateDomain(AcceptFramework.Domain.Common.Domain domain)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Domain>().Update(domain);
        }

        public static IEnumerable<AcceptFramework.Domain.Common.Domain> GetAllByUniverseId(Universe universe)
        {
            return new RepositoryBase<AcceptFramework.Domain.Common.Domain>().Select(a => a.DomainUniverse == universe);
        }
    
    
    
    }
}
