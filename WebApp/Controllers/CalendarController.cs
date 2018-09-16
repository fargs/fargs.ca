using FluentDateTime;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Views.Calendar;

public enum CalendarViewOptions
{
    Year, Month, Week, Day
}

namespace WebApp.Controllers
{
    public class CalendarController : BaseController
    {
        public CalendarController(DateTime now, IPrincipal principal) : base(now, principal)
        {
        }

        [ChildActionOnlyOrAjax]
        public PartialViewResult CalendarNavigation(DateTime? selectedDate, CalendarViewOptions viewOptions = CalendarViewOptions.Day)
        {
            var viewModel = new CalendarNavigationViewModel(selectedDate, now, Request, viewOptions);

            return PartialView(viewModel);
        }

    }
}