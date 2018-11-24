using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using System.Configuration;
using System.Web.Mvc;
using Google.Apis.Calendar.v3;
using ImeHub.Data;
using WebApp.Library.Extensions;
using System.Security.Principal;

namespace WebApp.Library.GoogleHelpers
{
    public class AppFlowMetadata : FlowMetadata
    {
        private IImeHubDbContext db;
        private IAuthorizationCodeFlow flow;
        public AppFlowMetadata(IImeHubDbContext db, Guid userId)
        {
            this.db = db;

            this.flow =
                new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = ConfigurationManager.AppSettings["ImeHubGoogleClientId"],
                        ClientSecret = ConfigurationManager.AppSettings["ImeHubGoogleClientSecret"]
                    },
                    Scopes = new[] {
                        "openid",
                        "profile",
                        "email",
                        Google.Apis.Gmail.v1.GmailService.Scope.GmailCompose,
                        Google.Apis.Gmail.v1.GmailService.Scope.GmailSend,
                        CalendarService.Scope.Calendar,  // Manage your calendars
                        CalendarService.Scope.CalendarReadonly, // View your Calendars
                    },
                    DataStore = new GoogleDatabaseStore(db, userId)
                });
        }

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            var user = controller.User.Identity.GetGuidUserId();
            if (user == null)
            {
                user = Guid.NewGuid();
                controller.Session["user"] = user;
            }
            return user.ToString();

        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}