using System.Collections.Generic;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    internal static class EvaluationQuestionItemsRepository
    {
        public static IEnumerable<EvaluationQuestionItem> GetAll()
        {
            return new RepositoryBase<EvaluationQuestionItem>().Select();
        }

        public static EvaluationQuestionItem Insert(EvaluationQuestionItem record)
        {
            return new RepositoryBase<EvaluationQuestionItem>().Create(record);
        }

        public static EvaluationQuestionItem Get(int id)
        {
            return new RepositoryBase<EvaluationQuestionItem>().Select(a => a.Id == id).FirstOrDefault();

        }

        public static EvaluationQuestionItem Update(EvaluationQuestionItem question)
        {
            return new RepositoryBase<EvaluationQuestionItem>().Update(question);
        }

        public static void Delete(EvaluationQuestionItem record)
        {
            new RepositoryBase<EvaluationQuestionItem>().Delete(record);
        }
    
    }
}
