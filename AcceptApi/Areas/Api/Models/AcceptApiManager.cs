using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AcceptApi.Areas.Api.Models.Acrolinx;
using AcceptFramework.Interfaces;
using AcceptFramework.Business;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Interfaces.Session;
using AcceptFramework.Domain.Audit;
using AcceptApi.Areas.Api.Models.Authentication;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Domain.Common;
using AcceptApi.Areas.Api.Models.Utils;
using System.Net;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.Session;
using AcceptFramework.Interfaces.Audit;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AcceptApi.Areas.Api.Models
{
    public class AcceptApiManager : IAcceptApiManager
    {
        private ApiGlobalSession _apiGlobalSession;
        private AcrolinxCore _aCrolinxApiObject;         
        private IAcceptApiServiceLocator _acceptServiceLocator;               
        private ISessionManager _acceptSessionManager;
        private IUserManager _acceptUserManager;
        private IProjectManager _projectManagerService;
        private IAuditManager _auditManager;        
         
        public AcceptApiManager()
        {
            _acceptServiceLocator = new AcceptApiServiceLocator();
            _acceptSessionManager = _acceptServiceLocator.GetSessionManagerService();
            _acceptUserManager = _acceptServiceLocator.GetUserManagerService();
            _aCrolinxApiObject = new AcrolinxCore(_acceptSessionManager.GetNewAcceptSession(), _acceptServiceLocator.GetAuditManagerService());
            _projectManagerService = _acceptServiceLocator.GetProjectManagerService();
            _auditManager = _acceptServiceLocator.GetAuditManagerService();
        }

        #region Properties

        public ISessionManager AcceptSessionManager
        {
            get { return _acceptSessionManager; }            
        }

        public AcrolinxCore ACrolinxApiObject
        {
            get { return _aCrolinxApiObject; }          
        }

        public IUserManager AcceptUserManager
        {
            get { return _acceptUserManager; }            
        }

        public IProjectManager ProjectManagerService
        {
            get { return _projectManagerService; }          
        }

        public IAuditManager AuditManager
        {
            get { return _auditManager; }
        }

        #endregion

        #region Core

        public CoreApiResponse GenericRealTimeRequest(Dictionary<string, string> parameters, string clientId, string requestedUrl, string index)
        {
            CoreApiResponse response; response = null;

            try
            {

                #region RequestAuthentication

                if (parameters.ContainsKey("password") && parameters["password"] == AcceptApiCoreUtils.AcrolinxUserPassword)                                       
                    ACrolinxApiObject.StartRealTimeAcceptSession(clientId,requestedUrl,index);                                      
                else                             
                    throw new Exception("Invalid credentials.");
                           
                #endregion

                #region Set Session Metadata String

                string sessionMetadata = string.Empty;                
                sessionMetadata += parameters.ContainsKey("sessionMetadata") ? parameters["sessionMetadata"] : string.Empty;

                #endregion
              
                CreateNewGlobalSession(sessionMetadata);

                response = ACrolinxApiObject.AcrolinxGenericRequest(parameters);                
                //update current response object with the global session id.
                ACrolinxApiObject.AcceptApiResponseManager.ResponseStatus.GlobalSessionId = this._apiGlobalSession.GlobalSessionId;
                //this session id was the old HTTP session if - now it is replaced with the global session id - (!).
                ACrolinxApiObject.AcceptSession.SessionId = this._apiGlobalSession.GlobalSessionId;
                //update current child session context.
                ACrolinxApiObject.AcceptSession.Context = parameters["text"];

                AcceptSessionManager.InsertSession(ACrolinxApiObject.EndSession());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return response;
        }

        public CoreApiResponse GenericRequest(Dictionary<string, string> parameters)
        {
            CoreApiResponse response; response = null;

            try
            {
                             
                #region RequestAuthentication

                string hostReferrer;

                if (parameters.ContainsKey("apiKey") && parameters["apiKey"].Length > 0)
                {
                    ApiKeys apiKey = AcceptUserManager.GetApiKey(parameters["apiKey"]);
                    if (apiKey != null)
                    {
                        //internet explorer fix (xdomainrequest not sending http referer).                        
                        if (parameters.ContainsKey("ieDomain") && parameters["ieDomain"].Length > 0)
                        {
                            try
                            {

                                Uri iesucks = new Uri(parameters["ieDomain"]);
                                hostReferrer = iesucks.Host;
                            }
                            catch { hostReferrer = "failed parsing string referrrer for ie"; }
                        }
                        else
                        {
                            hostReferrer = AcceptApiWebUtils.GetUrlReferrerHost();                        
                        }
                                              
                        if (hostReferrer != null && hostReferrer.Length > 0)
                        {
                            if ((hostReferrer != apiKey.KeyDns) || (apiKey.KeyStatus != 1))
                            {
                                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                return new AcceptApiException("Invalid Api Key", "Api Request");
                            }
                            else
                            {
                                parameters.Add("username", AcceptApiCoreUtils.AcrolinxUser);
                                parameters.Add("password", AcceptApiCoreUtils.AcrolinxUserPassword);
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;                     
                            return new AcceptApiException("Invalid Host Referer Detected - " + hostReferrer, "Api Request");
                        }

                        ACrolinxApiObject.StartAcceptSession(HttpContext.Current, apiKey.ApiKey,hostReferrer != null ? hostReferrer : "null");
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return new AcceptApiException("Api Key Not Found", "Api Request");
                    }
                }
                else
                {
                    //no API key on the request.
                    if (parameters.ContainsKey("password") && parameters["password"] == AcceptApiCoreUtils.AcrolinxUserPassword)                                            
                        ACrolinxApiObject.StartAcceptSession(HttpContext.Current);                                           
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return new AcceptApiException("Invalid Credentials", "Api Request");
                    }

                }

                #endregion
               
                #region Set Session Metadata String

                string sessionMetadata = string.Empty;               
                sessionMetadata += parameters.ContainsKey("sessionMetadata") ? parameters["sessionMetadata"] : string.Empty;

                #endregion

                if (parameters.ContainsKey("globalSessionId"))
                {
                    this._apiGlobalSession = AcceptSessionManager.GetGlobalSession(parameters["globalSessionId"]);                    
                    string[] lastCachedValues = AcceptSessionManager.GetSessionCachedValuesByCodeSessionId(this._apiGlobalSession.SessionId);                    
                    parameters.Add("lastValidAcrolinxSession",lastCachedValues[0]);
                    parameters.Add("lastValidAcrolinxCode",lastCachedValues[1]);                                      
                    response = ACrolinxApiObject.ReuseAcrolinxGenericeRequest(parameters);
                    //acrolinx session expired?
                    if (response == null)
                    {
                        CreateNewGlobalSession(sessionMetadata);
                        response = ACrolinxApiObject.AcrolinxGenericRequest(parameters);      
                    }
                    else
                    {
                        this._apiGlobalSession.SessionId = ACrolinxApiObject.AcceptSession.SessionCodeId;                        
                        AcceptSessionManager.UpdateGlobalSession(this._apiGlobalSession);
                    }                  
                }
                else
                {
                    CreateNewGlobalSession(sessionMetadata);                  
                    response = ACrolinxApiObject.AcrolinxGenericRequest(parameters);                  
                }

                //update current response object with the global session id.
                ACrolinxApiObject.AcceptApiResponseManager.ResponseStatus.GlobalSessionId = this._apiGlobalSession.GlobalSessionId;                
                //this session id was the old HTTP session if - now it is replaced with the global session id - (!).
                ACrolinxApiObject.AcceptSession.SessionId = this._apiGlobalSession.GlobalSessionId;                                
                //update current child session context.
                ACrolinxApiObject.AcceptSession.Context = parameters["text"];                
                AcceptSessionManager.InsertSession(ACrolinxApiObject.EndSession());
            }
            catch (Exception e)
            {
                string innerMessage;innerMessage =  e.InnerException.Message != null ? innerMessage = e.InnerException.Message : "null";
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;                
                return new AcceptApiException("Exception:" + e.Message + "InnerException:" + innerMessage, "Accept Request");                
            }
           
            return response;
        }

        private void CreateNewGlobalSession(string metaInfo)
        {                     
            this._apiGlobalSession = new ApiGlobalSession();           
            this._apiGlobalSession.StartTime = DateTime.UtcNow;                 
            this._apiGlobalSession.GlobalSessionId = AcceptFramework.Business.Utils.StringUtils.Generate32CharactersStringifiedGuid(ACrolinxApiObject.AcceptSession.SessionCodeId);        
            this._apiGlobalSession.SessionId = ACrolinxApiObject.AcceptSession.SessionCodeId;
            this._apiGlobalSession.Meta = metaInfo;                          
            this._apiGlobalSession = AcceptSessionManager.InsertGlobalSession(this._apiGlobalSession);                                                                    
        }

        private void CreateNewGlobalRealTimeSession(string metaInfo, string uniqueId)//AcceptSession currentChildSession
        {
            this._apiGlobalSession = new ApiGlobalSession();
            this._apiGlobalSession.StartTime = DateTime.UtcNow;
            this._apiGlobalSession.GlobalSessionId = AcceptFramework.Business.Utils.StringUtils.Generate32CharactersStringifiedGuid(ACrolinxApiObject.AcceptSession.SessionCodeId);
            this._apiGlobalSession.SessionId = ACrolinxApiObject.AcceptSession.SessionCodeId;
            this._apiGlobalSession.Meta = metaInfo;                             
            this._apiGlobalSession = AcceptSessionManager.InsertGlobalSession(this._apiGlobalSession);
        }

        public CoreApiResponse GetResponse(Dictionary<string, string> parameters)
        {
            try
            {
                ACrolinxApiObject.AcceptSession = AcceptSessionManager.GetSessionByCodeSessionId(parameters["session"]);               
                if (ACrolinxApiObject.AcceptSession.CachedValues.Length > 0)
                {
                    parameters.Add("sessionid", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[0]);
                    parameters.Add("code", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[1]);
                    ACrolinxApiObject.AcceptApiResponseManager.ResponseObject.AcceptSessionCode = parameters["session"];
                }
                else
                 return new AcceptApiException("Session Cached Values Not Set", "Get Response Status");

                return ACrolinxApiObject.GetResponse(parameters);

            }
            catch (Exception e)
            {               
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new AcceptApiException(e.Message, "Get Response");
            }
        }
        public CoreApiResponse GetRealTimeResponse(Dictionary<string, string> parameters)
        {
            try
            {
                ACrolinxApiObject.AcceptSession = AcceptSessionManager.GetSessionByCodeSessionId(parameters["session"]);               
                if (ACrolinxApiObject.AcceptSession.CachedValues.Length > 0)
                {
                    parameters.Add("sessionid", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[0]);
                    parameters.Add("code", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[1]);
                    ACrolinxApiObject.AcceptApiResponseManager.ResponseObject.AcceptSessionCode = parameters["session"];
                }
                else
                    return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, "exception: parsing sessionid and code."); 

                return ACrolinxApiObject.GetResponse(parameters);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message,e.InnerException);                
            }
        }

        public CoreApiResponse GetResponseStatus(Dictionary<string, string> parameters)
        {
            try
            {                            
                ACrolinxApiObject.AcceptSession = AcceptSessionManager.GetSessionByCodeSessionId(parameters["session"]);

                if (ACrolinxApiObject.AcceptSession.CachedValues.Length > 0)
                {
                    parameters.Add("sessionid", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[0]);
                    parameters.Add("code", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[1]);
                }
                else
                    return new AcceptApiException("Session Cached Values Not Set", "Get Response Status");

                return ACrolinxApiObject.GetResponseStatus(parameters);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;      
                return new AcceptApiException(e.Message, "Get Response Status");
            }

        }
        public CoreApiResponse GetRealTimeResponseStatus(Dictionary<string, string> parameters)
        {
            try
            {
                ACrolinxApiObject.AcceptSession = AcceptSessionManager.GetSessionByCodeSessionId(parameters["session"]);

                if (ACrolinxApiObject.AcceptSession.CachedValues.Length > 0)
                {
                    if (!parameters.ContainsKey("sessionid"))
                    parameters.Add("sessionid", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[0]);
                    if (!parameters.ContainsKey("code"))
                    parameters.Add("code", ACrolinxApiObject.AcceptSession.CachedValues.Split(',')[1]);
                }
                else
                    return new AcceptApiResponseStatus(CoreApiResponseStatus.Failed, "exception: parsing sessionid and code."); 

                return ACrolinxApiObject.GetResponseStatus(parameters);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }

        }

        public CoreApiResponse GetRuleSetNames(string language)
        {

            try
            {             
                AcrolinxServerCapabilitiesObject obj = ACrolinxApiObject.GetAcrolinxCapabilityObject(language);
                List<string> ruleNames = new List<string>();

                foreach (RuleSetCapabilities ruleSet in obj.ruleSetCapabilities)
                    ruleNames.Add(ruleSet.name);

                return new CoreApiCustomResponse(ruleNames);
            }
            catch (Exception e)
            {                
                throw;
            }
                
        }

        #endregion

        #region Grammar

        public CoreApiResponse GrammarRequest(Dictionary<string, string> parameters)
        {          
            ACrolinxApiObject.StartAcceptSession(HttpContext.Current);  

            AcceptApiResponseManager response;
            CoreApiResponse result;

            try
            {               
                result = ACrolinxApiObject.GrammarRequest(parameters);
                AcceptSessionManager.InsertSession(ACrolinxApiObject.EndSession());         
            }
            catch
            {                
                return new AcceptApiException();
            }
         
            return result;
        }

        public string GrammarLanguages()
        {
            return ACrolinxApiObject.GetAcrolinxServerLanguages();
        }

        #endregion

        #region Spelling

        public CoreApiResponse SpellCheckRequest(Dictionary<string, string> parameters)
        {
                        
            AcceptApiResponseManager response;
            CoreApiResponse result;
            try
            {
                ACrolinxApiObject.StartAcceptSession(HttpContext.Current);
                result = ACrolinxApiObject.SpellCheckRequestTest(parameters);
                AcceptSessionManager.InsertSession(ACrolinxApiObject.EndSession());
            }
            catch (Exception e)
            {              
                return new AcceptApiException(e.Message, "Spell Check");
            }

            return result;
        }

        public string SpellCheckLanguages()
        {
            return ACrolinxApiObject.GetAcrolinxServerLanguages();
        }

        #endregion

        #region Style

        public CoreApiResponse StyleCheckRequest(Dictionary<string, string> parameters)
        {                        
            AcceptApiResponseManager response;
            CoreApiResponse result;

            try
            {
                ACrolinxApiObject.StartAcceptSession(HttpContext.Current);                 
                result = ACrolinxApiObject.StyleCheckRequest(parameters);
                AcceptSessionManager.InsertSession(ACrolinxApiObject.EndSession()); 
                
            }
            catch (Exception e)
            {                
                return new AcceptApiException();
            }           
           
            return result;
        }

        public string StyleCheckLanguages()
        {
            return ACrolinxApiObject.GetAcrolinxServerLanguages();
        }

        #endregion

        #region Audit

        public CoreApiResponse AuditUserAction(CoreApiAudit audit)
        {
            try
            {
                audit.Validate();
                AuditUserAction audituseraction; audituseraction = new AuditUserAction();
                audituseraction.AuditContext = audit.AuditContext;
                audituseraction.AuditTypeId = audit.AuditTypeId;
                audituseraction.RuleName = audit.RuleName;
                audituseraction.SessionCodeId = audit.SessionCodeId;
                audituseraction.StartTime = audit.StartTime;
                audituseraction.EndTime = audit.EndTime;
                audituseraction.TextAfter = audit.TextAfter;
                audituseraction.TextBefore = audit.TextBefore;
                audituseraction.AuditContext = audit.AuditContext;
                _aCrolinxApiObject.AcceptAuditManager.InsertActionAudit(audituseraction);

                return new CoreApiResponse(CoreApiResponseStatus.Ok, DateTime.UtcNow, audit.SessionCodeId);
            }
            catch(Exception e)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new CoreApiException(e.Message, "AuditUserAction");
            
            }
        }        

        public CoreApiResponse AddAuditFlag(string flag, string action, string actionValue, string globalSessionId, string ignored, string name, string textBefore, string textAfter, DateTime timeStamp, string rawValue, string privateFlagId)
        {
            try
            {
                ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(globalSessionId);

                AuditFlag auditFlag = new AuditFlag(flag, action, actionValue, globalSession.SessionId, ignored, name, textBefore, textAfter, timeStamp, rawValue, privateFlagId);
                AuditManager.InsertAuditFlag(auditFlag);
                return new CoreApiResponse();
        
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new CoreApiException(e.Message, "AddAuditFlag");
               
            }

            
        }

        public CoreApiResponse AuditFinalContext(string globalSessionId, string textContent, DateTime timeStamp)
        {
            try
            {
                ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(globalSessionId);
                globalSession.FinalContext = textContent;
                globalSession.EndTime = timeStamp;
                AcceptSessionManager.UpdateGlobalSession(globalSession);
                return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "AuditFinalContext");
                                
            }
        
        }

        public CoreApiResponse GlobalSession(string globalSessionId)
        {

            try
            {
                ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(globalSessionId);
                List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);

                CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();

                if (childSessions != null && childSessions.Count > 0)
                {

                    AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                    AcrolinxRequestSettings acrolinxRequestSettings;
                    try
                    {
                        acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                    }
                    catch
                    {
                        acrolinxRequestSettings = new AcrolinxRequestSettings();
                    }

                    string[] globalSessionMetaData = globalSession.Meta.Split(';');
                   
                    globalSessionReportObject.Input = childSessions[0].Context;
                    globalSessionReportObject.Output = globalSession.FinalContext;
                    globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;
                    //rule name is the first position in the array.
                    globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;  //globalSession.Meta;
                    globalSessionReportObject.MetaData.Language = acrolinxRequestSettings.CheckSettings != null ? acrolinxRequestSettings.CheckSettings.LanguageId : string.Empty;
                    //user name is the second position in the array.
                    globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;

                    foreach (AcceptSession childSession in childSessions)
                    {
                        ChildSession session = new ChildSession();
                        List<AuditFlag> auditFlags = AuditManager.GetAllAuditFlagsBySessionId(childSession.SessionCodeId);
                        session.Context = acrolinxRequestSettings.Request;

                        foreach (AuditFlag auditFlag in auditFlags)
                        {                         
                            ReportFlag flag = new ReportFlag();
                            flag.Action = auditFlag.Action;
                            flag.ActionValue = auditFlag.ActionValue;                                                        
                            flag.Name = auditFlag.Name;
                            flag.FlagContext = auditFlag.Flag;
                            session.Results.Add(flag);
                        }

                        #region flags without suggestions
                        AuditApiRequest auditExternalApiResponseBody = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 3);                        
                        AcrolinxReport acrolinxReport = JsonConvert.DeserializeObject<AcrolinxReport>(auditExternalApiResponseBody.AuditContext);
                        session.ProviderResults.Add(acrolinxReport);                                               
                        #endregion
                        globalSessionReportObject.ChildSessions.Add(session);

                    }

                }
                
                
                return new CoreApiCustomResponse(globalSessionReportObject);
            }
            catch (Exception e)
            {                
                throw(e);
            }                        
        }

        public CoreApiResponse GlobalSessionRange(DateTime start, DateTime end)
        {

            try
            {
                List<ApiGlobalSession> globalSessions = AcceptSessionManager.GetGlobalSessionRange(start,end);
                List<CoreApiGlobalSessionReportObject> globalSessionReportObjects = new List<CoreApiGlobalSessionReportObject>();
                foreach (ApiGlobalSession globalSession in globalSessions)
                {

                    List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);
                    CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();
                    if (childSessions != null && childSessions.Count > 0)
                    {
                        AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                        AcrolinxRequestSettings acrolinxRequestSettings;
                        try
                        {
                            acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                        }
                        catch
                        {
                            acrolinxRequestSettings = new AcrolinxRequestSettings();
                        }
                      
                        string[] globalSessionMetaData = globalSession.Meta.Split(';');
                        globalSessionReportObject.Input = childSessions[0].Context;
                        globalSessionReportObject.Output = globalSession.FinalContext;
                        globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;
                        //rule name is the first position in the array.
                        globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;                        
                        globalSessionReportObject.MetaData.Language = string.Empty;
                        //user name is the second position in the array.
                        globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;

                        foreach (AcceptSession childSession in childSessions)
                        {
                            ChildSession session = new ChildSession();
                            List<AuditFlag> auditFlags = AuditManager.GetAllAuditFlagsBySessionId(childSession.SessionCodeId);
                            session.Context = acrolinxRequestSettings.Request;
                            foreach (AuditFlag auditFlag in auditFlags)
                            {
                                ReportFlag flag = new ReportFlag();
                                flag.Action = auditFlag.Action;
                                flag.ActionValue = auditFlag.ActionValue;                             
                                flag.Name = auditFlag.Name;
                                flag.FlagContext = auditFlag.Flag;
                                session.Results.Add(flag);
                            }

                            AuditApiRequest auditExternalApiResponseBody = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 3);                            
                            AcrolinxReport acrolinxReport = JsonConvert.DeserializeObject<AcrolinxReport>(auditExternalApiResponseBody.AuditContext);
                            session.ProviderResults.Add(acrolinxReport);                                                       
                            globalSessionReportObject.ChildSessions.Add(session);                           

                        }

                    }

                   globalSessionReportObjects.Add(globalSessionReportObject);
                }


                return new CoreApiCustomResponse(globalSessionReportObjects);
            }
            catch (Exception e)
            {
                throw (e);
            }



        }

        public CoreApiResponse SimpleGlobalSessionRange(DateTime start, DateTime end)
        {

            try
            {
                List<ApiGlobalSession> globalSessions = AcceptSessionManager.GetGlobalSessionRange(start, end);
                List<CoreApiGlobalSessionReportObject> globalSessionReportObjects = new List<CoreApiGlobalSessionReportObject>();

                foreach (ApiGlobalSession globalSession in globalSessions)
                {
                    List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);
                    CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();

                    if (childSessions != null && childSessions.Count > 0)
                    {
                        AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                        AcrolinxRequestSettings acrolinxRequestSettings;
                        try
                        {
                            acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                        }
                        catch
                        {
                            acrolinxRequestSettings = new AcrolinxRequestSettings();
                        }

                        string[] globalSessionMetaData = globalSession.Meta.Split(';');
                        
                        globalSessionReportObject.Input = childSessions[0].Context;
                        globalSessionReportObject.Output = globalSession.FinalContext;
                        globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;
                        //rule name is the first position in the array.
                        globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;
                        globalSessionReportObject.MetaData.Language = string.Empty;
                        //user name is the second position in the array.
                        globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;


                    }

                    globalSessionReportObjects.Add(globalSessionReportObject);
                }
                return new CoreApiCustomResponse(globalSessionReportObjects);
            }
            catch (Exception e)
            {
                throw (e);
            }



        }

        public CoreApiResponse GlobalSessionApiKey(string apiKey, DateTime? start, DateTime? end, string userSecretKey)
        {
            User u = AcceptUserManager.GetUserBySecretKey(userSecretKey);
            if (u == null)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return new AcceptApiException("Invalid User Secret", "GlobalSessionApiKey");
            }
            else
            {

                if (((from key in u.UserApiKeys
                      where key.ApiKey == apiKey
                      select key.ApiKey).Count()) == 0)
                {
                    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new AcceptApiException("Invalid User Api Key", "GlobalSessionApiKey");
                }
              
            }

          List<AcceptSession> distinctChildSessions;
                        
           if(start != null && end != null)
            distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinctRangedByDate(apiKey, start.GetValueOrDefault(), end.GetValueOrDefault());
           else
             distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinct(apiKey);

          try
          {
              List<CoreApiGlobalSessionReportObject> globalSessionReportObjects = new List<CoreApiGlobalSessionReportObject>();      

              foreach (AcceptSession s in distinctChildSessions)
              {

                  ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(s.SessionId);

                  if (globalSession != null)
                  {

                      List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);

                      CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();

                      if (childSessions != null && childSessions.Count > 0)
                      {
                          AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                          AcrolinxRequestSettings acrolinxRequestSettings;
                          try
                          {
                              acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                          }
                          catch
                          {
                              acrolinxRequestSettings = new AcrolinxRequestSettings();
                          }

                          string[] globalSessionMetaData = globalSession.Meta.Split(';');
                          //rule name is the first position in the array.
                          globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;  //globalSession.Meta;
                          //user name is the second position in the array.
                          globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;
                         
                          globalSessionReportObject.Output = globalSession.FinalContext;
                          globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;
                          globalSessionReportObject.MetaData.Language = acrolinxRequestSettings.CheckSettings != null ? acrolinxRequestSettings.CheckSettings.LanguageId : string.Empty;

                          try
                          {
                              globalSessionReportObject.GlobalStartTime = globalSession.StartTime.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                              globalSessionReportObject.GlobalEndTime = globalSession.EndTime.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");                              
                              childSessions[0].OriginHost.Remove(childSessions[0].OriginHost.IndexOf("&Host="), childSessions[0].OriginHost.Length - 1);                              
                              globalSessionReportObject.MetaData.OriginUrl = childSessions.Count > 0 ? globalSessionReportObject.MetaData.OriginUrl = childSessions[0].OriginHost.Replace("AbsoluteUri=","") : globalSessionReportObject.MetaData.OriginUrl = string.Empty;                              
                              globalSessionReportObject.Input = childSessions[0].Context;
                          }
                          catch
                          {
                              globalSessionReportObject.MetaData.OriginUrl = childSessions.Count > 0 ? globalSessionReportObject.MetaData.OriginUrl = childSessions[0].OriginHost.Replace("AbsoluteUri=", "") : globalSessionReportObject.MetaData.OriginUrl = string.Empty;
                              globalSessionReportObject.Input = childSessions[0].Context;
                          }

                          foreach (AcceptSession childSession in childSessions)
                          {
                              ChildSession session = new ChildSession();
                              List<AuditFlag> auditFlags = AuditManager.GetAllAuditFlagsBySessionId(childSession.SessionCodeId);                              
                              session.Context = childSession.Context;
                              List<AuditFlag> flagsWithSuggestions = (from o in auditFlags where o.Action == "accept_suggestion" select o).ToList<AuditFlag>();
                              List<AuditFlag> learntWords = (from o in auditFlags where o.Action == "learn_word" select o).ToList<AuditFlag>();
                              List<AuditFlag> removedLearntWords = (from o in auditFlags where o.Action == "remove_learn_word" select o).ToList<AuditFlag>();
                              List<AuditFlag> ignoredRules = (from o in auditFlags where o.Action == "ignore_rule" select o).ToList<AuditFlag>();
                              List<AuditFlag> removedIgnoredRules = (from o in auditFlags where o.Action == "remove_ignore_rule" select o).ToList<AuditFlag>();
                              List<AuditFlag> displayedTooltips = (List<AuditFlag>)(from o in auditFlags where o.Action == "display_tooltip" select o).ToList<AuditFlag>();
                              List<AuditFlag> displayedContextMenus = (from o in auditFlags where o.Action == "display_contextmenu" select o).ToList<AuditFlag>();
                                                                                     
                              #region LearntWords
                              try
                              {
                                  foreach (AuditFlag auditFlag in learntWords)
                                  {
                                      ReportFlag flag = new ReportFlag();
                                      flag.Action = auditFlag.Action;
                                      flag.Name = auditFlag.Name;                                                                        
                                      auditFlag.RawValue = auditFlag.RawValue.EndsWith("]") ? auditFlag.RawValue.Remove(auditFlag.RawValue.Length - 1, 1) : auditFlag.RawValue;
                                      auditFlag.RawValue = auditFlag.RawValue.StartsWith("[") ? auditFlag.RawValue.Remove(0, 1) : auditFlag.RawValue;
                                      flag.IndexStart = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().First()["StartPos"].ToString());
                                      flag.IndexEnd = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().Last()["EndPos"].ToString());                                      
                                      flag.ActionValue = string.Empty;
                                      
                                      try
                                      {
                                          flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                      }
                                      catch
                                      {
                                          flag.FlagContext = string.Empty;
                                      }

                                      session.Results.Add(flag);
                                  }

                              }
                              catch
                              {
                                  ReportFlag flag = new ReportFlag();
                                  flag.Action = "accept-LearntWords-error:" + childSession.SessionCodeId;
                                  session.Results.Add(flag);
                              }
                              #endregion                                                          
                              #region RemovedlearntWords
                              foreach (AuditFlag auditFlag in removedLearntWords)
                              {
                                    ClientReportFlag flag = new ClientReportFlag();
                                    flag.Action = auditFlag.Action;
                                    flag.Name = auditFlag.Name;
                                    flag.FlagContext = auditFlag.Flag;                                                                              
                                    flag.ActionValue = string.Empty;
                                    session.ClientResults.Add(flag);
                              }
                                #endregion
                              #region IgnoredRules
                              try{
                              foreach (AuditFlag auditFlag in ignoredRules)
                              {
                                  if (auditFlag.RawValue.EndsWith("]"))
                                  {
                                      List<dynamic> objects = JsonConvert.DeserializeObject<List<dynamic>>(auditFlag.RawValue);
                                      foreach (var f in objects)
                                      {
                                          ReportFlag flag = new ReportFlag();
                                          flag.Action = auditFlag.Action;
                                          flag.Name = f.uniqueId;
                                          flag.IndexStart = f.startpos;
                                          flag.IndexEnd = f.endpos;
                                          flag.ActionValue = string.Empty;
                                          try
                                          {
                                              flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                          }
                                          catch
                                          {
                                              flag.FlagContext = string.Empty;
                                          }
                                          session.Results.Add(flag);
                                      }

                                  }
                                  else
                                  {
                                      ReportFlag flag = new ReportFlag();
                                      flag.Action = auditFlag.Action;
                                      flag.Name = auditFlag.Name;
                                      //auditFlag.RawValue = auditFlag.RawValue.EndsWith("]") ? auditFlag.RawValue.Remove(auditFlag.RawValue.Length - 1, 1) : auditFlag.RawValue;
                                      //auditFlag.RawValue = auditFlag.RawValue.StartsWith("[") ? auditFlag.RawValue.Remove(0, 1) : auditFlag.RawValue;
                                      flag.IndexStart = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().First()["StartPos"].ToString());
                                      flag.IndexEnd = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().Last()["EndPos"].ToString());
                                      flag.ActionValue = string.Empty;
                                      try
                                      {
                                          flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                      }
                                      catch
                                      {
                                          flag.FlagContext = string.Empty;
                                      }
                                      session.Results.Add(flag);//session.IgnoredRules.Add(flag);
                                  }
                              }  
                              }
                              catch
                              {
                                  ReportFlag flag = new ReportFlag();
                                  flag.Action = "accept-IgnoredRules-error:" + childSession.SessionCodeId;
                                  session.Results.Add(flag);
                              }
                              #endregion
                              #region RemovedIgnoredRules
                              foreach (AuditFlag auditFlag in removedIgnoredRules)
                              {
                                  ClientReportFlag flag = new ClientReportFlag();
                                  flag.Action = auditFlag.Action;
                                  flag.Name = auditFlag.Name;                                                                  
                                  flag.ActionValue = string.Empty;
                                  flag.FlagContext = string.Empty;
                                  session.ClientResults.Add(flag);
                              }
                              #endregion
                              #region DisplayedTooltips 
                              try
                              {
                              var uniqueDisplayedTooltips = displayedTooltips.GroupBy(x => x.PrivateId);
                              foreach (var uniquePKey in uniqueDisplayedTooltips)
                              {
                                  if (learntWords.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0 && ignoredRules.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0 && flagsWithSuggestions.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0)
                                  {
                                      AuditFlag aux = (AuditFlag)uniqueDisplayedTooltips.SelectMany(n => n).Where(n => n.PrivateId == uniquePKey.Key).First();

                                      if (aux.RawValue.EndsWith("]"))
                                      {
                                          List<dynamic> objects = JsonConvert.DeserializeObject<List<dynamic>>(aux.RawValue);
                                          foreach (var f in objects)
                                          {
                                              ReportFlag flag = new ReportFlag();
                                              flag.Action = aux.Action;
                                              flag.Name = f.uniqueId;
                                              flag.IndexStart = f.startpos;
                                              flag.IndexEnd = f.endpos;
                                              flag.ActionValue = string.Empty;
                                              try
                                              {
                                                  flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                              }
                                              catch
                                              {
                                                  flag.FlagContext = string.Empty;
                                              }
                                              session.Results.Add(flag);
                                          }

                                      }
                                      else
                                      {
                                          ReportFlag flag = new ReportFlag();
                                          flag.Action = aux.Action;
                                          flag.Name = aux.Name;                                         
                                          flag.IndexStart = int.Parse(JObject.Parse(aux.RawValue)["contextpieces"].Children().First()["StartPos"].ToString());
                                          flag.IndexEnd = int.Parse(JObject.Parse(aux.RawValue)["contextpieces"].Children().Last()["EndPos"].ToString());
                                          flag.ActionValue = string.Empty;
                                          try
                                          {
                                              flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                          }
                                          catch
                                          {
                                              flag.FlagContext = string.Empty;
                                          }
                                          session.Results.Add(flag);
                                      }
                                  }
                              }
                              }catch
                              {
                                ReportFlag flag = new ReportFlag();
                                flag.Action = "accept-DisplayedTooltips-error:" + childSession.SessionCodeId;
                                session.Results.Add(flag);
                              }
                              #endregion
                              #region DisplayedContextMenus
                              
                              try{
                              var uniquedisplayedContextMenus = displayedContextMenus.GroupBy(x => x.PrivateId);
                           
                              foreach (var uniquePKey in uniquedisplayedContextMenus)
                              {
                                  if (learntWords.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0 && ignoredRules.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0 && flagsWithSuggestions.Select(n => n).Where(n => n.PrivateId == uniquePKey.Key).Count() == 0)
                                  {
                                      AuditFlag aux = (AuditFlag)uniquedisplayedContextMenus.SelectMany(n => n).Where(n => n.PrivateId == uniquePKey.Key).First();                                                                                                      
                                      if (aux.RawValue.EndsWith("]"))
                                      {
                                          List<dynamic> objects = JsonConvert.DeserializeObject<List<dynamic>>(aux.RawValue);                                         
                                          foreach (var f in objects)
                                          {
                                              ReportFlag flag = new ReportFlag();
                                              flag.Action = aux.Action;
                                              flag.Name = f.uniqueId;
                                              flag.IndexStart = f.startpos;
                                              flag.IndexEnd = f.endpos;
                                              flag.ActionValue = string.Empty;
                                              try
                                              {
                                                  flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                              }
                                              catch
                                              {
                                                  flag.FlagContext = string.Empty;
                                              }
                                              session.Results.Add(flag);
                                          }                                                                                
                                      }
                                      else
                                      {
                                          ReportFlag flag = new ReportFlag();
                                          flag.Action = aux.Action;
                                          flag.Name = aux.Name;
                                          flag.IndexStart = int.Parse(JObject.Parse(aux.RawValue)["contextpieces"].Children().First()["StartPos"].ToString());
                                          flag.IndexEnd = int.Parse(JObject.Parse(aux.RawValue)["contextpieces"].Children().Last()["EndPos"].ToString());
                                          flag.ActionValue = string.Empty;
                                          try
                                          {
                                              flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                          }
                                          catch
                                          {
                                              flag.FlagContext = string.Empty;
                                          }
                                          session.Results.Add(flag);//session.DisplayedContextMenus.Add(flag);
                                      }
                                  }
                              }
                               }catch
                              {
                                ReportFlag flag = new ReportFlag();
                                flag.Action = "accept-DisplayedContextMenus-error:(" + childSession.SessionCodeId + ")";
                                session.Results.Add(flag);
                              }
                              #endregion
                              #region FlagsWithSuggestions
                              try{
                                  foreach (AuditFlag auditFlag in flagsWithSuggestions)
                                  {

                                      if (auditFlag.RawValue.EndsWith("]"))
                                      {
                                          List<dynamic> objects = JsonConvert.DeserializeObject<List<dynamic>>(auditFlag.RawValue);
                                          foreach (var f in objects)
                                          {
                                              ReportFlag flag = new ReportFlag();
                                              flag.Action = auditFlag.Action;
                                              flag.Name = f.uniqueId;
                                              flag.IndexStart = f.startpos;
                                              flag.IndexEnd = f.endpos;
                                              flag.ActionValue = string.Empty;
                                              try
                                              {
                                                  flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                              }
                                              catch
                                              {
                                                  flag.FlagContext = string.Empty;
                                              }
                                              session.Results.Add(flag);
                                          }

                                      }
                                      else
                                      {
                                          ReportFlag flag = new ReportFlag();
                                          flag.Action = auditFlag.Action;
                                          flag.ActionValue = auditFlag.ActionValue;                                          
                                          flag.Name = auditFlag.Name;                                         
                                          flag.IndexStart = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().First()["StartPos"].ToString());
                                          flag.IndexEnd = int.Parse(JObject.Parse(auditFlag.RawValue)["contextpieces"].Children().Last()["EndPos"].ToString());
                                          try
                                          {
                                              flag.FlagContext = session.Context.Substring(flag.IndexStart, (flag.IndexEnd - flag.IndexStart));
                                          }
                                          catch
                                          {
                                              flag.FlagContext = string.Empty;
                                          }
                                          session.Results.Add(flag);
                                      }
                                  }
                                }catch
                                    {
                                        ReportFlag flag = new ReportFlag();
                                        flag.Action = "accept-FlagsWithSuggestions-error:" + childSession.SessionCodeId;
                                        session.Results.Add(flag);
                                    }
                              #endregion
                              #region FlagsWithoutSuggestions
                              try
                              {
                                  AuditApiRequest auditExternalApiResponseBody = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 3);
                                  AcrolinxReport acrolinxReport = JsonConvert.DeserializeObject<AcrolinxReport>(auditExternalApiResponseBody.AuditContext);
                                  session.ProviderResults.Add(acrolinxReport);                             
                              }
                              catch
                              {
                                  ReportFlag flag = new ReportFlag();
                                  flag.Action = "accept-FlagsWithoutSuggestions-error:" + childSession.SessionCodeId;
                                  session.ProviderResults.Add(flag);
                              }

                              #endregion
                          
                              globalSessionReportObject.ChildSessions.Add(session);

                          }

                      }

                      globalSessionReportObjects.Add(globalSessionReportObject);
                  }
                 

              }

              return new CoreApiCustomResponse(globalSessionReportObjects);
          }
          catch (Exception e)
          {
              throw (e);
          }




        }
        
        public CoreApiResponse SimpleGlobalSessionApiKey(string apiKey, DateTime? start, DateTime? end, string userSecretKey)
        {

            User u = AcceptUserManager.GetUserBySecretKey(userSecretKey);
            if (u == null)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return new AcceptApiException("Invalid User Secret", "GlobalSessionApiKey");
            }
            else
            {

                if (((from key in u.UserApiKeys
                      where key.ApiKey == apiKey
                      select key.ApiKey).Count()) == 0)
                {
                    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new AcceptApiException("Invalid User Api Key", "GlobalSessionApiKey");
                }

            }

            List<AcceptSession> distinctChildSessions;

            if (start != null && end != null)
                distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinctRangedByDate(apiKey, start.GetValueOrDefault(), end.GetValueOrDefault());
            else
                distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinct(apiKey);
            try
            {
                List<CoreApiGlobalSessionReportObject> globalSessionReportObjects = new List<CoreApiGlobalSessionReportObject>();

                foreach (AcceptSession s in distinctChildSessions)
                {
                    ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(s.SessionId);
                    if (globalSession != null)
                    {

                        List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);

                        CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();

                        if (childSessions != null && childSessions.Count > 0)
                        {
                            AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                            AcrolinxRequestSettings acrolinxRequestSettings;
                            try
                            {
                                acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                            }
                            catch
                            {
                                acrolinxRequestSettings = new AcrolinxRequestSettings();
                            }

                            string[] globalSessionMetaData = globalSession.Meta.Split(';');
                            //rule name is the first position in the array.
                            globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;  //globalSession.Meta;
                            //user name is the second position in the array.
                            globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;
                            globalSessionReportObject.Input = childSessions[0].Context;
                            globalSessionReportObject.Output = globalSession.FinalContext;
                            globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;                           
                            globalSessionReportObject.MetaData.Language = acrolinxRequestSettings.CheckSettings != null ? acrolinxRequestSettings.CheckSettings.LanguageId : string.Empty;
                           
                            globalSessionReportObject.GlobalStartTime = globalSession.StartTime.ToString();
                            globalSessionReportObject.GlobalEndTime = globalSession.EndTime.ToString();                                               
                        }                        
                        globalSessionReportObjects.Add(globalSessionReportObject);
                    }


                }              
                                                       
                return new CoreApiCustomResponse(globalSessionReportObjects);               
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public string SimpleGlobalSessionApiKeyCustom(string apiKey, DateTime? start, DateTime? end, string userSecretKey, string format)
        {

            User u = AcceptUserManager.GetUserBySecretKey(userSecretKey);
            if (u == null)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                throw new Exception("Invalid User Secret");
            }
            else
            {

                if (((from key in u.UserApiKeys
                      where key.ApiKey == apiKey
                      select key.ApiKey).Count()) == 0)
                {
                    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw new Exception("Invalid User Api Key");                   
                }

            }

            List<AcceptSession> distinctChildSessions;

            if (start != null && end != null)
                distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinctRangedByDate(apiKey, start.GetValueOrDefault(), end.GetValueOrDefault());
            else
                distinctChildSessions = AcceptSessionManager.GetChildSessionsByApiKeyDistinct(apiKey);

            try
            {
                List<CoreApiGlobalSessionReportObject> globalSessionReportObjects = new List<CoreApiGlobalSessionReportObject>();

                foreach (AcceptSession s in distinctChildSessions)
                {

                    ApiGlobalSession globalSession = AcceptSessionManager.GetGlobalSession(s.SessionId);

                    if (globalSession != null)
                    {

                        List<AcceptSession> childSessions = AcceptSessionManager.GetChildSessionByGlobalSessionId(globalSession.GlobalSessionId);

                        CoreApiGlobalSessionReportObject globalSessionReportObject = new CoreApiGlobalSessionReportObject();

                        if (childSessions != null && childSessions.Count > 0)
                        {
                            AuditApiRequest auditExternalApiRequestSettings = AuditManager.GetAuditApiRequestByTypeIdAndSessionId(childSessions[0].SessionCodeId, 1);
                            AcrolinxRequestSettings acrolinxRequestSettings;
                            try
                            {
                                acrolinxRequestSettings = Utils.AcceptApiWebUtils.FromJSON<AcrolinxRequestSettings>(new AcrolinxRequestSettings(), auditExternalApiRequestSettings.AuditContext);
                            }
                            catch
                            {
                                acrolinxRequestSettings = new AcrolinxRequestSettings();
                            }

                            string[] globalSessionMetaData = globalSession.Meta.Split(';');
                            //rule name is the first position in the array.
                            globalSessionReportObject.MetaData.RuleSet = globalSessionMetaData.Length > 0 ? globalSessionMetaData[0] : string.Empty;  //globalSession.Meta;
                            //user name is the second position in the array.
                            globalSessionReportObject.MetaData.User = globalSessionMetaData.Length > 1 ? globalSessionMetaData[1] : string.Empty; //string.Empty;
                            globalSessionReportObject.Input = childSessions[0].Context;
                            globalSessionReportObject.Output = globalSession.FinalContext;
                            globalSessionReportObject.MetaData.GlobalSessionId = globalSession.GlobalSessionId;                            
                            globalSessionReportObject.MetaData.Language = acrolinxRequestSettings.CheckSettings != null ? acrolinxRequestSettings.CheckSettings.LanguageId : string.Empty;
                            try
                            {
                                globalSessionReportObject.GlobalStartTime = globalSession.StartTime.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                globalSessionReportObject.GlobalEndTime = globalSession.EndTime.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                            }
                            catch
                            {                             
                            }
                        }
                        globalSessionReportObjects.Add(globalSessionReportObject);
                    }


                }

               StringBuilder builder; builder = new StringBuilder();
               if (format == "excel")
               {
                   string[] columnsHeader = { "GlobalSessionId", "RuleSet","User", "Language", "Input", "Output", "StartTime", "EndTime" };
                   builder.Append("<table border='" + "2px" + "'b>");
                   //write column headings.
                   builder.Append("<tr>");
                   foreach (string cheader in columnsHeader)
                   {
                       builder.Append("<td><b><font face=Arial size=2>" + cheader + "</font></b></td>");
                   }
                   builder.Append("</tr>");
                   //write table data.
                   foreach (var globalSession in globalSessionReportObjects)
                   {                     
                       builder.Append("<tr>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.MetaData.GlobalSessionId + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.MetaData.RuleSet + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.MetaData.User + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.MetaData.Language + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.Input + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.Output + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.GlobalStartTime + "</font></td>");
                       builder.Append("<td><font face=Arial size=" + "14px" + ">" + globalSession.GlobalEndTime + "</font></td>");
                       builder.Append("</tr>");
                   }
                   builder.Append("</table>");
               }
               else
               {
                   builder.Append("GlobalSessionId,RuleSet,User,Language,Input,Output,StartTime,EndTime");
                   string csvRow = "";
                   foreach (var globalSession in globalSessionReportObjects)
                   {
                       csvRow = globalSession.MetaData.GlobalSessionId + "," + globalSession.MetaData.RuleSet + "," + globalSession.MetaData.User + "," + globalSession.MetaData.Language + "," + globalSession.Input + "," + globalSession.Output + "," + globalSession.GlobalStartTime + "," + globalSession.GlobalEndTime;
                       builder.Append(csvRow);
                   }                              
               }
                                                         
                return builder.ToString();

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        #endregion

        #region Authentication

        public CoreApiResponse CreateUserKey(ApiKeyRequestObject requestObject)
        {
            try
            {
                if (requestObject.User != null && requestObject.User.Length > 0)
                {
                    ApiKeys key = AcceptUserManager.CreateApiKey(requestObject.User, requestObject.Dns, requestObject.Ip, requestObject.AplicationName, requestObject.Organization, requestObject.Description);
                    if (key != null && key.ApiKey.Length > 0)
                        return new ApiKeyResponseObject(new List<ApiKeys>() {key});
                }
                       
                return new CoreApiException("User is null or empty.", "CreateUserKey");
                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateUserKey");            
            }
        }

        public CoreApiResponse UpdateUserKey(ApiKeyRequestObject requestObject)
        {
            try
            {
                if (requestObject.User != null && requestObject.User.Length > 0)
                {
                    ApiKeys key = AcceptUserManager.UpdateApiKey(requestObject.User,requestObject.Key, requestObject.Dns, requestObject.Ip, requestObject.AplicationName,requestObject.Organization,requestObject.Description);
                    if (key != null && key.ApiKey.Length > 0)
                        return new ApiKeyResponseObject(new List<ApiKeys>() { key });
                }

                return new CoreApiException("User in not valid or key no longer exists.", "UpdateUserKey");

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateUserKey");
            }
        }

        public CoreApiResponse DeleteUserKey(ApiKeyRequestObject requestObject)
        {
            try
            {
                if (requestObject.User != null && requestObject.User.Length > 0)
                {
                    if (AcceptUserManager.DeleteApiKey(requestObject.User, requestObject.Key))
                        return new ApiKeyResponseObject( AcceptUserManager.GetAllApiKey(requestObject.User).ToList<ApiKeys>());                                            
                }

                return new CoreApiException("User is  null or empty.", "DeleteUserKey");

            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateUserKey");
            }
        }

        public CoreApiResponse GetUserKeys(ApiKeyRequestObject requestObject)
        {
            try
            {
                if (requestObject.User != null && requestObject.User.Length > 0)                
                    return new ApiKeyResponseObject(AcceptUserManager.GetAllApiKey(requestObject.User).ToList<ApiKeys>());

                return new CoreApiException("User is null or empty.", "GetUserKeys");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateUserKey");
            }
        }

        public CoreApiResponse GetKey(ApiKeyRequestObject requestObject)
        {
            try
            {
                if (requestObject.User != null && requestObject.User.Length > 0 && requestObject.Key != null && requestObject.Key.Length > 0)
                {
                  ApiKeys key = AcceptUserManager.GetKey(requestObject.User,requestObject.Key);
                  if (key != null)
                      return new ApiKeyResponseObject(new List<ApiKeys> { key });
                  else
                  {
                      return new CoreApiException("Key not found.", "GetKey");
                  }

                }

                return new CoreApiException("User or key is null or empty.", "GetKey");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateUserKey");
            }
        }

        public CoreApiResponse RecoverUserPassword(string userName)
        {
            try
            {
                string langUi = string.Empty;
                string confirmationCode = AcceptUserManager.RecoverUserPassword(userName,out langUi);
                if (confirmationCode.Length > 0)
                {
                    switch (langUi)
                    {
                        case "en-US": { EmailManager.SendConfirmationEmailForPasswordRecovery(AcceptApiCoreUtils.AcceptPortalEmailFrom, userName, confirmationCode, AcceptApiCoreUtils.AcceptPortalPasswordRecoveryEmailConfirmationAddress); } break;
                        case "de-DE": { EmailManager.SendConfirmationEmailForPasswordRecoveryGerman(AcceptApiCoreUtils.AcceptPortalEmailFrom, userName, confirmationCode, AcceptApiCoreUtils.AcceptPortalPasswordRecoveryEmailConfirmationAddress); } break;
                        case "fr-FR": { EmailManager.SendConfirmationEmailForPasswordRecoveryFrench(AcceptApiCoreUtils.AcceptPortalEmailFrom, userName, confirmationCode, AcceptApiCoreUtils.AcceptPortalPasswordRecoveryEmailConfirmationAddress); } break;
                        default:
                            {
                                EmailManager.SendConfirmationEmailForPasswordRecovery(AcceptApiCoreUtils.AcceptPortalEmailFrom, userName, confirmationCode, AcceptApiCoreUtils.AcceptPortalPasswordRecoveryEmailConfirmationAddress);

                            } break;
                    }
                    
                    return new CoreApiResponse();
                }
                else
                {
                    return new CoreApiException("Confirmation code is null or empty", "User Password Recovery");
                }
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Password Recovery");
            }

        }

        public CoreApiResponse ChangeUserPassword(string username, string password)
        {
            try
            {
                User user = AcceptUserManager.ChangeUserPassword(username, password);
                if (user != null)
                    return new CoreApiResponse();
                else
                    return new CoreApiException("Update user details failed, user updated is null.", "Update User");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Registration");
            }

        }

        public CoreApiResponse RegisterUser(string username, string password, string languageui)
        {
            try
            {
                string confirmationCode = AcceptUserManager.RegisterUser(username, password, languageui);

                if (confirmationCode.Length > 0)
                {

                    switch (languageui)
                    {
                        case "en-US": { EmailManager.SendConfirmationEmail(AcceptApiCoreUtils.AcceptPortalEmailFrom, username, confirmationCode, AcceptApiCoreUtils.AcceptPortalEmailConfirmationAddress); } break;
                        case "de-DE": { EmailManager.SendConfirmationEmailGerman(AcceptApiCoreUtils.AcceptPortalEmailFrom, username, confirmationCode, AcceptApiCoreUtils.AcceptPortalEmailConfirmationAddress); } break;
                        case "fr-FR": { EmailManager.SendConfirmationEmailFrench(AcceptApiCoreUtils.AcceptPortalEmailFrom, username, confirmationCode, AcceptApiCoreUtils.AcceptPortalEmailConfirmationAddress); } break;
                        default:
                        {
                            EmailManager.SendConfirmationEmail(AcceptApiCoreUtils.AcceptPortalEmailFrom, username, confirmationCode, AcceptApiCoreUtils.AcceptPortalEmailConfirmationAddress);
                        
                        }break;
                    }

                    #region DS:12-01-2015
                    //protecting from adding a new user to a non-existing post-edit demo project.

                    List<Project> demoProjects = null;
                    try
                    {
                        demoProjects = ProjectManagerService.GetDemoProjectsByAdminTokens(new string[] {AcceptApiCoreUtils.AcceptFrenchToEnglishPostEditDemoProjectToken, 
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanPostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptFrenchToEnglishCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToFrenchCollaborativePostEditDemoProjectToken,
                                              AcceptApiCoreUtils.AcceptEnglishToGermanCollaborativePostEditDemoProjectToken});
                    }
                    catch { demoProjects = new List<Project>(); }

                    if (demoProjects.Count > 0)
                    {
                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptEnglishToFrenchPostEditDemoProjectToken);
                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptFrenchToEnglishPostEditDemoProjectToken);
                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptEnglishToGermanPostEditDemoProjectToken);

                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptEnglishToFrenchCollaborativePostEditDemoProjectToken);
                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptFrenchToEnglishCollaborativePostEditDemoProjectToken);
                        ProjectManagerService.AddUserToProject(username, AcceptApiCoreUtils.AcceptEnglishToGermanCollaborativePostEditDemoProjectToken);
                    }
                    #endregion 



                    return new CoreApiResponse();
                }
                else
                {
                    return new CoreApiException("Confirmation code is null or empty.", "User Registration");
                }
            }
            catch (Exception e)
            {


                if (e.InnerException != null)
                    return new CoreApiException(e.Message + " inner ex: " + e.InnerException.Message, "User Registration");
                else
                    return new CoreApiException(e.Message, "User Registration for: " + username);

            }

        }

        public CoreApiResponse AuthenticateUser(string username, string password)
        {
            try
            {
                User s = AcceptUserManager.GetUser(username, password);
                if (s != null)
                {
                    if (s.SecretKeyCode == null || s.SecretKeyCode.Length == 0)
                    {
                        s.SecretKeyCode = AcceptFramework.Business.Utils.StringUtils.GenerateTinyHash(username + password + DateTime.UtcNow.ToString());
                        AcceptUserManager.UpdateUser(s);
                    }
                   
                    return new UserResponseObject(s.UserName, s.UILanguage,s.Roles.ToArray()[0].UniqueName);


                }
                else
                    return new CoreApiException("Invalid user credentials.", "User Authentication");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Authentication");
            }
        }

        public CoreApiResponse UserSecretKey(string username)
        {
            try
            {
                User s = AcceptUserManager.GetUserByUserName(username);
                if (s != null)
                {
                    if (s.SecretKeyCode == null || s.SecretKeyCode.Length == 0)
                    {
                        s.SecretKeyCode = AcceptFramework.Business.Utils.StringUtils.GenerateTinyHash(username + s.Password + DateTime.UtcNow.ToString());
                        AcceptUserManager.UpdateUser(s);
                    }

                    return new CoreApiCustomResponse(s.SecretKeyCode);

                }
                else
                    return new CoreApiException("Invalid user credentials.", "UserSecretKey");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UserSecretKey");
            }
        }

        public CoreApiResponse GetRole(string username)
        {
            try
            {
                                   
                User u = AcceptUserManager.GetUserByUserName(username);
                if (u != null)
                {                   
                    return new CoreApiCustomResponse(u.Roles);
                }
                else
                    return new CoreApiException("User Is Null", "Get User Roles");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Authentication");
            }
        }

        public CoreApiResponse AddRole(string username, string role)
        {
            try
            {

                User u = AcceptUserManager.AddRoleToUser(username,role);
                if (u != null)              
                    return new CoreApiCustomResponse(u.Roles);                
                else
                    return new CoreApiException("User Not Updated", "Add User Role");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Authentication");
            }
        }

        public CoreApiResponse AuthenticateUserByConfirmationCode(string code)
        {
            try
            {
                User user = AcceptUserManager.GetUser(code);
                if (user != null && user.ConfirmationCode != null)
                {
                    user.ConfirmationCode = null;
                    user = AcceptUserManager.UpdateUser(user);
                    //handle user pending invitations
                    List<ProjectInvitation> pendingInvitations = ProjectManagerService.GetProjectInvitationsByUserName(user.UserName);
                    if (pendingInvitations != null )
                    {
                        foreach (ProjectInvitation invitation in pendingInvitations)
                        {
                            ProjectManagerService.AddUserToProject(invitation.UserName, invitation.ProjectId);
                            //already a user so need to update the type of invitation.
                            invitation.Type = 1; 
                            ProjectManagerService.UpdateInvitation(invitation);                        
                        }                                                            
                    }
                    //handle user pending invitations.
                    return new UserResponseObject(user.UserName, user.UILanguage);
                }
                else
                    if (user != null && user.PasswordRecoveryCode != null)
                    {
                        user.PasswordRecoveryCode = null;
                        user = AcceptUserManager.UpdateUser(user);                        
                        return new UserResponseObject(user.UserName, user.UILanguage);
                    }
                    else
                        return new CoreApiException("Invalid user registration code.", "User Authentication");
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "User Authentication");
            }
        }

        public CoreApiResponse GetUser(int Id)
        {

            try
            {
                return new CoreApiCustomResponse(AcceptUserManager.GetUser(Id));
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetUser");
            }
        
        }

        #endregion
       
    }
}

