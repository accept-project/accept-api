using System.Collections.Generic;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    internal static class EvaluationLanguagesRepository
    {
      
        public static IEnumerable<EvaluationLanguage> GetAll()
        {
            return new RepositoryBase<EvaluationLanguage>().Select();
        }

        public static EvaluationLanguage Insert(EvaluationLanguage record)
        {
            return new RepositoryBase<EvaluationLanguage>().Create(record);
        }

        public static EvaluationLanguage Get(int id)
        {
            return new RepositoryBase<EvaluationLanguage>().Select(a => a.Id == id).FirstOrDefault();
        }

        public static EvaluationLanguage Update(EvaluationLanguage lang)
        {
            return new RepositoryBase<EvaluationLanguage>().Update(lang);
        }

    
    }
}
