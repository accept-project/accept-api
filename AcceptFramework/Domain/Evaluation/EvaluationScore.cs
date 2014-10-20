using System.Text.RegularExpressions;

namespace AcceptFramework.Domain.Evaluation
{
    public class EvaluationScore: DomainBase
    {
        public EvaluationScore()
        {
        }

        public EvaluationScore(int score)
        {
            Score = score;
        }

        public virtual int Score { get; set; }
    }
}