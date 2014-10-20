using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationTargetParagraphScoringRepository : RepositoryBase<EvaluationTargetParagraphScoring>
    {
        public void SaveOrCreate(EvaluationTargetParagraphScoring target)
        {
            EvaluationTargetParagraphScoring score = Select(s => s.SourceParagraph == target.SourceParagraph).FirstOrDefault();
            if (score != null)
            {
                Delete(score);
            }
            Create(target);
        }
    }
}
