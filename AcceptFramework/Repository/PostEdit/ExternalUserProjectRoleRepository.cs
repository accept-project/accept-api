using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal static class ExternalUserProjectRoleRepository
    {


        public static IEnumerable<ExternalUserProjectRole> GetAll()
        {
            return new RepositoryBase<ExternalUserProjectRole>().Select();
        }

        public static ExternalUserProjectRole Insert(ExternalUserProjectRole record)
        {
            return new RepositoryBase<ExternalUserProjectRole>().Create(record);
        }

        public static ExternalUserProjectRole UpdateExternalUserProjectRole(ExternalUserProjectRole record)
        {
            return new RepositoryBase<ExternalUserProjectRole>().Update(record);
        }


        public static ExternalUserProjectRole GetUserProjectRoleByExternalUserAndProject(ExternalPostEditUser user, Project project)
        {
            return new RepositoryBase<ExternalUserProjectRole>().Select(a => a.ExternalUser == user && a.Project == project).FirstOrDefault();

        }

        public static IEnumerable<ExternalUserProjectRole> GetUserProjectRoleByExternalUser(ExternalPostEditUser externalUser)
        {
            return new RepositoryBase<ExternalUserProjectRole>().Select(a => a.ExternalUser == externalUser);

        }

        public static IEnumerable<ExternalUserProjectRole> GetExternalUserProjectRoleByProject(Project project)
        {
            return new RepositoryBase<ExternalUserProjectRole>().Select(a => a.Project == project);
        }

        public static void Delete(ExternalUserProjectRole record)
        {
            new RepositoryBase<ExternalUserProjectRole>().Delete(record);
        }

    
    
    
    }
}
