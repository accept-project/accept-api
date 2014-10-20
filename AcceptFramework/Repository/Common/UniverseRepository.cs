using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.Common
{
    internal static class UniverseRepository
    {
        public static IEnumerable<Universe> GetAll()
        {
            return new RepositoryBase<Universe>().Select();
        }

        public static Universe Insert(Universe record)
        {
          return  new RepositoryBase<Universe>().Create(record);
        }


        public static Universe GetUniverse(int universeId)
        {
            return new RepositoryBase<Universe>().Select(a => a.Id == universeId).FirstOrDefault();

        }

        public static Universe UpdateUniverse(Universe universe)
        {
            return new RepositoryBase<Universe>().Update(universe);
        }

       
    }
}
