using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    #region EvaluationInternal
    internal class EvaluationUserProjectRoleRepository
    {
        public static IEnumerable<EvaluationUserProjectRole> GetAll()
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Select();
        }

        public static EvaluationUserProjectRole Insert(EvaluationUserProjectRole record)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Create(record);
        }

        public static void Delete(EvaluationUserProjectRole record)
        {
            new RepositoryBase<EvaluationUserProjectRole>().Delete(record);
        }

        public static EvaluationUserProjectRole UpdateEvaluationUserProjectRole(EvaluationUserProjectRole record)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Update(record);
        }

        public static EvaluationUserProjectRole GetEvaluationUserProjectRoleByUserAndProject(User user, EvaluationProject project)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Select(a => a.User == user && a.Project == project).FirstOrDefault();
        }

        public static IEnumerable<EvaluationUserProjectRole> GetEvaluationUserProjectRoleByUser(User user)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Select(a => a.User == user);
        }

        public static IEnumerable<EvaluationUserProjectRole> GetEvaluationUserProjectRoleByProject(EvaluationProject project)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Select(a => a.Project == project);
        }

        public static IEnumerable<EvaluationUserProjectRole> GetEvaluationUserProjectRoleByUserFilteredByRole(User user, Role role)
        {
            return new RepositoryBase<EvaluationUserProjectRole>().Select(a => a.User == user && a.Role != role);
        }

    }
    #endregion
}
