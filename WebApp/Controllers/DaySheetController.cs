using System;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using Orvosi.Data;
using System.Security.Principal;
using WebApp.Models;
using WebApp.Views.Calendar;
using WebApp.Views.Work.DaySheet;
using IndexViewModel = WebApp.Views.Work.DaySheet.IndexViewModel;

namespace WebApp.Controllers
{
    public class DaySheetController : BaseController
    {
        private DateTime _selectedDate;
        private OrvosiDbContext db;
        public DaySheetController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            // calendar navigation component
            var calendarNavigation = new CalendarNavigationViewModel(_selectedDate, now, this.Request);

            // day sheet component
            var daySheet = new DaySheetViewModel(_selectedDate, db, loggedInUserId, physicianId, loggedInRoleId);

            // this view model
            var viewModel = new IndexViewModel(calendarNavigation, daySheet);

            return View("~/Views/Work/DaySheet/Index.cshtml", viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult DaySheet(DateTime selectedDate)
        {
            var viewModel = new DaySheetViewModel(selectedDate, db, loggedInUserId, physicianId, loggedInRoleId);

            return PartialView(viewModel);
        }
    }
}