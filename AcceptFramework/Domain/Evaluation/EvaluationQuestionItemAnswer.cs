using System.Text.RegularExpressions;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationQuestionItemAnswer: DomainBase
    {
        public EvaluationQuestionItemAnswer()
        {
        }

        public EvaluationQuestionItemAnswer(string value, string name)
        {

            Name = name;
            Value = value;
            Count = 0;
        }

        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual int Count { get; set; }
    }
}