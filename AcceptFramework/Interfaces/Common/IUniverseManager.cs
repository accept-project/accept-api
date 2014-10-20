using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Interfaces.Common
{
    public interface IUniverseManager
    {
        Universe GetUniverse(int universeId);

        Universe CreateUniverse(string name);

        List<Universe> GetAllUniverse();


    
    }
}
