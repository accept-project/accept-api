using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptFramework.Interfaces.Audit;
using AcceptFramework.Interfaces;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Domain.Audit;
using AcceptFramework.Business;

namespace AcceptApi.Areas.Api.Models.Managers
{
    public class CoreAuditManager: ICoreAuditManager
    {
        private IAuditManager _auditManager;    
        private IAcceptApiServiceLocator _acceptServiceLocator;

        public IAuditManager AuditManager
        {
            get { return _auditManager; }
        }

        public CoreAuditManager()
        {
            _acceptServiceLocator = new AcceptApiServiceLocator();
            _auditManager = _acceptServiceLocator.GetAuditManagerService();        
        }

        public CoreApiResponse InsertPageVisitAudit(string userName, string type, string action, string description, string origin, string meta, string timeStamp, string language, string userAgent)
        {
            try
            {
                switch (action.ToUpper())
                {
                    case CoreAuditDefaultAction.CREATE: { AuditManager.InsertPageVisitAudit(new PageVisitAudit(userName, description, DateTime.Parse(timeStamp), CoreAuditType.Portal, CoreAuditAction.Create, origin, meta, language, userAgent)); } break;
                    case CoreAuditDefaultAction.READ: { AuditManager.InsertPageVisitAudit(new PageVisitAudit(userName, description, DateTime.Parse(timeStamp), CoreAuditType.Portal, CoreAuditAction.Read, origin, meta, language, userAgent)); } break;
                    case CoreAuditDefaultAction.UPDATE: { AuditManager.InsertPageVisitAudit(new PageVisitAudit(userName, description, DateTime.Parse(timeStamp), CoreAuditType.Portal, CoreAuditAction.Update, origin, meta, language, userAgent)); } break;
                    case CoreAuditDefaultAction.DELETE: { AuditManager.InsertPageVisitAudit(new PageVisitAudit(userName, description, DateTime.Parse(timeStamp), CoreAuditType.Portal, CoreAuditAction.Delete, origin, meta, language, userAgent)); } break;
                    default: { throw new Exception("Invalid action type provided."); } 
                }
                               
                return new CoreApiResponse();
            }
            catch (Exception e)
            {                
                throw(e);
            }                    
        }


    }
}