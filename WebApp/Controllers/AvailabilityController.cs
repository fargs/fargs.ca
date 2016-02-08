﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.Enums;
using WebApp.ViewModels.AvailabilityViewModels;
using System.Globalization;
using WebApp.Library;
using System.Net;

namespace WebApp.Controllers
{
    [Authorize]
    public class AvailabilityController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Availability
        public async Task<ActionResult> Index(string id)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            //TODO: this will not be limited to just physicians, intakes should be able to manage their availability as well.
            string physicianId;
            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if (user.RoleCategoryId == RoleCategory.Admin && !string.IsNullOrEmpty(id))
            {
                physicianId = id;
            }
            else
            {
                physicianId = user.Id;
            }

            var availableDays = await db.AvailableDays.Where(c => c.PhysicianId == physicianId).ToListAsync();

            var selectedUser = await db.Users.SingleOrDefaultAsync(c => c.Id == physicianId);

            var model = new IndexViewModel()
            {
                AvailableDays = availableDays,
                CurrentUser = user,
                SelectedUser = selectedUser,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AddDay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var physician = db.Users.Single(c => c.Id == id);

            var availableDays = await db.AvailableDays.Where(c => c.PhysicianId == id).ToListAsync();

            var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            ViewBag.AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));

            var model = new AvailableDay()
            {
                PhysicianId = id,
                PhysicianName = physician.DisplayName,
                Day = SystemTime.Now()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDay(AvailableDay model)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if (user.RoleCategoryId != RoleCategory.Admin && model.PhysicianId != user.Id)
            {
                throw new Exception("You are not allowed to update this users information.");
            }

            if (ModelState.IsValid)
            {
                using (var db = new OrvosiEntities(User.Identity.Name))
                {
                    var dates = Request.Form["AvailableDays"].Split(',');
                    foreach (var item in dates)
                    {
                        var day = new AvailableDay()
                        {
                            Day = DateTime.Parse(item),
                            PhysicianId = model.PhysicianId,
                            CompanyId = model.CompanyId,
                            LocationId = model.LocationId,
                            IsPrebook = model.IsPrebook
                        };
                        db.AvailableDays.Add(day);
                    }
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index", new { id = model.PhysicianId });
            }
            return View(model);
        }
    }
}