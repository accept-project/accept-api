using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    #region EvaluationInternal
    internal class EvaluationProjectInvitationRepository
    {
        public static IEnumerable<EvaluationProjectInvitation> GetAll()
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select();
        }

        public static EvaluationProjectInvitation Insert(EvaluationProjectInvitation record)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Create(record);
        }


        public static EvaluationProjectInvitation GetProject(int projectInvitationId)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.Id == projectInvitationId).FirstOrDefault();

        }

        public static EvaluationProjectInvitation UpdateProjectInvitation(EvaluationProjectInvitation projectInvitation)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Update(projectInvitation);
        }


        public static IEnumerable<EvaluationProjectInvitation> GetAllByConfirmationCode(string confirmationCode)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.ConfirmationCode == confirmationCode);
        }

        public static IEnumerable<EvaluationProjectInvitation> GetAllByProjectId(int projectId)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.ProjectId == projectId);
        }

        public static IEnumerable<EvaluationProjectInvitation> GetAllByUserName(string userName)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.UserName == userName);
        }

        public static EvaluationProjectInvitation GetProjecInvitationtByConfirmationCode(string projectInvitationCode)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.ConfirmationCode == projectInvitationCode).FirstOrDefault();

        }


        public static EvaluationProjectInvitation GetProjectInvitationByUserName(string userName)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.UserName == userName).FirstOrDefault();

        }

        public static EvaluationProjectInvitation GetNextValidProjectInvitationByUserName(string userName)
        {
            return new RepositoryBase<EvaluationProjectInvitation>().Select(a => a.UserName == userName && a.ConfirmationCode != string.Empty && a.ConfirmationDate != (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue).FirstOrDefault();

        }
    }
    #endregion
}
