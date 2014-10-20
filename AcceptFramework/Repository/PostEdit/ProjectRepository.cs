using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal static class ProjectRepository
    {
        public static IEnumerable<Project> GetAll()
        {
            return new RepositoryBase<Project>().Select();
        }

        public static Project Insert(Project record)
        {
           return  new RepositoryBase<Project>().Create(record);
        }


        public static Project GetProject(int projectId)
        {
            return new RepositoryBase<Project>().Select(a => a.Id == projectId).FirstOrDefault();

        }

        public static Project UpdateProject(Project project)
        {
            return new RepositoryBase<Project>().Update(project);
        }


        public static IEnumerable<Project> GetAllByDomain(int domainId)
        {
            return new RepositoryBase<Project>().Select(a => a.ProjectDomain.Id == domainId);
        }


        public static Project GetProjectByAdminToken(string token)
        {
            return new RepositoryBase<Project>().Select(a => a.AdminToken == token).FirstOrDefault();
        }

        public static IEnumerable<Project> GetProjectListByAdminToken(string[] token)
        {
            return new RepositoryBase<Project>().Select(a => token.Contains(a.AdminToken));
        }


    }
}
