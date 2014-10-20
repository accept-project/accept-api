using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Interfaces;
using AcceptApi.Areas.Api.Models.Managers;
using AcceptApi.Areas.Api.Models.ActionResults;
using System.Text;
using AcceptApi.Areas.Api.Models.Filters;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class MiscellaneousController : Controller
    {
        private IMiscellaneousManager miscellaneousManager;

        public IMiscellaneousManager MiscellaneousManager
        {
            get { return miscellaneousManager; }
        }

        public MiscellaneousController()
        {
            this.miscellaneousManager = new MiscellaneousManager();        
        }
      
        public JsonpResult ExternalUsageJsonP(string Id, int count, string meta)
        {          
            var model = MiscellaneousManager.UpdateExternalUsageItem(Id, count, HttpUtility.UrlDecode(meta, System.Text.Encoding.UTF8));
            return new JsonpResult(model);
        }
        
        public JsonpResult CreateExternalUsageJsonP(string Id, int count, string meta)
        {
            var model = MiscellaneousManager.CreateExternalUsageItem(Id, count, HttpUtility.UrlDecode(meta, System.Text.Encoding.UTF8));
            return new JsonpResult(model);
        }

        public JsonpResult GetExternalUsageCount(string Id)
        {
            var model = MiscellaneousManager.GetExternalUsageCount(Id);
            return new JsonpResult(model);
        }

        [HttpGet]
        public ActionResult ExternalUsageRange(string Id, string start, string end, string format)
        {
            if (start != null && end != null)
            {
                try
                {
                    DateTime startDatetime = DateTime.Parse(start);
                    DateTime endDatetime = DateTime.Parse(end);
                    if (startDatetime != null && endDatetime != null)                                          
                        return Json(MiscellaneousManager.GetExternalUsageReport(Id, startDatetime, endDatetime, string.Empty), "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
                    
                }catch(Exception e){
                    return Json(e.Message);
                
                }
            }
            return Json("invalid date");
        }
    }
}
