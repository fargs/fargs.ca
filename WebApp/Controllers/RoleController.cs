using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApp.Controllers
{
    public class RoleController : Controller
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
            var vm = new Models.Role.IndexViewModel()
            {
                Roles = GetList()
            };
            return View(vm);
        }

        public async Task<ActionResult> Remove(string id)
        {
            var obj = await this.RoleManager.FindByIdAsync(id);
            var result = await this.RoleManager.DeleteAsync(obj);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Update(string id, string name)
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
            var obj = new IdentityRole() { Name = name };
            var result = await this.RoleManager.CreateAsync(obj);
            var list = GetList();
            var vm = new
            {
                list = list,
                data = obj
            };
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        private List<IdentityRole> GetList()
        {
            return this.RoleManager.Roles.ToList();
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