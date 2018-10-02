using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Areas.Shared;
using WebApp.Areas.Work.Views.Schedule;
using WebApp.Models;

namespace WebApp.Areas.Work.Controllers
{
    public class ScheduleController : BaseController
    {
        private OrvosiDbContext db;

        public ScheduleController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            // week list component
            var weekList = new ScheduleViewModel(db, identity, now);

            // this view model
            var viewModel = new IndexViewModel(weekList);

            return View(viewModel);
        }

        public PartialViewResult Week(DateTime firstDayOfWeek)
        {
            var viewModel = new WeekViewModel(db, identity, now, firstDayOfWeek);
            
            return PartialView("Week/Week", viewModel);
        }

    }
}