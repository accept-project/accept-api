using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationContributorsMap: ClassMap<EvaluationContributor>
    {
        public EvaluationContributorsMap()
        {
            Table("EvaluationContributor");

            Id(e => e.Id);
            Map(e => e.ContributorName).Length(50);

            References(e => e.Language).Not.LazyLoad().Column("LanguageID");
        }
    }
}
