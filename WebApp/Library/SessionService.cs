using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Library
{
    public class SessionService
    {
        public IIdentity identity { get; }
        public LookupViewModel<Guid> userContext { get; }
        public Guid userId { get; }
        public Guid? physicianId { get; }
        public Guid currentContextId { get; }
        public Guid roleId { get; }

        private short[] authorizedFeatures;
        private OrvosiDbContext db;

        public SessionService(OrvosiDbContext db, IPrincipal principal)
        {
            this.db = db;
            identity = principal.Identity;
            userId = principal.Identity.GetGuidUserId();
            userContext = principal.Identity.GetPhysician();
            physicianId = principal.Identity.GetPhysicianId();
            currentContextId = physicianId.GetValueOrDefault(userId);
            roleId = identity.GetRoleId();
            authorizedFeatures = identity.GetFeatures();
        }

        public short[] AuthorizedFeatures
        {
            get
            {
                if (identity.IsAuthenticated && authorizedFeatures == null)
                {
                    var roleId = HttpContext.Current.User.Identity.GetRoleId();
                    if (roleId == AspNetRoles.SuperAdmin) // Features list is used to hide/show elements in the views so the entire list is needed.
                        authorizedFeatures = db.Features.Select(srf => srf.Id).ToArray();
                    else
                        authorizedFeatures  = db.AspNetRolesFeatures.Where(srf => srf.AspNetRolesId == roleId).Select(srf => srf.FeatureId).ToArray();
                }
                return authorizedFeatures;
            }
        }
    }
}
