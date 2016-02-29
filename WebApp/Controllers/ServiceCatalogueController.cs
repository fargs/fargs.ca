﻿using Model;
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
using WebApp.Library;

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
                var userGuid = new Guid(args.UserId);
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

        public async Task<ActionResult> EditRates(string ServiceProviderGuid, string CustomerGuid)
        {
            var serviceCatalogueRate = new ServiceCatalogueRate();

            var serviceProvider = await db.Users.SingleOrDefaultAsync(u => u.Id == ServiceProviderGuid);
            if (serviceProvider != null)
            {
                var serviceProviderGuid = new Guid(serviceProvider.Id);
                Guid? customerGuid = string.IsNullOrEmpty(CustomerGuid) ? Guid.Empty : new Guid(CustomerGuid);

                var customer = await db.Companies.SingleOrDefaultAsync(c => c.ObjectGuid == customerGuid);
                if (customer != null)
                {
                    serviceCatalogueRate = await db.ServiceCatalogueRates.SingleOrDefaultAsync(c => c.ServiceProviderGuid == serviceProviderGuid && c.CustomerGuid == customer.ObjectGuid);
                    if (serviceCatalogueRate == null)
                    {
                        serviceCatalogueRate = new ServiceCatalogueRate()
                        {
                            ServiceProviderGuid = serviceProviderGuid,
                            CustomerGuid = customerGuid
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
                            CustomerGuid = customerGuid
                        };
                    }
                }
                var inheritedValues = db.GetServiceCatalogueRate(serviceProviderGuid, customerGuid).First();
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