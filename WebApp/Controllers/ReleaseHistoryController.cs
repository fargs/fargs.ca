﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Filters;

namespace WebApp.Controllers
{
    [AuthorizeRole]
    public class ReleaseHistoryController : Controller
    {
        // GET: ReleaseHistory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Oct2017()
        {
            return View();
        }
    }
}