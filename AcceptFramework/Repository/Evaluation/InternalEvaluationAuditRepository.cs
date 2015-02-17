using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    #region EvaluationInternal
    internal class InternalEvaluationAuditRepository
    {
        public static InternalEvaluationAudit Insert(InternalEvaluationAudit record)
        {
            return new RepositoryBase<InternalEvaluationAudit>().Create(record);
        }

        public static IEnumerable<InternalEvaluationAudit> GetAllByProjectTokenAndUser(string token, string user, int status)
        {            
            return new RepositoryBase<InternalEvaluationAudit>().Select(a => a.ProjectToken == token && a.UserName == user);
        }

        public static IEnumerable<InternalEvaluationAudit> GetAllByProjectTokenAndUserAndMetadata(string token, string user, string meta)
        {          
            return new RepositoryBase<InternalEvaluationAudit>().Select(a => a.ProjectToken == token && a.UserName == user && a.Meta == meta);
        }

    }
    #endregion
}
