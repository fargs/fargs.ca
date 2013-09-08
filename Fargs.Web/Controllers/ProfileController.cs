using Fargs.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fargs.Web.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            var job = new Job()
            {
                StartDate = new DateTime(2004, 5, 1)
            };

            var jobs = new List<Job>();
            jobs.Add(job);

            var profile = new Profile()
            {
                Title = "Profile",
                Jobs = jobs
            };

            return View(profile);
        }

    }
}
