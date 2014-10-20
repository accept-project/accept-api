using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class PageVisitAudit: CoreAudit
    {
        public virtual string UserName { get; set; }
        public virtual string Language { get; set; }
        public virtual string UserAgent { get; set; }

        public PageVisitAudit()
            : base()
        {
            this.UserName = string.Empty;
            this.Language = string.Empty;
            this.UserAgent = string.Empty;
        }

        public PageVisitAudit(string userName, string description, DateTime timeStamp, CoreAuditType type, CoreAuditAction action, string origin, string meta, string language, string userAgent)
            : base(type, action, timeStamp, description, origin, meta)
        {
            this.UserName = userName;
            this.Language = language;
            this.UserAgent = userAgent;
        }
        
    }
}
