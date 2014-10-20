using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class AuditPostEditRevisionLock: DomainBase
    {
        public virtual string UserAction { get; set; }
        public virtual string UserName { get; set; }
        public virtual string TextId { get; set; }
        public virtual string Meta { get; set; }
        public virtual DateTime ActionTimeStamp { get; set; }


        public AuditPostEditRevisionLock()
        {
            this.UserAction = string.Empty;
            this.UserName = string.Empty;
            this.Meta = string.Empty;
            this.ActionTimeStamp = DateTime.UtcNow;
            this.TextId = string.Empty;
        }

        public AuditPostEditRevisionLock(string action, string user, string meta, DateTime timeStamp, string textId) {
            this.UserAction = action;
            this.UserName = user;
            this.Meta = meta;
            this.ActionTimeStamp = timeStamp;
            this.TextId = textId;
        }
    }
}
