using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Utils;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Interfaces;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Business;
using AcceptFramework.Interfaces.PostEditAudit;
using AcceptFramework.Domain.PostEditAudit;
using AcceptApi.Areas.Api.Models.PostEdit;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using AcceptFramework.Domain.Common;
using AcceptFramework.Interfaces.Common;
using System.Net;
using AcceptApi.Areas.Api.Models.Acrolinx;

namespace AcceptApi.Areas.Api.Models.Managers
{
    public class PostEditManager: IPostEditManager
    {
        private IAcceptApiServiceLocator _acceptServiceLocator;
        private IProjectManager _projectManagerService;
        private IPostEditAuditManager _postEditAuditManager;
        private IUserManager _userManager;
       
        public PostEditManager()
        {
            _acceptServiceLocator = new AcceptApiServiceLocator();
            _projectManagerService = _acceptServiceLocator.GetProjectManagerService();
            _postEditAuditManager = _acceptServiceLocator.GetPostEditAuditManagerService();
            _userManager = _acceptServiceLocator.GetUserManagerService();
        }

        public IProjectManager ProjectManagerService
        {
            get { return _projectManagerService; }
        }
        public IPostEditAuditManager PostEditAuditManager
        {
            get { return _postEditAuditManager; }
        }

        public IUserManager UserManager{get { return _userManager; }}                                

        public CoreApiResponse GetPostEditContent()
        {
            try
            {
                System.IO.StreamReader myFile = new System.IO.StreamReader("c:\\test4.txt");
                string myString = myFile.ReadToEnd();
                myFile.Close();
                Document doc = new Document();
                doc = AcceptApiWebUtils.FromJSON<Document>(new Document(), myString);                                
                return new CoreApiCustomResponse(doc);
                                         
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetPostEditContent");
            }                          
        }

        public Document GetPostEditContentRaw()
        {
            try
            {        
                System.IO.StreamReader myFile = new System.IO.StreamReader("c:\\test3_.txt");
                string myString = myFile.ReadToEnd();
                myFile.Close();
                Document doc = new Document();
                doc = AcceptApiWebUtils.FromJSON<Document>(new Document(), myString);
                return doc;          
                    
            }
            catch (Exception e)
            {
                return new Document();
            }
        }
             
        #region Add Documents 
        public CoreApiResponse AddDocumentToProjectRaw(string jsonRaw, int projectId)
        {
            try
            {               
                Document doc = new Document();

                try
                {
                    doc = AcceptApiWebUtils.FromJSON<Document>(new Document(), jsonRaw);
                }
                catch (Exception ex)
                {
                    return new CoreApiException(ex.Message, "AddDocumentToProjectRaw-InvalidJson");
                }

                try
                {
                    //validate document                    
                    if (doc.tgt_sentences == null)
                        return new CoreApiException("No target exceptions found.", "AddDocumentToProjectRaw-InvalidSourceTarget");

                    if (doc.src_sentences != null && (doc.src_sentences.Count != doc.tgt_sentences.Count))
                        return new CoreApiException("Source and Target sentences length dont match.", "AddDocumentToProjectRaw-InvalidSourceTarget");

                    if (doc.tgt_templates != null && doc.tgt_templates.Count > 0)
                        if((doc.tgt_templates.Count != doc.tgt_sentences.Count))
                            return new CoreApiException("Target Templates and Target sentences length dont match.", "AddDocumentToProjectRaw-InvalidSourceTarget");
                    
                   //remove empty options - TODO

                }
                catch (Exception ex2)
                {
                    return new CoreApiException(ex2.Message, "AddDocumentToProjectRaw-InvalidSourceTarget");
                }

                #region Single Revision
                //try
                //{
                    //if (doc.IsSingleRevision && doc.UniqueReviewerId != null && doc.UniqueReviewerId.Length == 0)
                    //    return new CoreApiException("The unique reviewer ID is not valid.", "AddDocumentToProjectRaw-InvalidUniqueReviewer");                   
                //}
                //catch (Exception ex3)
                //{
                //    return new CoreApiException(ex3.Message, "AddDocumentToProjectRaw-InvalidUniqueReviewer");
                //}
                #endregion

                //avoid dupliaction
                Document d = ProjectManagerService.GetDocumentByTextId(doc.text_id + "_proj" + projectId);
                if (d != null)
                    return new CoreApiException("The document already exists.", "AddDocumentToProjectRaw-DuplicatedTextId");

                ProjectManagerService.AddDocument(doc, projectId);
                return new CoreApiResponse();
                //return new CoreApiCustomResponse(ProjectManagerService.AddDocument(doc, projectId)); /*THIS WAS CRASHING THE RESPONSE - HTTP 500*/

                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddDocumentToProjectRaw");
            }
        }
        public CoreApiResponse AddDocumentToProject(string  token, Document document)
        {
            try
            {
                Project p = ProjectManagerService.GetProjectByProjectAdminToken(token);
                if (p == null)
                    throw new ArgumentException("Invalid Project Token.", "token");

                    //allowing documents to be added to internal projects through the API public method
                    //if (!p.External)
                    //    throw new Exception("Project is not External.");
                try
                {
                    //validate document                    
                    if (document.tgt_sentences == null)
                        return new CoreApiException("No target exceptions found.", "AddDocumentToProject");

                    if (document.src_sentences != null && (document.src_sentences.Count != document.tgt_sentences.Count))
                        return new CoreApiException("Source and target sentences length don't match.", "AddDocumentToProject");

                    if (document.tgt_templates != null && document.tgt_templates.Count > 0)
                        if ((document.tgt_templates.Count != document.tgt_sentences.Count))
                            return new CoreApiException("Target templates and target sentences length don't match.", "AddDocumentToProject");

                    //remove empty options - TODO

                    #region Single Revision
                    //try
                    //{
                    //    if (document.IsSingleRevision && document.UniqueReviewerId != null && document.UniqueReviewerId.Length == 0)
                    //        return new CoreApiException("The unique reviewer ID is not valid.", "AddDocumentToProjectRaw-InvalidUniqueReviewer");
                    //}
                    //catch (Exception ex3)
                    //{
                    //    return new CoreApiException(ex3.Message, "AddDocumentToProjectRaw-InvalidUniqueReviewer");
                    //}
                    #endregion

                    document.tgt_templates = document.tgt_templates.Select(c => { c.markup = HttpUtility.UrlDecode(c.markup, System.Text.Encoding.UTF8); return c; }).ToList();
                    document.tgt_sentences = document.tgt_sentences.Select(c => { c.text = HttpUtility.UrlDecode(c.text, System.Text.Encoding.UTF8); return c; }).ToList();
                    document.src_sentences = document.src_sentences.Select(c => { c.text = HttpUtility.UrlDecode(c.text, System.Text.Encoding.UTF8); return c; }).ToList();
                    document.src_text = HttpUtility.UrlDecode(document.src_text, System.Text.Encoding.UTF8);
                    document.tgt_text = HttpUtility.UrlDecode(document.src_text, System.Text.Encoding.UTF8);

                    document.MtContactEmail = HttpUtility.UrlDecode(document.MtContactEmail, System.Text.Encoding.UTF8);
                    document.MtTool = HttpUtility.UrlDecode(document.MtTool, System.Text.Encoding.UTF8);
                    document.MtToolId = HttpUtility.UrlDecode(document.MtToolId, System.Text.Encoding.UTF8);
                    document.Original = HttpUtility.UrlDecode(document.Original, System.Text.Encoding.UTF8);
                    document.ProductName = HttpUtility.UrlDecode(document.ProductName, System.Text.Encoding.UTF8);
                    document.DataType = HttpUtility.UrlDecode(document.DataType, System.Text.Encoding.UTF8);
                    document.Category = HttpUtility.UrlDecode(document.Category, System.Text.Encoding.UTF8);

                }
                catch (Exception ex2)
                {
                    return new CoreApiException(ex2.Message, "AddDocumentToProject");
                }

                //avoid dupliaction
                Document d = ProjectManagerService.GetDocumentByTextId(document.text_id + "_proj" + p.Id);
                if (d != null)
                    return new CoreApiException("The document already exists", "AddDocumentToProject.");

                ProjectManagerService.AddDocument(document, p.Id);
                return new CoreApiResponse();
                //return new CoreApiCustomResponse(ProjectManagerService.AddDocument(document, projectId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AddDocumentToProject.");
            }
        }       
        #endregion

        #region GetDocument

        public CoreApiResponse GetDocumentByDocumentId(int documentId)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetDocumentByDocumentId(documentId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetDocument");
            }
        }
        public CoreApiResponse GetDocumentByTextId(string textId)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetDocumentByTextId(textId));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetDocument");
            }
        }

        #endregion

        #region DeleteDocument

        public CoreApiResponse DeleteDocument(string textId, string userName)
        {
            try
            {
                ProjectManagerService.DeleteDocument(textId, userName);
                return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "DeleteDocument");
                throw;
            }
        
        
        }

        #endregion
             
        public CoreApiResponse GetDocumentForPostEditByUserId(string textId, string userIdentifier)
        {

            string t = string.Empty;
            try
            {               
                Document document; document = null;
                DateTime? lastBlockDate = null;

                try
                {
                    document = PostEditAuditManager.GetDocumentForPostEditByUserId(textId, userIdentifier, out lastBlockDate);
                    if (document == null)
                        return new CoreApiException("Document not found.", "GetDocumentForPostEditByUserId-I");

                }
                catch (Exception ex)
                {
                    if (ex.Message == "Document revision is currently locked.")
                    {                        
                        return new CoreApiException("Document blocked.", "GetDocumentForPostEditByUserId-II"); 
                    }

                    return new CoreApiException(ex.Message, "GetDocumentForPostEditByUserId - III");                                
                }
               
                DocumentRequestObject documentToReturn = new DocumentRequestObject();
                documentToReturn.category = document.Category;
                documentToReturn.dataType = document.DataType;
                documentToReturn.original = document.Original;
                documentToReturn.productName = document.ProductName;
                
                documentToReturn.sourceLanguage = document.SourceLanguage;
                documentToReturn.targetLanguage = document.TargetLanguage;

                if (documentToReturn.targetLanguage == string.Empty)
                { 
                    if(document.Project.TargetLanguage.Code.StartsWith("en"))
                        documentToReturn.targetLanguage = "en";
                    else
                       if(document.Project.TargetLanguage.Code.StartsWith("fr"))
                           documentToReturn.targetLanguage = "fr";
                            else
                                if(document.Project.TargetLanguage.Code.StartsWith("de"))
                                    documentToReturn.targetLanguage = "de";
                }
                
                documentToReturn.text_id = document.text_id;
                documentToReturn.src_sentences = document.src_sentences;
                documentToReturn.tgt_sentences = document.tgt_sentences;
                documentToReturn.tgt_templates = document.tgt_templates;                
                documentToReturn.tgt_text = document.tgt_text;
                documentToReturn.configurationId = document.Project.InterfaceConfiguration.ToString();

                if (document.Project.ProjectQuestion != null && document.Project.ProjectQuestion.ToArray<ProjectQuestion>().Length > 0)
                {
                    documentToReturn.questionIdentifier = document.Project.ProjectQuestion.ToArray<ProjectQuestion>()[0].Id.ToString();
                    documentToReturn.projectQuestion = document.Project.ProjectQuestion.ToArray<ProjectQuestion>()[0].Question;
                }

                if (document.Project.ProjectEditOptions != null && document.Project.ProjectEditOptions.ToArray<PostEditOption>().Length > 0)
                {
                    foreach (PostEditOption opt in document.Project.ProjectEditOptions.ToList<PostEditOption>())
                         documentToReturn.editOptions.Add(opt.EditOption);                  
                }

                //set to 1 show translation options,  set to 2 hide translation options(in the client side).
                documentToReturn.displayTranslationOptions = document.Project.TranslationOptions;
                documentToReturn.customInterface = document.Project.CustomInterfaceConfiguration;

                #region Segment History
                //this will load all segment revisions for the current user.
                //documentToReturn.targetRevisions = PostEditAuditManager.GetAllTargetSentencesForPostEditDocument(textId, userIdentifier);
                #endregion
              
                #region Single Revision
                Project documentProject = document.Project;
                if (documentProject != null)
                {
                    documentToReturn.isSingleRevisionProject = documentProject.IsSingleRevision;
                    try
                    {
                        documentToReturn.maxThreshold = documentProject.MaxThreshold.ToString();                        
                        documentToReturn.lastBlockDate = lastBlockDate != null ? lastBlockDate.ToString() : string.Empty;                       
                    }
                    catch
                    {
                        documentToReturn.maxThreshold = string.Empty;
                    }
                }
                #endregion               
                
                #region Paraphrasing & Interactive Check

                documentToReturn.interactiveCheckMeta = string.Empty;
                documentToReturn.interactiveCheck = document.Project.InteractiveCheck;
                if (documentToReturn.interactiveCheck == 1 && document.Project.InteractiveCheckMetadata != null)
                {
                    try
                    {
                        AcrolinxCore objAcrolinx = new AcrolinxCore();
                        AcrolinxServerCapabilitiesObject obj = objAcrolinx.GetAcrolinxCapabilityObject(documentToReturn.targetLanguage);                       
                        foreach (RuleSetCapabilities ruleSet in obj.ruleSetCapabilities)
                        {
                            if (ruleSet.name.Trim().ToUpper() == document.Project.InteractiveCheckMetadata.Trim().ToUpper())
                                documentToReturn.interactiveCheckMeta = document.Project.InteractiveCheckMetadata;
                        }
                    }
                    catch { }         
                }
                
                documentToReturn.paraphrasingService = document.Project.ParaphrasingMode;
                documentToReturn.paraphrasingServiceMeta = document.Project.ParaphrasingMetadata;

                #endregion

                return new CoreApiCustomResponse(documentToReturn);
            }
            catch (Exception e)
            {                
                return new CoreApiException(e.Message, "GetDocumentForPostEditByUserId-III");
            }
        
        }

        public CoreApiResponse CompleteDocument(string textId, string userIdentifier, int projectQuestionId, string reply)
       {
           try
           {

               DocumentRevision document = PostEditAuditManager.CompleteDocument(textId, userIdentifier, projectQuestionId, reply);               
               if (document == null)
                   return new CoreApiException("Document not completed", "CompleteDocument");

               //send notification to project manager.
               Document originalDocument = ProjectManagerService.GetDocumentByTextId(textId);
               EmailManager.SendTaskCompletedNotification(AcceptApiCoreUtils.AcceptPortalEmailFrom, originalDocument.Project.ProjectOwner,userIdentifier,string.Format(AcceptApiCoreUtils.AcceptPortalTaskCompleteUrl, userIdentifier, textId),originalDocument.Project.Name,originalDocument.Project.TargetLanguage.Name);
                                            
               return new CoreApiResponse();

           }
           catch (Exception e)
           {
               return new CoreApiException(e.Message, "CompleteDocument");
           }
              
       }

        public CoreApiResponse UpdateRevisionTotalEditingTime(string textId, string userIdentifier, string seconds)
        {
            try
            {
               DocumentRevision document = PostEditAuditManager.UpdateRevisionTotalEditingTime(textId, userIdentifier, seconds);
               if (document == null)
                   return new CoreApiException("Document not completed", "UpdateRevisionTotalEditingTime");

               return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateRevisionTotalEditingTime");                
            }
        
        }

        public CoreApiResponse SaveRevisionPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, Phase phase, List<PhaseCount> phaseCounts)
        {
            try
            {                            
                if(phase.Notes.Count > 0)
                    foreach (PhaseNote n in phase.Notes)
                        n.Note = HttpUtility.UrlDecode(n.Note, System.Text.Encoding.UTF8);

                phase.Date = phase.Date.ToUniversalTime();
                timeStamp = timeStamp.ToUniversalTime();
                phase.PhaseCountGroup = new PhaseCountGroup(index.ToString(), phaseCounts,DateTime.UtcNow);                

                if(phase.PhaseName.StartsWith("r"))
                    PostEditAuditManager.AddTranslationRevisionPhase(userIdentifier, textId, index, timeStamp, state, phaseName, HttpUtility.UrlDecode(source, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(target, System.Text.Encoding.UTF8), phase);
                else
                    if(phase.PhaseName.StartsWith("t"))
                        PostEditAuditManager.AddTranslationRevisionThinkingPhase(userIdentifier, textId, index, timeStamp, state, phaseName, HttpUtility.UrlDecode(source, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(target, System.Text.Encoding.UTF8), new ThinkPhase(phase.PhaseName,phase.ProcessName,phase.Date,phase.JobId,phase.Tool,phase.ToolId,phase.ContactEmail, phase.Notes.ToList(),phase.PhaseCountGroup));

                return new CoreApiResponse();

            }
            catch (Exception e)
            {
                if (e.Message == "HTTP 403 - Document revision is currently locked.")                     
                    return new CoreApiException("Document blocked.", "SaveTranslationRevision.");              
                return new CoreApiException(e.Message, "SaveTranslationRevision");
            }

        }      

        #region XLIFF

        public string GenerateXliffFormat(string textId, string userIdentifier)
        {
            try
            {
                string xliff = PostEditAuditManager.GenerateXliffFormatWithAlternativeTranslations(textId, userIdentifier, AcceptApiCoreUtils.PostEditReportsStartPhaseToolName,
                    AcceptApiCoreUtils.PostEditReportsStartPhaseToolID, AcceptApiCoreUtils.PostEditReportsStartPhaseProcessName);
              return xliff;
            }
            catch (Exception e)
            {
                throw (e);
            }

        }

        public string GenerateProjectOwnerXliff(string projectToken, bool showUserInfo)//string projectOwner, 
        {
            try
            {
                string xliff = PostEditAuditManager.GetProjectOwnerXliff(projectToken, showUserInfo, AcceptApiCoreUtils.PostEditReportsStartPhaseToolName,
                    AcceptApiCoreUtils.PostEditReportsStartPhaseToolID, AcceptApiCoreUtils.PostEditReportsStartPhaseProcessName);
                return xliff;         
            }
            catch
            {
                return string.Empty;
            }        
        }

        public string GenerateTaskRevisionsXliff(string projectToken, string textId)
        {
            try
            {
                string xliff = PostEditAuditManager.GetAllUserRevisionsPerTaskXliff(projectToken, textId, AcceptApiCoreUtils.PostEditReportsStartPhaseToolName,
                    AcceptApiCoreUtils.PostEditReportsStartPhaseToolID, AcceptApiCoreUtils.PostEditReportsStartPhaseProcessName);
                return xliff;         
            }
            catch
            {
                return string.Empty;
            }
        
        }

        public CoreApiResponse GetTaskStatus(string textId)
        {
            try
            {
                Document d = ProjectManagerService.GetDocumentByTextId(textId);
                if (d == null)
                    throw new Exception("Document not found.");


                List<DocumentRevision> allRevisionsForDocument = PostEditAuditManager.GetAllDocumentRevisionsByTextId(textId);
                if (allRevisionsForDocument == null || allRevisionsForDocument.Count == 0)
                {
                    // throw new Exception("Document revision not found.");
                    TaskStatusRequestObject taskStatus = new TaskStatusRequestObject();
                    taskStatus.LastUpdate = string.Empty;
                    taskStatus.Status = 0;
                    taskStatus.TextId = textId;
                    taskStatus.UserId = string.Empty;

                    return new CoreApiCustomResponse(taskStatus);
                }
                else
                {


                    allRevisionsForDocument.OrderBy(x => x.DateLastUpdate);

                    TaskStatusRequestObject taskStatus = new TaskStatusRequestObject();
                    taskStatus.LastUpdate = allRevisionsForDocument[allRevisionsForDocument.Count - 1].DateLastUpdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                    taskStatus.Status = allRevisionsForDocument[allRevisionsForDocument.Count - 1].Status;
                    taskStatus.TextId = textId;
                    taskStatus.UserId = allRevisionsForDocument[allRevisionsForDocument.Count - 1].UserIdentifier;

                    return new CoreApiCustomResponse(taskStatus);
                }
            }
            catch (Exception e)
            {                
                throw(e);
            }
        
        }
               
        public CoreApiResponse GetProjectTaskStatus(int projectId, string token)
        {
            try
            {
                List<TaskStatusRequestObject> taskStatusRequestObjects; taskStatusRequestObjects = new List<TaskStatusRequestObject>();
                Project project;

                project = token != null ? ProjectManagerService.GetProjectByProjectAdminToken(token) : ProjectManagerService.GetProject(projectId);

                if (project == null)                
                  return  token != null ?  new CoreApiException("Project not found, check your project token.", "GetProjectTaskStatus") :  new CoreApiException("Project not found.", "GetProjectTaskStatus") ;

                project.ProjectDocuments = ProjectManagerService.GetAllDocumentByProject(project);


                if (project.External)
                {
                    #region External Projects

                    List<ExternalUserProjectRole> userProjectRoles = ProjectManagerService.GetExternalUserInProject(project);

                    if (userProjectRoles == null)
                        return new CoreApiException("Users Roles in Project are null", "GetProjectTaskStatus");

                    
                    foreach (ExternalUserProjectRole upr in userProjectRoles)
                    {
                      
                        foreach (Document d in project.ProjectDocuments)
                        {
                            DocumentRevision dr = PostEditAuditManager.GetDocumentRevisionByUserIdAndTextId(upr.ExternalUser.ExternalUserName, d.text_id);
                            if (dr == null)
                                taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.ExternalUser.ExternalUserName, 0,string.Empty ));
                            else
                            {
                                if (dr.Status == 2)
                                    taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.ExternalUser.ExternalUserName, 2, dr.CompleteDate != null ? dr.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") : string.Empty));
                                else
                                    taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.ExternalUser.ExternalUserName, 1, dr.DateLastUpdate != null ? dr.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") : string.Empty));
                            }
                        }
                    }

                    return new CoreApiCustomResponse(taskStatusRequestObjects);
                    
                    #endregion
                }
                else
                {
                    #region Non External Projects

                    List<UserProjectRole> userProjectRoles = ProjectManagerService.GetUserInProject(project);

                    if (userProjectRoles == null)
                        return new CoreApiException("Users Roles in Project are null", "GetProjectTaskStatus");

                    foreach (UserProjectRole upr in userProjectRoles)
                    {
                        foreach (Document d in project.ProjectDocuments)
                        {
                            DocumentRevision dr = PostEditAuditManager.GetDocumentRevisionByUserIdAndTextId(upr.User.UserName, d.text_id);
                            if (dr == null)
                                taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.User.UserName, 0, string.Empty));
                            else
                            {
                                if (dr.Status == 2)
                                    taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.User.UserName, 2, dr.DateLastUpdate != null ? dr.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") : string.Empty));
                                else
                                    taskStatusRequestObjects.Add(new TaskStatusRequestObject(d.text_id, upr.User.UserName, 1, dr.CompleteDate != null ? dr.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") : string.Empty));
                            }
                        }
                    }

                    return new CoreApiCustomResponse(taskStatusRequestObjects);

                    #endregion
                }
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message,"GetTaskStatus");
            }                        
        }

        #endregion      

        public CoreApiResponse InviteUsersToProject(string[] emails, int projectId, string uniqueRoleName, string projectOwner)
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
                    return new CoreApiException("Project is null", "InviteUsersToProject");
                
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

                //guest vs projuser                
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
                catch(Exception e) {

                    return new CoreApiException(e.Message, "SendNotificationEmailToProjectOwnerForExistingUser");
                }

                return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "InviteUsersToProject");
            }
        
        }

        public CoreApiResponse GetProjectInvitation(string code)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetProjectInvite(code));
            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "GetProjectInvitation");
                throw;
            }
        
        }

        public CoreApiResponse GetProjectInvitationByUserName(string userName)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.GetProjectInviteByUserName(userName));
            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "GetProjectInvitationByUserName");
                throw;
            }

        }

        public CoreApiResponse UpdateInvitationCode(string code)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.UpdateProjectInvitationConfirmationCode(code));
            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "UpdateInvitationCode");
                throw;
            }

        }

        public CoreApiResponse UpdateInvitationConfirmationDateByCode(string code)
        {
            try
            {
                return new CoreApiCustomResponse(ProjectManagerService.UpdateProjectInvitationConfirmationDate(code));
            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "UpdateInvitationConfirmationDateByCode");
                throw;
            }

        }

        public CoreApiResponse GetProjectDocumentsByUser(int projectId, string userName)
        {         
            try
            {

                Project p = ProjectManagerService.GetProject(projectId);
                bool isInternalUserAndProjectMaintainer = false;

                if (p == null)
                    return new CoreApiException("Project is null", "GetProjectDocumentsByUser");
                else
                {
                    if (p.External && p.ProjectOwner.Trim().ToUpper() != userName.Trim().ToUpper())
                    {
                        #region External User

                        ExternalPostEditUser u = UserManager.GetExternalUserByUserName(userName);
                        if (u == null)
                        {
                            //return new CoreApiException("External User is null", "GetProjectDocumentsByUser");
                            //if user is not in the external users table maybe he is an external project maintainer.
                            User userInternal = UserManager.GetUserByUserName(userName);
                            if(userInternal == null)
                                return new CoreApiException("User is Null", "GetProjectDocumentsByUser");

                            UserProjectRole userProjectRole = ProjectManagerService.GetRoleInProjectByProjectAndUser(p, userInternal);
                            if (userProjectRole == null || userProjectRole.Role.UniqueName.CompareTo("PostEditProjMaintainer") != 0)
                                return new CoreApiException("User is not in Project", "GetProjectDocumentsByUser");

                            //if it reached out this far is an internal user with admin rights in the external project.
                            isInternalUserAndProjectMaintainer = true;
                        }
                        else
                        {
                            ExternalUserProjectRole userProjectRole = ProjectManagerService.GetRoleInExternalProjectByProjectAndUser(p, u);
                            if (userProjectRole == null)
                                return new CoreApiException("External User is not in Project", "GetProjectDocumentsByUser");
                        }

                        #endregion
                    }
                    else
                    {
                        #region User

                        User u = UserManager.GetUserByUserName(userName);
                        if (u == null)
                            return new CoreApiException("User is null", "GetProjectDocumentsByUser");

                        //check if user is in project.
                        UserProjectRole userProjectRole = ProjectManagerService.GetRoleInProjectByProjectAndUser(p, u);
                        if (userProjectRole == null)
                            return new CoreApiException("User is not in Project", "GetProjectDocumentsByUser");                  

                        #endregion
                    }

                    #region Get Project Documents

                    if (userName == p.ProjectOwner || p.ProjectDocuments.Count == 0 || isInternalUserAndProjectMaintainer)
                        return new CoreApiCustomResponse(p);
                    else
                    {
                        int totalProjects = p.ProjectDocuments.Count;
                        int completedProjects = 0;

                        for (int i = (p.ProjectDocuments.Count - 1); i >= 0; i--)
                        {
                            DocumentRevision drev = PostEditAuditManager.GetDocumentRevisionByUserIdAndTextId(userName, p.ProjectDocuments.ToList<Document>()[i].text_id);
                            if (drev != null)
                            {
                                if (drev.Status == 2)
                                {
                                    completedProjects++;
                                    p.ProjectDocuments.Remove(p.ProjectDocuments.ToList<Document>()[i]);

                                }

                            }
                        }

                        if (completedProjects == totalProjects && p.ProjectDocuments.Count == 0)
                        {
                            return new CoreApiCustomResponse(new { Completed = 1, SurveyLink = p.SurveyLink, ProjectOwner = p.ProjectOwner, Id = p.Id, Name = p.Name });
                        }
                        else
                        {
                            return new CoreApiCustomResponse(p);
                        }

                    }

                    #endregion

                }
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetProjectDocumentsByUser");                
            }

        }
       
    }
}