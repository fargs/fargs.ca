using System;
using System.Security.Principal;
using System.Web.Mvc;
using Orvosi.Data;
using WebApp.Areas.Shared;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Views.Calendar;
using WebApp.Areas.Work.Views.DaySheet;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Work.Controllers
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
            var daySheet = new DaySheetViewModel(_selectedDate, db, identity, now);

            // this view model
            var viewModel = new IndexViewModel(calendarNavigation, daySheet);

            return View(viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult DaySheet(DateTime selectedDate)
        {
            var viewModel = new DaySheetViewModel(selectedDate, db, identity, now);

            return PartialView(viewModel);
        }

        //[Route("Work/DaySheet/ServiceRequest/ActionMenu")]
        //[ChildActionOnlyOrAjax]
        //[AuthorizeRole(Feature = Features.ServiceRequest.View)]
        //public PartialViewResult ServiceRequestActionMenu(int serviceRequestId)
        //{
        //    var viewModel = new ActionMenuViewModel(db, serviceRequestId, identity, now);
            
        //    return PartialView("~/Views/Work/DaySheet/ServiceRequest/ActionMenu.cshtml", viewModel);
        //}

        //[Route("Work/DaySheet/ServiceRequest/Invoices")]
        //[ChildActionOnlyOrAjax]
        //[AuthorizeRole(Feature = Features.Accounting.ManageInvoices)]
        //public PartialViewResult ServiceRequestInvoiceList(int serviceRequestId)
        //{
        //    var viewModel = new InvoiceListViewModel(db, serviceRequestId, identity, now);

        //    return PartialView("~/Views/Work/DaySheet/ServiceRequest/InvoiceList/InvoiceList.cshtml", viewModel);
        //}
    }
}