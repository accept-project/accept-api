using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class ProjectActiveLanguagePairMap : ClassMap<ProjectActiveLanguagePair>
    {
        public ProjectActiveLanguagePairMap()
        {
            Table("EvaluationProjectActiveLanguagePairs");

            Id(e => e.Id);
            Map(e => e.ProjectID);
            Map(e => e.LanguagePairID);
        }
    }

    public class EvaluationProjectLanguagePairMap : ClassMap<EvaluationProjectLanguagePair>
    {
        public EvaluationProjectLanguagePairMap()
        {
            Table("EvaluationProjectLanguagePairs");

            Id(e => e.Id);
            Map(e => e.ProjectID);
            Map(e => e.LanguagePairID);
        }
    }
}