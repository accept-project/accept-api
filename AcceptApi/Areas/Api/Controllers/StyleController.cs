using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AcceptApi.Areas.Api.Models.Filters;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class StyleController : Controller
    {
        private IAcceptApiManager acceptCoreManager;

        public IAcceptApiManager AcceptCoreManager
        {
            get { return acceptCoreManager; }
        }

        public StyleController()
        {
            this.acceptCoreManager = new AcceptApiManager();
        }
        
        [RestHttpVerbFilter]
        public JsonResult StyleCheckRequest(CoreApiRequest requestObject, string httpVerb)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username", requestObject.User);
            parameters.Add("password", requestObject.Password);
            parameters.Add("text", HttpUtility.UrlDecode(requestObject.Text));
            parameters.Add("lang", requestObject.Language);
            parameters.Add("rule", requestObject.Rule);

            var model = AcceptCoreManager.StyleCheckRequest(parameters);
            return Json(model);           
        }

        [HttpGet]
        public JsonResult StyleCheckLanguages()
        {
            var model = AcceptCoreManager.StyleCheckLanguages();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
    }
}
