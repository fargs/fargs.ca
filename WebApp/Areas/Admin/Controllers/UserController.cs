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
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using System.Net;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRole(Feature = Features.SecurityAdmin.UserManagement)]
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

        public ActionResult Index(byte? parentId)
        {
            var list = db.AspNetUsers.AsQueryable();
            if (parentId.HasValue)
            {
                list = list.Where(u => u.AspNetUserRoles.FirstOrDefault().AspNetRole.RoleCategoryId == parentId);
            }
            else
            {
                list = list.Where(u => !u.AspNetUserRoles.Any());
            }

            var result = list
                .AsEnumerable()
                .Select(u => new ListViewItem
                {
                    Id = u.Id,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    DisplayName = u.GetDisplayName(),
                    RoleId = !u.AspNetUserRoles.Any() ? (Guid?)null : u.GetRoleId(),
                    RoleName = !u.AspNetUserRoles.Any() ? string.Empty : u.GetRole().Name,
                    IsTestRecord = u.IsTestRecord,
                    UserName = u.UserName
                })
                .ToList();

            var vm = new ListViewModel()
            {
                Users = result
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

            var user = db.AspNetUsers.Find(profile.Id);
            user.Title = profile.Title;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.LogoCssClass = profile.LogoCssClass;
            user.EmployeeId = profile.EmployeeId;
            user.IsTestRecord = profile.IsTestRecord;
            user.ColorCode = profile.ColorCode;
            user.HstNumber = profile.HstNumber;
            user.BoxFolderId = profile.BoxFolderId;
            user.BoxUserId = profile.BoxUserId;
            user.ModifiedDate = SystemTime.Now();
            user.ModifiedUser = User.Identity.GetGuidUserId().ToString();

            var result = await db.SaveChangesAsync();

            var roleId = user.GetRoleId();
            var roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId).RoleCategoryId;
            return RedirectToAction("Index", new { parentId = roleCategoryId });

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

            var roles = db.AspNetRoles.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();

            var vm = new AccountViewModel()
            {
                Account = obj,
                Companies = new SelectList(companies, "Value", "Text", obj.CompanyId),
                Roles = new SelectList(roles, "Value", "Text", obj.RoleId)
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

            var user = db.AspNetUsers.Find(account.Id);
            user.UserName = account.UserName;
            user.Email = account.Email;
            user.EmailConfirmed = account.EmailConfirmed;
            user.LockoutEndDateUtc = account.LockoutEndDateUtc;
            user.LockoutEnabled = account.LockoutEnabled;
            user.AccessFailedCount = account.AccessFailedCount;
            user.PhoneNumber = account.PhoneNumber;
            user.PhoneNumberConfirmed = account.PhoneNumberConfirmed;
            user.TwoFactorEnabled = account.TwoFactorEnabled;
            user.ModifiedDate = SystemTime.Now();
            user.ModifiedUser = User.Identity.GetGuidUserId().ToString();
            user.IsAppTester = account.IsAppTester;

            var userRoles = db.AspNetUserRoles.Where(ur => ur.UserId == account.Id);
            db.AspNetUserRoles.RemoveRange(userRoles);
            byte? roleCategoryId = null;
            if (account.RoleId.HasValue)
            {
                var userRole = new AspNetUserRole() { UserId = account.Id, RoleId = account.RoleId.Value, ModifiedDate = SystemTime.UtcNow(), ModifiedUser = User.Identity.GetGuidUserId().ToString() };
                db.AspNetUserRoles.Add(userRole);
                roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == userRole.RoleId).RoleCategoryId;
            }

            var result = await db.SaveChangesAsync();
            return RedirectToAction("Index", new { parentId = roleCategoryId });
        }

        public ActionResult Companies(Guid userId, Nullable<byte> parentId = null)
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

        public ActionResult Notes(Guid id)
        {
            var notes = db.AspNetUsers.Where(u => u.Id == id).Select(u => new Orvosi.Shared.Model.Person
            {
                Id = u.Id,
                Title = u.Title,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Notes = u.Notes,
            })
            .First();

            var user = db.AspNetUsers.Find(id);
            var roleId = user.GetRoleId();
            ViewBag.ParentId = db.AspNetRoles.FirstOrDefault(r => r.Id == roleId).RoleCategoryId;
            return View(notes);
        }

        [HttpPost]
        public ActionResult Notes(Orvosi.Shared.Model.Person person)
        {
            var user = db.AspNetUsers.Find(person.Id);
            user.Notes = person.Notes;
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //public async Task<ActionResult> Edit(string id)
        //{
        //    

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
        //    
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

        //    
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
        //    
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
            
            return RedirectToAction("Index", new { parentId = 1 });
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

            var userRoleId = user.Roles.First().RoleId;
            var roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == userRoleId).RoleCategoryId;
            return RedirectToAction("Index", new { parentId = roleCategoryId });
        }

        public async Task<ActionResult> ResetPassword(Guid userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            var userRoleId = user.Roles.First().RoleId;
            var roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == userRoleId).RoleCategoryId;
            return RedirectToAction("Index", new { parentId = roleCategoryId });
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
                var userRoleId = user.Roles.First().RoleId;
                var roleCategoryId = db.AspNetRoles.FirstOrDefault(r => r.Id == userRoleId).RoleCategoryId;
                return RedirectToAction("Index", new { parentId = roleCategoryId });
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