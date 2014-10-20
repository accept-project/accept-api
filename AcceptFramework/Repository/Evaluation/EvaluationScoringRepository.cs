using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Repository;

namespace EP.Business.Repository
{
    [DataObject]
    public class EvaluationScoringRepository : RepositoryBase<EvaluationScoring>
    {
     
        public void SaveOrCreate(EvaluationScoring scoring)
        {
            EvaluationScoring score = Select(s => s == scoring).FirstOrDefault();
            if (score != null)
            {
                Update(score);
            }
            else Create(scoring);
        }
    }
}
