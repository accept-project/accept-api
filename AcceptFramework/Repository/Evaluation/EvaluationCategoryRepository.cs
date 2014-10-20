using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationCategoryRepository : RepositoryBase<EvaluationCategory>
    {
        public EvaluationCategory SelectOrCreate(string categoryName)
        {
            EvaluationCategory category = Select(c => c.CategoryName == categoryName).FirstOrDefault();
            if (category == null)
            {
                category = new EvaluationCategory { CategoryName = categoryName };
                Create(category);
            }
            return category;
        }
    }
}
