using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Areas.Pricing.Views.Home;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;
using System.Threading.Tasks;
using System.Net;

namespace WebApp.Areas.Pricing.Controllers
{
    public class HomeController : BaseController
    {
        private OrvosiDbContext db;

        public HomeController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        public ViewResult Index()
        {
            var serviceCatalogue = new ServiceCatalogueViewModel(db, identity, now);
            var viewModel = new IndexViewModel(serviceCatalogue);
            return View(viewModel);
        }
        public PartialViewResult ServiceCatalogue()
        {
            var serviceCatalogue = new ServiceCatalogueViewModel(db, identity, now);
            return PartialView(serviceCatalogue);
        }
        public PartialViewResult ShowServiceCatalogueForm()
        {
            var formModel = new ServiceCatalogueForm(db, identity, now);
            return PartialView("ServiceCatalogueForm", formModel);
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceCatalogue.Manage)]
        public async Task<ActionResult> SaveServiceCatalogueForm(ServiceCatalogueForm form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return PartialView("ServiceCatalogueForm", form);
            }

            var sc = new Orvosi.Data.ServiceCatalogue()
            {
                PhysicianId = physicianId,
                LocationId = form.CityId,
                ServiceId = form.ServiceId,
                CompanyId = form.CompanyId,
                Price = form.Price
            };
            db.ServiceCatalogues.Add(sc);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = sc.Id
            });
        }
    }
}