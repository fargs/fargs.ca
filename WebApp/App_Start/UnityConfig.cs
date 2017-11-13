using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Orvosi.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp.Models;
using Microsoft.Owin.Security;
using System.Web;
using System;
using System.Security.Principal;
using System.Security.Claims;
using WebApp.Library;
using WebApp.Areas.Reports.Data;

namespace WebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterTypes(AllClasses.FromAssemblies(), WithMappings.FromMatchingInterface, WithName.Default);

            container.RegisterType<DateTime>(new InjectionFactory(c => DateTime.Now));

            container.RegisterType<IPrincipal>(new InjectionFactory(c => HttpContext.Current.User));

            container.RegisterType<ReportsContext>(new HierarchicalLifetimeManager(), new InjectionConstructor());

            container.RegisterType<OrvosiDbContext>(new HierarchicalLifetimeManager(), new InjectionConstructor());

            container.RegisterType<SessionService>(new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(OrvosiDbContext), typeof(IPrincipal)));

            container.RegisterType<WorkService>(new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(OrvosiDbContext), typeof(IPrincipal)));

            container.RegisterType<AccountingService>(new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(OrvosiDbContext), typeof(IPrincipal)));

            container.RegisterType<ViewDataService>(new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(OrvosiDbContext), typeof(IPrincipal)));

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser, Guid>, UserStore<ApplicationUser, ApplicationRole, Guid,  ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));

            container.RegisterType<IRoleStore<ApplicationRole, Guid>, RoleStore<ApplicationRole, Guid, ApplicationUserRole>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}