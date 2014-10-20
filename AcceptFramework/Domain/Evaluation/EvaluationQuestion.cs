using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationQuestion: DomainBase
    {
        public EvaluationQuestion()
        {
        }

        public EvaluationQuestion(string _name)
        {
            Name = _name;
            Count = 0;
            LanguageQuestions = new List<EvaluationQuestionItem>();
        }

        public virtual string Name { get; set; }
        public virtual IList<EvaluationQuestionItem> LanguageQuestions { get; set; }
        public virtual int Count { get; set; }
   
    }
}