using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationCategoryMap : ClassMap<EvaluationCategory>
    {
        public EvaluationCategoryMap()
        {
            Table("EvaluationCategory");

            Id(e => e.Id);
            Map(e => e.CategoryName).Length(50);
        }
    }
}
