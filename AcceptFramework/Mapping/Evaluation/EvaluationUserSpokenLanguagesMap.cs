using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationUserSpokenLanguagesMap: ClassMap<EvaluationUserSpokenLanguages>
    {
        public EvaluationUserSpokenLanguagesMap()
        {
            Table("EvaluationUserSpokenLanguages");

            Id(e => e.Id);
            Map(e => e.LanguageID);
            Map(e => e.UserID);
        }
    }
}
