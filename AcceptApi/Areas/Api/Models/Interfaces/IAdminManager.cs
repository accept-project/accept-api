using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.PostEdit;

namespace AcceptApi.Areas.Api.Models.Interfaces
{
    public interface IAdminManager
    {
        CoreApiResponse CreateUniverse(string name);

        CoreApiResponse GetUniverse(int universeId);

        CoreApiResponse CreateDomain(string name, int universeId);

        CoreApiResponse GetDomain(int domainId);

        CoreApiResponse CreateProject(string name, int domainId, int status);

        CoreApiResponse GetProject(int projectId);

        #region External Project Methods

        CoreApiResponse AddUserToProject(string userName, string userRole, string token);

        CoreApiResponse GetProjectGeneralInfo(string token);

        #endregion

        CoreApiResponse AddUserToDomain(string userName, int domainId, string userRoleinDomain);

        CoreApiResponse GetAllUniverse();

        CoreApiResponse GetDomainsByUniverse(int universeId);

        CoreApiResponse GetProjectsByDomain(int domainId);

        CoreApiResponse GetAllProjects();

        CoreApiResponse GetDemoProjects();

        CoreApiResponse GetAllDomains();

        CoreApiResponse UpdateProjectStatus(int projectId, int status);

        CoreApiResponse GetAllUsers();

        CoreApiResponse CreateProject(ProjectRequestObject project);

        CoreApiResponse GetProjectsByUser(string userName);

        CoreApiResponse UpdateProject(ProjectRequestObject project);

        CoreApiResponse IsProjectStarted(int projectId);

        CoreApiResponse SendFeedbackEmail(string userName, string userEmail, string feedbackLink, string feedbackMessage, string subject);

        CoreApiResponse UserRolePostEditProject(string userName, int projectId, string role);

        CoreApiResponse RemoveUserFromProject(string userName, string userRoleInProject, string token);

        CoreApiResponse GetUsersInProject(string token, string role);

        CoreApiResponse InitAccept();

    }
}
