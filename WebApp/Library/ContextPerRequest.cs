using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.ViewModels;
using WebApp.Library.Extensions;

namespace WebApp.Library
{
    public class ContextPerRequest
    {
        public const string dbKey = "dbContext";
        public const string workServiceKey = "workServiceKey";

        public const string userKey = "userContext";
        public const string physicianKey = "physicianContext";

        public Guid UserId { get; set; }
        public Guid PhysicianId { get; set; }

        public ContextPerRequest(IIdentity identity)
        {
            HttpContext.Current.Items[userKey] = identity.GetUserContext();

            HttpContext.Current.Items[physicianKey] = identity.GetApplicationUser();

        }

        public static UserContextViewModel User
        {
            get
            {
                return HttpContext.Current.Items[userKey] as UserContextViewModel;
            }
        }

        public static UserContextViewModel Physician
        {
            get
            {
                return HttpContext.Current.Items[physicianKey] as UserContextViewModel;
            }
        }

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
    }
}