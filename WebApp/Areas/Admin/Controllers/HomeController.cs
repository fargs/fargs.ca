﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        //[Authorize(Roles = "Super Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}