using System.Web.Mvc;

namespace AcceptApi.Areas.RealTimeApi
{
    public class RealTimeApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RealTimeApi/v1";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RealTimeApi_default",
                "RealTimeApi/v1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
