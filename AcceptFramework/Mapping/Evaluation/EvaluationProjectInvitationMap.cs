using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    #region EvaluationInternal
    public class EvaluationProjectInvitationMap : ClassMap<EvaluationProjectInvitation>
    {
        public EvaluationProjectInvitationMap()
        {
            Table("EvaluationProjectInvitation");
            Id(e => e.Id);
            Map(e => e.ProjectId);
            Map(e => e.UserName);
            Map(e => e.InvitationDate);
            Map(e => e.ConfirmationCode);
            Map(e => e.ConfirmationDate);

            Map(e => e.Type);
        }
    }
    #endregion
}
