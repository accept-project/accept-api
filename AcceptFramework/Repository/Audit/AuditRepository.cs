using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Repository.Audit
{
    internal static class AuditRepository
    {
        public static IEnumerable<AuditApiRequest> GetAll()
        {
            return new RepositoryBase<AuditApiRequest>().Select();
        }

        public static void Insert(AuditApiRequest record)
        {
            new RepositoryBase<AuditApiRequest>().Create(record);
        }

        public static IEnumerable<AuditApiRequest> GetBetween(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return GetAll();
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.AddYears(10);
            }

            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddYears(-10);
            }

            return
                new RepositoryBase<AuditApiRequest>().Select(
                    s => s.StartTime >= startDate && s.EndTime <= endDate);
        }


        public static IEnumerable<AuditApiRequest> GetAllBySessionId(string sessionId)
        {
            return new RepositoryBase<AuditApiRequest>().Select(a=>a.SessionCodeId == sessionId);
        }

        public static AuditApiRequest GetAuditApiRequestByTypeIdAndSessionId(int typeId, string sessionId)
        {
            return new RepositoryBase<AuditApiRequest>().Select(a => a.SessionCodeId == sessionId && a.AuditTypeId == typeId).FirstOrDefault();
        }

    }
}
