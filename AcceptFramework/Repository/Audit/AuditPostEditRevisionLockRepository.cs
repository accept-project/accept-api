using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Repository.Audit
{
    internal static class AuditPostEditRevisionLockRepository
    {

        public static IEnumerable<AuditPostEditRevisionLock> GetAll()
        {
            return new RepositoryBase<AuditPostEditRevisionLock>().Select();
        }

        public static void Insert(AuditPostEditRevisionLock record)
        {
            new RepositoryBase<AuditPostEditRevisionLock>().Create(record);
        }

        public static IEnumerable<AuditPostEditRevisionLock> GetAllByAction(string action)
        {
            return new RepositoryBase<AuditPostEditRevisionLock>().Select(a => a.UserAction == action);
        }
    
            
    }
}
