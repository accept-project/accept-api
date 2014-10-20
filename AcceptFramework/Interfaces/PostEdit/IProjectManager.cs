using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Interfaces.PostEdit
{
    public interface IProjectManager
    {
        Project CreateProject(string name, int domainId, int status);
        
        Project GetProject(int projectId);

        List<Project> GetProjectsByDomain(int domainId);

        List<Project> GetAllProjects();

        List<Project> GetDemoProjectsByAdminTokens(string[] adminToken);

        Document AddDocument(Document document, int projectId);

        Document GetDocumentByDocumentId(int documentId);

        Document GetDocumentByTextId(string textId);

        Project UpdateProjectStatus(int projectId, int status);       

        Project CreateProject(string name, int domainId, int status, List<string> questions, int sourceLangId, int targetLangId, string projOwner, int InterfaceConfigId, DateTime creationDate, List<string> options, int transOptions, string emailBodyMessage, string surveyLink, string projectOrganization, int customInterfaceConfig, bool isExternalProject, bool isSingleRevision, TimeSpan maxThreshold, int paraphrasingMode, int interactiveCheck, string paraMeta, string interCheckMeta);

        ProjectInvitation[] GenerateInvitations(string[] emails, int projectId, string uniqueRoleName, out string projectName);

        ProjectInvitation GetProjectInvite(string code);

        ProjectInvitation GetProjectInviteByUserName(string userName);

        ProjectInvitation UpdateProjectInvitationConfirmationCode(string code);

        List<Project> GetProjectsByUser(string userName, string[] demoProjectsTokens);

        void DeleteDocument(string textId, string userId);

        ProjectInvitation UpdateProjectInvitationConfirmationDate(string code);

        List<ProjectInvitation> GetProjectInvitationsByUserName(string userName);

        void AddUserToProject(string userName, int projectId);
        
        void AddUserToProject(string userName, string projectAdminToken);

        ProjectInvitation UpdateInvitation(ProjectInvitation projectInvitation);

        List<UserProjectRole> GetUserInProject(Project project);

        List<Document> GetAllDocumentByProject(Project project);

        Project UpdateProject(Project project);

        Project GetProjectWithCompleteDocuments(int projectId);

        List<ProjectInvitation> GetInvitationsByProject(int projectId);

        UserProjectRole GetRoleInProjectByProjectAndUser(Project project, User user);

        #region ExternalProject
        
        ExternalUserProjectRole GetRoleInExternalProjectByProjectAndUser(Project project, ExternalPostEditUser user);

        List<ExternalUserProjectRole> GetExternalUserInProject(Project project);

        Project GetProjectByProjectAdminToken(string token);

        #endregion



    }
}
