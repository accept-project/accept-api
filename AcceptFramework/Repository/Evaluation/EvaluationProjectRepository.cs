using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationProjectRepository : RepositoryBase<EvaluationProject>
    {
        public static IEnumerable<EvaluationProject> GetAll()
        {
            return new RepositoryBase<EvaluationProject>().Select();
        }

        public static IEnumerable<EvaluationProject> GetByCreator(int userId)
        {
            return new RepositoryBase<EvaluationProject>().Select().Where(p => p.Creator.Id == userId);
        }

        public static EvaluationProject Insert(EvaluationProject record)
        {
            return new RepositoryBase<EvaluationProject>().Create(record);
        }


        public static EvaluationProject GetProject(int projectId)
        {
            return new RepositoryBase<EvaluationProject>().Select(a => a.Id == projectId).FirstOrDefault();

        }

        public static EvaluationProject UpdateProject(EvaluationProject project)
        {
            return new RepositoryBase<EvaluationProject>().Update(project);
        }

        public static void Delete(EvaluationProject record)
        {
            new RepositoryBase<EvaluationProject>().Delete(record);
        }
    
    }
}
