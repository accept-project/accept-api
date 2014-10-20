using System.Collections.Generic;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationQuestionItem: DomainBase
    {
        public EvaluationQuestionItem()
        {
        }

        public EvaluationQuestionItem(string _question, string _action, string _confirmation, EvaluationLanguage _language)
        {
            Language = _language;
            Question = _question;
            Action = _action;
            Confirmation = _confirmation;
            Answers = new List<EvaluationQuestionItemAnswer>();
            Count = 0;
        }

        public virtual string Question { get; set; }
        public virtual string Action { get; set; }
        public virtual string Confirmation { get; set; }
        public virtual IList<EvaluationQuestionItemAnswer> Answers { get; set; }
        public virtual EvaluationLanguage Language { get; set; }
        public virtual int Count { get; set; }
    }
}