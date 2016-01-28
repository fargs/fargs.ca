using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebApp.Controllers" }
            );

            routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(name: "signin-google", url: "signin-google", defaults: new { controller = "Account", action = "ExternalLoginCallback" });
        }
    }
}
