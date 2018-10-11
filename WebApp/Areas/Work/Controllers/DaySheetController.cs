using Orvosi.Data;
using System;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Areas.Work.Views.DaySheet;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using FluentDateTime;
using WebApp.Areas.Work.Views.DaySheet.ServiceRequest;
using WebApp.Areas.Work.Views.DaySheet.ServiceRequest.TaskList;

namespace WebApp.Areas.Work.Controllers
{
    public class DaySheetController : BaseController
    {
        private OrvosiDbContext db;

        public DaySheetController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index(DateTime? selectedDate)
        {
            selectedDate = selectedDate.GetValueOrDefault(now);
            
            // calendar navigation component
            var calendarNavigation = new CalendarNavigationViewModel(db, selectedDate.Value, Request, identity, now);

            // day sheet component
            var daySheet = new DaySheetViewModel(selectedDate.Value, db, identity, now);

            // this view model
            var viewModel = new IndexViewModel(calendarNavigation, daySheet);

            return View(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult CalendarNavigation(DateTime selectedDate)
        {
            var viewModel = new CalendarNavigationViewModel(db, selectedDate, Request, identity, now);

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult DaySheet(DateTime selectedDate)
        {
            var viewModel = new DaySheetViewModel(selectedDate, db, identity, now);

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult TaskList(int serviceRequestId)
        {
            var viewModel = new TaskListViewModel(serviceRequestId, db, identity, now);

            return PartialView("ServiceRequest/TaskList/TaskList", viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult ServiceRequestSummary(int serviceRequestId)
        {
            var viewModel = new SummaryViewModel(serviceRequestId, db);

            return PartialView("ServiceRequest/Summary", viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public PartialViewResult ServiceRequestActionMenu(int serviceRequestId)
        {
            var viewModel = new ActionMenuViewModel(serviceRequestId, db, identity, now);

            return PartialView("ServiceRequest/ActionMenu", viewModel);
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