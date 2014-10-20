using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Audit;

namespace AcceptFramework.Repository.Audit
{
    internal static class PageVisitRepository
    {
        public static IEnumerable<PageVisitAudit> GetAll()
        {
            return new RepositoryBase<PageVisitAudit>().Select();
        }

        public static PageVisitAudit Insert(PageVisitAudit record)
        {
            return new RepositoryBase<PageVisitAudit>().Create(record);
        }

     
        
    }
}
