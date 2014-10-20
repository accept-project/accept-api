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
    public class SpellController : Controller
    {
        private IAcceptApiManager acceptCoreManager;

        public IAcceptApiManager AcceptCoreManager
        {
            get { return acceptCoreManager; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public SpellController()
        {
            this.acceptCoreManager = new AcceptApiManager();

        }
       
        [RestHttpVerbFilter]
        public JsonResult SpellCheckRequest(CoreApiRequest requestObject, string httpVerb)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username", requestObject.User);
            parameters.Add("password", requestObject.Password);
            parameters.Add("text", HttpUtility.UrlDecode(requestObject.Text));
            parameters.Add("lang", requestObject.Language);
            parameters.Add("rule", requestObject.Rule);
            var model = AcceptCoreManager.SpellCheckRequest(parameters);
            return Json(model);                     
        }
       
        [HttpGet]
        public JsonResult SpellCheckLanguages()
        {
            var model = AcceptCoreManager.GrammarLanguages();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }    
}
