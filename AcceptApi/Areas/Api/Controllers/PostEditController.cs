using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Filters;
using AcceptApi.Areas.Api.Models;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Managers;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.PostEditAudit;
using AcceptApi.Areas.Api.Models.PostEdit;
using AcceptApi.Areas.Api.Models.Core;
using System.Text;
using AcceptApi.Areas.Api.Models.ActionResults;
using AcceptApi.Areas.Api.Models.Utils;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class PostEditController : Controller
    {
        
        private IPostEditManager postEditManager;
        public IPostEditManager PostEditManager
        {
            get { return postEditManager; }
        }

        public PostEditController()
        {
            this.postEditManager = new PostEditManager();        
        }      

        #region Core

        [RestHttpVerbFilter]
        public JsonResult PostEditDefaults(string lang, string httpVerb)
        {
            try
            {
                if (httpVerb.ToUpper() == "OPTIONS")
                {
                    return new JsonResult();
                }
                else
                    if (httpVerb.ToUpper() == "GET")
                    {
                        switch (lang)
                        {
                            case "en":
                                {
                                    var defaults = new
                                    {
                                        systemId = AcceptApiCoreUtils.ParaphrasingDefaultEnglishSystemId,
                                        maxResults = AcceptApiCoreUtils.ParaphrasingDefaultEnglishMaxResults,
                                        languageCode = AcceptApiCoreUtils.ParaphrasingDefaultEnglishLanguage,
                                        interactiveCheckDefaultRuleSet = AcceptApiCoreUtils.InteractiveCheckRuleSet
                                    };
                                    return Json(new CoreApiCustomResponse(defaults), JsonRequestBehavior.AllowGet);
                                }
                            case "fr":
                                {
                                    var defaults = new
                                    {
                                        systemId = AcceptApiCoreUtils.ParaphrasingDefaultFrenchSystemId,
                                        maxResults = AcceptApiCoreUtils.ParaphrasingDefaultFrenchMaxResults,
                                        languageCode = AcceptApiCoreUtils.ParaphrasingDefaultFrenchLanguage,
                                        interactiveCheckDefaultRuleSet = AcceptApiCoreUtils.InteractiveCheckRuleSet

                                    };
                                    return Json(new CoreApiCustomResponse(defaults), JsonRequestBehavior.AllowGet);
                                }
                            default:
                                {
                                    var defaults = new
                                    {
                                        frSystemId = AcceptApiCoreUtils.ParaphrasingDefaultFrenchSystemId,
                                        frMaxResults = AcceptApiCoreUtils.ParaphrasingDefaultFrenchMaxResults,
                                        frLanguageCode = AcceptApiCoreUtils.ParaphrasingDefaultFrenchLanguage,
                                        enSystemId = AcceptApiCoreUtils.ParaphrasingDefaultEnglishSystemId,
                                        enMaxResults = AcceptApiCoreUtils.ParaphrasingDefaultEnglishMaxResults,
                                        enLanguageCode = AcceptApiCoreUtils.ParaphrasingDefaultEnglishLanguage,
                                        interactiveCheckDefaultRuleSet = AcceptApiCoreUtils.InteractiveCheckRuleSet

                                    };
                                    return Json(new CoreApiCustomResponse(defaults), JsonRequestBehavior.AllowGet);
                                }
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Http Verb detected.");
                    }

            }
            catch (Exception e)
            {
                var model = new CoreApiException(e.Message, "Paraphrasing");
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }
        
        #endregion

        #region AddDocuments

        [RestHttpVerbFilter]
        public JsonResult AddDocumentToProjectRaw(string jsonRaw, int projectId, string httpVerb)
        {
            try
            {
                string decoded = string.Empty;
                try
                {
                    decoded = HttpUtility.UrlDecode(jsonRaw, System.Text.Encoding.UTF8);
                }
                catch (Exception e) { throw (e); }

                var model = PostEditManager.AddDocumentToProjectRaw(decoded, projectId);
                return Json(model);
            }
            catch (Exception e) { return Json(new CoreApiException(e.Message, "AddDocumentToProjectRaw")); }
        }

        [HttpPost]
        public JsonResult GetProjectWithValidDocuments(int? projectId, string userName)
        {
            var model = PostEditManager.GetProjectDocumentsByUser(projectId.GetValueOrDefault(-1), userName);
            return Json(model);
        }

        [HttpGet]
        public JsonResult GetProjectTaskStatus(string token)
        {
            var model = PostEditManager.GetProjectTaskStatus(-1, token);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaskStatus(string textId)
        {
            var model = PostEditManager.GetTaskStatus(textId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #region External Project Methods
        
        [HttpPost]            
        public JsonResult AddDocumentToProject(string token, Document document)
        {              
        var model = PostEditManager.AddDocumentToProject(token, document);
        return Json(model);
        }
         
        #endregion


        #endregion

        #region GetDocuments

        [RestHttpVerbFilter]
        public JsonResult GetDocument(int Id)
        {
            var model = PostEditManager.GetDocumentByDocumentId(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [RestHttpVerbFilter]
        public JsonResult GetDocumentByTextId(string textId)
        {
            var model = PostEditManager.GetDocumentByTextId(textId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonpResult DocumentJsonP(string textId, string userId)
        {
            var model = PostEditManager.GetDocumentForPostEditByUserId(textId, userId);
            return new JsonpResult(model);
        }

        [RestHttpVerbFilter]
        public JsonResult Document(DocumentRetrieveObject documentRetriveObject, string httpVerb)
        {           
            if (httpVerb == "POST")
            {
                if (documentRetriveObject.textId == null || documentRetriveObject.textId.Length == 0)
                {
                    DocumentRetrieveObject objectInputStream = new DocumentRetrieveObject();
                    objectInputStream = (DocumentRetrieveObject)(new DataContractJsonSerializer(documentRetriveObject.GetType())).ReadObject(Request.InputStream);                    
                    var model = PostEditManager.GetDocumentForPostEditByUserId(objectInputStream.textId, objectInputStream.userId);
                    return Json(model);
                }
                else
                {
                    var model = PostEditManager.GetDocumentForPostEditByUserId(documentRetriveObject.textId, documentRetriveObject.userId);
                    return Json(model);
                }
            }
            else
                if (httpVerb == "DELETE")
                {
                    var model = PostEditManager.DeleteDocument(documentRetriveObject.textId, documentRetriveObject.userId);
                    return Json(model);                
                }
                else
                {
                    var error = new AcceptApiException();
                    error.ResponseStatus = CoreApiResponseStatus.Failed;
                    error.Context = "Http verb not allowed";
                    return Json(error);
                }
        }

        [RestHttpVerbFilter]
        public JsonResult CompleteDocument(string textId, string userId,int? questionId, string finalReply, string httpVerb)
        {
            if (httpVerb == "POST")
            {
                var model = PostEditManager.CompleteDocument(textId, userId, questionId.GetValueOrDefault(-1), finalReply);
                return Json(model);
            }
            else
            {
                var error = new AcceptApiException();
                error.ResponseStatus = CoreApiResponseStatus.Failed;
                error.Context = "Http verb not allowed";
                return Json(error);

            }
        }
      
        public JsonpResult CompleteDocumentJsonP(string textId, string userId, int? questionId, string finalReply)
        {
            var model = PostEditManager.CompleteDocument(textId, userId, questionId.GetValueOrDefault(-1), HttpUtility.UrlDecode(finalReply, System.Text.Encoding.UTF8));
           return new JsonpResult(model);            
        }

        [RestHttpVerbFilter]
        public JsonResult UpdateEditionTime(string textId, string userId, string seconds, string httpVerb)
        {
                    
            if (httpVerb == "POST")
            {
                var model = PostEditManager.UpdateRevisionTotalEditingTime(textId,userId,seconds);                
                return Json(model);
            }
            else
            {
                var error = new AcceptApiException();
                error.ResponseStatus = CoreApiResponseStatus.Failed;
                error.Context = "Http verb not allowed";
                return Json(error);

            }

        }

        public JsonpResult UpdateEditionTimeJsonP(string textId, string userId, string seconds)
        {                       
            var model = PostEditManager.UpdateRevisionTotalEditingTime(textId, userId, seconds);
            return new JsonpResult(model);                       
        }

        #endregion
             
        #region Audit Authenticated

        [RestHttpVerbFilter]
        public JsonResult TranslationRevisionPhase(string userIdentifier, string textId, string index, string timeStamp, string state, string source, string target, Phase phase, List<PhaseCount> phaseCounts, string httpVerb)
        {           
            if (httpVerb == "POST")
            {                                                     
                var model = PostEditManager.SaveRevisionPhase(userIdentifier, textId, int.Parse(index), DateTime.Parse(timeStamp), state,string.Empty, source, target, phase, phaseCounts);
                return Json(model);               
            }
            else
            {
                var error = new AcceptApiException();
                error.ResponseStatus = CoreApiResponseStatus.Failed;
                error.Context = "Http verb not allowed.";
                return Json(error);

            }
        }
       
        [HttpPost]
        public JsonResult TranslationRevisionPhaseIe(TranslationRevisionPhaseIeHelper jsonRawStringObject)
        {

            try
            {                
                string jsonRawStringDecoded; jsonRawStringDecoded = string.Empty;
                string jsonRawStringEncoded; jsonRawStringEncoded = string.Empty;
                TranslationRevisionPhaseIeHelper objectTranslationRevisionPhaseIeHelper = new TranslationRevisionPhaseIeHelper();
                if (jsonRawStringObject == null || jsonRawStringObject.jsonRawString.Length == 0)
                {                                           
                        objectTranslationRevisionPhaseIeHelper = (TranslationRevisionPhaseIeHelper)(new DataContractJsonSerializer(jsonRawStringObject.GetType())).ReadObject(Request.InputStream);
                        jsonRawStringEncoded = objectTranslationRevisionPhaseIeHelper.jsonRawString;                  
                }
                else
                    jsonRawStringEncoded = objectTranslationRevisionPhaseIeHelper.jsonRawString;                 
              
                jsonRawStringDecoded = HttpUtility.UrlDecode(jsonRawStringEncoded, System.Text.Encoding.UTF8);                
                TranslationRevisionRequestObject translationRevision = new TranslationRevisionRequestObject();                               
                translationRevision = AcceptApiWebUtils.FromJSON<TranslationRevisionRequestObject>(new TranslationRevisionRequestObject(), jsonRawStringDecoded);
                translationRevision.phase.Date = DateTime.Parse(translationRevision.phaseDate);
                             
                Phase p = new Phase();
                p.ContactEmail = translationRevision.phase.ContactEmail;
                p.Date = translationRevision.phase.Date.GetValueOrDefault();
                p.JobId = translationRevision.phase.JobId;
                p.Notes = new List<PhaseNote>();
                p.PhaseCountGroup = new PhaseCountGroup();
                p.PhaseName = translationRevision.phase.PhaseName;
                p.ProcessName = translationRevision.phase.ProcessName;
                p.Tool = translationRevision.phase.Tool;
                p.ToolId = translationRevision.phase.ToolId;

                foreach (PhaseNotesRequestObject n in translationRevision.phase.Notes)             
                    p.Notes.Add(new PhaseNote(n.Annotates, n.Note, n.NoteFrom));
                
                var model = PostEditManager.SaveRevisionPhase(translationRevision.userIdentifier, translationRevision.textId, int.Parse(translationRevision.index), DateTime.Parse(translationRevision.timeStamp), translationRevision.state, string.Empty, translationRevision.source, translationRevision.target, p, translationRevision.phaseCounts);
                return Json(model);               

            }
            catch (Exception e)
            {
                var error = new CoreApiException(e.Message, "TranslationRevisionPhaseIe");
                return Json(error);            
            }


        }
                
        [ValidateInput(false)]
         public JsonpResult TranslationRevisionPhaseJsonP(string jsonRawString)
        {
          
                try
                {
                    if (jsonRawString == null)
                        return new JsonpResult(new CoreApiResponse("JSON input is null", DateTime.Now));                                     
                    string jsonRawStringDecoded = HttpUtility.UrlDecode(jsonRawString, System.Text.Encoding.UTF8);
                    TranslationRevisionRequestObject translationRevision = new TranslationRevisionRequestObject();
                    translationRevision = AcceptApiWebUtils.FromJSON<TranslationRevisionRequestObject>(new TranslationRevisionRequestObject(), jsonRawStringDecoded);
                    translationRevision.phase.Date = DateTime.Parse(translationRevision.phaseDate);
                    Phase p = new Phase();
                    p.ContactEmail = translationRevision.phase.ContactEmail;
                    p.Date = translationRevision.phase.Date.GetValueOrDefault(); 
                    p.JobId =  translationRevision.phase.JobId;
                    p.Notes = new List<PhaseNote>();
                    p.PhaseCountGroup = new PhaseCountGroup();
                    p.PhaseName = translationRevision.phase.PhaseName;
                    p.ProcessName = translationRevision.phase.ProcessName;
                    p.Tool = translationRevision.phase.Tool;
                    p.ToolId = translationRevision.phase.ToolId;

                    foreach (PhaseNotesRequestObject n in translationRevision.phase.Notes)
                    {
                        p.Notes.Add(new PhaseNote(n.Annotates, n.Note, n.NoteFrom));
                    }
                   
                    var model = PostEditManager.SaveRevisionPhase(translationRevision.userIdentifier, translationRevision.textId, int.Parse(translationRevision.index), DateTime.Parse(translationRevision.timeStamp), translationRevision.state, string.Empty, translationRevision.source, translationRevision.target, p, translationRevision.phaseCounts);
                    return new JsonpResult(model);
                    
                }
                catch(Exception e)
                {
                    var error = new CoreApiException(e.Message, "TranslationRevisionPhaseJsonP");
                    return new JsonpResult(error); 
                }
                       
        }

        #region XLIFF Reports

        [HttpGet]
        public string DocumentXliff(string userId, string textId)
        {
            try
            {
                var model = PostEditManager.GenerateXliffFormat(textId, userId);
                Response.ContentType = "text/xml; charset=\"utf-8\"";
                return model;
            }
            catch (Exception e)
            {
                return e.Message;
            }
           
        }
        
        [HttpGet]
        public string ProjectXliff(string token) 
        {
            var model = PostEditManager.GenerateProjectOwnerXliff(token, true);
            Response.ContentType = "text/xml; charset=\"utf-8\"";
            return model;
        }

        [HttpGet]
        public ActionResult ProjectReport(string token)
        {
            var model = PostEditManager.GenerateProjectOwnerXliff(token, false);            
            TempData.Add("xmlResult", model);
            return new RedirectResult(Url.Action("Report") + "?reportId=" + AcceptFramework.Business.Utils.StringUtils.GenerateTinyHash(DateTime.UtcNow.ToString()));           
        }

        [HttpGet]
        public string TaskXliff(string token, string textId)
        {
            var model = PostEditManager.GenerateTaskRevisionsXliff(token, textId);
            Response.ContentType = "text/xml; charset=\"utf-8\"";
            return model;
        }

        #endregion
                   
        #endregion

        #region ProjectInvitations

        [HttpPost]
        public JsonResult InviteUsers(string[] usersList, int? projectId, string uniqueRoleName, string projectOwner)
         {
             var model = PostEditManager.InviteUsersToProject(usersList, projectId.GetValueOrDefault(-1), uniqueRoleName, projectOwner);
             return Json(model);
         }

        [RestHttpVerbFilter]
        public JsonResult Invite(string code, string httpVerb)
         {

                switch(httpVerb)
                {
                     case "GET":
                     {
                         var model = PostEditManager.GetProjectInvitation(code);
                         return Json(model, JsonRequestBehavior.AllowGet);
                     }
                    case "POST": 
                     {

                         var model = PostEditManager.UpdateInvitationCode(code);
                         return Json(model);
                     
                     }

                    }

             return Json(new CoreApiException("Invalid Http Verb","Invite"));

         }

        [RestHttpVerbFilter]
        public JsonResult InviteByUserName(string userName)
         {
             var model = PostEditManager.GetProjectInvitationByUserName(userName);
             return Json(model, JsonRequestBehavior.AllowGet);
         }

        [HttpGet]
        public JsonResult UpdateProjectInviteConfirmationDateByCode(string code)
        {
             var model = PostEditManager.UpdateInvitationConfirmationDateByCode(code);
             return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
       
        #region Paraphrasing

        [RestHttpVerbFilter]
        public JsonResult Paraphrasing(string maxResults, string sysId, string lang, string source, string target, string context, string httpVerb)
        {
            try
            {
                if (httpVerb.ToUpper() == "OPTIONS")
                {
                    return new JsonResult();
                }
                else
                {                                                    
                    //content to check.
                    string query; query = HttpUtility.UrlDecode(target, System.Text.Encoding.UTF8);
                    query = HttpUtility.UrlEncode(target, System.Text.Encoding.UTF8);
                    //the paraprasing response object.
                    ParaphraseResponse response; response = new ParaphraseResponse();                
                    //parse the paraphrasing api result.
                    JObject jObject; jObject = new JObject();
                    string jsonRawResponse;
              
                    try
                    {
                        jsonRawResponse = AcceptApiWebUtils.GetJsonWithTimeout(AcceptApiCoreUtils.ParaphrasingEndpoint, "max=" + maxResults + "&sys=" + sysId + "&lang=" + lang + "&q=" + query + "", AcceptApiCoreUtils.ParaphrasingTimeoutPeriod);
                    }
                    catch (Exception e)
                    {
                        throw(e);
                    }
                                                                   
                    try
                    {
                        jObject = JObject.Parse(jsonRawResponse);
                        //query the json object: looking for the paraphrases property.                         
                        response.resultSet = JsonConvert.DeserializeObject<List<string[]>>(jObject["paraphrases"].ToString());
                        response.status = "OK";
                        response.error = string.Empty;
                    }catch
                    {
                        
                        try
                        {
                            response.error = JsonConvert.DeserializeObject<string>(jObject["error"].ToString());
                            response.status = JsonConvert.DeserializeObject<string>(jObject["status"].ToString());
                        }
                        catch
                        {
                            throw (new Exception("error parsing raw paraphrase api response status."));
                        }


                    }
                                                                                                                                                      
                    return Json(new CoreApiCustomResponse(response), JsonRequestBehavior.AllowGet);     
                }

            }
            catch (Exception e)
            {
                var model = new CoreApiException(e.Message, "Paraphrasing");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        
        }

        #endregion

    }
}

