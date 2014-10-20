using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Repository.Common;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Business.Common
{
    internal class UniverseManager: IUniverseManager
    {
      
          public Universe  CreateUniverse(string name)
          {             
             return UniverseRepository.Insert(new Universe(name));             
          }

          public Universe GetUniverse(int universeId)
          {
              return UniverseRepository.GetUniverse(universeId);
          }

          public List<Universe> GetAllUniverse()
          {
              return UniverseRepository.GetAll().ToList<Universe>();
          }
        
    
    }
}
