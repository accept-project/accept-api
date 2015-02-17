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

        #region EvaluationInternal

        public static IEnumerable<EvaluationSimpleScore> GetProjectScoresFiltered(int Id, string filter)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(a => a.ProjectID == Id && (a.Var1 == filter || a.Var2 == filter || a.Var3 == filter || a.Var4 == filter || a.Var5 == filter || a.Var6 == filter || a.Var7 == filter || a.Var8 == filter || a.Var9 == filter || a.Var10 == filter));
        }

        public static IEnumerable<string> GetUserHistoryOnInternalEvaluationProject(int Id, string user)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(a => a.ProjectID == Id && a.Var7 == user).Select(t => t.Var6).ToList<string>();
        }

        public static IEnumerable<EvaluationSimpleScore> GetInternalScores(int Id, string user)
        {
            return new RepositoryBase<EvaluationSimpleScore>().Select(a => a.ProjectID == Id && a.Var7 == user);
        }

        #endregion

    }
}
