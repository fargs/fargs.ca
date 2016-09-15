using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Data;
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
        private OrvosiDbContext context = new OrvosiDbContext();
        [ChildActionOnly]
        public ActionResult _BoxIntegration(BoxFolder folder) => PartialView(folder);

        public ActionResult Users()
        {
                var box = new BoxManager();
                var boxUsers = box.GetUsers();
                var users = context.AspNetUsers.Where(u => boxUsers.Entries.Select(bu => bu.Login).Contains(u.Email));
                foreach (var user in users)
                {
                    var boxUser = boxUsers.Entries.First(bu => bu.Login == user.Email);
                    user.BoxUserId = boxUser.Id;
                }
                context.SaveChanges();
                return View(users);
        }

        public ActionResult LogInAs(string id)
        {
            var box = new BoxManager();
            var user = box.Client(id).UsersManager.GetCurrentUserInformationAsync().Result;
            return View(user);
        }
    }
}