using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Projections;

namespace WebApp.Controllers.Google
{
    public class CalendarController : Controller
    {
        
        [Route("Google/Calendar/Get")]
        public ActionResult Get(int serviceRequestId)
        {
            
            using (var context = new OrvosiDbContext())
            {
                var sr = context.ServiceRequests
                    .WithId(serviceRequestId)
                    .Select(ServiceRequestProjections.ForGoogleCalendar(Request.GetBaseUrl()))
                    .Single();

                if (string.IsNullOrEmpty(sr.Physician.Email)
                    || !sr.AppointmentDate.HasValue)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var service = new WebApp.Library.GoogleServices().GetCalendarService(GetPhysicianCalendarEmail(sr.Physician.Email));

                Event result = null;
                if (!string.IsNullOrEmpty(sr.CalendarEventId))
                {
                    // Define parameters of request.
                    result = service.Events.Get("primary", sr.CalendarEventId).Execute();
                }
                else
                {
                    var startTime = sr.AppointmentDate.Value.AddTicks(sr.StartTime.Value.Ticks + 1);
                    var endTime = sr.AppointmentDate.Value.AddTicks(sr.EndTime.Value.Ticks + 1);
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(sr.Address.TimeZone);

                    var startTimeOffset = new DateTimeOffset(startTime, timeZone.GetUtcOffset(startTime));
                    var endTimeOffset = new DateTimeOffset(endTime, timeZone.GetUtcOffset(endTime));

                    EventsResource.ListRequest request = service.Events.List("primary");
                    request.TimeZone = sr.Address.TimeZoneIana;
                    request.TimeMin = startTimeOffset.LocalDateTime;
                    request.TimeMax = endTimeOffset.LocalDateTime;
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.MaxResults = 10;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
                    Events list = request.Execute();
                    result = list.Items.FirstOrDefault();
                }

                if (result == null)
                {
                    return PartialView("~/Views/Google/Calendar/Add.cshtml", sr.Id);
                }

                if (result.Attendees == null) result.Attendees = new List<EventAttendee>();
                return PartialView("~/Views/Google/Calendar/Get.cshtml", result);
            }
        }

        private string GetPhysicianCalendarEmail(string email)
        {
            if (email == "zwaseem@orvosi.ca")
            {
                return "ime@orvosi.ca";
            }
            return email;
        }

        [Route("Google/Calendar/List")]
        public ActionResult List(int serviceRequestId)
        {
            using (var context = new OrvosiDbContext())
            {
                var sr = context.ServiceRequests
                    .WithId(serviceRequestId)
                    .Select(ServiceRequestProjections.ForGoogleCalendar(Request.GetBaseUrl()))
                    .Single();

                if (string.IsNullOrEmpty(sr.Physician.Email))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (string.IsNullOrEmpty(sr.CalendarEventId))
                {
                    return PartialView("~/Views/Google/Calendar/Add.cshtml", sr.Id);
                }

                var service = new WebApp.Library.GoogleServices().GetCalendarService(GetPhysicianCalendarEmail(sr.Physician.Email));

                // Define parameters of request.
                Event result = service.Events.Get("primary", sr.CalendarEventId).Execute();
                return PartialView("~/Views/Google/Calendar/Get.cshtml", result);
            }
        }

        [Route("Google/Calendar/Add")]
        public ActionResult Add(int serviceRequestId)
        {
            using (var context = new OrvosiDbContext())
            {
                var sr = context.ServiceRequests.WithId(serviceRequestId)
                    .Select(WebApp.Library.Projections.ServiceRequestProjections.ForGoogleCalendar(Request.GetBaseUrl()))
                    .Single();

                var service = new WebApp.Library.GoogleServices().GetCalendarService(GetPhysicianCalendarEmail(sr.Physician.Email));

                var startTime = sr.AppointmentDate.Value.AddTicks(sr.StartTime.Value.Ticks + 1);
                var endTime = sr.AppointmentDate.Value.AddTicks(sr.EndTime.Value.Ticks + 1);
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(sr.Address.TimeZone);

                var startTimeOffset = new DateTimeOffset(startTime, timeZone.GetUtcOffset(startTime));
                var endTimeOffset = new DateTimeOffset(endTime, timeZone.GetUtcOffset(endTime));

                // Define parameters of request.
                Event e = new Event()
                {
                    Summary = sr.CalendarEventTitle,
                    Location = sr.Address.ToString(),
                    Description = string.Format("<div>Case Details Link: <a href='{0}'>{0}</a></div>", sr.URL),
                    Start = new EventDateTime()
                    {
                        DateTime = startTimeOffset.LocalDateTime,
                        TimeZone = sr.Address.TimeZoneIana
                    },
                    End = new EventDateTime()
                    {
                        DateTime = endTimeOffset.LocalDateTime,
                        TimeZone = sr.Address.TimeZoneIana
                    }
                };

                EventsResource.InsertRequest request = service.Events.Insert(e, "primary");
                request.SendNotifications = true;
                Event result = request.Execute();

                // Save the Id to our database
                sr.CalendarEventId = result.Id;
                context.SaveChanges();

                return Json(new
                {
                    eventId = result.Id
                });
            }
        }
    }
}