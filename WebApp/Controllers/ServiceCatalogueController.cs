using Orvosi.Data;
using Orvosi.Shared.Enums;
using WebApp.Library.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApp.ViewModels.ServiceCatalogueViewModels;
using WebApp.Library;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Super Admin, Case Coordinator")]
    public class ServiceCatalogueController : Controller
    {
        OrvosiDbContext db = new OrvosiDbContext();

        // GET: Admin/ServiceCatalogue
        public async Task<ActionResult> Index(FilterArgs args)
        {
            var vm = new IndexViewModel();

            vm.FilterArgs = args;

            vm.SelectedUser = await db.AspNetUsers.SingleOrDefaultAsync(u => u.Id == args.UserId);
            if (vm.SelectedUser != null)
            {
                vm.SelectedCompany = await db.Companies.SingleOrDefaultAsync(c => c.Id == args.CompanyId);
                var userGuid = args.UserId;
                if (vm.SelectedCompany != null)
                {
                    vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(args.UserId, args.CompanyId).OrderBy(c => c.LocationName).ToList();
                }
                else
                {
                    vm.ServiceCatalogues = db.GetServiceCatalogue(args.UserId).OrderBy(c => c.LocationName).ToList();
                }
                var inheritedValues = db.GetServiceCatalogueRate(userGuid, vm.SelectedCompany != null ? vm.SelectedCompany.ObjectGuid : Guid.Empty).First();
                vm.ServiceCatalogueRate.NoShowRate = inheritedValues.NoShowRate;
                vm.ServiceCatalogueRate.LateCancellationRate = inheritedValues.LateCancellationRate;
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
                        Price = form.Price,
                        ModifiedUser = User.Identity.Name
                    };
                    db.ServiceCatalogues.Add(sc);
            }
            else
            {
                var sc = await db.ServiceCatalogues.SingleAsync(c => c.CompanyId == form.CompanyId && c.PhysicianId == form.PhysicianId && c.LocationId == form.LocationId && c.ServiceId == form.ServiceId);
                if (form.Price.HasValue)
                {
                    sc.Price = form.Price;
                }
                else
                {
                    db.ServiceCatalogues.Remove(sc);
                }
            }
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index", new FilterArgs() { CompanyId = form.CompanyId, UserId = form.PhysicianId });
        }

        public async Task<ActionResult> EditRates(Guid ServiceProviderGuid, Guid? CustomerGuid)
        {
            var serviceCatalogueRate = new ServiceCatalogueRate();

            var serviceProvider = await db.AspNetUsers.SingleOrDefaultAsync(u => u.Id == ServiceProviderGuid);
            if (serviceProvider != null)
            {
                var serviceProviderGuid = serviceProvider.Id;

                var customer = await db.Companies.SingleOrDefaultAsync(c => c.ObjectGuid == CustomerGuid);
                if (customer != null)
                {
                    serviceCatalogueRate = await db.ServiceCatalogueRates.SingleOrDefaultAsync(c => c.ServiceProviderGuid == serviceProviderGuid && c.CustomerGuid == customer.ObjectGuid);
                    if (serviceCatalogueRate == null)
                    {
                        serviceCatalogueRate = new ServiceCatalogueRate()
                        {
                            ServiceProviderGuid = serviceProviderGuid,
                            CustomerGuid = CustomerGuid
                        };
                    }
                }
                else
                {
                    serviceCatalogueRate = await db.ServiceCatalogueRates.SingleOrDefaultAsync(c => c.ServiceProviderGuid == serviceProviderGuid && !c.CustomerGuid.HasValue);
                    if (serviceCatalogueRate == null)
                    {
                        serviceCatalogueRate = new ServiceCatalogueRate()
                        {
                            ServiceProviderGuid = serviceProviderGuid,
                            CustomerGuid = CustomerGuid
                        };
                    }
                }
                var inheritedValues = db.GetServiceCatalogueRate(serviceProviderGuid, CustomerGuid).First();
                serviceCatalogueRate.NoShowRate = inheritedValues.NoShowRate;
                serviceCatalogueRate.LateCancellationRate = inheritedValues.LateCancellationRate;
            }
            else
            {
                throw new Exception("Incorrect parameters were passed.");
            }
            return View(serviceCatalogueRate);
        }

        [HttpPost]
        public async Task<ActionResult> EditRates(ServiceCatalogueRate form)
        {
            var exists = await db.ServiceCatalogueRates.AnyAsync(c => c.ServiceProviderGuid == form.ServiceProviderGuid && c.CustomerGuid == form.CustomerGuid);
            if (!exists)
            {
                var rate = new ServiceCatalogueRate()
                {
                    ServiceProviderGuid = form.ServiceProviderGuid,
                    CustomerGuid = form.CustomerGuid,
                    NoShowRate = form.NoShowRate,
                    LateCancellationRate = form.LateCancellationRate,
                    ModifiedDate = SystemTime.Now(),
                    ModifiedUser = User.Identity.Name
                };
                db.ServiceCatalogueRates.Add(rate);
            }
            else
            {
                var rate = await db.ServiceCatalogueRates.SingleAsync(c => c.ServiceProviderGuid == form.ServiceProviderGuid && c.CustomerGuid == form.CustomerGuid);
                rate.NoShowRate = form.NoShowRate.GetValueOrDefault(0);
                rate.LateCancellationRate = form.LateCancellationRate.GetValueOrDefault(0);
                rate.ModifiedDate = SystemTime.Now();
                rate.ModifiedUser = User.Identity.Name;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ServiceProviderGuid = form.ServiceProviderGuid, CustomerGuid = form.CustomerGuid });
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