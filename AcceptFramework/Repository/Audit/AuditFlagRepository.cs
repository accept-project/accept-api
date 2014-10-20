using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Repository.Audit
{
    internal static class AuditFlagRepository
    {

        public static IEnumerable<AuditFlag> GetAll()
        {
            return new RepositoryBase<AuditFlag>().Select();
        }

        public static void Insert(AuditFlag record)
        {
            new RepositoryBase<AuditFlag>().Create(record);
        }

        public static IEnumerable<AuditFlag> GetAllBySessionId(string sessionId)
        {
            return new RepositoryBase<AuditFlag>().Select(a => a.SessionCodeId == sessionId);
        }
       
                    
    }
}
