using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationParagraphScoringMap : ClassMap<EvaluationParagraphScoring>
    {
        public EvaluationParagraphScoringMap()
        {
            Table("EvaluationParagraphScoring");

            Id(e => e.Id);
            Map(e => e.Completed);
            Map(e => e.CompletionTime);

            References(e => e.SourceParagraph).Column("ParagraphID");
            References(e => e.Scoring).Column("ScoringID");
        
        }
    }
}
