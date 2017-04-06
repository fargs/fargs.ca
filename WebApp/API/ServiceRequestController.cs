using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Orvosi.Data;
using System.Data.Entity;
using System.Web.Http.Results;
using WebApp.Library.Extensions;
using WebApp.Library;
using WebApp.Models;
using Orvosi.Data.Filters;
using FluentDateTime;

namespace WebApp.API
{
    public class ServiceRequestController: ApiController
    {
        private OrvosiDbContext db;

        public ServiceRequestController()
        {
            db = ContextPerRequest.db;
        }

        /// <summary>
        /// View References:
        ///  - Views/Dashboard/Schedule
        /// </summary>
        /// <param name="serviceRequestId"></param>
        /// <returns></returns>
        [Route("api/servicerequest/appointmentdate/{serviceRequestId}")]
        public IHttpActionResult GetAppointmentDate(int serviceRequestId)
        {
            var result = db.ServiceRequests
                .HaveAppointment()
                .WithId(serviceRequestId)
                .Select(sr => new
                {
                    sr.Id,
                    sr.AppointmentDate,
                })
                .Single();

            return Ok(new {
                result.Id,
                result.AppointmentDate,
                FirstDayOfWeek = result.AppointmentDate.Value.FirstDayOfWeek(),
                FirstDayOfWeekTicks = result.AppointmentDate.Value.FirstDayOfWeek().Ticks
            });
        }

    }
}