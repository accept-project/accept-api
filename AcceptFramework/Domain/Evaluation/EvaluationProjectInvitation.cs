using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptFramework.Domain.Evaluation
{
    #region EvaluationInternal
    public class EvaluationProjectInvitation : DomainBase
    {
        public virtual int ProjectId { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime InvitationDate { get; set; }
        public virtual string ConfirmationCode { get; set; }
        public virtual DateTime ConfirmationDate { get; set; }
        public virtual int Type { get; set; }

        public EvaluationProjectInvitation()
        {
            this.InvitationDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            this.ConfirmationDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            this.ProjectId = -1;
            this.UserName = string.Empty;
            this.Type = 0;
        }
    }
    #endregion
}
