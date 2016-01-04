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
    public class ServiceCatalogueController : BaseController
    {
        OrvosiEntities db = new OrvosiEntities();
        
        // GET: Admin/ServiceCatalogue
        public ActionResult Index(string physicianId, short? companyId)
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

            vm.LocationAreas = db.LocationAreas.OrderBy(c => c.ItemText).ToList();

            if (companyId.HasValue)
            {
                vm.Company = db.Companies.Single(c => c.Id == companyId);
                vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(physicianId, companyId).OrderBy(c => c.LocationName).ToList();
            }
            else
            {
                vm.ServiceCatalogues = db.GetServiceCatalogue(physicianId).OrderBy(c => c.LocationName).ToList();
            }

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