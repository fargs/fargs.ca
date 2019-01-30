using System.Web.Mvc;

namespace WebApp.Areas.ServiceRequests
{
    public class ServiceRequestsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ServiceRequests";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ServiceRequests_default",
                "ServiceRequests/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}