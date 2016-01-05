using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Model;
using System.Linq;
using Model.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
            {
                var identity = (User.Identity as ClaimsIdentity);
                var identityId = identity.FindFirst(ClaimTypes.Sid).Value;
                var user = db.Users.Single(u => u.Id == identityId);

                if (user.RoleCategoryId == RoleCategory.Staff)
                {
                    return RedirectToAction("Index", "Home", new { area = "Staff", staffId = identityId, lookAhead = 7 });
                }
                else if (user.RoleCategoryId == RoleCategory.Company)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else if (user.RoleCategoryId == RoleCategory.Physician)
                {
                    return RedirectToAction("Index", "Home", new { area = "Physicians", physicianId = identityId, lookAhead = 7 });
                }
                else if (user.RoleCategoryId == RoleCategory.Admin)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Landing()
        {
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult Index2()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult ReleaseHistory()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult TestHelper()
        {
            return View();
        }
    }
}