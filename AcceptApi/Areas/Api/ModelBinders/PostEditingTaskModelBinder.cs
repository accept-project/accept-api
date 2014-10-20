using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcceptFramework.Domain.PostEdit;
using System.Web.Script.Serialization;

namespace AcceptApi.Areas.Api.ModelBinders
{
 
    public class JsonFilter : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Document model;

            if (controllerContext.RequestContext.HttpContext.Request.AcceptTypes.Contains("application/json"))
            {
                var serializer = new JavaScriptSerializer();
                var form = controllerContext.RequestContext.HttpContext.Request.Form.ToString();
                model = serializer.Deserialize<Document>(HttpUtility.UrlDecode(form));
            }
            else
            {                
                model = null;
            }

            return model;
        }
    }

}