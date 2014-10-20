using System.Collections.Generic;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    internal static class EvaluationQuestionItemAnswersRepository
    {
        public static IEnumerable<EvaluationQuestionItemAnswer> GetAll()
        {
            return new RepositoryBase<EvaluationQuestionItemAnswer>().Select();
        }

        public static EvaluationQuestionItemAnswer Insert(EvaluationQuestionItemAnswer record)
        {
            return new RepositoryBase<EvaluationQuestionItemAnswer>().Create(record);
        }

        public static EvaluationQuestionItemAnswer Get(int id)
        {
            return new RepositoryBase<EvaluationQuestionItemAnswer>().Select(a => a.Id == id).FirstOrDefault();
        }

        public static EvaluationQuestionItemAnswer Update(EvaluationQuestionItemAnswer record)
        {
            return new RepositoryBase<EvaluationQuestionItemAnswer>().Update(record);
        }

        public static void Delete(EvaluationQuestionItemAnswer record)
        {
            new RepositoryBase<EvaluationQuestionItemAnswer>().Delete(record);
        }
    
    }
}
