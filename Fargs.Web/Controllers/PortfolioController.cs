using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fargs.Web.Models;

namespace Fargs.Web.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /Portfolio/

        public ActionResult Index()
        {
            var portfolio = new Portfolio();

            portfolio.Projects.Add(new Project() { Name = "RE-LY" });
            portfolio.Software.Add(new Software() { Name = "ROME" });

            return View(portfolio);
        }

        public ActionResult RandomizedControlledTrials()
        {
            var portfolio = new Portfolio();

            portfolio.Projects.Add(new Project() { Name = "RE-LY" });
            portfolio.Software.Add(new Software() { Name = "ROME" });

            return View(portfolio);
        }

        public ActionResult EpidemiologyStudies()
        {
            var portfolio = new Portfolio();

            portfolio.Projects.Add(new Project() { Name = "RE-LY" });
            portfolio.Software.Add(new Software() { Name = "ROME" });

            return View(portfolio);
        }

    }
}
