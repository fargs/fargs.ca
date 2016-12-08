using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orvosi.Data;
using System.Data.Entity;
using System.Web.Http.Results;

namespace WebApp.API
{
    public class AvailableSlotController : ApiController
    {
        OrvosiDbContext db = new OrvosiDbContext();

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
                    CompanyName = ad.Company.Name,
                    LocationId = ad.LocationId,
                    LocationName = ad.Address.Name,
                    LocationOwner = ad.Address.OwnerGuid,
                    Slots = ad.AvailableSlots
                        .OrderBy(s => s.StartTime)
                        .Select(s => new
                        {
                            Id = s.Id,
                            StartTime = s.StartTime,
                            Duration = s.Duration,
                            Title = (s.ServiceRequests.Any() ? s.ServiceRequests.FirstOrDefault().ClaimantName + " - " + s.ServiceRequests.FirstOrDefault().Id.ToString() : string.Empty),
                            IsAvailable = !s.ServiceRequests.Any()
                        })
                });
        }
    }
}