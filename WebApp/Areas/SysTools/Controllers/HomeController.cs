using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Extensions;

namespace WebApp.Areas.SysTools.Controllers
{
    public class HomeController : Controller
    {
        private ImeHub.Data.IImeHubDbContext db;
        public HomeController(ImeHub.Data.IImeHubDbContext db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SeedDatabase()
        {
            var initializer = new ImeHub.Models.Database.DbInitializer(db);
            initializer.SeedDatabase();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult GetPrimaryCalendar(string emailTo)
        {
            var clientId = ConfigurationManager.AppSettings["ImeHubGoogleClientId"];
            var clientSecret = ConfigurationManager.AppSettings["ImeHubGoogleClientSecret"];

            string[] scopes = new string[] {
                CalendarService.Scope.Calendar  ,  // Manage your calendars
                CalendarService.Scope.CalendarReadonly    // View your Calendars
            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                    , scopes
                                                                    , emailTo
                                                                    , CancellationToken.None
                                                                    , new GoogleDatabaseStore(db, User.Identity.GetGuidUserId())).Result;


                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar API Sample",
                });
                return View();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }
        }
        public ActionResult SendEmailFromUsersLinkedEmailAccount(string emailTo)
        {
            var clientId = ConfigurationManager.AppSettings["ImeHubGoogleClientId"];
            var clientSecret = ConfigurationManager.AppSettings["ImeHubGoogleClientSecret"];

            string[] scopes = new string[] {
                CalendarService.Scope.Calendar  ,  // Manage your calendars
                CalendarService.Scope.CalendarReadonly    // View your Calendars
            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                    , scopes
                                                                    , emailTo
                                                                    , CancellationToken.None
                                                                    , new GoogleDatabaseStore(db, User.Identity.GetGuidUserId())).Result;


                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar API Sample",
                });


                return View();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }
        }

    }
}