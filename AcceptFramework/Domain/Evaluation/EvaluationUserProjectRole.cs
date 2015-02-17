using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Domain.Evaluation
{
    #region EvaluationInternal
    public class EvaluationUserProjectRole : DomainBase
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual EvaluationProject Project { get; set; }
        public EvaluationUserProjectRole() { }
        public EvaluationUserProjectRole(User u, Role r, EvaluationProject p) { User = u; Role = r; Project = p; }
    }
    #endregion
}
