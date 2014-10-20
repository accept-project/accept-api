using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.PostEdit
{
    internal class UserProjectRoleRepository
    {
        public static IEnumerable<UserProjectRole> GetAll()
        {
            return new RepositoryBase<UserProjectRole>().Select();
        }

        public static UserProjectRole Insert(UserProjectRole record)
        {
            return new RepositoryBase<UserProjectRole>().Create(record);
        }

        public static void Delete(UserProjectRole record)
        {
          new RepositoryBase<UserProjectRole>().Delete(record);
        }

        public static UserProjectRole UpdateUserProjectRole(UserProjectRole record)
        {
            return new RepositoryBase<UserProjectRole>().Update(record);
        }

        public static UserProjectRole GetUserProjectRoleByUserAndProject(User user, Project project)
        {
            return new RepositoryBase<UserProjectRole>().Select(a => a.User == user && a.Project == project).FirstOrDefault();
        }

        public static IEnumerable<UserProjectRole> GetUserProjectRoleByUser(User user)
        {
            return new RepositoryBase<UserProjectRole>().Select(a => a.User == user);
        }

        public static IEnumerable<UserProjectRole> GetUserProjectRoleByProject(Project project)
        {
            return new RepositoryBase<UserProjectRole>().Select(a => a.Project == project);
        }

    }
}
