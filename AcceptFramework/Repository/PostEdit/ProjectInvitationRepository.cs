using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal class ProjectInvitationRepository
    {

        public static IEnumerable<ProjectInvitation> GetAll()
        {
            return new RepositoryBase<ProjectInvitation>().Select();
        }

        public static ProjectInvitation Insert(ProjectInvitation record)
        {
            return new RepositoryBase<ProjectInvitation>().Create(record);
        }


        public static ProjectInvitation GetProject(int projectInvitationId)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.Id == projectInvitationId).FirstOrDefault();

        }

        public static ProjectInvitation UpdateProjectInvitation(ProjectInvitation project)
        {
            return new RepositoryBase<ProjectInvitation>().Update(project);
        }


        public static IEnumerable<ProjectInvitation> GetAllByConfirmationCode(string confirmationCode)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.ConfirmationCode == confirmationCode);
        }

        public static IEnumerable<ProjectInvitation> GetAllByProjectId(int projectId)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.ProjectId == projectId);
        }

        public static IEnumerable<ProjectInvitation> GetAllByUserName(string userName)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.UserName == userName);
        }

        public static ProjectInvitation GetProjecInvitationtByConfirmationCode(string projectInvitationCode)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.ConfirmationCode == projectInvitationCode).FirstOrDefault();

        }


        public static ProjectInvitation GetProjectInvitationByUserName(string userName)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.UserName == userName).FirstOrDefault();

        }

        public static ProjectInvitation GetNextValidProjectInvitationByUserName(string userName)
        {
            return new RepositoryBase<ProjectInvitation>().Select(a => a.UserName == userName && a.ConfirmationCode != string.Empty && a.ConfirmationDate != (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue).FirstOrDefault();

        }

    }
}
