using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Session
{
    public class AcceptSession : DomainBase
    {            
        public virtual string SessionId { get; set; }
        public virtual string SessionCodeId { get; set; }
        public virtual string ApiKey { get; set; }
        public virtual string OriginIp { get; set; }
        public virtual string OriginHost { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
        public virtual string RequestedUrl { get; set; }
        public virtual string CachedValues { get; set; }
        public virtual string Context { get; set; }

        public AcceptSession() 
        { 
            this.SessionId = string.Empty; 
            this.StartTime = DateTime.UtcNow; 
            this.SessionCodeId = string.Empty;
            this.EndTime = DateTime.MaxValue;
            this.RequestedUrl = string.Empty;
            this.CachedValues = string.Empty;
            this.ApiKey = string.Empty;
            this.OriginHost = string.Empty;
            this.OriginIp = string.Empty;
            this.Context = string.Empty;
        }

      
        public AcceptSession(string sessionid, string sessioncode, DateTime start, DateTime end, string requestedurl, string cachedvalues, string apiKey, string originHost, string originIp, string context)
        { 
            this.SessionId = sessionid; 
            this.StartTime = start;
            this.EndTime = end;
            this.SessionCodeId = sessioncode;
            this.RequestedUrl = requestedurl;
            this.CachedValues = cachedvalues;
            this.ApiKey = apiKey;
            this.OriginHost = originHost;
            this.OriginIp = originIp;
            this.Context = context;
        }    



    }
}
