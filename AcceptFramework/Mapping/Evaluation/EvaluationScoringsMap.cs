using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationScoringsMap : ClassMap<EvaluationScoring>
    {
        public EvaluationScoringsMap()
        {
            Table("EvaluationScoring");

            Id(e => e.Id);

            Map(e => e.ComprehensibilityScore);
            Map(e => e.ComprehensibilityComment).Length(1000);
            Map(e => e.FidelityScore);
            Map(e => e.FidelityComment).Length(1000);
            Map(e => e.CreationDate);
            Map(e => e.ScoringDate);
            Map(e => e.Timestamp);

            References(e => e.LanguagePair).Column("LanguagePairID");
            References(e => e.Provider).Column("ProviderID");
            References(e => e.User).Column("UserID");
            References(e => e.SourceLanguage).Column("SourceLanguageID");
            References(e => e.TargetLanguage).Column("TargetLanguageID");
        }
    }
}
