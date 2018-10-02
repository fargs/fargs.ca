using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Views.Shared
{
    public abstract class ViewModelBase
    {
        public ViewModelBase() { }
        public ViewModelBase(IIdentity identity, DateTime now)
        {
            LoggedInRoleId = identity.GetRoleId();
            LoggedInUserId = identity.GetGuidUserId();
            PhysicianId = identity.GetUserContext().Id;
            AuthorizedFeatures = identity.GetFeatures();
            Now = now;
        }

        public Guid LoggedInRoleId { get; set; }
        public Guid LoggedInUserId { get; set; }
        public Guid PhysicianId { get; set; }
        public IEnumerable<short> AuthorizedFeatures { get; set; }
        public DateTime Now { get; set; }
    }
}