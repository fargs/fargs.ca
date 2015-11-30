using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Models.Enums;
using WebApp.Models;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace WebApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _db;

        public UserController()
        {
            Mapper.CreateMap<ApplicationUser, User>();
            Mapper.CreateMap<User, ApplicationUser>();
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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
        // GET: User
        public ActionResult Index()
        {
            var vm = new ListViewModel()
            {
                Users = GetList()
            };
            return View(vm);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User to be deleted does not exist");
            }
            await this.UserManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var m = await this.UserManager.FindByIdAsync(id);
            if (m == null)
            {
                throw new Exception("User does not exist");
            }

            var user = GetDetail(m);
            var companies = Db.Companies.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            //companies.Insert(0, new SelectListItem() { Text = "Select a company", Value = "" });

            var vm = new DetailViewModel()
            {
                User = user,
                Companies = new SelectList(companies, "Value", "Text", user.CompanyId)
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                var vm = new DetailViewModel()
                {
                    User = user
                };
                return View(vm);
            }
            
            var m = await this.UserManager.FindByIdAsync(user.Id);
            if (m == null)
            {
                throw new Exception("User does not exist");
            }

            Mapper.Map(user, m);

            //if (m.CompanyId == 0)
            //{
            //    m.CompanyId = null;
            //}
            IdentityResult result = await this.UserManager.UpdateAsync(m);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View();
        }

        public async Task<ActionResult> SendActivationEmail(string userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area="", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            await messenger.SendActivationEmail(user.Email, user.UserName, callbackUrl, Roles.Company);

            return Json(new
            {
                success = true,
                message = "Email was sent successfully"
            });
        }

        public async Task<ActionResult> ResetPassword(string userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { area="", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            return Json(new
            {
                success = true,
                message = "Email was sent successfully"
            });
        }

        private List<User> GetList()
        {
            var result = new List<User>();
            foreach (var item in this.UserManager.Users.ToList())
            {
                var obj = GetDetail(item);
                result.Add(obj);

            }
            return result;
        }

        private User GetDetail(ApplicationUser item)
        {
            // Auto map properties
            var obj = Mapper.Map<User>(item);

            // Set properties we not will not be mapped automatically.
            // TODO: See if this could be configured in AutoMapper configuration.
            string roleId = string.Empty;
            if (item.Roles.Count > 0)
                roleId = item.Roles.First().RoleId;
            else
                roleId = string.Empty;

            if (item.Roles.Count > 0)
                obj.RoleName = this.RoleManager.Roles.SingleOrDefault(c => c.Id == roleId).Name;
            else
                obj.RoleName = string.Empty;

            obj.CompanyNameSubmitted = item.CompanyName;
            if (item.CompanyId.HasValue)
                obj.CompanyName = Db.Companies.Single(c => c.Id == item.CompanyId).Name;
            else
                obj.CompanyName = string.Empty;

            return obj;
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