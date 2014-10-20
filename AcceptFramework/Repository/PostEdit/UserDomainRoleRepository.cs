using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.PostEdit
{
    internal static class UserDomainRoleRepository
    {

        public static IEnumerable<UserDomainRole> GetAll()
        {
            return new RepositoryBase<UserDomainRole>().Select();
        }

        public static UserDomainRole Insert(UserDomainRole record)
        {
            return new RepositoryBase<UserDomainRole>().Create(record);
        }

        public static UserDomainRole UpdateUserDomainRole(UserDomainRole record)
        {
            return new RepositoryBase<UserDomainRole>().Update(record);
        }


        public static UserDomainRole GetUserDomainRoleByUserAndDomain(User user, Domain.Common.Domain domain)
        {
            return new RepositoryBase<UserDomainRole>().Select(a => a.User == user && a.Domain == domain).FirstOrDefault();
        }


        public static IEnumerable<UserDomainRole> GetUserDomainRoleByUser(User user)
        {
            return new RepositoryBase<UserDomainRole>().Select(a => a.User == user).ToList<UserDomainRole>();

        }

    }
}
