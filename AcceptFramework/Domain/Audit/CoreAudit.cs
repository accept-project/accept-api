using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class CoreAudit: DomainBase
    {
        public virtual CoreAuditType Type { get; set; }        
        public virtual CoreAuditAction Action { get; set; }
        public virtual DateTime? TimeStamp {get; set;}
        public virtual string Description { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Meta { get; set; }
        
        public CoreAudit()
        {
            this.Type = CoreAuditType.NotSet;
            this.Action = CoreAuditAction.NotSet;
            this.TimeStamp = null;
            this.Description = string.Empty;
            this.Origin = string.Empty;
            this.Meta = string.Empty;    
        }

        public CoreAudit(CoreAuditType type, CoreAuditAction action, DateTime timeStamp, string description, string origin, string meta)
        {
            this.Type = type;
            this.Action = action;
            this.TimeStamp = timeStamp;
            this.Description = description;
            this.Meta = meta;
            this.Origin = origin;
        }

    }
}
