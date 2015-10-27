﻿using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _db;

        public async Task<ActionResult> Index(string schedulingProcess = "ByPhysician")
        {
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            _db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new DashboardViewModel();
            model.UserDisplayName = user.DisplayName;
            model.SchedulingProcess = schedulingProcess;
            if (user.Roles.Count > 0)
            {
                var roleId = user.Roles.First().RoleId;
                model.UserRoleName = _roleManager.Roles.SingleOrDefault(c => c.Id == roleId).Name;
            }
            var company = _db.Companies.SingleOrDefault(c => c.Id == user.CompanyId);
            if (company != null)
            {
                model.UserCompanyDisplayName = company.Name;
                model.UserCompanyLogoCssClass = company.LogoCssClass;

                if (model.SchedulingProcess == "ByTime")
                {
                    model.BookingPageName = company.MasterBookingPageByTime;
                    model.Instructions = "Pick your file, pick your time, and we will find you a physician";
                }
                else if (model.SchedulingProcess == "Teleconference")
                {
                    model.BookingPageName = company.MasterBookingPageTeleconference;
                    model.Instructions = "If you do not see your physician they do not have set hours. Please click here to submit your request.";
                }
                else if (model.SchedulingProcess == "ByPhysician")
                {
                    model.BookingPageName = company.MasterBookingPageByPhysician;
                    model.Instructions = "Pick your file, pick your physician, then pick your time";
                }
            }
            return View(model);
        }

        
    }
}