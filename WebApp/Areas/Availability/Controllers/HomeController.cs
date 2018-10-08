using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Areas.Availability.Views.Home;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Availability.Controllers
{
    public class HomeController : BaseController
    {
        private DateTime _selectedDate;
        private OrvosiDbContext db;

        public HomeController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            // calendar navigation component
            var calendarNavigation = new CalendarNavigationViewModel(_selectedDate, this.Request, now);

            // availability component
            var availability = new AvailabilityViewModel(_selectedDate, db, identity, now);

            // this view model
            var viewModel = new IndexViewModel(calendarNavigation, availability, identity, now);

            return View(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Availability.AvailabilitySection)]
        public PartialViewResult Availability(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            var viewModel = new AvailabilityViewModel(_selectedDate, db, identity, now);

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Availability.AvailabilitySection)]
        public PartialViewResult CalendarNavigation(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            var viewModel = new CalendarNavigationViewModel(_selectedDate, Request, now);

            return PartialView(viewModel);
        }
    }
}