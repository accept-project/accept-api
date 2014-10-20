using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace AcceptApi.Areas.Api.Models.ActionResults
{
    public class JsonpResult : System.Web.Mvc.ActionResult
    {

        private readonly object _obj;

        public JsonpResult(object obj)
        {
            _obj = obj;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new JavaScriptSerializer();
            var callbackname = context.HttpContext.Request["callback"];
            var jsonp = string.Format("{0}({1})", callbackname, serializer.Serialize(_obj));
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(jsonp);
        }


 


    }
}