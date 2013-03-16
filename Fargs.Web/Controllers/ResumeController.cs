using Fargs.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fargs.Web.Controllers
{
    public class ResumeController : Controller
    {
        //
        // GET: /Resume/

        public ActionResult Index()
        {
            var job = new Job()
            {
                StartDate = new DateTime(2004, 5, 1)
            };

            var jobs = new List<Job>();
            jobs.Add(job);

            var resume = new Resume()
            {
                Title = "Resume",
                Jobs = jobs
            };

            return View(resume);
        }

    }
}
