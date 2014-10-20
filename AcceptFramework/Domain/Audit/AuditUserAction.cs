using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class AuditUserAction : AcceptAudit   
    {   
        public virtual string RuleName { get; set; }       
        public virtual string TextAfter { get; set; }
        public virtual string TextBefore { get; set; }
        
        public AuditUserAction()
        :base()
        {
            RuleName = string.Empty;
            TextAfter = string.Empty;
            TextBefore = string.Empty;                 
        }

    }
}
