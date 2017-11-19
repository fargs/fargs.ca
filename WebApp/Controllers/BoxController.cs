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
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    [AuthorizeRole(Feature = Features.ServiceRequest_Box.ManageBoxFolder)]
    public class BoxController : Controller
    {
        private OrvosiDbContext context = new OrvosiDbContext();
        [ChildActionOnly]
        public PartialViewResult _BoxIntegration(BoxFolder folder) => PartialView(folder);

        public ViewResult Users()
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
                            Role = u.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
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

        public ViewResult BoxUsers()
        {
            var box = new BoxManager();
            var boxUsers = box.GetUsers();
            return View(boxUsers);
        }

        public ViewResult LogInAs(string id)
        {
            var box = new BoxManager();
            var user = box.UserClient(id).UsersManager.GetCurrentUserInformationAsync().Result;
            return View(user);
        }
    }
    public class BoxPerson : Orvosi.Shared.Model.Person
    {
        public BoxUser BoxUser { get; set; }

    }
}