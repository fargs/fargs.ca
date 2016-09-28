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
    [Authorize(Roles = "Super Admin")]
    public class BoxController : Controller
    {
        private OrvosiDbContext context = new OrvosiDbContext();
        [ChildActionOnly]
        public ActionResult _BoxIntegration(BoxFolder folder) => PartialView(folder);

        public ActionResult Users()
        {
            var box = new BoxManager();
            var boxUsers = box.GetUsers();
            var users = (from u in context.AspNetUsers
                        select new BoxPerson
                        {
                            Id = u.Id,
                            Title = u.Title,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            ColorCode = u.ColorCode,
                            BoxUserId = u.BoxUserId,
                            Email = u.Email,
                            Role = u.AspNetUserRoles.Select(r => new Models.ServiceRequestModels2.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        })
                        .AsEnumerable();

            users = from u in users
                         select new BoxPerson
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Title = u.Title,
                             BoxUser = boxUsers.Entries.FirstOrDefault(bu => bu.Id == u.BoxUserId)
                         };

            return View(users);
        }

        public ActionResult LogInAs(string id)
        {
            var box = new BoxManager();
            var user = box.Client(id).UsersManager.GetCurrentUserInformationAsync().Result;
            return View(user);
        }
    }
    public class BoxPerson : Models.ServiceRequestModels2.Person
    {
        public BoxUser BoxUser { get; set; }

    }
}