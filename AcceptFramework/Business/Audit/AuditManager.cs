using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces;
using AcceptFramework.Domain.Audit;
using AcceptFramework.Repository.Audit;
using AcceptFramework.Interfaces.Audit;

namespace AcceptFramework.Business.Audit
{
    internal class AuditManager: IAuditManager
    {
       
        public AuditManager()
        {            
            //TODO: create all audit type objects(currently types are not used...).
        }
      
        public void InsertAudit(AuditApiRequest audit)
        {
            AuditRepository.Insert(audit);        
        }
    

        public void InsertActionAudit(AuditUserAction audit)
        {
            UserAuditRepository.Insert(audit);        
        }

        public void InsertAuditFlag(AuditFlag auditFlag)
        {
            auditFlag.Validate();
            AuditFlagRepository.Insert(auditFlag);
        }


        public List<AuditFlag> GetAllAuditFlagsBySessionId(string sessionId)
        {
            return AuditFlagRepository.GetAllBySessionId(sessionId).ToList();
        }

        public List<AuditApiRequest> GetAllApiAuditRequestsBySessionId(string sessionId)
        {
            return AuditRepository.GetAllBySessionId(sessionId).ToList();
        }

        public AuditApiRequest GetAuditApiRequestByTypeIdAndSessionId(string sessionId, int auditTypeId)
        {
            return AuditRepository.GetAuditApiRequestByTypeIdAndSessionId(auditTypeId, sessionId);     
        }

        public PageVisitAudit InsertPageVisitAudit(PageVisitAudit pageVisitAudit)
        {
            return PageVisitRepository.Insert(pageVisitAudit);
        }

    }
}
