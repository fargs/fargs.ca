using System.Web.Mvc;

namespace WebApp.Areas.Physicians
{
    public class PhysiciansAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Physicians";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Physicians_default",
                "Physicians/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}