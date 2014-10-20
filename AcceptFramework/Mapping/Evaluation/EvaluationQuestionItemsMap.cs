using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationQuestionItemsMap : ClassMap<EvaluationQuestionItem>
    {
        public EvaluationQuestionItemsMap()
        {
            Table("EvaluationQuestionItems");

            Id(e => e.Id);
            Map(e => e.Question).Length(250);
            Map(e => e.Action).Length(250);
            Map(e => e.Confirmation).Length(250);
            Map(e => e.Count);
            References(e => e.Language).Not.LazyLoad().Column("LanguageID");

            HasManyToMany(e => e.Answers).
                Table("EvaluationProjectQuestionItemAnswers").Not.LazyLoad().
                ParentKeyColumn("QuestionItemID").
                ChildKeyColumn("AnswerID");

        }
    }
}
