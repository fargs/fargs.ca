using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp.Areas.Admin.ViewModels.Role;
using WebApp.Models;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Admin.Controllers
{
    [AuthorizeRole(Feature = Features.SecurityAdmin.RoleManagement)]
    public class RoleController : BaseController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Role
        public ActionResult Index()
        {
            var vm = new Areas.Admin.ViewModels.Role.IndexViewModel()
            {
                Roles = GetList()
            };
            return View(vm);
        }

        public async Task<ActionResult> Remove(Guid id)
        {
            var obj = await this.RoleManager.FindByIdAsync(id);
            var result = await this.RoleManager.DeleteAsync(obj);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Update(Guid id, string name)
        {
            var obj = await this.RoleManager.FindByIdAsync(id);
            obj.Name = name;
            var result = await this.RoleManager.UpdateAsync(obj);
            var list = GetList();
            var vm = new
            {
                list = list,
                data = obj
            };
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Insert(string name)
        {
            var obj = new ApplicationRole() { Name = name };
            var result = await this.RoleManager.CreateAsync(obj);
            var list = GetList();
            var vm = new
            {
                list = list,
                data = obj
            };
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> AssignUsers(Guid id)
        {
            var vm = await GetAssignUsersViewModel(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<PartialViewResult> AssignUser (Guid roleID, Guid userID, bool isAssigned)
        {
            var role = await this.RoleManager.FindByIdAsync(roleID);
            var userRole = role.Users.SingleOrDefault(c => c.RoleId == roleID && c.UserId == userID);

            if (isAssigned)
            {
                if (userRole != null)
                {
                    throw new Exception("User role assignment being added already exists.");
                }
                userRole = new ApplicationUserRole() { UserId = userID, RoleId = roleID };
                role.Users.Add(userRole);
            }
            else if (!isAssigned)
            {
                if (userRole == null)
                {
                    throw new Exception("User role assignment to remove does not exist.");
                }
                role.Users.Remove(userRole);
            }
            else
            {
                throw new Exception("isAssigned parameter was not set correctly.");
            }
            
            await this.RoleManager.UpdateAsync(role);

            var vm = await GetAssignUsersViewModel(roleID);

            return PartialView("~/Areas/Admin/Views/Role/_AssignUsersList.cshtml", vm);
        }

        private List<Role> GetList()
        {
            var list = this.RoleManager.Roles.Select(c => new Role() {
                Id = c.Id,
                Name = c.Name,
                UserCount = c.Users.Count
            }).ToList();
            return list;
        }

        private async Task<AssignUsersViewModel> GetAssignUsersViewModel(Guid roleID)
        {
            var vm = new AssignUsersViewModel();

            var role = await this.RoleManager.FindByIdAsync(roleID);
            vm.RoleId = role.Id;
            vm.RoleName = role.Name;
            vm.Users = this.UserManager.Users.Select(c => new User()
            {
                UserId = c.Id,
                UserName = c.UserName,
                DisplayName = "",
                Email = c.Email
            }).ToList();

            foreach (var ur in role.Users)
            {
                foreach (var user in vm.Users)
                {
                    if (ur.UserId == user.UserId)
                    {
                        user.IsAssigned = true;
                        continue;
                    }
                }
            }
            return vm;
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