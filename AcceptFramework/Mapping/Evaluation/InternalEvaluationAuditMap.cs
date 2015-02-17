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
    public class InternalEvaluationAuditMap : ClassMap<InternalEvaluationAudit>
    {
        public InternalEvaluationAuditMap()
        {
            Table("InternalEvaluationAudit");

            Id(e => e.Id);
            Map(e => e.UserName).Length(250);
            Map(e => e.ProjectToken).Length(250);
            Map(e => e.InternalEvaluationStatus);
            Map(e => e.Meta).Length(2500);
        }

    }
    #endregion
}
