using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class AuditApiRequest: AcceptAudit
    {                
        public AuditApiRequest()
        :base()
        {
                       
        }

        public AuditApiRequest(string sessioncodeid)
        :base(sessioncodeid)
        {
           
        }

    }
}
