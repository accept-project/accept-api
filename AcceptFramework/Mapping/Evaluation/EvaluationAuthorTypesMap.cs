using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationAuthorTypesMap : ClassMap<EvaluationAuthorType>
    {
        public EvaluationAuthorTypesMap()
        {
            Table("EvaluationAuthorTypes");

            Id(e => e.Id);
            Map(e => e.AuthorTypeName).Length(50);
        }
    }
}
