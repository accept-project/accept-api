using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptApi.Areas.Api.Models.Log;
using AcceptApi.Areas.Api.Models.Core;
using AcceptApi.Areas.Api.Models.Filters;

namespace AcceptApi.Areas.Api.Controllers
{
    [CrossSiteJsonCall]
    public class ErrorsController : Controller
    {
        public JsonResult General(Exception exception)
        {
            var error = new AcceptApiException();
            error.ResponseStatus = CoreApiResponseStatus.Failed;
            error.Context = "General";
            error.Exception = exception.Message;
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Http404(Exception exception)
        {
            var error = new AcceptApiException();
            error.ResponseStatus = CoreApiResponseStatus.Failed;
            error.Context = "Http404";
            error.Exception = exception.Message;
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Http403(Exception exception)
        {
            var error = new AcceptApiException();
            error.ResponseStatus = CoreApiResponseStatus.Failed;
            error.Context = "Http403";
            error.Exception = exception.Message;
            return Json(error, JsonRequestBehavior.AllowGet);
        }
    }
       

    

}
