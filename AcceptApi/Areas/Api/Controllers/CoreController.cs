using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models;

using AcceptApi.Areas.Api.Models.Filters;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Domain.Audit;
using AcceptApi.Areas.Api.Models.Log;
using AcceptApi.Areas.Api.Models.ActionResults;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using AcceptApi.Areas.Api.Models.Utils;

namespace AcceptApi.Areas.Api.Controllers
{
    //[RequireHttps]
    [CrossSiteJsonCall]
    public class CoreController : Controller
    {       
        private IAcceptApiManager acceptCoreManager;
        public IAcceptApiManager AcceptCoreManager
        {
            get { return acceptCoreManager; }
        }       
              
        public CoreController()
        {
            this.acceptCoreManager = new AcceptApiManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcceptRequest(CoreApiRequest requestObject)
        {
            try
            {
                if (requestObject == null || requestObject.Text.Length == 0)
                    requestObject = (CoreApiRequest)(new DataContractJsonSerializer(requestObject.GetType())).ReadObject(Request.InputStream);

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                if (requestObject.ApiKey != null && requestObject.ApiKey.Length > 0)
                {
                    parameters.Add("apiKey", requestObject.ApiKey);
                }
                else
                {
                    parameters.Add("username", requestObject.User);
                    parameters.Add("password", requestObject.Password);
                }
                
                string text = HttpUtility.UrlDecode(requestObject.Text, System.Text.Encoding.UTF8);  
        
                parameters.Add("text", text);
                parameters.Add("languageid", requestObject.Language);
                parameters.Add("ruleset", requestObject.Rule);
                parameters.Add("grammar", requestObject.Grammar);
                parameters.Add("spell", requestObject.Spell);
                parameters.Add("style", requestObject.Style);

                parameters.Add("requestFormat", requestObject.RequestFormat);

                if (requestObject.GlobalSessionId != null && requestObject.GlobalSessionId.Length > 0)
                {
                    parameters.Add("globalSessionId", requestObject.GlobalSessionId);
                }

                if (requestObject.IEDomain != null && requestObject.IEDomain.Length > 0)
                    parameters.Add("ieDomain", requestObject.IEDomain);


                if (requestObject.SessionMetadata != null && requestObject.SessionMetadata.Length > 0)
                    parameters.Add("sessionMetadata", requestObject.SessionMetadata);

                var model = AcceptCoreManager.GenericRequest(parameters);
                return Json(model);


            }
            catch (Exception e)
            {
                var errorModel = new CoreApiException(e.Message, "Accept Request Controller");
                return Json(errorModel);
            }

        }

        [RestHttpVerbFilter]    
        public JsonResult GetResponse(string session)
        {
            Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();           
            parameters.Add("session", session);
            var model = AcceptCoreManager.GetResponse(parameters);                                   
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [RestHttpVerbFilter]
        public JsonResult GetResponseStatus(string session)
        {
            Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();        
            parameters.Add("session", session);
            var model = AcceptCoreManager.GetResponseStatus(parameters);                                 
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLanguageRulesSet(string language)
        {
            var model = AcceptCoreManager.GetRuleSetNames(language);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [RestHttpVerbFilter]
        public JsonResult AcceptRequest(CoreApiRequest requestObject, string httpVerb)
        {
            try
            {

                if (httpVerb == "POST")
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    if (requestObject.ApiKey != null && requestObject.ApiKey.Length > 0)
                    {
                        parameters.Add("apiKey", requestObject.ApiKey);
                    }
                    else
                    {
                        parameters.Add("username", requestObject.User);
                        parameters.Add("password", requestObject.Password);
                    }                
                    string text = HttpUtility.UrlDecode(requestObject.Text, System.Text.Encoding.UTF8);
                    parameters.Add("text", text);
                    parameters.Add("languageid", requestObject.Language);
                    parameters.Add("ruleset", requestObject.Rule);
                    parameters.Add("grammar", requestObject.Grammar);
                    parameters.Add("spell", requestObject.Spell);
                    parameters.Add("style", requestObject.Style);
                    parameters.Add("requestFormat", requestObject.RequestFormat);
                    if (requestObject.GlobalSessionId != null && requestObject.GlobalSessionId.Length > 0)
                    {
                        parameters.Add("globalSessionId", requestObject.GlobalSessionId);
                    }
                    var model = AcceptCoreManager.GenericRequest(parameters);
                    return Json(model);
                }
                else
                    if (httpVerb == "OPTIONS")
                    {
                        //Firefox sends a OPTIONS for all POST AJAX calls.
                        return new JsonResult();
                    }
                    else
                    {
                        var notAllowedModel = new CoreApiException("Http Verb not allowed.", "Accept Request");
                        return Json(notAllowedModel);                     
                    }
            }
            catch (Exception e)
            {
                var errorModel = new CoreApiException(e.Message, "Accept Request Controller");
                return Json(errorModel);
            }

        }
               
        #region JSONP Methods
  
        [HttpGet]     
        public JsonpResult GetResponseStatusJsonP(string session)
        {
            Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();
            parameters.Add("session", session);
            var model = AcceptCoreManager.GetResponseStatus(parameters);
            Response.AppendHeader("Cache-Control", "no-cache");
            return new JsonpResult(model);
        }

        [HttpGet]
        public JsonpResult GetResponseJsonP(string session)
        {
            Dictionary<string, string> parameters; parameters = new Dictionary<string, string>();
            parameters.Add("session", session);
            var model = AcceptCoreManager.GetResponse(parameters);
            return new JsonpResult(model);
        }
       
        [HttpGet]
        public JsonpResult AuditFlagJsonP(string globalSessionId, string flag, string userAction, string actionValue, string ignored, string name, string textBefore, string textAfter, string timeStamp, string jsonRaw, string privateId)
        {            
            var model = AcceptCoreManager.AddAuditFlag(HttpUtility.UrlDecode(flag, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(userAction, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(actionValue, System.Text.Encoding.UTF8), globalSessionId, HttpUtility.UrlDecode(ignored, System.Text.Encoding.UTF8), name, HttpUtility.UrlDecode(textBefore, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(textAfter, System.Text.Encoding.UTF8), DateTime.UtcNow, HttpUtility.UrlDecode(jsonRaw, System.Text.Encoding.UTF8), privateId);
            return new JsonpResult(model);       
        }

        [HttpGet]
        public JsonpResult AuditFinalContextJsonP(string globalSessionId, string textContent, string timeStamp)
        {            
            var model = AcceptCoreManager.AuditFinalContext(globalSessionId, HttpUtility.UrlDecode(textContent, System.Text.Encoding.UTF8), DateTime.UtcNow);
            return new JsonpResult(model);       
        }
      
        #endregion            
      
        #region Reports

        [HttpGet]
        public ActionResult GlobalSessionDomain(string id, string start, string end, string userKey, string httpVerb)
        {
            try
            {              
                DateTime? startDatetime;
                DateTime? endDatetime;
                if (start != null && end != null)
                {
                    try
                    {
                        startDatetime = DateTime.Parse(start);
                        endDatetime = DateTime.Parse(end);
                    }
                    catch
                    {
                        startDatetime = null;
                        endDatetime = null;                       
                    }

                    if (startDatetime != null && endDatetime != null)                                                                    
                        return new LargeJsonResult() { Data = AcceptCoreManager.GlobalSessionApiKey(id, startDatetime, endDatetime, userKey), MaxJsonLength = int.MaxValue, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };                                         
                    throw new ArgumentNullException();
                }               
                return new LargeJsonResult() { Data = AcceptCoreManager.GlobalSessionApiKey(id, null, null, userKey), MaxJsonLength = int.MaxValue, ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }
            catch (Exception e)
            {                
                return Json(new CoreApiException(e.Message, "GlobalSessionDomain"), "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);                
            }
        }

        [HttpGet]
        public ActionResult SimpleGlobalSessionDomain(string id, string start, string end, string userKey, string format)
        {
            try
            {
                if (start != null && end != null)
                {
                    DateTime? startDatetime = DateTime.Parse(start);
                    DateTime? endDatetime = DateTime.Parse(end);

                    if (startDatetime != null && endDatetime != null)
                    {
                        if (format != null && format.ToLower() != "json")
                        {
                            switch (format.ToLower())
                            {
                                case "excel":
                                {                                    
                                    this.Response.ClearContent();
                                    string fileName = "report" + DateTime.UtcNow.ToString() + ".xls";
                                    this.Response.ContentType = "application/excel";
                                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(AcceptCoreManager.SimpleGlobalSessionApiKeyCustom(id, startDatetime, endDatetime, userKey, "excel"));                                  
                                    return File(buffer, "application/ms-excel", fileName);                                   
                                }
                               default:
                                {
                                    string fileName = "report" + DateTime.UtcNow.ToString() + ".csv";
                                    return File(System.Text.Encoding.UTF8.GetBytes(AcceptCoreManager.SimpleGlobalSessionApiKeyCustom(id, startDatetime, endDatetime, userKey, "csv")), "text/csv", fileName);                                                                                              
                                };
                            }
                        }
                        else
                            return Json(AcceptCoreManager.SimpleGlobalSessionApiKey(id, startDatetime, endDatetime, userKey), "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
                       
                    }
                    throw new ArgumentNullException();
                }

                return Json(AcceptCoreManager.SimpleGlobalSessionApiKey(id, null, null, userKey), "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new CoreApiException(e.Message, "SimpleGlobalSessionDomain"), "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        [RestHttpVerbFilter]
        public ActionResult AuditFlag(AuditFlagRequest auditFlag, string httpVerb)
        {
            switch (httpVerb.ToUpper())
            {
                case "POST":
                    {
                        try
                        {
                            if (auditFlag == null || auditFlag.globalSessionId.Length == 0)
                            {
                                auditFlag = (AuditFlagRequest)(new DataContractJsonSerializer(auditFlag.GetType())).ReadObject(Request.InputStream);
                                if (auditFlag.privateId.Length == 0)
                                    auditFlag.privateId = null;
                            
                            }                            
                            var model = AcceptCoreManager.AddAuditFlag(HttpUtility.UrlDecode(auditFlag.flag, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(auditFlag.userAction, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(auditFlag.actionValue, System.Text.Encoding.UTF8), auditFlag.globalSessionId, HttpUtility.UrlDecode(auditFlag.ignored, System.Text.Encoding.UTF8), auditFlag.name, HttpUtility.UrlDecode(auditFlag.textBefore, System.Text.Encoding.UTF8), HttpUtility.UrlDecode(auditFlag.textAfter, System.Text.Encoding.UTF8), DateTime.UtcNow, HttpUtility.UrlDecode(auditFlag.jsonRaw, System.Text.Encoding.UTF8), auditFlag.privateId);
                            return Json(model);

                        }
                        catch (Exception e)
                        {
                            var errorModel = new CoreApiException(e.Message, "AuditFlag");
                            return Json(errorModel);

                        }
                    }
                case "OPTIONS": {
                    return new JsonResult();                
                }
                default:
                    {
                        var notAllowedModel = new CoreApiException("Http Verb not allowed.", "AuditFlag");
                        return Json(notAllowedModel);
                    }       
            }
           
        }

        [RestHttpVerbFilter]
        public ActionResult AuditFinalContext(AuditFinalContextRequest auditFinalContext, string httpVerb)
        {
            switch(httpVerb.ToUpper())
            {
                case "POST":
                {
                    try
                    {
                        if (auditFinalContext == null || auditFinalContext.globalSessionId.Length == 0)
                        auditFinalContext = (AuditFinalContextRequest)(new DataContractJsonSerializer(auditFinalContext.GetType())).ReadObject(Request.InputStream);                        
                        var model = AcceptCoreManager.AuditFinalContext(auditFinalContext.globalSessionId, HttpUtility.UrlDecode(auditFinalContext.textContent, System.Text.Encoding.UTF8), DateTime.UtcNow);
                        return Json(model);
                    }
                    catch (Exception e)
                    {
                    var errorModel = new CoreApiException(e.Message, "AuditFinalContext");
                    return Json(errorModel);
                    }
                }
                case "OPTIONS":                    
                {
                    return new JsonResult();
                
                }
                default: 
                {
                    var notAllowedModel = new CoreApiException("Http Verb not allowed.", "AuditFinalContext");
                    return Json(notAllowedModel);
                }                
            }
        }

        public string SendTestEmail(string from, string to)
        {
            try
            {
                EmailManager.SentTestEmail(from, to);
                return "success";
            }
            catch (Exception e)
            {
                return "failed: " + e.Message;                
            }
        }

    }
}
