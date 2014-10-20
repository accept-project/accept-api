using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationContributorRepository : RepositoryBase<EvaluationContributor>
    {
        public EvaluationContributor SelectOrCreate(string contributorName)
        {
            EvaluationContributor contributor = Select(c => c.ContributorName == contributorName).FirstOrDefault();
            if (contributor == null)
            {
                contributor = new EvaluationContributor { ContributorName = contributorName };
                Create(contributor);
            }
            return contributor;
        }
    }
}
