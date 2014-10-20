using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{

    public class AudiType : DomainBase
    {
        public virtual string Name { get; set; }
        public virtual string Context { get; set; }        
    }

    public enum AcceptAuditType
    {
        NotSet = -1,
        AcrolinxRequestPayload = 1,
        AcrolinxRawResult = 2,
        AcrolinxResponse = 3,
        AcceptResponse = 4,
        AcrolinxResponseStatus = 5,
        AcceptResponseStatus = 6,
        AcceptRuleUsed = 7,
        AcceptRuleIgnored = 8,        
        OriginalAndFinalText = 9,                
    }

  


}
