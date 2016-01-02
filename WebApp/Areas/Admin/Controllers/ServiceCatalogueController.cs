using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.ServiceCatalogueViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    public class ServiceCatalogueController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();
        
        // GET: Admin/ServiceCatalogue
        public ActionResult Index(string physicianId)
        {
            var vm = new IndexViewModel();

            vm.Physician = db.Physicians.Single(u => u.Id == physicianId);
            if (vm.Physician == null)
            {
                throw new Exception("Physician does not exist");
            }

            vm.Services = db.Services
                .Where(c => c.ServicePortfolioId == ServicePortfolios.Physician) // Canada
                .ToList();

            vm.LocationAreas = db.LocationAreas.ToList();

            vm.ServiceCatalogues = db.GetServiceCatalogue(physicianId).OrderBy(c => c.LocationName).ToList();

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}