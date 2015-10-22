using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public async Task<ActionResult> Index()
        {
            _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new DashboardViewModel();
            model.UserDisplayName = user.DisplayName;
            if (user.Roles.Count > 0)
            {
                var roleId = user.Roles.First().RoleId;
                model.UserRoleName = _roleManager.Roles.SingleOrDefault(c => c.Id == roleId).Name;
            }
            return View(model);
        }
    }
}