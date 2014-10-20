using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Interfaces;
using AcceptFramework.Business;
using AcceptApi.Areas.Api.Models.PostEdit;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.PostEditAudit;
using AcceptFramework.Interfaces.PostEditAudit;
using System.Net;
using AcceptApi.Areas.Api.Models.Utils;
using System.Web.Configuration;
using AcceptFramework.Domain.Common;
using AcceptFramework.Interfaces.Evaluation;

namespace AcceptApi.Areas.Api.Models.Managers
{
    public class AdminManager: IAdminManager
    {
        private IAcceptApiServiceLocator _acceptServiceLocator;
        private IUniverseManager _universeManagerService;
        private IDomainManager _domainManagerService;
        private IProjectManager _projectManagerService;
        private IPostEditAuditManager _postEditManager;       
        private IUserManager _userManager;
        private IEvaluationProjectManager _evaluationManager;
                             
        public AdminManager() 
        {
            _acceptServiceLocator = new AcceptApiServiceLocator();
            _universeManagerService = _acceptServiceLocator.GetUniverseManagerService();
            _domainManagerService = _acceptServiceLocator.GetDomainManagerService();
            _projectManagerService = _acceptServiceLocator.GetProjectManagerService();
            _userManager = _acceptServiceLocator.GetUserManagerService();
            _postEditManager = _acceptServiceLocator.GetPostEditAuditManagerService();
            _evaluationManager = _acceptServiceLocator.GetEvaluationProjectManagerService();
        }

        #region Properties

        public IDomainManager DomainManagerService
        {
            get { return _domainManagerService; }
        }

        public IUniverseManager UniverseManagerService
        {
            get { return _universeManagerService; }
        }

        public IProjectManager ProjectManagerService
        {
            get { return _projectManagerService; }
        }

        public IUserManager UserManager
        {
            get { return _userManager; }
        }

        public IPostEditAuditManager PostEditManager
        {
            get { return _postEditManager; }
        }
        
        #endregion

        #region Universes

        public CoreApiResponse CreateUniverse(string name)
        {
            try
            {
                return new CoreApiCustomResponse(UniverseManagerService.CreateUniverse(name));
                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message,"CreateUniverse");        
            }

          
        }

        public CoreApiResponse GetUniverse(int universeId)
        {
            try
            {                             
               return new CoreApiCustomResponse(UniverseManagerService.GetUniverse(universeId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message,"GetUniverse");        
            }
        
        }

        public CoreApiResponse GetAllUniverse()
        {
            try
            {
                return new CoreApiCustomResponse(UniverseManagerService.GetAllUniverse());
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllUniverse");
            }

        }
        
        #endregion

        #region Domains

        public CoreApiResponse CreateDomain(string name, int universeId)
        {

            try
            {
                 return new CoreApiCustomResponse(DomainManagerService.CreateDomain(name,universeId));
                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateDomain");        
            }

        }

        public CoreApiResponse GetDomain(int domainId)
        {
            try
            {
                return new CoreApiCustomResponse(DomainManagerService.GetDomain(domainId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetDomain");
            }

        }

        public CoreApiResponse AddUserToDomain(string userName, int domainId, string userRoleinDomain)
        {
            try
            {
                return new CoreApiCustomResponse(UserManager.AddUserToDomain(userName, domainId, userRoleinDomain));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddUserToDomain");
            }

        }

        public CoreApiResponse GetDomainsByUniverse(int universeId)
        {
            try
            {
                return new CoreApiCustomResponse(DomainManagerService.GetDomainsByUniverseId(universeId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetDomainsByUniverse");
            }

        }

        public CoreApiResponse GetAllDomains()
        {
            try
            {
                return new CoreApiCustomResponse(DomainManagerService.GetAllDomains());
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllDomains");
            }

        }
        
        #endregion

        #region Projects

        public CoreApiResponse CreateProject(string name, int domainId, int status)
        {
            try
            {
                 return new CoreApiCustomResponse(ProjectManagerService.CreateProject(name,domainId,status));
               
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateProject");
            }

        }

        public CoreApiResponse CreateProject(ProjectRequestObject project)
        {
            try
            {
                Project newProject = ProjectManagerService.CreateProject(project.name, project.domainId, project.status, project.projectQuestions, project.sourceLangId, project.targetLangId, project.projectOwner, project.interfaceConfig, project.creationDate, project.projectOptions, project.translationOptions ,project.emailMessage, project.surveyLink, project.projectOrganization, project.customInterfaceConfiguration, project.isExternalProject,project.isSingleRevision,TimeSpan.Parse(project.maxThreshold),project.paraphrasingMode,project.interactiveCheck,project.paraphrasingMetadata,project.interactiveCheckMetadata);

                if (newProject != null)
                    UserManager.AddUserToProject(newProject, newProject.ProjectOwner, newProject.Id, "ProjAdmin");

                return new CoreApiCustomResponse(newProject);

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateProject");
            }
        
        }

        public CoreApiResponse UpdateProject(ProjectRequestObject project)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectWithCompleteDocuments(project.ID);
                                
                if(p == null)
                        return new CoreApiException("Project not found","UpdateProject");

                p.EmailBodyMessage = project.emailMessage;
                p.Name = project.name;
                p.ProjectOrganization = project.projectOrganization;
                p.TranslationOptions = project.translationOptions;
                p.SurveyLink = project.surveyLink;
                p.CustomInterfaceConfiguration = project.customInterfaceConfiguration;
                p.External = project.isExternalProject;
                p.IsSingleRevision = project.isSingleRevision;
                p.MaxThreshold = TimeSpan.Parse(project.maxThreshold);
                #region updating paraprasing and interactive check settings
                p.ParaphrasingMetadata = project.paraphrasingMetadata;
                p.ParaphrasingMode = project.paraphrasingMode;
                p.InteractiveCheck = project.interactiveCheck;
                p.InteractiveCheckMetadata = project.interactiveCheckMetadata;
                #endregion
                if (ProjectManagerService.GetInvitationsByProject(project.ID).Count > 0 && p.ProjectDocuments != null && p.ProjectDocuments.Count > 0)
                {
                    ProjectManagerService.UpdateProject(p);                    
                    return new CoreApiResponse(CoreApiResponseStatus.Incomplete, DateTime.UtcNow, string.Empty);
                }
                else
                {                                       
                    p.InterfaceConfiguration = project.interfaceConfig;

                    if (p.ProjectQuestion == null || p.ProjectQuestion.ToArray<ProjectQuestion>().Length == 0)
                    {
                        p.ProjectQuestion = new List<ProjectQuestion>();
                        foreach (string question in project.projectQuestions)                        
                            p.ProjectQuestion.Add(new ProjectQuestion(question));                        
                    }
                    else
                    {
                        if (project.projectQuestions != null && project.projectQuestions.Count > 0)
                            p.ProjectQuestion.ToArray<ProjectQuestion>()[0].Question = project.projectQuestions[0];
                    }

                    p.ProjectEditOptions = new List<PostEditOption>();
                    foreach (string option in project.projectOptions)
                    {
                        p.ProjectEditOptions.Add(new PostEditOption(option));
                    }
                }
               
                ProjectManagerService.UpdateProject(p);
                return new CoreApiResponse();


            }
            catch (Exception e)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new CoreApiException(e.Message, "UpdateProject");
            }
        
        }

        public CoreApiResponse IsProjectStarted(int projectId)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectWithCompleteDocuments(projectId);

                if (p == null)
                    return new CoreApiException("Project not found", "UpdateProject");

                if (ProjectManagerService.GetInvitationsByProject(projectId).Count > 0 && p.ProjectDocuments != null && p.ProjectDocuments.Count > 0)
                    return new CoreApiCustomResponse(true);

                return new CoreApiCustomResponse(false);   
            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "CheckIfProjectStarted");
            }
        
        
        }

        public CoreApiResponse GetProject(int projectId)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetProject(projectId));
                                                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProject");
            }

        }

        public CoreApiResponse GetAllProjects()
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetAllProjects());
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllProjects");
            }

        }

        public CoreApiResponse GetDemoProjects()
        {
            try
            {
                string[] demoProjectsTokens = {AcceptApiCoreUtils.AcceptFrenchToEnglishPostEditDemoProjectToken, 
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptFrenchToEnglishCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanCollaborativePostEditDemoProjectToken};

                return new CoreApiCustomResponse(ProjectManagerService.GetDemoProjectsByAdminTokens(demoProjectsTokens));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetAllProjects");
            }

        }


     
        public CoreApiResponse RemoveUserFromProject(string userName, string userRoleInProject, string token)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectByProjectAdminToken(token);

                if (p == null || token == null)
                    throw new ArgumentException("Invalid Project Token", "token");

                if (p.External)
                {
                    ExternalPostEditUser eu = UserManager.GetExternalPostEditUserByUserName(userName);
                    if (eu == null)
                        throw new ArgumentException("Invalid User", "userName");

                    UserManager.RemoveUserFromPostEditProject(p,null,eu);
                }
                else
                {
                    User u = UserManager.GetUserByUserName(userName);
                    if (u == null)
                        throw new ArgumentException("Invalid User", "userName");

                    UserManager.RemoveUserFromPostEditProject(p, u, null);
                }

                return new CoreApiResponse(CoreApiResponseStatus.Ok, DateTime.UtcNow);


            }
            catch (Exception e)
            {
                
                throw(e);
            }
        
        }
        
        /// <summary>
        /// adds post edit users to projects.
        /// </summary>
        /// <param name="userName">user name to add.</param>
        /// <param name="userRoleInProject">the role in the project currently is set by default to "ProjUser" in the controller.</param>
        /// <param name="token">project admin token.</param>
        /// <returns></returns>    
        public CoreApiResponse AddUserToProject(string userName, string userRoleInProject, string token)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectByProjectAdminToken(token);

                if (p == null || token == null)
                    throw new ArgumentException("Invalid Project Token", "token");

                if (p.External)   
                    UserManager.AddExternalUserToExternalProject(p, userName, p.Id, userRoleInProject);
                else
                {
                    User u = UserManager.GetUserByUserName(userName);
                    if(u == null)
                        throw new ArgumentException("Invalid User", "userName");
                    
                    InviteUsersToProjects(new string[] { "" }, p.Id, userRoleInProject, p.ProjectOwner);                                                                              
                }
                    
                return new CoreApiResponse(CoreApiResponseStatus.Ok, DateTime.UtcNow);

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddUserToProject");
            }

        
        }

        public CoreApiResponse GetProjectGeneralInfo(string token)
        { 
            try
            {
                Project p = ProjectManagerService.GetProjectByProjectAdminToken(token);
                List<object[]> projectInfo; projectInfo = new List<object[]>();
                List<object> usersInProject = new List<object>();
                List<object> docsInProject = new List<object>();

                if (p == null || token == null)
                    throw new ArgumentException("Invalid Project Token", "token");

                if (!p.External)
                {                   
                    p.ProjectDocuments = ProjectManagerService.GetAllDocumentByProject(p);
                    foreach (Document doc in p.ProjectDocuments)
                        docsInProject.Add(doc.text_id);

                    List<UserProjectRole> usersRolesInProject = ProjectManagerService.GetUserInProject(p);
                    foreach (UserProjectRole roleinProject in usersRolesInProject)
                    {
                        if (roleinProject.User.UserName != p.ProjectOwner && !roleinProject.User.IsDeleted)
                            usersInProject.Add(roleinProject.User.UserName);
                    }
                
                }
                else
                {
                    p.ProjectDocuments = ProjectManagerService.GetAllDocumentByProject(p);
                    foreach (Document doc in p.ProjectDocuments)
                        docsInProject.Add(doc.text_id);

                    List<ExternalUserProjectRole> usersRolesInProject = ProjectManagerService.GetExternalUserInProject(p);
                    foreach (ExternalUserProjectRole roleinProject in usersRolesInProject)
                    {
                        if (roleinProject.ExternalUser.ExternalUserName != p.ProjectOwner && !roleinProject.ExternalUser.isDeleted)
                            usersInProject.Add(roleinProject.ExternalUser.ExternalUserName);
                    }
                }

                projectInfo.Add(docsInProject.ToArray());
                projectInfo.Add(usersInProject.ToArray());
                return new CoreApiCustomResponse(projectInfo);                           
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddUserToProject");
            }
        }

        private bool InviteUsersToProjects(string[] emails, int projectId, string uniqueRoleName, string projectOwner)
        {
            try
            {
                List<string> emailsFailed = new List<string>();
                List<string> emailsSent = new List<string>();
                string projectName = string.Empty;
                string invitationLanguage = string.Empty;
                string projectInvitationUrl = string.Empty;

                Project project = ProjectManagerService.GetProject(projectId);
                if (project == null && project.Status == -1)
                    throw new Exception("Project is null");

                if (project.TargetLanguage.Code.StartsWith("fr"))
                {
                    invitationLanguage = "fr";
                    projectInvitationUrl = AcceptApiCoreUtils.AcceptPortalProjectInvitationAddressFrench;

                }
                else
                    if (project.TargetLanguage.Code.StartsWith("de"))
                    {
                        invitationLanguage = "de";
                        projectInvitationUrl = AcceptApiCoreUtils.AcceptPortalProjectInvitationAddressGerman;
                    }
                    else
                    {
                        projectInvitationUrl = AcceptApiCoreUtils.AcceptPortalProjectInvitationAddress;
                    }
              
                ProjectInvitation[] projectInvitationList = ProjectManagerService.GenerateInvitations(emails, projectId, "ProjUser", out projectName);
                foreach (ProjectInvitation projInvite in projectInvitationList)
                {
                    try
                    {
                        Utils.EmailManager.SendInvitationEmail(AcceptApiCoreUtils.AcceptPortalEmailFrom, projInvite.UserName, projectName, projectInvitationUrl + projInvite.ConfirmationCode, invitationLanguage, project.EmailBodyMessage, project.ProjectOwner);
                        emailsSent.Add(projInvite.UserName);
                    }
                    catch (Exception)
                    {
                        emailsFailed.Add(projInvite.UserName);
                    }

                }

                try
                {
                    Utils.EmailManager.SendNotificationEmailToProjectOwnerForExistingUser(projectName, emailsFailed, emailsSent, projectOwner, AcceptApiCoreUtils.AcceptPortalEmailFrom, invitationLanguage);
                }
                catch (Exception e)
                {
                    throw (e);
                }

               return true;
            }
            catch (Exception e)
            {
                throw(e);
            }                
        }

        public CoreApiResponse GetUsersInProject(string token, string role)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectByProjectAdminToken(token);

                if (p == null || token == null)
                    throw new ArgumentException("Invalid Project Token", "token");

                if (p.External)
                {
                    List<ExternalPostEditUser> allUsersInProject = UserManager.GetAllExternalPostEditUserInProject(p);
                    return new CoreApiCustomResponse(allUsersInProject);
                }
                else
                {
                    List<User> allUsersInProject = UserManager.GetAllPostEditUserInProject(p);
                    return new CoreApiCustomResponse(allUsersInProject);
                }

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddUserToProject");
            }


        }

        public CoreApiResponse GetProjectsByDomain(int domainId)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetProjectsByDomain(domainId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProjectsByDomain");
            }
        }

        public CoreApiResponse UpdateProjectStatus(int projectId, int status)
        {

            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.UpdateProjectStatus(projectId,status));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateProjectStatus");
            }
        
        }

        /// <summary>
        /// Waht to do with External Users ? I mean they don't have an account makes no sense to see the projects
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CoreApiResponse GetProjectsByUser(string userName)
        {
            try
            {
                string[] demoProjectsTokens = {AcceptApiCoreUtils.AcceptFrenchToEnglishPostEditDemoProjectToken, 
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptFrenchToEnglishCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanCollaborativePostEditDemoProjectToken};


                return new CoreApiCustomResponse(ProjectManagerService.GetProjectsByUser(userName, demoProjectsTokens));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProjectsByUser");
            }                
        }

        public CoreApiResponse SendFeedbackEmail(string userName, string userEmail, string feedbackLink, string feedbackMessage, string subject)
        {
            try
            {

                EmailManager.SendFeedbackEmail(AcceptApiCoreUtils.AcceptPortalEmailFrom, WebConfigurationManager.AppSettings["FeedbackEmailRecipients"].Split(';'), userName, userEmail, feedbackLink, feedbackMessage, subject);
                return new CoreApiResponse();
            }
            catch (Exception e)
            {                
                 return new CoreApiException(e.Message, "SendFeedbackEmail");
            }
        }

        public CoreApiResponse UserRolePostEditProject(string userName, int projectId, string role)
        {
            try
            {
                AcceptFramework.Domain.Common.User user = UserManager.GetUserByUserName(userName);
                Project project = ProjectManagerService.GetProject(projectId);

                if (user == null)
                    throw new Exception("User not found.");

                if (project == null)
                    throw new Exception("Project not found.");

                UserProjectRole userRoleinProject = ProjectManagerService.GetRoleInProjectByProjectAndUser(project, user);

                return new CoreApiCustomResponse(userRoleinProject.Role.UniqueName);
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProjectsByUser");
            }                        
        
        }

        #endregion

        #region Users

        public CoreApiResponse GetAllUsers()
        {
            try
            {
                return new CoreApiCustomResponse(UserManager.GetAll());
            }
            catch (Exception e)
            {                
                throw(e);
            }
        
        }

        #endregion

        #region Evaluation
        public IEvaluationProjectManager EvaluationManager
        {
            get { return _evaluationManager; }
        }
        #endregion

        #region Init
        public CoreApiResponse InitAceept()
        {
            try
            {
                UserManager.GenerateUserRoles();                
                EvaluationManager.GenerateLanguages();

                return new CoreApiCustomResponse("The ACCEPT system was successfully initialized.");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "InitAceept");        
            }

          
        }        
        #endregion
    }
}