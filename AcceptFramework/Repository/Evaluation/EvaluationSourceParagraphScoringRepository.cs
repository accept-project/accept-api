using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationSourceParagraphScoringRepository : RepositoryBase<EvaluationSourceParagraphScoring>
    {
        public void SaveOrCreate(EvaluationSourceParagraphScoring source)
        {
            EvaluationSourceParagraphScoring score = Select(s => s.SourceParagraph == source.SourceParagraph).FirstOrDefault();
            if (score != null)
            {
                Delete(score);
            }
            Create(source);
        }

    }
}
