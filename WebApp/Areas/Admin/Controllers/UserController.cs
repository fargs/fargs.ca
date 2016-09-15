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
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System.Data.Entity;

namespace WebApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _db;
        private OrvosiDbContext db = new OrvosiDbContext();

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

        public UserController()
        {
            //Mapper.CreateMap<ApplicationUser, User>();
            //Mapper.CreateMap<User, ApplicationUser>();
            //Mapper.CreateMap<User, User>();
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ActionResult Index(byte parentId)
        {
            var list = db.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == parentId).ToList();

            var vm = new ListViewModel()
            {
                Users = list
            };

            return View(vm);
        }

        public ActionResult UserProfile(Guid id)
        {
            var obj = db.Profiles.Single(u => u.Id == id);
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
        public async Task<ActionResult> UserProfile(Profile profile)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ProfileViewModel()
                {
                    Profile = profile
                };
                return View(vm);
            }

            profile.ModifiedUser = User.Identity.GetUserId();
            db.Entry(profile).State = EntityState.Modified;

            var result = await db.SaveChangesAsync();

            return RedirectToAction("Index", new { parentId = 1 });

        }

        public ActionResult Account(Guid id)
        {
            var obj = db.Accounts.Single(u => u.Id == id);
            if (obj == null)
            {
                throw new Exception("User does not exist");
            }

            // drop down lists
            var companies = db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            var vm = new AccountViewModel()
            {
                Account = obj,
                Companies = new SelectList(companies, "Value", "Text", obj.CompanyId)
            };
            return View(vm);

        }

        [HttpPost]
        public async Task<ActionResult> Account(Account account)
        {
            if (!ModelState.IsValid)
            {
                var vm = new AccountViewModel()
                {
                    Account = account
                };
                return View(vm);
            }

            account.ModifiedUser = User.Identity.GetUserId();
            db.Entry(account).State = EntityState.Modified;

            var result = await db.SaveChangesAsync();

            return RedirectToAction("Index", new { parentId = 1 });
        }

        public ActionResult Companies(Guid userId, Nullable<byte> parentId = null)
        {
            using (var db = new OrvosiDbContext(User.Identity.GetUserId()))
            {
                var obj = db.AspNetUsers.Single(u => u.Id == userId);
                if (obj == null)
                {
                    throw new Exception("User does not exist");
                }

                // drop down lists
                var companies = db.PhysicianCompanies
                    .Include(pc => pc.Company)
                    .Include(pc => pc.PhysicianCompanyStatu)
                    .Where(p => p.PhysicianId == userId)
                    .ToList(); //&& p.ParentId == parentId || p.CompanyId == p.ParentId).ToList(); // exclude examworks and scm

                var vm = new CompaniesViewModel()
                {
                    User = obj,
                    Companies = companies
                };
                return View("Companies", vm);
            }
        }

        //public async Task<ActionResult> Edit(string id)
        //{
        //    using (var db = new OrvosiDbContext(User.Identity.GetUserId()))
        //    {

        //        var obj = db.Users.Single(u => u.Id == id);
        //        if (obj == null)
        //        {
        //            throw new Exception("User does not exist");
        //        }

        //        // drop down lists
        //        var companies = db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

        //        var vm = new DetailViewModel()
        //        {
        //            User = obj,
        //            Companies = new SelectList(companies, "Value", "Text", obj.CompanyId)
        //        };

        //        return View(vm);
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var vm = new DetailViewModel()
        //        {
        //            User = user
        //        };
        //        return View(vm);
        //    }

        //    using (var db = new OrvosiDbContext(User.Identity.GetUserId()))
        //    {
        //        var existing = db.Users.Single(u => u.Id == user.Id);
        //        if (existing == null)
        //        {
        //            throw new Exception("User does not exist");
        //        }

        //        Mapper.Map(user, existing);

        //        var result = await db.SaveChangesAsync();

        //        if (result == 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View();
        //}

        public async Task<ActionResult> Delete(Guid id)
        {
            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User to be deleted does not exist");
            }
            await this.UserManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        //public async Task<ActionResult> AssessorPackages(string id)
        //{
        //    var m = await this.UserManager.FindByIdAsync(id);
        //    if (m == null)
        //    {
        //        throw new Exception("User does not exist");
        //    }

        //    var user = GetDetail(m);
        //    var companies = Db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
        //    var physicianDocs = orvosidb.PhysicianDocuments.Where(pd => pd.PhysicianId == new Guid(m.Id)).ToList();
        //    var assessorPackages = orvosidb.PhysicianAssessorPackages.Where(pd => pd.PhysicianId == new Guid(m.Id)).ToList();

        //    var vm = new AssessorPackagesViewModel()
        //    {
        //        User = user,
        //        Companies = new SelectList(companies, "Value", "Text", user.CompanyId),
        //        PhysicianDocuments = physicianDocs,
        //        PhysicianAssessorPackages = assessorPackages
        //    };
        //    return View(vm);
        //}

        public async Task<ActionResult> SendActivationEmail(Guid userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            await messenger.SendActivationEmail(user.Email, user.UserName, callbackUrl, AspNetRoles.Company);

            return RedirectToAction("Index", new { parentId = 1 });
        }

        public async Task<ActionResult> ResetPassword(Guid userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            return RedirectToAction("Index", new { parentId = 1 });
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
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { parent = 1 });
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