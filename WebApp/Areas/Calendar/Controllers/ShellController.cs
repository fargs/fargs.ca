using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Areas.Calendar.Controllers
{
    public class ShellController : BaseController
    {
        private CalendarService service;

        public ShellController(CalendarService service)
        {
            this.service = service;
        }
        // GET: Calendar/Shell
        public ActionResult Index()
        {
            
            return View();
        }
    }
}