using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationProvidersMap : ClassMap<EvaluationProvider>
    {
        public EvaluationProvidersMap()
        {
            Table("EvaluationProviders");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
        }
    }
}