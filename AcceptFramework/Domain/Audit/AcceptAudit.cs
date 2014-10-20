using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class AcceptAudit : DomainBase
    {
        public virtual string SessionCodeId { get; set; }
        public virtual int AuditTypeId { get; set; }        
        public virtual string AuditContext { get; set; }       
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }

        public AcceptAudit()
        { 
           SessionCodeId = string.Empty;
           AuditTypeId = (int)AcceptAuditType.NotSet;          
           AuditContext = string.Empty;          
           StartTime = DateTime.UtcNow;
           EndTime = DateTime.MaxValue;        
        }
        
        public AcceptAudit(string sessioncodeid)
        {
           SessionCodeId = sessioncodeid;
           AuditTypeId = (int)AcceptAuditType.NotSet;          
           AuditContext = string.Empty;
           StartTime = DateTime.UtcNow;
           EndTime = DateTime.MaxValue;        
        }

        public override void Validate()
        {
            if(this.AuditTypeId == (int)AcceptAuditType.NotSet)
            throw new ArgumentException("Audit Type Identifier Not Set", "AudiTypeId");
            //base.Validate();
        }
    }
}
