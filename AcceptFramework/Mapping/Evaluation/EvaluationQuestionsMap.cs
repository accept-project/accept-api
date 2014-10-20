using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationQuestionsMap : ClassMap<EvaluationQuestion>
    {
        public EvaluationQuestionsMap()
        {
            Table("EvaluationQuestions");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.Count);

            HasManyToMany(e => e.LanguageQuestions).
                Table("EvaluationProjectQuestionItems").Not.LazyLoad().
                ParentKeyColumn("QuestionID").
                ChildKeyColumn("QuestionItemID");

        }
    }
}
