using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using m = WebApp.Areas.Admin.Models.User;

namespace WebApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UserController()
        {

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

        // GET: User
        public ActionResult Index()
        {
            var vm = GetIndexViewModel();
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
            var vm = GetIndexViewModel();
            return PartialView("~/Areas/Admin/Views/User/_UserTableRows.cshtml", vm);
        }

        public async Task<ActionResult> Update(string id, DateTime lockoutEndDateUtc)
        {
            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User to be deleted does not exist");
            }
            user.LockoutEndDateUtc = lockoutEndDateUtc;
            await this.UserManager.UpdateAsync(user);
            var vm = GetIndexViewModel();
            return PartialView("~/Areas/Admin/Views/User/_UserTableRows.cshtml", vm);
        }

        private m.IndexViewModel GetIndexViewModel()
        {
            var vm = new m.IndexViewModel()
            {
                Users = GetList()
            };
            return vm;
        }

        private List<m.User> GetList()
        {
            var result = new List<m.User>();
            foreach (var item in this.UserManager.Users.ToList())
            {
                string roleId = string.Empty;
                if (item.Roles.Count > 0)
                    roleId = item.Roles.First().RoleId;
                else
                    roleId = string.Empty;

                var obj = new m.User();
                obj.Id = item.Id;
                obj.DisplayName = item.DisplayName;
                obj.UserName = item.UserName;
                obj.Email = item.Email;
                obj.LockoutEnabled = item.LockoutEnabled;
                obj.LockoutEndDateUtc = item.LockoutEndDateUtc;
                obj.AccessFailedCount = item.AccessFailedCount;
                if (item.Roles.Count > 0)
                    obj.RoleName = this.RoleManager.Roles.SingleOrDefault(c => c.Id == roleId).Name;
                else
                    obj.RoleName = string.Empty;

                result.Add(obj);

            }
            return result;
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
            }

            base.Dispose(disposing);
        }

    }
}