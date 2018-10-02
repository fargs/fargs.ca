using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;
using WebApp.Areas.Shared;
using WebApp.Areas.Work.Views.Tasks;

namespace WebApp.Areas.Work.Controllers
{
    public class TasksController : BaseController
    {
        private OrvosiDbContext db;

        public TasksController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            var tasks = new TasksViewModel(db, Request, identity, now);

            var viewModel = new IndexViewModel(tasks);

            return View(viewModel);
        }
    }
}