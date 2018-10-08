using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Areas.Work.Views.Additionals;

namespace WebApp.Areas.Work.Controllers
{
    public class AdditionalsController : BaseController
    {
        private OrvosiDbContext db;

        public AdditionalsController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            var additionals = new AdditionalsViewModel(db, identity, now);

            var viewModel = new IndexViewModel(additionals);

            return View(viewModel);
        }
        public ActionResult AdditionalsCount()
        {
            var count = db.ServiceRequests
                .CanAccess(this.loggedInUserId, physicianId, loggedInRoleId)
                .AreNotClosed()
                .HaveNoAppointment()
                .Count();

            return PartialView("AdditionalsHeading", count);
        }
    }
}