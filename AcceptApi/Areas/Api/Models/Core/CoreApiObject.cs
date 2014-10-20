using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Domain.Session;
using AcceptFramework.Interfaces.Audit;
using AcceptApi.Areas.Api.Models.Utils;
using AcceptFramework.Interfaces.Session;
using AcceptFramework.Domain.Audit;
using AcceptFramework.Business.Utils;

namespace AcceptApi.Areas.Api.Models.Core
{
    public class CoreApiObject
    {
       
        private AcceptApiResponseManager _acceptApiResponseManager;      
        private IAuditManager _acceptAuditManager;
        private List<AuditApiRequest> _auditApiRequests;
        private AcceptSession _acceptSession;

        public AcceptSession AcceptSession
        {
            get { return _acceptSession; }
            set { _acceptSession = value; }
        }

        public IAuditManager AcceptAuditManager
        {
            get { return _acceptAuditManager; }
            set { _acceptAuditManager = value; }
        }
             
        public AcceptApiResponseManager AcceptApiResponseManager
        {
            get { return _acceptApiResponseManager; }
            set { _acceptApiResponseManager = value; }
        }

        public CoreApiObject(AcceptSession acceptSession, IAuditManager acceptAuditManager)
        {
            _acceptApiResponseManager = new AcceptApiResponseManager();            
            _acceptAuditManager = acceptAuditManager;
            _acceptSession = acceptSession;      
        }

        public void StartAcceptSession(HttpContext context)
        {
            this._acceptSession.StartTime = DateTime.UtcNow;
            this._acceptSession.SessionId = context.Session.SessionID;
            this._acceptSession.RequestedUrl = context.Request.Url.AbsoluteUri;
            this._acceptSession.SessionCodeId = StringUtils.GenerateTinyHash(string.Format("{0}?t={1}", context.Request.Url, DateTime.UtcNow));
            if (context.Request.UrlReferrer != null)
            this._acceptSession.OriginHost = "AbsoluteUri=" + context.Request.UrlReferrer.AbsoluteUri + "&Host=" + context.Request.UrlReferrer.Host + "&Authority=" + context.Request.UrlReferrer.Authority;                        
            this._acceptSession.OriginIp = AcceptApiWebUtils.GetHostRequesterIpAddress();            
        } 
       
        public void StartAcceptSession(HttpContext context, string ApiKey, string ieHttpReferer)
        {
            this._acceptSession.StartTime = DateTime.UtcNow;
            this._acceptSession.SessionId = context.Session.SessionID;
            this._acceptSession.RequestedUrl = context.Request.Url.AbsoluteUri;
            this._acceptSession.SessionCodeId = StringUtils.GenerateTinyHash(string.Format("{0}?t={1}", context.Request.Url, DateTime.UtcNow));
            
            if(context.Request.UrlReferrer != null)
            this._acceptSession.OriginHost = "AbsoluteUri=" + context.Request.UrlReferrer.AbsoluteUri + "&Host=" + context.Request.UrlReferrer.Host + "&Authority=" + context.Request.UrlReferrer.Authority;
            else
                this._acceptSession.OriginHost = "AbsoluteUri=" + string.Empty + "&Host=" + ieHttpReferer + "&Authority=" + string.Empty;
            
            this._acceptSession.OriginIp = AcceptApiWebUtils.GetHostRequesterIpAddress();
            this._acceptSession.ApiKey = ApiKey;
            this._acceptSession.OriginIp = this._acceptSession.OriginIp + "&HTTP_REFERER=" + context.Request.UrlReferrer;
        }

        public void StartRealTimeAcceptSession(string realtimeSessionId, string requestedUrl, string index)
        {
            this._acceptSession.StartTime = DateTime.UtcNow;
            this._acceptSession.SessionId = realtimeSessionId;
            this._acceptSession.RequestedUrl = requestedUrl;
            this._acceptSession.SessionCodeId = StringUtils.GenerateTinyHash(requestedUrl+DateTime.UtcNow+realtimeSessionId+index);           
            this._acceptSession.OriginHost = "AbsoluteUri=" + "N.A" + "&Host=" + "N.A" + "&Authority=" + "N.A";
            this._acceptSession.OriginIp = "N.A.";
        }
        public void StartRealTimeAcceptSession(string realtimeSessionId, string requestedUrl, string apiKey, string index)
        {
            this._acceptSession.StartTime = DateTime.UtcNow;
            this._acceptSession.SessionId = realtimeSessionId;
            this._acceptSession.RequestedUrl = requestedUrl;
            this._acceptSession.SessionCodeId = StringUtils.GenerateTinyHash(string.Format("{0}?t={1}", requestedUrl, DateTime.UtcNow));

            if (requestedUrl != null)
                this._acceptSession.OriginHost = "AbsoluteUri=" + "N.A" + "&Host=" + "N.A" + "&Authority=" + "N.A";

            this._acceptSession.OriginIp = "N.A";
            this._acceptSession.ApiKey = apiKey;
        }



        public AcceptSession EndSession()
        {
            this._acceptSession.EndTime = DateTime.UtcNow;
            return this._acceptSession;
        }
   
        public void StartAudits()
        {        
            this._auditApiRequests = new List<AuditApiRequest>();
        }

        public void AppendAuditApiRequest(AuditApiRequest auditApiRequest)
        {
            this._auditApiRequests.Add(auditApiRequest);        
        }

        public void SaveAudits()
        {
            for (int i = 0; i < this._auditApiRequests.Count; i++)
                this._acceptAuditManager.InsertAudit(_auditApiRequests[i]);
        }
    
        public virtual CoreApiResponse GrammarRequest(Dictionary<string, string> parameters)
        {
            return new CoreApiResponse();
        }

        public virtual CoreApiResponse SpellCheckRequest(Dictionary<string, string> parameters)
        {
            return new CoreApiResponse();
        }

        public virtual CoreApiResponse StyleCheckRequest(Dictionary<string, string> parameters)
        {
            return new CoreApiResponse();
        }
                       
        public virtual CoreApiResponse GetResponse(IDictionary<string, string> parameters)
        {
            return this._acceptApiResponseManager.ResponseObject;
        }

        public string AcceptResponseObjectToJson()
        {
            return _acceptApiResponseManager.ResponseObject.ToJSON<AcceptApiResponse>();
        }

        public void AppendAcceptSessionCachedValues(string val)
        {
            this._acceptSession.CachedValues += this._acceptSession.CachedValues.Length == 0 ? this._acceptSession.CachedValues += val : this._acceptSession.CachedValues = "," + val;
        }
       
        #region Core Configuration Methods

        public virtual void BuildAuthenticationRequestSettings(Dictionary<string, string> parameters)
        {        
        }

        public virtual void BuildSpellCheckRequestSettings(Dictionary<string, string> settings)
        {        
        }

        public virtual void BuildGrammarRequestSettings(Dictionary<string, string> settings)
        {           
        }

        public virtual void BuildStyleRequestSettings(Dictionary<string, string> settings)
        {            
        }
                
        #endregion
    }
}