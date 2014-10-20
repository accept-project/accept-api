using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Session
{
    public class ApiGlobalSession: DomainBase
    {
        public virtual string GlobalSessionId { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string Meta { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
        public virtual string FinalContext { get; set; }

        public ApiGlobalSession()
        {
            this.GlobalSessionId = string.Empty;
            this.SessionId = string.Empty;
            this.StartTime = DateTime.UtcNow;
            this.EndTime = null;
            this.Meta = string.Empty;            
            this.FinalContext = string.Empty;
        }

        public ApiGlobalSession(string globalSessionId, string sessionId, DateTime startTime, DateTime endTime, string meta, string finalContext)
        {
            this.GlobalSessionId = globalSessionId;
            this.SessionId = sessionId;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Meta = meta;                      
            this.FinalContext = finalContext;
        }



    }
}
