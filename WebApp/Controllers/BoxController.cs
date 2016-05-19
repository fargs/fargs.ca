using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;

namespace WebApp.Controllers
{
    public class BoxController : Controller
    {
        [ChildActionOnly]
        public ActionResult _BoxIntegration(BoxFolder folder) => PartialView(folder);

        public ActionResult Users()
        {
            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                var box = new BoxManager();
                var users = box.GetUsers();
                foreach (var item in users.Entries)
                {
                    var user = db.Profiles.Single(u => u.Email == item.Login);
                    user.BoxUserId = item.Id;
                }
                db.SaveChanges();
                return View(users);
            }
        }

        public ActionResult LogInAs(string id)
        {
            var box = new BoxManager();
            var user = box.Client(id).UsersManager.GetCurrentUserInformationAsync().Result;
            return View(user);
        }
    }
}