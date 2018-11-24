using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.GoogleHelpers;

namespace WebApp.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private IImeHubDbContext db;
        private IIdentity identity;
        public AuthCallbackController(IImeHubDbContext db, IIdentity identity)
        {
            this.identity = identity;
            this.db = db;
        }
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(db, identity.GetGuidUserId()); }
        }
    }
}