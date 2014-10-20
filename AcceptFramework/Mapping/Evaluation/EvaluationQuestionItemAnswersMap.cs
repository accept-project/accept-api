using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationQuestionItemAnswersMap : ClassMap<EvaluationQuestionItemAnswer>
    {
        public EvaluationQuestionItemAnswersMap()
        {
            Table("EvaluationQuestionItemAnswers");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.Value);
            Map(e => e.Count);
        }
    }
}
