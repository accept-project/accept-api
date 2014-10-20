using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationLanguagePairsMap : ClassMap<EvaluationLanguagePair>
    {
        public EvaluationLanguagePairsMap()
        {
            Table("EvaluationLanguagePairs");

            Id(e => e.Id);
            References(e => e.SourceLanguage).Not.LazyLoad().Column("SourceLanguageID");
            References(e => e.TargetLanguage).Not.LazyLoad().Column("TargetLanguageID");
        }
    }
}