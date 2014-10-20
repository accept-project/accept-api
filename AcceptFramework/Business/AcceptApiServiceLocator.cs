using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Business.Evaluation;
using AcceptFramework.Interfaces;
using AcceptFramework.Business.Audit;
using AcceptFramework.Interfaces.Audit;
using AcceptFramework.Interfaces.Evaluation;
using AcceptFramework.Interfaces.Session;
using AcceptFramework.Business.Session;
using AcceptFramework.Interfaces.Common;
using AcceptFramework.Business.Common;
using AcceptFramework.Interfaces.PostEdit;
using AcceptFramework.Business.PostEdit;
using AcceptFramework.Interfaces.PostEditAudit;
using AcceptFramework.Business.PostEditAudit;
using AcceptFramework.Interfaces.Miscellaneous;
using AcceptFramework.Business.Miscellaneous;

namespace AcceptFramework.Business
{
    public class AcceptApiServiceLocator : IAcceptApiServiceLocator
    {

        #region Managers

        public IAuditManager GetAuditManagerService()
        {             
                return new AuditManager();
        }

        public ISessionManager GetSessionManagerService()
        {
            return new SessionManager();
        }

        public IUserManager GetUserManagerService()
        { 
            return new UserManager();        
        }

        public IDomainManager GetDomainManagerService()
        {
            return new DomainManager();        
        }

        public IUniverseManager GetUniverseManagerService()
        {
            return new UniverseManager();
        }

        public IProjectManager GetProjectManagerService()
        {
            return new ProjectManager();
        }

        public IPostEditAuditManager GetPostEditAuditManagerService()
        {
            return new PostEditAuditManager();
        }

        public IEvaluationProjectManager GetEvaluationProjectManagerService()
        {
            return new EvaluationProjectManager();
        }

        public IMiscellaneousManagerService GetMiscellaneousManagerService()
        {
            return new MiscellaneousManager();
        }

        #endregion

        #region ServiceLocators


        public IAcceptApiServiceLocator GetAcceptServiceLocator()
        {
            return new AcceptApiServiceLocator();
        }


        #endregion
    }
}
