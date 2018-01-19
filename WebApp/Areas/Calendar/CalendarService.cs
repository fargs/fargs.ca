using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Calendar
{
    public class CalendarService
    {
        private DateTime now;
        private OrvosiDbContext context;
        private int lookAheadInDays = 90;

        public CalendarService(OrvosiDbContext context, DateTime now)
        {
            this.context = context;
            this.now = now;
        }


    }
}