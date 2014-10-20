using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Repository.Common;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Business.Common
{
    internal class DomainManager : IDomainManager
    {
        public AcceptFramework.Domain.Common.Domain CreateDomain(string name, int universeId)
        {
            Universe domainUniverse = UniverseRepository.GetUniverse(universeId);
            if (domainUniverse != null)
                return DomainRepository.Insert(new AcceptFramework.Domain.Common.Domain(name, domainUniverse, new List<Language>() { }));
            else
                throw new ArgumentNullException("Universe","Universe is Null");
        }

        public AcceptFramework.Domain.Common.Domain GetDomain(int domainId)
        {
            return DomainRepository.GetDomain(domainId);
        }

        public List<AcceptFramework.Domain.Common.Domain> GetDomainsByUniverseId(int universeId)
        {

            Universe u = UniverseRepository.GetUniverse(universeId);
           
            if (u == null || u.Status != 1)
                throw new ArgumentException("Universe with the ID " + universeId + " not valid", "universeId");

            return DomainRepository.GetAllByUniverseId(u).ToList<AcceptFramework.Domain.Common.Domain>();
        }

        public List<AcceptFramework.Domain.Common.Domain> GetAllDomains()
        {
            return DomainRepository.GetAll().ToList < AcceptFramework.Domain.Common.Domain>();
        }
               
    }
}
