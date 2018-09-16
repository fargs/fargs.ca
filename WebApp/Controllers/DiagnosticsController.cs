using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity.Owin;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    [AuthorizeRole()]
    public class DiagnosticsController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult CodeFirstContextTests()
        {
            var db = new Orvosi.Data.OrvosiDbContext();
            return View(db.Invoices.First());
        }
        public PartialViewResult ConnectToBox()
        {
            var manager = new BoxManager();
            var client = manager.AdminClientAsUser("257722377");
            //var folder = await client.FoldersManager.GetInformationAsync("0");

            var userClient = manager.UserClient("257722377");
            var folder = userClient.FoldersManager.GetInformationAsync("0").Result;

            return PartialView(folder);
        }
        public HttpNotFoundResult CreateBoxFolder()
        {
            //var box = new BoxManager();

            //    var request = db.ServiceRequests.Single(sr => sr.Id == 131);
            //    var caseFolder = box.CreateCaseFolder("7027883033", request.ProvinceName, request.AppointmentDate.Value, request.Title, );
            //    return PartialView(caseFolder);

            return HttpNotFound();
        }
        // GET: Diagnostics
        public async Task<PartialViewResult> SendEmail(string to)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            await userManager.SendEmailAsync(user.Id, "Test Email Subject", "Test Email Body");
            return PartialView();
        }
        public async Task<PartialViewResult> SendActivationEmail(string email)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(email);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendActivationEmail(user.Email, user.UserName, callbackUrl, AspNetRoles.Company);

            return PartialView("SendEmail");
        }
        public async Task<PartialViewResult> SendResetPasswordEmail(string email)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(email);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            return PartialView("SendEmail");
        }
        public ViewResult DashboardPerformanceTest()
        {
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var context = new OrvosiDbContext();
                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
                var now = SystemTime.Now();
                var userId = new Guid("8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9");

                var requests = new List<GetAssignedServiceRequestsReturnModel>(); // await context.API_GetAssignedServiceRequestsAsync(userId, now);

                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
                return View(requests);
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
            }
            return View();

        }
        [HttpPost]
        public JsonResult GetListOfGoogleCalendars(string email)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);
            CalendarList calendars = service.CalendarList.List().Execute();
            var model = new
            {
                data = calendars
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetListOfEventsGoogleCalendar(string email, DateTime start, DateTime end)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = start;
            request.TimeMax = end;
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

        [HttpPost]
        public JsonResult GetEventGoogleCalendar(string email, string eventId)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);

            // Define parameters of request.
            Event e = service.Events.Get("primary", eventId).Execute();
            var model = new
            {
                data = e
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddEventToGoogleCalendar(string email, DateTime start, bool notify = false)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);

            Event e = new Event()
            {
                Summary = "Test Event",
                Location = "14 Farmingdale Cres, Stoney Creek ON",
                Start = new EventDateTime()
                {
                    DateTime = start,
                    TimeZone = "America/New_York"
                },
                End = new EventDateTime()
                {
                    DateTime = start.AddHours(1),
                    TimeZone = "America/New_York"
                }
            };

            EventsResource.InsertRequest request = service.Events.Insert(e, "primary");
            request.SendNotifications = true;
            Event thisevent = request.Execute(); // Another error. "Does not contain a definition for Fetch"
            
            return Json(thisevent, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelEventGoogleCalendar(string email, string eventId)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);

            // Define parameters of request.
            var request = service.Events.Delete("primary", eventId);
            request.SendNotifications = true;
            var result = request.Execute();
            var model = new
            {
                data = result
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateEventToGoogleCalendar(string email, string eventId, DateTime start)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(email);

            var patch = new Event()
            {
                Start = new EventDateTime()
                {
                    DateTime = start,
                    TimeZone = "America/Toronto"
                },
                End = new EventDateTime()
                {
                    DateTime = start.AddHours(1),
                    TimeZone = "America/Toronto"
                }
            };
            var request = service.Events.Patch(patch, "primary", eventId);
            request.SendNotifications = true;
            var result = request.Execute();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAttendeeToEventGoogleCalendar(string ownerEmail, string attendeeEmail, string eventId)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(ownerEmail);

            Event originalEvent = service.Events.Get("primary", eventId).Execute();
            EventAttendee attendee = new EventAttendee()
            {
                Email = attendeeEmail,
                ResponseStatus = "accepted"
            };
            if (originalEvent.Attendees == null)
            {
                originalEvent.Attendees = new List<EventAttendee>();
            }
            originalEvent.Attendees.Add(attendee);
            var request = service.Events.Update(
                originalEvent,
                "primary",
                eventId);
            request.SendNotifications = true;
            var result = request.Execute();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAttendeeToEventGoogleCalendar(string ownerEmail, string attendeeEmail, string eventId)
        {
            var service = new WebApp.Library.GoogleServices().GetCalendarService(ownerEmail);

            Event originalEvent = service.Events.Get("primary", eventId).Execute();
            if (originalEvent.Attendees != null)
            {
                var attendee = originalEvent.Attendees.SingleOrDefault(a => a.Email == attendeeEmail);
                if (attendee != null)
                {
                    originalEvent.Attendees.Remove(attendee);
                }
            }
            var request = service.Events.Update(
                originalEvent,
                "primary",
                eventId);
            request.SendNotifications = true;
            var result = request.Execute();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task SendEmailFrom(string from, string to)
        {
            var message = BuildSendTestMailMessage(to, from, Request.GetBaseUrl());
            var service = new GoogleServices();
            await service.SendEmailAsync(message);
        }

        [AuthorizeRole(Feature = features.Accounting.CreateInvoice)]
        public HttpStatusCodeResult AuthorizationAttribute_WithCreateInvoiceFeature()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [AuthorizeRole(Features = new short[2] { features.Accounting.CreateInvoice, features.Accounting.ViewInvoice })]
        public HttpStatusCodeResult AuthorizationAttribute_WithCreateInvoiceAndReadInvoiceFeatures()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [AuthorizeRole]
        public HttpStatusCodeResult AuthorizationAttribute_WithNoFeatures()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public HttpStatusCodeResult AuthorizationAttribute_NoAttribute()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private MailMessage BuildSendTestMailMessage(string to, string from, string baseUrl)
        {
            var message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = string.Format("Orvosi Diagnostics - Email from {0} - {1}", from, to);
            message.IsBodyHtml = true;
            message.Bcc.Add("lfarago@orvosi.ca,afarago@orvosi.ca");

            ViewData["BaseUrl"] = baseUrl; //This is needed because the full address needs to be included in the email download link
            message.Body = WebApp.Library.Helpers.HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Shared/NotificationTemplates/TestEmail.cshtml", "http://www.google.ca");

            return message;
        }

    }
}