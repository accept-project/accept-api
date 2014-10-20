using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AcceptApi.Areas.Api.Controllers;
using AcceptApi.Areas.Api.Models.Log;
using System.Xml.Linq;
using AcceptApi.Areas.Api.Models.Cache;
using System.Xml;
using System.Web.Configuration;
using System.IO;
using AcceptFramework.Domain.PostEdit;
using AcceptApi.Areas.Api.ModelBinders;

namespace AcceptApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);

        }

        protected void Application_Start()
        {
                     
            if (!CacheStore.Exists<XDocument>("Acrolinx_de"))
            {
                string tempFilePath = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["AcrolinxDocumentsPath"] + "Acrolinx_de.xml");
                XDocument enRulesXDoc = XDocument.Load(tempFilePath);
                CacheStore.Add<XDocument>("Acrolinx_de", enRulesXDoc);                                                                
            }
            if (!CacheStore.Exists<XDocument>("Acrolinx_fr"))
            {
                string tempFilePath = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["AcrolinxDocumentsPath"] + "Acrolinx_fr.xml");
                XDocument frRulesXDoc = XDocument.Load(tempFilePath);
                CacheStore.Add<XDocument>("Acrolinx_fr", frRulesXDoc);                
            }
            if (!CacheStore.Exists<XDocument>("Acrolinx_en"))
            {
                string tempFilePath = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["AcrolinxDocumentsPath"] + "Acrolinx_en.xml");
                XDocument deRulesXDoc = XDocument.Load(tempFilePath);
                CacheStore.Add<XDocument>("Acrolinx_en", deRulesXDoc);                
            }
                      
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            //avoid IIS7 getting in the middle.
            Response.TrySkipIisCustomErrors = true;
            IController errorsController = new ErrorsController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            errorsController.Execute(rc);
        }


    }
}