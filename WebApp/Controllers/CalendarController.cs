using FluentDateTime;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Areas.Work.Views.DaySheet;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;

namespace WebApp.Controllers
{
    public class CalendarController : BaseController
    {
        public CalendarController(DateTime now, IPrincipal principal) : base(now, principal)
        {
        }

        

    }
}