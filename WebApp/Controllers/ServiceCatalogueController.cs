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
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;
using WebApp.Library.Extensions;
using WebApp.FormModels;
using System.Security.Principal;

namespace WebApp.Controllers
{
    [AuthorizeRole(Feature = Features.Services.Manage)]
    public class ServiceCatalogueController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;

        public ServiceCatalogueController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
        }
        public async Task<ActionResult> Index(FilterArgs args)
        {
            var vm = new IndexViewModel();
            vm.FilterArgs = args;
            
            vm.SelectedCompany = await db.Companies.SingleOrDefaultAsync(c => c.Id == args.CompanyId);
            if (vm.SelectedCompany != null)
            {
                vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(physicianOrLoggedInUserId, args.CompanyId).OrderBy(c => c.LocationName).ToList();
            }
            else
            {
                vm.ServiceCatalogues = db.GetServiceCatalogue(physicianOrLoggedInUserId).OrderBy(c => c.LocationName).ToList();
            }
            var inheritedValues = db.GetServiceCatalogueRate(physicianOrLoggedInUserId, vm.SelectedCompany != null ? vm.SelectedCompany.ObjectGuid : Guid.Empty).First();
            vm.ServiceCatalogueRate.NoShowRate = inheritedValues.NoShowRate;
            vm.ServiceCatalogueRate.LateCancellationRate = inheritedValues.LateCancellationRate;
            return View(vm);
        }

        public async Task<ActionResult> _Details(FilterArgs args)
        {
            var vm = new IndexViewModel();
            vm.FilterArgs = args;

            vm.SelectedCompany = await db.Companies.SingleOrDefaultAsync(c => c.Id == args.CompanyId);
            if (vm.SelectedCompany != null)
            {
                vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(physicianOrLoggedInUserId, args.CompanyId).OrderBy(c => c.LocationName).ToList();
            }
            else
            {
                vm.ServiceCatalogues = db.GetServiceCatalogue(physicianOrLoggedInUserId).OrderBy(c => c.LocationName).ToList();
            }

            var service = db.Services.Single(s => s.Id == args.ServiceId);
            var view = "_Exams";
            if (!service.IsLocationRequired)
            {
                view = "_AddOns";
            }
            return PartialView(view, vm);
        }

        public async Task<ActionResult> Edit(FilterArgs args)
        {
            var entity = await db.ServiceCatalogues
                .SingleOrDefaultAsync(c => c.PhysicianId == physicianOrLoggedInUserId && c.CompanyId == args.CompanyId && c.LocationId == args.LocationId && c.ServiceId == args.ServiceId);

            var sc = new ServiceCatalogueForm()
            {
                CompanyId = args.CompanyId,
                LocationId = args.LocationId,
                ServiceId = args.ServiceId
            };

            if (entity == null)
            {
                return PartialView(sc);
            }

            sc.CompanyId = entity.CompanyId;
            sc.ServiceId = entity.ServiceId;
            sc.LocationId = entity.LocationId;
            sc.Price = entity.Price;

            return PartialView(sc);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ServiceCatalogueForm form)
        {
            var exists = await db.ServiceCatalogues.AnyAsync(c => c.CompanyId == form.CompanyId && c.PhysicianId == physicianOrLoggedInUserId && c.LocationId == form.LocationId && c.ServiceId == form.ServiceId);
            if (!exists)
            {
                var sc = new ServiceCatalogue()
                {
                    CompanyId = form.CompanyId,
                    PhysicianId = physicianOrLoggedInUserId,
                    LocationId = form.LocationId,
                    ServiceId = form.ServiceId,
                    Price = form.Price,
                    ModifiedUser = User.Identity.Name
                };
                db.ServiceCatalogues.Add(sc);
            }
            else
            {
                var sc = await db.ServiceCatalogues.SingleAsync(c => c.CompanyId == form.CompanyId && c.PhysicianId == physicianOrLoggedInUserId && c.LocationId == form.LocationId && c.ServiceId == form.ServiceId);
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

            return Json(form);
        }

        public async Task<ActionResult> EditRates(Guid? CustomerGuid)
        {
            var serviceCatalogueRate = new ServiceCatalogueRate();

            var serviceProvider = await db.AspNetUsers.SingleOrDefaultAsync(u => u.Id == physicianOrLoggedInUserId);
            if (serviceProvider != null)
            {
                var customer = await db.Companies.SingleOrDefaultAsync(c => c.ObjectGuid == CustomerGuid);
                if (customer != null)
                {
                    serviceCatalogueRate = await db.ServiceCatalogueRates.SingleOrDefaultAsync(c => c.ServiceProviderGuid == physicianOrLoggedInUserId && c.CustomerGuid == customer.ObjectGuid);
                    if (serviceCatalogueRate == null)
                    {
                        serviceCatalogueRate = new ServiceCatalogueRate()
                        {
                            ServiceProviderGuid = physicianOrLoggedInUserId,
                            CustomerGuid = CustomerGuid
                        };
                    }
                }
                else
                {
                    serviceCatalogueRate = await db.ServiceCatalogueRates.SingleOrDefaultAsync(c => c.ServiceProviderGuid == physicianOrLoggedInUserId && !c.CustomerGuid.HasValue);
                    if (serviceCatalogueRate == null)
                    {
                        serviceCatalogueRate = new ServiceCatalogueRate()
                        {
                            ServiceProviderGuid = physicianOrLoggedInUserId,
                            CustomerGuid = CustomerGuid
                        };
                    }
                }
                var inheritedValues = db.GetServiceCatalogueRate(physicianOrLoggedInUserId, CustomerGuid).First();
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
            var exists = await db.ServiceCatalogueRates.AnyAsync(c => c.ServiceProviderGuid == physicianOrLoggedInUserId && c.CustomerGuid == form.CustomerGuid);
            if (!exists)
            {
                var rate = new ServiceCatalogueRate()
                {
                    ServiceProviderGuid = physicianOrLoggedInUserId,
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
                var rate = await db.ServiceCatalogueRates.SingleAsync(c => c.ServiceProviderGuid == physicianOrLoggedInUserId && c.CustomerGuid == form.CustomerGuid);
                rate.NoShowRate = form.NoShowRate.GetValueOrDefault(0);
                rate.LateCancellationRate = form.LateCancellationRate.GetValueOrDefault(0);
                rate.ModifiedDate = SystemTime.Now();
                rate.ModifiedUser = User.Identity.Name;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ServiceProviderGuid = physicianOrLoggedInUserId, CustomerGuid = form.CustomerGuid });
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