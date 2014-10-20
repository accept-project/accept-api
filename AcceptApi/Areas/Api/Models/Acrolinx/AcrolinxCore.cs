using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Domain.Session;
using AcceptApi.Areas.Api.Models.Utils;
using AcceptFramework.Interfaces.Session;
using AcceptFramework.Interfaces.Audit;
using AcceptFramework.Domain.Audit;
using AcceptFramework.Mapping.Audit;
using System.Globalization;
using System.Xml;
using AcceptApi.Areas.Api.Models.Cache;
using System.IO;
using System.Web.Configuration;

namespace AcceptApi.Areas.Api.Models.Acrolinx
{
    public class AcrolinxCore : CoreApiObject
    {
        private string _serverId;
        private string _username;
        private string _password;
        private string _authtoken;
        private string _signature;
        private string _session;
        private string _responsestatus;
        private string _codenumber;
        private string _acrolinxresponse;
        private string _textcontent;
        private AcrolinxRequestSettings _requestSettingsObject;
        private AcrolinxResponseStatus _acrolinxresponseStatus;
        private string _requestSettingsJson;

        public AcrolinxCore(AcceptSession acceptSession, IAuditManager acceptAuditManager)
            : base(acceptSession, acceptAuditManager)
        {
            _serverId = string.Empty;
            _username = string.Empty;
            _password = string.Empty;
            _authtoken = string.Empty;
            _signature = string.Empty;
            _session = string.Empty;
            _responsestatus = string.Empty;
            _codenumber = string.Empty;
            _textcontent = string.Empty;
            _requestSettingsObject = null;
            _requestSettingsJson = string.Empty;
            _acrolinxresponse = string.Empty;
            _acrolinxresponseStatus = null;
            _requestSettingsObject = null;
        }

        public AcrolinxCore()
            : base(null, null)
        {
            _serverId = string.Empty;
            _username = string.Empty;
            _password = string.Empty;
            _authtoken = string.Empty;
            _signature = string.Empty;
            _session = string.Empty;
            _responsestatus = string.Empty;
            _codenumber = string.Empty;
            _textcontent = string.Empty;
            _requestSettingsObject = null;
            _requestSettingsJson = string.Empty;
            _acrolinxresponse = string.Empty;
            _acrolinxresponseStatus = null;
            _requestSettingsObject = null;
        }
           
        private CoreApiResponse HandleAcrolinxRequestStatus()
        {
            string acrolinxresponse = ParseAcrolinxResponseStatus(_responsestatus);
            switch (acrolinxresponse)
            {
                case AcrolinxResponseStatusTypes.Waiting: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.Waiting, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase
                case AcrolinxResponseStatusTypes.Done: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.Done, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase, this.AcceptSession.SessionCodeId, this._codenumber
                case AcrolinxResponseStatusTypes.RunningProcessing: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.RunningProcessing, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase , this.AcceptSession.SessionCodeId, this._codenumber
                case AcrolinxResponseStatusTypes.RunningPostProcessing: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.RunningPostProcessing, this.AcceptSession.SessionCodeId); } break;// this._acrolinxresponseStatus.PercentCurrentRunningPhase , this.AcceptSession.SessionCodeId, this._codenumber
                case AcrolinxResponseStatusTypes.RunningPreProcessing: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.RunningPreProcessing, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase, , this.AcceptSession.SessionCodeId, this._codenumber
                case AcrolinxResponseStatusTypes.Failed: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.Failed, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase , this.AcceptSession.SessionCodeId, this._codenumber
                default: { this.AcceptApiResponseManager.ResponseStatus = new AcceptApiResponseStatus(AcrolinxResponseStatusTypes.Failed, this.AcceptSession.SessionCodeId); } break;//this._acrolinxresponseStatus.PercentCurrentRunningPhase , this.AcceptSession.SessionCodeId, this._codenumber
            }
            base.AppendAcceptSessionCachedValues(this._session);
            base.AppendAcceptSessionCachedValues(this._codenumber);           
            return this.AcceptApiResponseManager.ResponseStatus;
        }

        public string ParseAcrolinxResponseStatus(string jsonresponse)
        {
            if (_acrolinxresponseStatus == null)
                _acrolinxresponseStatus = AcceptApiWebUtils.FromJSON<AcrolinxResponseStatus>(_acrolinxresponseStatus, jsonresponse);

            return _acrolinxresponseStatus.State;
        }

        public override void BuildStyleRequestSettings(Dictionary<string, string> settings)
        {
            _requestSettingsObject = new AcrolinxRequestSettings();
            _requestSettingsObject.Request = this._textcontent;
            _requestSettingsObject.SessionId = this._session;
            _requestSettingsObject.CheckPriority = "INTERACTIVE";
            _requestSettingsObject.CheckReportFormats = new string[] { "JSON", "XML" };
            _requestSettingsObject.RequestFormat = "TEXT";
            _requestSettingsObject.RequestedCheckResultTypes = new string[] { "CHECK_REPORT" };
            _requestSettingsObject.ClientLocale = "en";
            _requestSettingsObject.RequestDescription = new RequestDescription(string.Empty, "null", string.Empty, "TEXT", true, string.Empty);
            _requestSettingsObject.CheckSettings = new CheckSettings(settings["lang"], settings["rule"], new string[] { "STYLE" }, new string[] { });

            this._requestSettingsJson = AcceptApiWebUtils.ToJSON<AcrolinxRequestSettings>(_requestSettingsObject);
        }      

        private string ConvertToUTF8String(string s_unicode)
        {
            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;
            string s_unicode2 = string.Empty;
            //convert a string to utf-8 bytes.
            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(s_unicode);
            //convert utf-8 bytes to a string.
            return  s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);	
        }

        public void BuildGenericAcrolinxRequestSettings(string languageid, string rulesetname, string[] flagtypes, string[] termSetNames, string checkpriority, string[] reportformats, string requestformat, string[] requestedresulttypes, string clientlocale, string acrolinxSessionId)
        {
            _requestSettingsObject = new AcrolinxRequestSettings();            
            _requestSettingsObject.Request = this._textcontent;
            //the default break line character within the acrolinx system is pre-defined to be the character "\u0F0E". 
            _requestSettingsObject.Request = _requestSettingsObject.Request.Replace("\n", "\u0F0E");                 
            _requestSettingsObject.SessionId = acrolinxSessionId;
            _requestSettingsObject.CheckPriority = checkpriority.Length == 0 ? _requestSettingsObject.CheckPriority = "INTERACTIVE" : _requestSettingsObject.CheckPriority = checkpriority;
            _requestSettingsObject.CheckReportFormats = reportformats.Length == 0 ? _requestSettingsObject.CheckReportFormats = new string[] { "JSON", "XML" } : _requestSettingsObject.CheckReportFormats = reportformats;
            #region enforce text format
            //the format used to communicate with the acrolinx endpoint is text - the HTML strip process is performed by the ACCEPT client.
            _requestSettingsObject.RequestFormat = "TEXT";
            //requestformat.Length == 0 ? _requestSettingsObject.RequestFormat = "TEXT" : _requestSettingsObject.RequestFormat = requestformat;
            #endregion
            _requestSettingsObject.RequestedCheckResultTypes = requestedresulttypes.Length == 0 ? _requestSettingsObject.RequestedCheckResultTypes = new string[] { "CHECK_REPORT" } : _requestSettingsObject.RequestedCheckResultTypes = requestedresulttypes;
            _requestSettingsObject.ClientLocale = clientlocale.Length == 0 ? _requestSettingsObject.ClientLocale = "en" : _requestSettingsObject.ClientLocale = clientlocale;
            _requestSettingsObject.RequestDescription = new RequestDescription(string.Empty, "null", string.Empty, _requestSettingsObject.RequestFormat, true, string.Empty);
            _requestSettingsObject.CheckSettings = new CheckSettings(languageid, rulesetname, flagtypes, termSetNames);            
            this._requestSettingsJson = AcceptApiWebUtils.ToJSON_UTF8<AcrolinxRequestSettings>(_requestSettingsObject);            
        }

        public CoreApiResponse ReuseAcrolinxGenericeRequest(Dictionary<string, string> parameters)
        {
            StartAudits();
            List<string> flags = new List<string>();
            AuditApiRequest acrolinxGenericRequestAudit = new AuditApiRequest();
            acrolinxGenericRequestAudit.SessionCodeId = AcceptSession.SessionCodeId;
            this.BuildAuthenticationRequestSettings(parameters);
            #region set checking types

            if (parameters["grammar"] == "1")
                flags.Add("GRAMMAR");

            if (parameters["spell"] == "1")
                flags.Add("SPELLING");

            if (parameters["style"] == "1")
                flags.Add("STYLE");

            #endregion            
            this._session = parameters["lastValidAcrolinxSession"];
            this._codenumber = parameters["lastValidAcrolinxCode"];
            this.BuildGenericAcrolinxRequestSettings(parameters["languageid"], parameters["ruleset"], flags.ToArray(), new string[] { }, string.Empty, new string[] { "JSON" }, parameters["requestFormat"], new string[] { }, string.Empty, this._session);//this.BuildGrammarRequestSettings();                                                                  
            this._codenumber = RequestCodeNumber(this._session, this._textcontent);
            if (this._codenumber.Length == 0)
                return null;            
            this._responsestatus = GetAcrolinxResponseStatus(this._session, this._codenumber);            
            acrolinxGenericRequestAudit.EndTime = DateTime.UtcNow;
            acrolinxGenericRequestAudit.AuditTypeId = (int)AcceptAuditType.AcrolinxRequestPayload;
            acrolinxGenericRequestAudit.AuditContext = this._requestSettingsJson;
            AppendAuditApiRequest(acrolinxGenericRequestAudit);
            SaveAudits();
            return this.HandleAcrolinxRequestStatus();                     
        }
       
        public CoreApiResponse  AcrolinxGenericRequest(Dictionary<string, string> parameters)
        {           
            StartAudits();
            AuditApiRequest acrolinxGenericRequestAudit = new AuditApiRequest();
            acrolinxGenericRequestAudit.SessionCodeId = AcceptSession.SessionCodeId;
            this.BuildAuthenticationRequestSettings(parameters);                        
            this._authtoken = GetAuthenticationToken(this._username, this._password);                        
            this._signature = GetSignatureChallenge(this._authtoken);            
            this._session = RequestSession(this._authtoken);                        
            List<string> flags = new List<string>();
            #region set checking types
            if (parameters["grammar"] == "1")
                flags.Add("GRAMMAR");
            if (parameters["spell"] == "1")
                flags.Add("SPELLING");
            if (parameters["style"] == "1")
                flags.Add("STYLE");            
            #endregion
            #region manage acrolinx session - check if last one is still up and rebuild settings object.
            this.BuildGenericAcrolinxRequestSettings(parameters["languageid"], parameters["ruleset"], flags.ToArray(), new string[] { }, string.Empty, new string[] { "JSON" }, parameters["requestFormat"], new string[] { }, string.Empty, this._session);                                                     
            #endregion
            this._codenumber = RequestCodeNumber(this._session, this._textcontent);               
            this._responsestatus = GetAcrolinxResponseStatus(this._session, this._codenumber);          
            acrolinxGenericRequestAudit.EndTime = DateTime.UtcNow;
            acrolinxGenericRequestAudit.AuditTypeId = (int)AcceptAuditType.AcrolinxRequestPayload;
            acrolinxGenericRequestAudit.AuditContext = this._requestSettingsJson;
            AppendAuditApiRequest(acrolinxGenericRequestAudit);                        
            SaveAudits();
            return this.HandleAcrolinxRequestStatus();
        }

        public string GetAcrolinxServerLanguages()
        {
            string result = string.Empty;
            try
            {               
                HttpWebRequest req = WebRequest.Create(AcceptApiCoreUtils.FullAcrolinxServerLanguagesPath) as HttpWebRequest;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Method = "GET";
                req.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.75 Safari/535.7";
                WebHeaderCollection collection; collection = new WebHeaderCollection();
                collection.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                collection.Add("Accept-Encoding", "gzip,deflate,sdch");
                collection.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                collection.Add("clientLocale", "en");
                req.Headers = collection;
                result = AcceptApiWebUtils.GetHttpWebResponse(req);
                return result;

            }
            catch (Exception e)
            {
                throw (e);
            }
        }     

        public override CoreApiResponse  GetResponse(IDictionary<string, string> parameters)
        {           
            StartAudits();
            AuditApiRequest acrolinxGetRawResultsAudit = new AuditApiRequest();
            acrolinxGetRawResultsAudit.SessionCodeId = AcceptSession.SessionCodeId;
            string rawresponse; rawresponse = string.Empty;
            AcrolinxResponse responseObj; responseObj = null;
            string jsonReportRaw; jsonReportRaw = string.Empty;
            AcrolinxReport reportObj; reportObj = null;
            //get JSON with flags.
            rawresponse = GetFinalResultDetails(parameters["sessionid"], parameters["code"]);                                         
            acrolinxGetRawResultsAudit.EndTime = DateTime.UtcNow;
            acrolinxGetRawResultsAudit.AuditTypeId = (int)AcceptAuditType.AcrolinxRawResult;
            acrolinxGetRawResultsAudit.AuditContext = rawresponse;
            AppendAuditApiRequest(acrolinxGetRawResultsAudit); 
            responseObj = AcceptApiWebUtils.FromJSON<AcrolinxResponse>(responseObj, rawresponse);                  
            AuditApiRequest acrolinxJsonResultAudit = new AuditApiRequest();
            acrolinxJsonResultAudit.SessionCodeId = AcceptSession.SessionCodeId;            
            jsonReportRaw = GetAcrolinxJsonReport(responseObj.CheckReportJsonUrl);                        
            acrolinxJsonResultAudit.EndTime = DateTime.UtcNow;
            acrolinxJsonResultAudit.AuditTypeId = ((int)AcceptAuditType.AcrolinxResponse);
            acrolinxJsonResultAudit.AuditContext = jsonReportRaw;
            AppendAuditApiRequest(acrolinxJsonResultAudit);
            reportObj = AcceptApiWebUtils.FromJSON<AcrolinxReport>(reportObj, jsonReportRaw);
            //wrap acrolinx json response in the accept response format.
            ParseAcrolinxFlags(reportObj);            
            this.AcceptApiResponseManager.AppendResultSetList(this.AcceptApiResponseManager.ResultSet);
            this.AcceptApiResponseManager.MarkResultSetComplete();
                        
          
            AuditApiRequest acceptJsonResultAudit = new AuditApiRequest();
            acceptJsonResultAudit.SessionCodeId = AcceptSession.SessionCodeId;

            acceptJsonResultAudit.EndTime = DateTime.UtcNow;
            acceptJsonResultAudit.AuditTypeId = (int)AcceptAuditType.AcceptResponse;
            acceptJsonResultAudit.AuditContext = this.AcceptResponseObjectToJson();

            AppendAuditApiRequest(acceptJsonResultAudit);

            SaveAudits();

            return this.AcceptApiResponseManager.ResponseObject;           
        }

        public CoreApiResponse GetResponseStatus(Dictionary<string, string> parameters)
        {          
            this._session = parameters["sessionid"];
            this._codenumber = parameters["code"];            
            StartAudits();
            AuditApiRequest acrolinxGenericRequestStatusAudit = new AuditApiRequest();
            acrolinxGenericRequestStatusAudit.SessionCodeId = AcceptSession.SessionCodeId;
            this._responsestatus = GetAcrolinxResponseStatus(_session, _codenumber);
            acrolinxGenericRequestStatusAudit.EndTime = DateTime.UtcNow;
            acrolinxGenericRequestStatusAudit.AuditTypeId = (int)AcceptAuditType.AcrolinxResponseStatus;
            acrolinxGenericRequestStatusAudit.AuditContext = this._responsestatus;
            AppendAuditApiRequest(acrolinxGenericRequestStatusAudit);
            SaveAudits();
            return HandleAcrolinxRequestStatus();
        }

        private string GetAcrolinxRuleDescription(string ruleDescriptionUrl, out string ruleUniqueIdentifier)
        {
            string ruleDescription; ruleDescription = string.Empty;
            string documentName = string.Empty;
            ruleUniqueIdentifier = string.Empty;
          
            try
            {
                if(ruleDescriptionUrl.Contains("/de/rules"))
                    documentName = "Acrolinx_de";
                else
                 if(ruleDescriptionUrl.Contains("/fr/rules"))
                    documentName = "Acrolinx_fr";
                else
                    if (ruleDescriptionUrl.Contains("/en/rules"))
                    documentName = "Acrolinx_en";

                if (CacheStore.Exists<XDocument>(documentName))
                {
                    XDocument xDoc = CacheStore.Get<XDocument>(documentName);
                    string ruleName = ruleDescriptionUrl.Substring(ruleDescriptionUrl.IndexOf("help/"));
                    XElement element = (from xml in xDoc.Descendants("rule")
                                        where xml.Element("title").Value == ruleName
                                        select xml.Element("summary")).FirstOrDefault();
                    if (element != null)
                        ruleDescription = element.Value;
                    else
                    {
                        ruleDescription = GetRuleDescription(ruleDescriptionUrl);
                        xDoc.Root.Add(new XElement("rule", new XElement("title", ruleName), new XElement("summary", ruleDescription)));
                        try
                        {
                            string tempFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["AcrolinxDocumentsPath"] + documentName + ".xml");
                            xDoc.Save(tempFile, SaveOptions.None);                                                                      
                        }
                        catch 
                        { 
                        
                        }
                    }

                    try
                    {
                        ruleUniqueIdentifier = ruleName.Split('/')[1].Split('.')[0];
                    }
                    catch
                    {
                        ruleUniqueIdentifier = "NA";
                    }
                }
                else
                {                  
                    //TODO: Load File ???
                }
            }
            catch
            {
                return string.Empty;        
            }
        

            return ruleDescription;
        }

        private void ParseAcrolinxFlags(AcrolinxReport reportObj)
        {
            ResultSet resultSet = new ResultSet();
            List<Result> lstResultsForResultSet = new List<Result>();
           
            if (reportObj.Report == null)
                reportObj.Report = new Report();

            foreach (Flag f in reportObj.Report.Flags)
            {
                Result result = new Result();
                List<string> acrolinxsuggestions; acrolinxsuggestions = new List<string>();
                result.Header.Type = f.Type;                
                result.Header.ContextType = 1;
                string ruleUniqueIdentifier = string.Empty;
                result.Header.Description = GetAcrolinxRuleDescription(f.Help, out ruleUniqueIdentifier);
                result.Header.UniqueId = ruleUniqueIdentifier;
                if (f.Description != null)
                {
                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    result.Header.Rule = myTI.ToTitleCase(f.Description.Replace('_', ' '));                    
                }                                         

                foreach (Sugestion s in f.Suggestions)
                    acrolinxsuggestions.Add(s.Surface);

                result.Body.Suggestion = acrolinxsuggestions.ToArray();
                result.Body.StartPos = f.PositionalInformation.Matches[0].Begin;
                result.Body.EndPos = f.PositionalInformation.Matches[f.PositionalInformation.Matches.Length - 1].End;

                if (f.PositionalInformation.Matches.Length > 0)
                {
                    List<ContextPiece> contextpieces = new List<ContextPiece>();
                    if (f.PositionalInformation.Matches.Length == 1)
                    {
                        ContextPiece c = new ContextPiece(f.PositionalInformation.Matches[0].Part,f.PositionalInformation.Matches[0].Begin,f.PositionalInformation.Matches[0].End);                        
                        contextpieces.Add(c);
                        result.Body.Context += f.PositionalInformation.Matches[0].Part;                       
                    }
                    else
                    {
                        if (f.Type == "STYLE")
                        {
                            result.Header.ContextType = 2;
                            result.Body.Context = string.Empty;

                            for (int i = 0; i < f.PositionalInformation.Matches.Length; i++)
                            {
                                ContextPiece cp = new ContextPiece(f.PositionalInformation.Matches[i].Part, f.PositionalInformation.Matches[i].Begin, f.PositionalInformation.Matches[i].End);
                                contextpieces.Add(cp);
                            }
                         
                        }
                        else
                        {
                            for (int i = 0; i < f.PositionalInformation.Matches.Length; i++)
                            {
                                //old approach.
                                result.Body.Context += f.PositionalInformation.Matches[i].Part;                               
                                if (i != 0 && f.PositionalInformation.Matches[i].Part.Length > 1 && f.PositionalInformation.Matches[i - 1].Part.Length >= 1 && f.PositionalInformation.Matches[i - 1].Part != " ") 
                                    result.Header.ContextType = 2; 
                                                              
                                ContextPiece cp = new ContextPiece(f.PositionalInformation.Matches[i].Part, f.PositionalInformation.Matches[i].Begin, f.PositionalInformation.Matches[i].End);
                                contextpieces.Add(cp);
                            }

                            if (result.Header.ContextType == 2)                            
                                result.Body.Context = string.Empty;                                                         
                        }
                    }

                    result.Body.ContextPieces = contextpieces.ToArray();

                }

                lstResultsForResultSet.Add(result);
            }
            resultSet.Results = lstResultsForResultSet.ToArray();
            this.AcceptApiResponseManager.ResultSet.Add(resultSet);
        }

        private string GetRuleDescription(string link)
        {
            try
            {               
               XDocument xDoc = XDocument.Parse(AcceptApiWebUtils.GetHttpWebResponse(AcceptApiWebUtils.CreateRequest(link, new WebHeaderCollection(), "GET", "text/xml", "*/*")));              
               var reader = xDoc.Element("errorHelp").Element("error").Element("description").Element("shortText").CreateReader();
               reader.MoveToContent();
               return reader.ReadInnerXml();               
            }
            catch
            {
                return string.Empty;             
            }        
        }

        public override void BuildAuthenticationRequestSettings(Dictionary<string, string> parameters)
        {
            this._textcontent = parameters["text"];
            this._username = parameters["username"];
            this._password = parameters["password"];
        }

        public AcrolinxServerCapabilitiesObject GetAcrolinxCapabilityObject(string language)
        {
            try
            {
                AcrolinxServerCapabilitiesObject obj = new AcrolinxServerCapabilitiesObject();
                string jsonRaw = AcrolinxServerCapabilities(language);
                obj = AcceptApiWebUtils.FromJSON<AcrolinxServerCapabilitiesObject>(obj, jsonRaw);
                return obj;
            }
            catch (Exception e)
            {                
                throw new Exception(e.Message);
            }
            
        }


        #region Private Methods

        private string AcrolinxServerId()
        {
            return AcceptApiWebUtils.GetHttpWebResponse(AcceptApiWebUtils.CreateRequest(AcceptApiCoreUtils.FullAcrolinxServerIdRestPath));
        }

        private string AcrolinxServerCapabilities(string language)
        {
            return AcceptApiWebUtils.GetHttpWebResponse(AcceptApiWebUtils.CreateRequest(string.Format(AcceptApiCoreUtils.FullAcrolinxServerCapabilitiesRestPath,language)));
        }

        private string AcrolinxCheckIfUserSelfRegistrationEnabled()
        {
            return AcceptApiWebUtils.GetHttpWebResponse(AcceptApiWebUtils.CreateRequest(AcceptApiCoreUtils.FullAcrolinxUserSelfRegistrationPath));
        }

        private string CreateUser(string username, string password)
        {
            WebHeaderCollection requestheader = new WebHeaderCollection();
            requestheader.Add("username", username);
            requestheader.Add("password", password);
            HttpWebRequest request = AcceptApiWebUtils.CreateRequest(AcceptApiCoreUtils.FullAcrolinxCreateUserPath, requestheader, "PUT", "application/json", "*/*");
            return AcceptApiWebUtils.GetHttpWebResponse(request);
        }

        private string GetAuthenticationToken(string username, string encryptedOrPlaintextPassword)
        {
            WebHeaderCollection requestheader = new WebHeaderCollection();
            requestheader.Add("username", username);
            requestheader.Add("encryptedOrPlaintextPassword", encryptedOrPlaintextPassword);
            HttpWebRequest request = AcceptApiWebUtils.CreateRequest(AcceptApiCoreUtils.FullAcrolinxAuthenticationTokenPath, requestheader, "POST", "application/json", "*/*");
            return AcceptApiWebUtils.GetHttpWebResponse(request);
        }

        private string GetSignatureChallenge(string authToken)
        {
            WebHeaderCollection requestheader; requestheader = new WebHeaderCollection();
            requestheader.Add("authToken", authToken);
            HttpWebRequest request = AcceptApiWebUtils.CreateRequest(AcceptApiCoreUtils.FullAcrolinxSignatureTokenPath, requestheader, "POST", "application/json", "*/*");
            return AcceptApiWebUtils.GetHttpWebResponse(request);
        }

        private string RequestSession(string authToken)
        {
            WebHeaderCollection requestheader = new WebHeaderCollection();
            requestheader.Add("authToken", authToken);
            requestheader.Add("ContentType", "application/json");
            return AcceptApiWebUtils.PostJson(AcceptApiCoreUtils.FullAcrolinxRequestSessionPath, AcceptApiCoreUtils.AcrolinxRequestSessionBody, requestheader);
        }

        /// <summary>
        /// builds the settings (JSON format) used in acrolinx api call = all required features are defined at this time.
        /// </summary>
        /// <returns></returns>
        private string BuildJsonBodyForAcrolinxCodeNumberRequest(string sessionId, string inputText)
        {
            return string.Format(AcceptApiCoreUtils.AcrolinxCodeNumberBody, inputText, sessionId);
        }

        private string RequestCodeNumber(string sessionId, string inputText)
        {
            return AcceptApiWebUtils.PostJson(string.Format(AcceptApiCoreUtils.AcrolinxCodeNumberPath, sessionId), this._requestSettingsJson, "POST", "application/json");
        }

        private string GetAcrolinxResponseStatus(string sessionId, string codenum)
        {
            HttpWebRequest request = AcceptApiWebUtils.CreateRequest(string.Format(AcceptApiCoreUtils.AcrolinxBeforeFinalResultPath, sessionId, codenum), new WebHeaderCollection(), "GET", "application/json", "*/*");           
            return AcceptApiWebUtils.GetHttpWebResponse(request);

        }

        private string GetFinalResultDetails(string sessionId, string codenum)
        {
            string jsonResponseRaw = AcceptApiWebUtils.GetJson(string.Format(AcceptApiCoreUtils.AcrolinxFinalResultPath, sessionId, codenum), string.Empty, new WebHeaderCollection(), "application/json", "*/*");                       
            return jsonResponseRaw;

        }

        public string GetAcrolinxJsonReport(string reportUrl)
        {
            return AcceptApiWebUtils.GetJson(reportUrl, string.Empty);
        }
       
        #endregion

        #region SpellCheck

        public CoreApiResponse SpellCheckRequestTest(Dictionary<string, string> parameters)
        {            
            this.BuildAuthenticationRequestSettings(parameters);
            this._authtoken = GetAuthenticationToken(this._username, this._password);
            this._signature = GetSignatureChallenge(this._authtoken);
            this._session = RequestSession(this._authtoken);
            this.BuildSpellCheckRequestSettings(parameters);
            this._codenumber = RequestCodeNumber(this._session, this._textcontent);
            this._responsestatus = GetAcrolinxResponseStatus(this._session, this._codenumber);
            return HandleAcrolinxRequestStatus();
        }

        public override void BuildSpellCheckRequestSettings(Dictionary<string, string> settings)
        {
            _requestSettingsObject = new AcrolinxRequestSettings();
            _requestSettingsObject.Request = this._textcontent;
            _requestSettingsObject.SessionId = this._session;
            _requestSettingsObject.CheckPriority = "INTERACTIVE";
            _requestSettingsObject.CheckReportFormats = new string[] { "JSON", "XML" };
            _requestSettingsObject.RequestFormat = "TEXT";
            _requestSettingsObject.RequestedCheckResultTypes = new string[] { "CHECK_REPORT" };
            _requestSettingsObject.ClientLocale = "en";
            _requestSettingsObject.RequestDescription = new RequestDescription(string.Empty, "null", string.Empty, "TEXT", true, string.Empty);
            _requestSettingsObject.CheckSettings = new CheckSettings(settings["lang"], settings["rule"], new string[] { "SPELLING" }, new string[] { });
            this._requestSettingsJson = AcceptApiWebUtils.ToJSON<AcrolinxRequestSettings>(_requestSettingsObject);
        }

        #endregion

        #region Grammar

        public override CoreApiResponse GrammarRequest(Dictionary<string, string> parameters)
        {
            this.BuildAuthenticationRequestSettings(parameters);
            this._authtoken = GetAuthenticationToken(this._username, this._password);
            this._signature = GetSignatureChallenge(this._authtoken);
            this._session = RequestSession(this._authtoken);
            this.BuildGrammarRequestSettings(parameters);
            this._codenumber = RequestCodeNumber(this._session, this._textcontent);
            this._responsestatus = GetAcrolinxResponseStatus(this._session, this._codenumber);
            return HandleAcrolinxRequestStatus();          
        }

        public override void BuildGrammarRequestSettings(Dictionary<string, string> settings)
        {
            _requestSettingsObject = new AcrolinxRequestSettings();
            _requestSettingsObject.Request = this._textcontent;
            _requestSettingsObject.SessionId = this._session;
            _requestSettingsObject.CheckPriority = "INTERACTIVE";
            _requestSettingsObject.CheckReportFormats = new string[] { "JSON", "XML" };
            _requestSettingsObject.RequestFormat = "TEXT";
            _requestSettingsObject.RequestedCheckResultTypes = new string[] { "CHECK_REPORT" };
            _requestSettingsObject.ClientLocale = "en";
            _requestSettingsObject.RequestDescription = new RequestDescription(string.Empty, "null", string.Empty, "TEXT", true, string.Empty);
            _requestSettingsObject.CheckSettings = new CheckSettings(settings["lang"], settings["rule"], new string[] { "GRAMMAR" }, new string[] { });
            this._requestSettingsJson = AcceptApiWebUtils.ToJSON<AcrolinxRequestSettings>(_requestSettingsObject);
        }

        #endregion Grammar

        #region Style

        public override CoreApiResponse StyleCheckRequest(Dictionary<string, string> parameters)
        {
            this.BuildAuthenticationRequestSettings(parameters);
            this._authtoken = GetAuthenticationToken(this._username, this._password);
            this._signature = GetSignatureChallenge(this._authtoken);
            this._session = RequestSession(this._authtoken);
            this.BuildStyleRequestSettings(parameters);
            this._codenumber = RequestCodeNumber(this._session, this._textcontent);
            this._responsestatus = GetAcrolinxResponseStatus(this._session, this._codenumber);
            return HandleAcrolinxRequestStatus();
        }

        #endregion









    }
}