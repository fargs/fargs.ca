using FluentValidation.Mvc;
using Microsoft.ApplicationInsights.Extensibility;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.Library;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FluentValidationModelValidatorProvider.Configure();
            // This turns off code first migrations on the database
            Database.SetInitializer<Models.ApplicationDbContext>(null);
        }

        protected virtual void Application_EndRequest()
        {
            //var entityContext = HttpContext.Current.Items[ContextPerRequest.dbKey] as OrvosiDbContext;
            //if (entityContext != null)
            //    entityContext.Dispose();
        }
    }
}
