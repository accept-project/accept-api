using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Interfaces.Audit
{
    public interface IAuditManager
    {
        void InsertAudit(AuditApiRequest audit);

        void InsertActionAudit(AuditUserAction audit);

        void InsertAuditFlag(AuditFlag auditFlag);

        List<AuditFlag> GetAllAuditFlagsBySessionId(string sessionId);

        List<AuditApiRequest> GetAllApiAuditRequestsBySessionId(string sessionId);

        AuditApiRequest GetAuditApiRequestByTypeIdAndSessionId(string sessionId, int auditTypeId);

        PageVisitAudit InsertPageVisitAudit(PageVisitAudit pageVisitAudit);
    }
}
