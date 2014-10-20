using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcceptApi.Areas.Api.Models.Filters
{
    public class CrossSiteJsonCall : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-type,X-Requested-With");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Max-Age", "1728000");                                    
            base.OnActionExecuting(filterContext);
          
        }
    }
}