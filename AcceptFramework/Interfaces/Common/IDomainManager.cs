using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Interfaces.Common
{
    public interface IDomainManager
    {
        AcceptFramework.Domain.Common.Domain CreateDomain(string name, int universeId);

        AcceptFramework.Domain.Common.Domain GetDomain(int domainId);

        List<AcceptFramework.Domain.Common.Domain> GetDomainsByUniverseId(int universeId);

        List<AcceptFramework.Domain.Common.Domain> GetAllDomains();
    
    }
}
