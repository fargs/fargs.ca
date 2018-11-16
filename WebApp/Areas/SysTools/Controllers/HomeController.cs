using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.SysTools.Controllers
{
    public class HomeController : Controller
    {
        private ImeHub.Data.IImeHubDbContext db;
        public HomeController(ImeHub.Data.IImeHubDbContext db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SeedDatabase()
        {
            var initializer = new ImeHub.Models.Util.DbInitializer(db);
            initializer.SeedDatabase();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}