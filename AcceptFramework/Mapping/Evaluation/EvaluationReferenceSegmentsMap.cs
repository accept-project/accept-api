using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationReferenceSegmentsMap : ClassMap<EvaluationReferenceSegment>
    {
        public EvaluationReferenceSegmentsMap()
        {
            Table("EvaluationReferenceSegment");

            Id(e => e.Id);
            Map(e => e.ReferenceString).Length(1000);

            References(e => e.Contributor).Not.LazyLoad().Column("ContributorID");

            HasMany(x => x.TargetSegments).Cascade.All().KeyColumn("ReferenceSegmentID");
        }
    }
}
