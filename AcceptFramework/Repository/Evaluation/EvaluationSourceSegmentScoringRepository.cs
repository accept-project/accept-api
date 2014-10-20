using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationSourceSegmentScoringRepository : RepositoryBase<EvaluationSourceSegmentScoring>
    {
        public void SaveOrCreate(EvaluationSourceSegmentScoring source)
        {
            EvaluationSourceSegmentScoring score = Select(s => s.SourceSegment == source.SourceSegment).FirstOrDefault();
            if (score != null)
            {
                Delete(score);
            }
            Create(source);
        }
    }
}
