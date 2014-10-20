using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Repository.Audit
{
    internal static class UserAuditRepository
    {

        public static IEnumerable<AuditUserAction> GetAll()
        {
            return new RepositoryBase<AuditUserAction>().Select();
        }

        public static void Insert(AuditUserAction record)
        {
            new RepositoryBase<AuditUserAction>().Create(record);
        }

        public static IEnumerable<AuditUserAction> GetBetween(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return GetAll();
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.UtcNow.AddYears(10);
            }

            if (!startDate.HasValue)
            {
                startDate = DateTime.UtcNow.AddYears(-10);
            }

            return
                new RepositoryBase<AuditUserAction>().Select(
                    s => s.StartTime >= startDate && s.EndTime <= endDate);
        } 
    
    
    
    }
}
