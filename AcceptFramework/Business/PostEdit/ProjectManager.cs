using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Repository.PostEdit;
using AcceptFramework.Repository.Common;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Repository.Evaluation;
using AcceptFramework.Business.Common;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Business.PostEdit
{
    internal class ProjectManager: IProjectManager
    {

        public Project CreateProject(string name, int domainId, int status)
        {
            AcceptFramework.Domain.Common.Domain projectDomain = DomainRepository.GetDomain(domainId);
            if(projectDomain != null)
                return ProjectRepository.Insert(new Project(name, projectDomain, status, new List<Document>(), new List<ProjectQuestion>(), new EvaluationLanguage(), new EvaluationLanguage(), string.Empty, -1, DateTime.UtcNow, new List<PostEditOption>(), 0, string.Empty, string.Empty, string.Empty, 0, false, CreateProjectAdminToken(name),false,new TimeSpan(),paraphrasingMode:0,interactiveCheck:0,interactiveCheckMetadata:null, paraphrasingMetadata:null));
            else 
                throw new ArgumentNullException("Domain", "Domain is Null.");
        }

        public Project CreateProject(string name, int domainId, int status,List<string> questions, int sourceLangId, int targetLangId, string projOwner, int InterfaceConfigId, DateTime creationDate, List<string> options, int transOptions, string emailBodyMessage, string surveyLink, string projectOrganization, int customInterfaceConfig, bool isExternalProject, bool isSingleRevision, TimeSpan maxThreshold, int paraphrasingMode, int interactiveCheck, string paraMeta, string interCheckMeta)
        {        
                AcceptFramework.Domain.Common.Domain projectDomain = DomainRepository.GetDomain(domainId);
                if (projectDomain != null)
                {
                    EvaluationLanguage sourceLanguage = EvaluationLanguagesRepository.Get(sourceLangId);
                    EvaluationLanguage targetLanguage = EvaluationLanguagesRepository.Get(targetLangId);
                    if (sourceLanguage == null || targetLanguage == null)
                        throw new ArgumentNullException("Language", "Language is Null");

                    User user = UserRepository.GetUserByUserName(projOwner);
                    if (user == null || user.IsDeleted)
                        throw new ArgumentNullException("User", "User is Null or Deleted");

                    Project p = new Project(name, projectDomain, status, new List<Document>(), new List<ProjectQuestion>(), sourceLanguage, targetLanguage, projOwner, InterfaceConfigId, creationDate, new List<PostEditOption>(), transOptions, emailBodyMessage, surveyLink, projectOrganization, customInterfaceConfig, isExternalProject, CreateProjectAdminToken(name), isSingleRevision, maxThreshold, paraphrasingMode, interactiveCheck, interCheckMeta, paraMeta);

                    foreach (string question in questions)
                        p.ProjectQuestion.Add(new ProjectQuestion(question));

                    foreach (string option in options)
                        p.ProjectEditOptions.Add(new PostEditOption(option));
                   
                    p = ProjectRepository.Insert(p);
                    UserProjectRole userProjectRole = new UserProjectRole(user, RolesRepository.GetRole("ProjAdmin"), p);
                    UserProjectRoleRepository.Insert(userProjectRole);
                    return p;
                
                }
                else
                    throw new ArgumentNullException("Domain", "Domain is Null");            
        }

        public Project GetProject(int projectId)
        {           
            Project p = ProjectRepository.GetProject(projectId);        
            p.ProjectDocuments = DocumentRepository.GetAllByProjectId(p).ToList<Document>();
            if (p.AdminToken == null || p.AdminToken == string.Empty)
            {
                p.AdminToken = CreateProjectAdminToken(p.Name + p.Id.ToString());
                ProjectRepository.UpdateProject(p);            
            }
            foreach (Document d in p.ProjectDocuments)
            {
                d.Project = null;                
                //fix lazy loading timeout.
                d.DocumentRevisions = null;
            }                        
            return p;
        }

        public Project UpdateProject(Project project)
        {                  
            return ProjectRepository.UpdateProject(project);          
        }

        public Project GetProjectWithCompleteDocuments(int projectId)
        {          
            Project p = ProjectRepository.GetProject(projectId);
            p.ProjectDocuments = DocumentRepository.GetAllByProjectId(p).ToList<Document>();          
            return p;
        }
        
        public List<Project> GetProjectsByDomain(int domainId)
        {           
            List<Project> projects =  ProjectRepository.GetAllByDomain(domainId).ToList<Project>();           
            foreach (Project p in projects)
                p.ProjectDocuments = new List<Document>();
            return projects;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = ProjectRepository.GetAll().ToList<Project>();           
            foreach (Project p in projects)
                p.ProjectDocuments = new List<Document>();
            return projects;        
        }

        public List<Project> GetDemoProjectsByAdminTokens(string[] adminToken)
        {
            List<Project> projects = ProjectRepository.GetProjectListByAdminToken(adminToken).ToList<Project>();            
            foreach (Project p in projects)
                p.ProjectDocuments = new List<Document>();
            return projects;
        }

        public Document AddDocument(Document document, int projectId)
        {
            Project p = ProjectRepository.GetProject(projectId);
            if (p == null || p.Status != 1)
                throw new ArgumentNullException("Project is null or not active.","projectId");
           
            #region Single Revision
                        
            if (p.IsSingleRevision != null && p.IsSingleRevision)
            {
                document.IsSingleRevision = true;
                document.UniqueReviewerId = p.ProjectOwner;
            }
            else           
                document.IsSingleRevision = false;
                       
            #endregion

            document.Validate();
            document.Project = p;
            //allow duplicated ids being added under different projects by adding the prefix project id.
            document.text_id = document.text_id + "_proj" + p.Id; 
            Document newDocument = DocumentRepository.Insert(document);
            p.ProjectDocuments = DocumentRepository.GetAllByProjectId(p).ToList<Document>();            
            p.ProjectDocuments.Add(document);
            ProjectRepository.UpdateProject(p);

            return newDocument;
        }

        public void DeleteDocument(string textId, string userId)
        {

            Document d = DocumentRepository.GetDocumentByTextId(textId);
            if (d == null)
                throw new Exception("Document not found");

            d.Project.ProjectDocuments = DocumentRepository.GetAllByProjectId(d.Project).ToList<Document>();

            for (int i = 0; i < d.Project.ProjectDocuments.Count; i++)
                if (d.Project.ProjectDocuments.ToArray()[i].text_id == textId)
                    d.Project.ProjectDocuments.Remove(d.Project.ProjectDocuments.ToArray()[i]);
           
            ProjectRepository.UpdateProject(d.Project);

            if (d.Project.ProjectOwner == userId)
                DocumentRepository.DeleteDocument(d);
            else
                throw new Exception("User name not the document owner");          
        }

        public Document GetDocumentByDocumentId(int documentId)
        {
            return DocumentRepository.GetDocument(documentId);
        }

        public Document GetDocumentByTextId(string textId)
        {
            return DocumentRepository.GetDocumentByTextId(textId);
        }

        public Project UpdateProjectStatus(int projectId, int status)
        {
            Project p = ProjectRepository.GetProject(projectId);
            p.Status = status;
            p  = ProjectRepository.UpdateProject(p);
                
            p.ProjectDocuments = new List<Document>();
            return p;
        }

        public ProjectInvitation[] GenerateInvitations(string[] emails, int projectId, string uniqueRoleName, out string projectName)
        {

            Project p = ProjectRepository.GetProject(projectId);
            
            if (p != null && p.Status == 1)
            {

                projectName = p.Name;

                List<ProjectInvitation> newUsersList = new List<ProjectInvitation>();

                foreach (string email in emails)
                {

                    if (Utils.StringUtils.EmailValidator(email.Trim()))
                    {
                        User u = UserRepository.GetUserByUserName(email.Trim());

                        if (u != null)
                        {
                            //existing user: just associate him to the project.
                            UserProjectRole userProjectRole = UserProjectRoleRepository.GetUserProjectRoleByUserAndProject(u, p);
                            if (userProjectRole == null)
                            {
                                //means the user as no connection to the project at all.
                                userProjectRole = new UserProjectRole(u, RolesRepository.GetRole(uniqueRoleName), p);
                                UserProjectRoleRepository.Insert(userProjectRole);
                                ProjectInvitation projectInvitation = new ProjectInvitation();
                                projectInvitation.UserName = email.Trim();
                                projectInvitation.ProjectId = p.Id;
                                projectInvitation.ConfirmationCode = Utils.StringUtils.Generate32CharactersStringifiedGuid(); 
                                projectInvitation.InvitationDate = DateTime.UtcNow;                                
                                projectInvitation.Type = 1;
                                ProjectInvitationRepository.Insert(projectInvitation);
                                newUsersList.Add(projectInvitation);                                
                            }
                            else
                            {
                                //what to do if the user already exists? Currently nothing...                                                          
                            }

                        }
                        else
                        {
                            //new user goes to invitation list.
                            ProjectInvitation projectInvitation = new ProjectInvitation();
                            projectInvitation.UserName = email.Trim();
                            projectInvitation.ProjectId = p.Id;
                            projectInvitation.ConfirmationCode = Utils.StringUtils.Generate32CharactersStringifiedGuid();                            
                            projectInvitation.InvitationDate = DateTime.UtcNow;
                            projectInvitation.Type = 2;
                            ProjectInvitationRepository.Insert(projectInvitation);
                            newUsersList.Add(projectInvitation);
                        }

                    }

                }


                return newUsersList.ToArray();

            }

            projectName = string.Empty;
            return new ProjectInvitation[] { }; ;
        
        }

        public ProjectInvitation GetProjectInvite(string code)
        {
            return ProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
        
        }

        public ProjectInvitation GetProjectInviteByUserName(string userName)
        {            
            return ProjectInvitationRepository.GetNextValidProjectInvitationByUserName(userName);
        }

        public ProjectInvitation UpdateProjectInvitationConfirmationCode(string code)
        {
           ProjectInvitation projInvitation = ProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
           User u = UserRepository.GetUserByUserName(projInvitation.UserName);
           Project p = ProjectRepository.GetProject(projInvitation.ProjectId);
           UserProjectRole userProjectRole = new UserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);           
           UserProjectRoleRepository.Insert(userProjectRole);          
           projInvitation.ConfirmationCode = string.Empty;
           return ProjectInvitationRepository.UpdateProjectInvitation(projInvitation);
        }

        public ProjectInvitation UpdateProjectInvitationConfirmationDate(string code)
        {
            ProjectInvitation projInvitation = ProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
            projInvitation.ConfirmationCode = string.Empty;     
            projInvitation.ConfirmationDate = DateTime.UtcNow;
            return ProjectInvitationRepository.UpdateProjectInvitation(projInvitation);
        }

        public List<ProjectInvitation> GetInvitationsByProject(int projectId)
        {
           return ProjectInvitationRepository.GetAllByProjectId(projectId).ToList<ProjectInvitation>();
        }

        public ProjectInvitation UpdateInvitation(ProjectInvitation projectInvitation)
        {
            return ProjectInvitationRepository.UpdateProjectInvitation(projectInvitation);          
        }

        public void AddUserToProject(string userName, int projectId)
        {
            User u = UserRepository.GetUserByUserName(userName);
            if (u == null)
                throw new Exception("User is null.");

            Project p = ProjectRepository.GetProject(projectId);
            if (p == null)
                throw new Exception("Project is null.");

            UserProjectRole userProjectRole = new UserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);
            UserProjectRoleRepository.Insert(userProjectRole);           
        }

        public void AddUserToProject(string userName, string projectAdminToken)
        {
            User u = UserRepository.GetUserByUserName(userName);
            if (u == null)
                throw new Exception("User is null.");

            Project p = ProjectRepository.GetProjectByAdminToken(projectAdminToken);
            if (p == null)
                throw new Exception("Project is null.");

            UserProjectRole userProjectRole = new UserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);

            UserProjectRoleRepository.Insert(userProjectRole);
        }

        public List<ProjectInvitation> GetProjectInvitationsByUserName(string userName)
        {
            return ProjectInvitationRepository.GetAllByUserName(userName).ToList<ProjectInvitation>();
        }

        public List<Project> GetProjectsByUser(string userName, string[] demoProjectsTokens)
        {
            List<Project> userProjects = new List<Project>();
            User u = UserRepository.GetUserByUserName(userName);
            
            if(u == null)
                throw new ArgumentNullException("User", "User is Null");

            List<UserProjectRole> userProjectRoles = UserProjectRoleRepository.GetUserProjectRoleByUser(u).ToList<UserProjectRole>();
            userProjectRoles.RemoveAll(r => demoProjectsTokens.Contains(r.Project.AdminToken));
         
            if (userProjectRoles != null && userProjectRoles.Count > 0)
            { 
            
                foreach(UserProjectRole userProjectRole in userProjectRoles)
                {
                    if(userProjectRole.Project.Status == 1)
                    {
                        userProjectRole.Project.ProjectDocuments = new List<Document>();
                        userProjects.Add(userProjectRole.Project);
                    }
                }            
            }

            return userProjects;
        }

        public List<UserProjectRole> GetUserInProject(Project project)
        {
            return UserProjectRoleRepository.GetUserProjectRoleByProject(project).ToList<UserProjectRole>();        
        }

        public List<Document> GetAllDocumentByProject(Project project)
        {
            return DocumentRepository.GetAllByProjectId(project).ToList<Document>();
        }

        public UserProjectRole GetRoleInProjectByProjectAndUser(Project project, User user)
        {
            return UserProjectRoleRepository.GetUserProjectRoleByUserAndProject(user, project);
        }

        #region ExternalProject

        public ExternalUserProjectRole GetRoleInExternalProjectByProjectAndUser(Project project, ExternalPostEditUser user)
        {
            return ExternalUserProjectRoleRepository.GetUserProjectRoleByExternalUserAndProject(user, project);
        }

        public List<ExternalUserProjectRole> GetExternalUserInProject(Project project)
        {
            return ExternalUserProjectRoleRepository.GetExternalUserProjectRoleByProject(project).ToList<ExternalUserProjectRole>();
        }

        public Project GetProjectByProjectAdminToken(string token)
        {
            return ProjectRepository.GetProjectByAdminToken(token);
        }


        #endregion

        private string CreateProjectAdminToken(string projectName)
        {
            return Utils.StringUtils.GenerateTinyHash(projectName + DateTime.UtcNow.ToString());
        }
    }
}
