using Model;
using Model.Enums;
using WebApp.Library.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApp.ViewModels.ServiceCatalogueViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class ServiceCatalogueController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/ServiceCatalogue
        public async Task<ActionResult> Index(FilterArgs args)
        {
            var vm = new IndexViewModel();

            vm.FilterArgs = args;

            vm.CurrentUser = db.Users.Single(u => u.UserName == User.Identity.Name);

            vm.SelectedUser = await db.Users.SingleOrDefaultAsync(u => u.Id == args.UserId);
            if (vm.SelectedUser != null)
            {
                vm.SelectedCompany = await db.Companies.SingleOrDefaultAsync(c => c.Id == args.CompanyId);
                if (vm.SelectedCompany != null)
                {
                    vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(args.UserId, args.CompanyId).OrderBy(c => c.LocationName).ToList();
                }
                else
                {
                    vm.ServiceCatalogues = db.GetServiceCatalogue(args.UserId).OrderBy(c => c.LocationName).ToList();
                }
            }
            return View(vm);
        }

        public async Task<ActionResult> Edit(FilterArgs args)
        {
            ServiceCatalogue sc = null;
            sc = await db.ServiceCatalogues
                .SingleOrDefaultAsync(c => c.PhysicianId == args.UserId && c.CompanyId == args.CompanyId && c.LocationId == args.LocationId && c.ServiceId == args.ServiceId);
            if (sc == null)
            {
                sc = new ServiceCatalogue()
                {
                    CompanyId = args.CompanyId,
                    PhysicianId = args.UserId,
                    LocationId = args.LocationId,
                    ServiceId = args.ServiceId
                };
            }
            return View(sc);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ServiceCatalogue form)
        {
            var exists = await db.ServiceCatalogues.AnyAsync(c => c.CompanyId == form.CompanyId && c.PhysicianId == form.PhysicianId && c.LocationId == form.LocationId && c.ServiceId == form.ServiceId);
            if (!exists)
            {
                var sc = new ServiceCatalogue()
                    {
                        CompanyId = form.CompanyId,
                        PhysicianId = form.PhysicianId,
                        LocationId = form.LocationId,
                        ServiceId = form.ServiceId,
                        ServiceCataloguePriceOverride = form.ServiceCataloguePriceOverride,
                        ModifiedUser = User.Identity.Name,
                        ServiceName = string.Empty // this field is getting marked as required, it does not get persisted
                    };
                    db.ServiceCatalogues.Add(sc);
            }
            else
            {
                var sc = await db.ServiceCatalogues.SingleAsync(c => c.CompanyId == form.CompanyId && c.PhysicianId == form.PhysicianId && c.LocationId == form.LocationId && c.ServiceId == form.ServiceId);
                if (form.ServiceCataloguePriceOverride.HasValue)
                {
                    sc.ServiceCataloguePriceOverride = form.ServiceCataloguePriceOverride;
                }
                else
                {
                    db.ServiceCatalogues.Remove(sc);
                }
            }
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index", new FilterArgs() { CompanyId = form.CompanyId, UserId = form.PhysicianId });
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