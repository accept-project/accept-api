using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationAuthorTypeRepository : RepositoryBase<EvaluationAuthorType>
    {
        public EvaluationAuthorType SelectOrCreate(string authorTypeName)
        {
            EvaluationAuthorType authorType = Select(c => c.AuthorTypeName == authorTypeName).FirstOrDefault();
            if (authorType == null)
            {
                authorType = new EvaluationAuthorType { AuthorTypeName = authorTypeName };
                Create(authorType);
            }
            return authorType;
        }
    }
}
