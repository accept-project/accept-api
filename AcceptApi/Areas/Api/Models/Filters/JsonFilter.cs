using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using AcceptFramework.Domain.PostEdit;

namespace AcceptApi.Areas.Api.Models.Filters
{

    public class JsonFilter : ActionFilterAttribute
    {
        public string Parameter { get; set; }
        public Type JsonDataType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.ContentType.Contains("application/json"))
            {
                string inputContent;
                using (var sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
                {
                    inputContent = sr.ReadToEnd();
                }
                var serializer = new JavaScriptSerializer();
                var result = serializer.Deserialize<Type>(HttpUtility.UrlDecode(inputContent));               
                filterContext.ActionParameters[Parameter] = result;
            }
        }
    }


}