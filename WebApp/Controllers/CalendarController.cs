using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using WebApp.Library.Google;

namespace WebApp.Controllers
{
    public class CalendarController : Controller
    {
        Model.OrvosiEntities db = new Model.OrvosiEntities();

        public ActionResult GetAvailability(string id)
        {
            //TODO: We could abstract the calendar provider to allow physicians to use calendars outside of orvosi's google apps
            // Get the physician google apps account
            var result = db.GetPhysicianGoogleAccount(id).ToList();

            Model.GetPhysicianGoogleAccount_Result account = null;

            if (result.Count != 1)
            {
                throw new Exception("The physician must have a google apps for work account with orvosi.ca.");
            }
            else
            {
                account = result.First();    
            }

            // p12 certificate path.
            string GoogleOAuth2CertificatePath = ConfigurationManager.AppSettings["GoogleOAuth2CertificatePath"];

            // @developer... e-mail address.
            string GoogleOAuth2EmailAddress = ConfigurationManager.AppSettings["GoogleOAuth2EmailAddress"];

            // certificate password ("notasecret").
            string GoogleOAuth2PrivateKey = ConfigurationManager.AppSettings["GoogleOAuth2PrivateKey"];

            CalendarService service = Authentication.AuthenticateServiceAccount(GoogleOAuth2EmailAddress, Path.Combine(Server.MapPath("~/"), GoogleOAuth2CertificatePath), account.Email);

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now.AddDays(-7);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Console.WriteLine("Upcoming events:");
            Events events = request.Execute();
            var model = new
            {
                data = events
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}