using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationSourceSegmentsMap : ClassMap<EvaluationSourceSegment>
    {
        public EvaluationSourceSegmentsMap()
        {
            Table("EvaluationSourceSegment");

            Id(e => e.Id);
            Map(e => e.SourceString).Length(1000);
            Map(e => e.WordCount);
            Map(e => e.Length);
            Map(e => e.IQScore);
            Map(e => e.SpellCheckFlags);

            References(e => e.SourceParagraph).Not.LazyLoad().Column("ParagraphID");
            
            HasManyToMany(x => x.SourceScorings)
                .Table("EvaluationSourceSegmentScoring")
                .ParentKeyColumn("SegmentID")
                .ChildKeyColumn("ScoringID")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.BilingualScorings)
                .Table("EvaluationBilingualSegmentScoring")
                .ParentKeyColumn("SegmentID")
                .ChildKeyColumn("ScoringID")
                .Cascade.SaveUpdate();
        }
    }
}
