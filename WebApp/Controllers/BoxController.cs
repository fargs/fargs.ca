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

        [HttpGet]
        public ActionResult GetBoxCaseFolder(int ServiceRequestId)
        {
            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                var request = db.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);
                if (!string.IsNullOrEmpty(request.BoxCaseFolderId))
                    return PartialView("CreateCaseFolder");

                var box = new BoxManager();
                var caseFolder = box.GetFolder(request.BoxCaseFolderId);
                return PartialView("_BoxFolder", caseFolder);
            };
        }

        [HttpPost]
        public ActionResult CreateBoxCaseFolder(int ServiceRequestId)
        {
            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                // Get the request
                var request = db.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);

                // Get the request and assert they have a Box Folder Id
                var physician = db.Physicians.Single(p => p.Id == request.PhysicianId);
                if (string.IsNullOrEmpty(physician.BoxFolderId))
                    return PartialView("Error", "Physician does not have a cases folder setup in Box.");

                // Get the province which is used in the case folder path
                var province = db.Provinces.Single(p => p.Id == request.ProvinceId);

                // Create the case folder
                var box = new BoxManager();
                var caseFolder = box.CreateCaseFolder(physician.BoxFolderId, province.ProvinceName, request.AppointmentDate.Value, request.Title);

                // Persist the new case folder Id to the database.
                request.BoxCaseFolderId = caseFolder.Id;
                db.SaveChanges();

                // Redirect to display the Box Folder
                return RedirectToAction("GetBoxFolder", new { FolderId = caseFolder.Id });
            }
        }

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