using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class ProjectActiveProviderMap : ClassMap<ProjectActiveProvider>
    {
        public ProjectActiveProviderMap()
        {
            Table("EvaluationProjectActiveProviders");

            Id(e => e.Id);
            Map(e => e.ProjectID);
            Map(e => e.ProviderID);
        }
    }

    public class EvaluationProjectProviderMap : ClassMap<EvaluationProjectProvider>
    {
        public EvaluationProjectProviderMap()
        {
            Table("EvaluationProjectProviders");

            Id(e => e.Id);
            Map(e => e.ProjectID);
            Map(e => e.ProviderID);
        }
    }
}