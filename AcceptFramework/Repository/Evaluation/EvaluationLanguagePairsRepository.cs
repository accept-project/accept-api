using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationLanguagePairsRepository : RepositoryBase<EvaluationLanguagePair>
    {
        public static IEnumerable<EvaluationLanguagePair> GetAll()
        {
            return new RepositoryBase<EvaluationLanguagePair>().Select();
        }

        public static EvaluationLanguagePair Insert(EvaluationLanguagePair record)
        {
            return new RepositoryBase<EvaluationLanguagePair>().Create(record);
        }

        public static EvaluationLanguagePair Get(int id)
        {
            return new RepositoryBase<EvaluationLanguagePair>().Select(a => a.Id == id).FirstOrDefault();

        }

        public static EvaluationLanguagePair Update(EvaluationLanguagePair langpair)
        {
            return new RepositoryBase<EvaluationLanguagePair>().Update(langpair);
        }

    
    }
}
