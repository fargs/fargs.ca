using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using System.Data.Entity;
using System.Web.Http.Results;

namespace WebApp.API
{
    public class AvailableSlotController : ApiController
    {
        OrvosiEntities db = new OrvosiEntities();
        
        [Route("api/physician/{physicianId}/day/{day}/slots")]
        public HttpResponseMessage GetByAvailableDay(DateTime day, Guid physicianId)
        {
            var ad = db.AvailableDays
                .SingleOrDefault(c => c.PhysicianId == physicianId && c.Day == day);

            if (ad == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "Not available this day.");
            }

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new
                {
                    Day = ad.Day,
                    IsPrebook = ad.IsPrebook,
                    CompanyId = ad.CompanyId,
                    CompanyName = ad.CompanyName,
                    LocationId = ad.LocationId,
                    LocationName = ad.LocationName,
                    LocationOwner = ad.LocationOwnerDisplayName,
                    Slots = ad.AvailableSlots
                        .OrderBy(s => s.StartTime)
                        .Select(s => new
                        {
                            Id = s.Id,
                            StartTime = s.StartTime,
                            Duration = s.Duration,
                            Title = s.Title,
                            IsAvailable = !s.ServiceRequestId.HasValue
                        })
                });
        }
    }
}