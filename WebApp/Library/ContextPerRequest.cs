using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.ViewModels;
using WebApp.Library.Extensions;
using Orvosi.Shared.Enums;

namespace WebApp.Library
{
    public class ContextPerRequest
    {
        public const string dbKey = "dbContext";
        public const string workServiceKey = "workServiceKey";
        public const string viewDataServiceKey = "viewDataServiceKey";
        public const string featuresKey = "features";

        public static OrvosiDbContext db
        {
            get
            {
                var db = HttpContext.Current.Items[dbKey] as OrvosiDbContext;
                if (db == null)
                {
                    HttpContext.Current.Items[dbKey] = new OrvosiDbContext();
                }
                return HttpContext.Current.Items[dbKey] as OrvosiDbContext;
            }
        }

        public static WorkService WorkService
        {
            get
            {
                var service = HttpContext.Current.Items[workServiceKey] as WorkService;
                if (service == null)
                {
                    service = new WorkService(db, null);
                    HttpContext.Current.Items[dbKey] = service;
                }
                return service;
            }
        }

        public static ViewDataService ViewDataService
        {
            get
            {
                var service = HttpContext.Current.Items[viewDataServiceKey] as ViewDataService;
                if (service == null)
                {
                    service = new ViewDataService(HttpContext.Current.User.Identity);
                    HttpContext.Current.Items[viewDataServiceKey] = service;
                }
                return service;
            }
        }

        public static short[] AuthorizedFeatures
        {
            get
            {
                short[] roleFeatures = HttpContext.Current.Items[featuresKey] as short[];
                var identity = HttpContext.Current.User.Identity;
                if (identity.IsAuthenticated && roleFeatures == null)
                {
                    var roleId = HttpContext.Current.User.Identity.GetRoleId();
                    if (roleId == AspNetRoles.SuperAdmin) // Features list is used to hide/show elements in the views so the entire list is needed.
                        roleFeatures = db.Features.Select(srf => srf.Id).ToArray();
                    else
                        roleFeatures = db.AspNetRolesFeatures.Where(srf => srf.AspNetRolesId == roleId).Select(srf => srf.FeatureId).ToArray();

                    HttpContext.Current.Items[featuresKey] = roleFeatures;

                }
                return roleFeatures;
            }
        }
    }
}