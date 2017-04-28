using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.API
{
    public class ServiceRequestTaskController : BaseController
    {
        [HttpGet]
        public IHttpActionResult UrgentTaskCount()
        {
            var count = db.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .AreDueBetween(DateTime.MinValue, now)
                .AreActiveOrDone()
                .Count();

            return Ok(count);
        }
    }
}