using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class DiagnosticsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Diagnostics
        public async Task<ActionResult> SendEmail(string to)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            await userManager.SendEmailAsync(user.Id, "Test Email Subject", "Test Email Body");
            return PartialView();
        }
    }
}