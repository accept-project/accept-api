using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Managers;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Filters;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class AuditController : Controller
    {
        private ICoreAuditManager _coreAuditManager;
        
        public ICoreAuditManager CoreAuditManager
        {
            get { return _coreAuditManager; }         
        }
        
        public AuditController()
        {
            this._coreAuditManager = new CoreAuditManager();
        }
                     
        [HttpPost]
        public ActionResult PageVisit(string userName, string type, string action, string description, string origin, string meta, string timeStamp, string language, string userAgent)
        {
            try
            {
                var model = CoreAuditManager.InsertPageVisitAudit(userName, type, action, description, origin, meta, timeStamp, language, userAgent);
                return Json(model);
            }
            catch (Exception e)
            {
                return Json(new CoreApiException(e.Message, "PageVisit"));
            }        
        }
    }
}
