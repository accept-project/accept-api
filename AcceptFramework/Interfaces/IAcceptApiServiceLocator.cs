using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.Audit;
using AcceptFramework.Interfaces.Evaluation;
using AcceptFramework.Interfaces.Session;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Interfaces.PostEditAudit;
using AcceptFramework.Interfaces.Miscellaneous;

namespace AcceptFramework.Interfaces
{
    public interface IAcceptApiServiceLocator
    {

        IAcceptApiServiceLocator GetAcceptServiceLocator();

        IAuditManager GetAuditManagerService();

        ISessionManager GetSessionManagerService();

        IUserManager GetUserManagerService();

        IDomainManager GetDomainManagerService();

        IUniverseManager GetUniverseManagerService();

        IProjectManager GetProjectManagerService();

        IEvaluationProjectManager GetEvaluationProjectManagerService();

        IPostEditAuditManager GetPostEditAuditManagerService();

        IMiscellaneousManagerService GetMiscellaneousManagerService();
    }
}
