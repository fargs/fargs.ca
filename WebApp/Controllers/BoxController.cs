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

        public ActionResult CreateBoxFolder(int ServiceRequestId)
        {
            var box = new BoxManager();
            using (var db = new OrvosiEntities(User.Identity.Name))
            {

                var request = db.ServiceRequests.Single(sr => sr.Id == ServiceRequestId);
                var physician = db.Physicians.Single(p => p.Id == request.PhysicianId);
                var province = db.Provinces.Single(p => p.Id == request.ProvinceId);
                var caseFolder = box.CreateCaseFolder("7027883033", province.ProvinceName, request.AppointmentDate.Value, request.Title);
                return PartialView(caseFolder);
            }
        }
    }
}