using System.Web.Mvc;

namespace WebApp.Areas.Availability
{
    public class AvailabilityAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Availability";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Availability_default",
                "Availability/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}