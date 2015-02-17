using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptFramework.Domain.Evaluation
{
    #region EvaluationInternal
    public class InternalEvaluationAudit : DomainBase
    {
        public virtual string ProjectToken { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual int InternalEvaluationStatus { get; set; }
        public virtual string Meta { get; set; }

        public InternalEvaluationAudit()
        {
            this.ProjectToken = string.Empty;
            this.UserName = string.Empty;
            this.CreationDate = DateTime.MinValue;
            this.InternalEvaluationStatus = -1;
            this.Meta = string.Empty;
        }

        public InternalEvaluationAudit(string token, string user, DateTime date, int status, string meta)
        {
            this.ProjectToken = token;
            this.UserName = user;
            this.CreationDate = date;
            this.InternalEvaluationStatus = status;
            this.Meta = meta;
        }
    }
    #endregion
}
