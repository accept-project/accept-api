using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationLanguagesMap : ClassMap<EvaluationLanguage>
    {
        public EvaluationLanguagesMap()
        {
            Table("EvaluationLanguages");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.Code).Length(5);
        }
    }
}
