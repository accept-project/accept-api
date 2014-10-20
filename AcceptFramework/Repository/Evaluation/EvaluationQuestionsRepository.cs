using System.Collections.Generic;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    internal static class EvaluationQuestionsRepository
    {
        public static IEnumerable<EvaluationQuestion> GetAll()
        {
            return new RepositoryBase<EvaluationQuestion>().Select();
        }

        public static EvaluationQuestion Insert(EvaluationQuestion record)
        {
            return new RepositoryBase<EvaluationQuestion>().Create(record);
        }

        public static EvaluationQuestion Get(int id)
        {
            return new RepositoryBase<EvaluationQuestion>().Select(a => a.Id == id).FirstOrDefault();

        }

        public static EvaluationQuestion Update(EvaluationQuestion question)
        {
            return new RepositoryBase<EvaluationQuestion>().Update(question);
        }

        public static void Delete(EvaluationQuestion record)
        {
            new RepositoryBase<EvaluationQuestion>().Delete(record);
        }
   
    }
}
