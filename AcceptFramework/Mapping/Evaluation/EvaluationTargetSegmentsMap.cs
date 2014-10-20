using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationTargetSegmentsMap : ClassMap<EvaluationTargetSegment>
    {
        public EvaluationTargetSegmentsMap()
        {
            Table("EvaluationTargetSegment");

            Id(e => e.Id);
            Map(e => e.TargetString).Length(1000);
            Map(e => e.WordCount);
            Map(e => e.Length);
            Map(e => e.GTM);
            Map(e => e.BLEU);
            Map(e => e.TER);
            Map(e => e.Meteor);
            Map(e => e.EditDistance);
            Map(e => e.SpellCheckFlags);
            Map(e => e.IQScore);

            References(e => e.SourceParagraph).Not.LazyLoad().Column("ParagraphID");
            References(e => e.ReferenceSegment).Not.LazyLoad().Column("ReferenceSegmentID");
            References(e => e.SourceSegment).Not.LazyLoad().Column("SourceSegmentID");
            HasManyToMany(x => x.TargetScorings)
                .Table("EvaluationTargetSegmentScoring")
                .ParentKeyColumn("SegmentID")
                .ChildKeyColumn("ScoringID")
                .Cascade.SaveUpdate();
        }
    }
}
