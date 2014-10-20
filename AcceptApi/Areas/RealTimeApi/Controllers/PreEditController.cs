using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.RealTimeApi.Controllers
{
    public class PreEditController : Controller
    {
        private IAcceptApiManager acceptCoreManager;
        public IAcceptApiManager AcceptCoreManager
        {
            get { return acceptCoreManager; }
        }

        public PreEditController()
        {
            this.acceptCoreManager = new AcceptApiManager();
        }



        //
        // GET: /RealTimeApi/PreEdit/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcceptRequest(CoreApiRequest requestObject)
        {
            try
            {
                if (requestObject == null || requestObject.Text.Length == 0)
                    requestObject = (CoreApiRequest)(new DataContractJsonSerializer(requestObject.GetType())).ReadObject(Request.InputStream);

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                if (requestObject.ApiKey != null && requestObject.ApiKey.Length > 0)
                {
                    parameters.Add("apiKey", requestObject.ApiKey);
                }
                else
                {
                    parameters.Add("username", requestObject.User);
                    parameters.Add("password", requestObject.Password);
                }

                string text = HttpUtility.UrlDecode(requestObject.Text, System.Text.Encoding.UTF8);

                parameters.Add("text", text);
                parameters.Add("languageid", requestObject.Language);
                parameters.Add("ruleset", requestObject.Rule);
                parameters.Add("grammar", requestObject.Grammar);
                parameters.Add("spell", requestObject.Spell);
                parameters.Add("style", requestObject.Style);

                parameters.Add("requestFormat", requestObject.RequestFormat);

                if (requestObject.GlobalSessionId != null && requestObject.GlobalSessionId.Length > 0)
                {
                    parameters.Add("globalSessionId", requestObject.GlobalSessionId);
                }

                if (requestObject.IEDomain != null && requestObject.IEDomain.Length > 0)
                    parameters.Add("ieDomain", requestObject.IEDomain);


                if (requestObject.SessionMetadata != null && requestObject.SessionMetadata.Length > 0)
                    parameters.Add("sessionMetadata", requestObject.SessionMetadata);

                var model = AcceptCoreManager.GenericRequest(parameters);
                return Json(model);


            }
            catch (Exception e)
            {
                var errorModel = new CoreApiException(e.Message, "Accept Request Controller");
                return Json(errorModel);
            }

        }








    }
}
