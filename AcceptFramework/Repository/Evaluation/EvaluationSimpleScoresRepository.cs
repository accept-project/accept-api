using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationSimpleScoresRepository : RepositoryBase<EvaluationSimpleScore>
    {
        public static IEnumerable<EvaluationSimpleScore> GetAll()
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select();
        }

        public static EvaluationSimpleScore Insert(EvaluationSimpleScore record)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Create(record);
        }

        public static EvaluationSimpleScore Get(int Id)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(a => a.Id == Id).FirstOrDefault();
        }


        public static int GetCount(int AnswerId)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(s => s.AnswerId == AnswerId).Count();
        }

        public static IEnumerable<EvaluationSimpleScore> GetProjectScores(int Id)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(a => a.ProjectID == Id);
        }


        public static EvaluationSimpleScore Update(EvaluationSimpleScore item)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Update(item);
        }


    }
}
