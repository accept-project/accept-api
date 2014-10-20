using System;
using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationDocumentUserStatusesMap : ClassMap<EvaluationDocumentUserStatus>
    {
        public EvaluationDocumentUserStatusesMap()
        {
            Table("EvaluationDocumentUserStatuses");

            Id(e => e.Id);

            Map(e => e.Status).Column("Status").CustomType(typeof(Int32));
            Map(e => e.CompletionDate);
            Map(e => e.CompletionTime);
            Map(e => e.Direction).Column("Direction").CustomType(typeof(Int32));

            References(e => e.User).Column("UserID");
            References(e => e.Document).Column("DocumentID");
        }
    }
}