using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Repository;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationTargetSegmentScoringRepository : RepositoryBase<EvaluationTargetSegmentScoring>
    {
        public void SaveOrCreate(EvaluationTargetSegmentScoring target)
        {
            EvaluationTargetSegmentScoring score = Select(s => s.TargetSegment == target.TargetSegment).FirstOrDefault();
            if (score != null)
            {
                Delete(score);
            }
            Create(target);
        }
    }
}
