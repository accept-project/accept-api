using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.Common
{
    internal class RolesRepository
    {
        public static IEnumerable<Role> GetAll()
        {
            return new RepositoryBase<Role>().Select();
        }

        public static Role Insert(Role record)
        {
            return new RepositoryBase<Role>().Create(record);
        }


        public static Role GetRole(int roleId)
        {
            return new RepositoryBase<Role>().Select(a => a.Id == roleId).FirstOrDefault();

        }

        public static Role GetRole(string uniqueName)
        {
            return new RepositoryBase<Role>().Select(a => a.UniqueName == uniqueName).FirstOrDefault();

        }


        public static Role UpdateRole(Role role)
        {
            return new RepositoryBase<Role>().Update(role);
        }
    
    }
}
