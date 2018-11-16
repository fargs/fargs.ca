using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Models;
//using AutoMapper;
using ImeHub.Data;
using ImeHub.Models.Enums;
using System.Data.Entity;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using System.Net;
using Features = ImeHub.Models.Enums.Features;
using ImeHub.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRole(Feature = Features.UserPortal.Users.Manage)]
    public class UserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _db;
        private ImeHubDbContext db;

        public UserController(ImeHubDbContext db)
        {
            this.db = db;
        }

        public UserController(ImeHubDbContext db, ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager) : this(db)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationDbContext Db
        {
            get
            {
                return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _db = value;
            }
        }

        public ActionResult Index(byte? parentId)
        {
            var result = db.Users
                .Select(u => new ImeHub.Models.UserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Title = u.Title,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    RoleId = u.RoleId,
                    IsTestRecord = u.IsTestRecord,
                    UserName = u.UserName,
                    Role = new ImeHub.Models.UserModel.RoleModel
                    {
                        Id = u.Role.Id,
                        Name = u.Role.Name
                    }
                })
                .ToList();

            var list = result
                .Select(u => new ListViewItem
                {
                    Id = u.Id,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    DisplayName = u.DisplayName,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    IsTestRecord = u.IsTestRecord,
                    UserName = u.UserName
                })
                .ToList();

            var vm = new ListViewModel()
            {
                Users = list
            };

            return View(vm);
        }

        public ActionResult UserProfile(Guid id)
        {
            var obj = db.Users.Select(u => new ImeHub.Models.UserModel
            {
                Id = u.Id,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                Title = u.Title,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RoleId = u.RoleId,
                IsTestRecord = u.IsTestRecord,
                UserName = u.UserName
            }).Single(u => u.Id == id);

            if (obj == null)
            {
                throw new Exception("User does not exist");
            }

            // drop down lists
            var companies = db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            var vm = new ProfileViewModel()
            {
                Profile = obj
            };
            return View(vm);

        }

        [HttpPost]
        public async Task<ActionResult> UserProfile(UserModel profile)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ProfileViewModel()
                {
                    Profile = profile
                };
                return View(vm);
            }

            var user = db.Users.Find(profile.Id);
            user.Title = profile.Title;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.IsTestRecord = profile.IsTestRecord;
            user.ColorCode = profile.ColorCode;
            user.RoleId = profile.RoleId;
            
            var result = await db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        
        public async Task<ActionResult> SendActivationEmail(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            await messenger.SendActivationEmail(user.Email, user.UserName, callbackUrl);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ResetPassword(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword(Guid id)
        {
            var vm = new ViewModels.ChangePasswordViewModel() { UserId = id };
            return View(vm);
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ViewModels.ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }

                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}