using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Companies.Views.Company;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.Views.Shared;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Companies.Controllers
{
    public class CompanyController : BaseController
    {
        private OrvosiDbContext db;

        public CompanyController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public ViewResult Index(Guid? companyId)
        {
            var list = new ListViewModel(companyId, db, identity, now);

            ReadOnlyViewModel readOnly = null;
            if (companyId.HasValue)
            {
                readOnly = new ReadOnlyViewModel(companyId.Value, db, identity, now);
            }
            
            var viewModel = new IndexViewModel(list, readOnly, identity, now);

            return View(viewModel);
        }

        #region Views

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public PartialViewResult List(Guid? companyId)
        {
            var viewModel = new ListViewModel(companyId, db, identity, now);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public PartialViewResult ReadOnly(Guid companyId)
        {
            var readOnly = new ReadOnlyViewModel(companyId, db, identity, now);

            return PartialView(readOnly);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Search)]
        public PartialViewResult ReadOnlyMenu(Guid companyId)
        {
            var readOnly = new ReadOnlyViewModel(companyId, db, identity, now);

            return PartialView(readOnly);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowNewCompanyForm()
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new CompanyForm(physicianId.Value);

            return PartialView("CompanyForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowEditCompanyForm(Guid companyId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new CompanyForm(companyId, physicianId.Value, db);

            return PartialView("CompanyForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowDeleteCompanyConfirmation(Guid companyId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new CompanyForm(companyId, physicianId.Value, db);

            return PartialView("DeleteCompanyConfirmation", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowNewAddressForm(Guid companyId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new AddressFormModel(companyId, physicianId.Value, db);

            return PartialView("Address/AddressForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowAddServiceForm(Guid companyId, Guid? selectedServiceId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new AddServiceFormModel(companyId, selectedServiceId, db, physicianId.Value);

            return PartialView("Service/AddServiceForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowEditServiceForm(Guid companyId, Guid companyServiceId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new ServiceFormModel(companyId, companyServiceId, db);

            return PartialView("Service/ServiceForm", formModel);
        }

        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowNewTravelPriceForm(Guid companyId, Guid companyServiceId, short cityId)
        {
            var formModel = new TravelPriceFormModel(companyId, companyServiceId, cityId);

            return PartialView("Pricing/NewTravelPriceForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowEditTravelPriceForm(Guid companyId, Guid travelPriceId)
        {
            var formModel = new EditTravelPriceFormModel(companyId, travelPriceId, db);

            return PartialView("Pricing/EditTravelPriceForm", formModel);
        }
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public PartialViewResult ShowEditCancellationPolicyForm(Guid companyId)
        {
            var formModel = new EditCancellationPolicyFormModel(companyId, db);

            return PartialView("Pricing/EditCancellationPolicyForm", formModel);
        }
        #endregion

        #region API

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveNewCompanyForm(CompanyForm form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("CompanyForm", form);
            }

            var company = new CompanyV2
            {
                Id = Guid.NewGuid(),
                PhysicianId = form.PhysicianId,
                Name = form.Name,
                Description = form.Description,
                Code = form.Code,
                ColorCode = form.ColorCode,
                BillingEmail = form.BillingEmail,
                ReportsEmail = form.ReportsEmail,
                PhoneNumber = form.PhoneNumber
            };
            db.CompanyV2.Add(company);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = company.Id
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveEditCompanyForm(CompanyForm form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("CompanyForm", form);
            }

            var company = db.CompanyV2.Single(s => s.Id == form.CompanyId);
            company.Name = form.Name;
            company.Description = form.Description;
            company.Code = form.Code;
            company.ColorCode = form.ColorCode;
            company.BillingEmail = form.BillingEmail;
            company.ReportsEmail = form.ReportsEmail;
            company.PhoneNumber = form.PhoneNumber;

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public ActionResult Remove(Guid companyId)
        {
            var entity = db.CompanyV2.Single(c => c.Id == companyId);
            db.CompanyV2.Remove(entity);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveNewAddressForm(AddressFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("AddressForm", form);
            }

            var address = new AddressV2
            {
                Id = Guid.NewGuid(),
                CompanyId = form.CompanyId,
                Name = form.Name,
                ProvinceId = form.ProvinceId,
                CityId = form.CityId,
                PostalCode = form.PostalCode,
                TimeZoneId = form.TimeZoneId,
                Address1 = form.Address1,
                Address2 = form.Address2,
                AddressTypeId = form.AddressTypeId
            };
            db.AddressV2.Add(address);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = address.Id
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveAddServiceForm(AddServiceFormModel form)
        {
            if (!ModelState.IsValid)
            {
                form.LoadViewData(db, physicianId.Value);
                return PartialView("Service/AddServiceForm", form);
            }

            var service = new CompanyService
            {
                Id = Guid.NewGuid(),
                CompanyId = form.CompanyId,
                ServiceId = form.ServiceId,
                Name = form.Name,
                Price = form.Price,
                IsTravelRequired = form.IsTravelRequired.GetValueOrDefault(false)
            };
            db.CompanyServices.Add(service);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = service.Id
            });
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveNewTravelPriceForm(TravelPriceFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("NewTravelPriceForm", form);
            }

            var travelPrice = new TravelPrice
            {
                Id = Guid.NewGuid(),
                CompanyServiceId = form.CompanyServiceId,
                CityId = form.CityId,
                Price = form.Price
            };
            db.TravelPrices.Add(travelPrice);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = travelPrice.Id
            });
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveEditTravelPriceForm(EditTravelPriceFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditTravelPriceForm", form);
            }

            var travelPrice = db.TravelPrices.Single(tp => tp.Id == form.Id);
            travelPrice.Price = form.Price;
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.PhysicianCompany.Create)]
        public async Task<ActionResult> SaveEditCancellationPolicyForm(EditCancellationPolicyFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditCancellationPolicyForm", form);
            }

            var company = db.CompanyV2.Single(tp => tp.Id == form.CompanyId);
            company.NoShowRate = form.NoShowRate;
            company.NoShowRateFormat = (byte)form.NoShowRateFormat;
            company.LateCancellationRate = form.LateCancellationRate;
            company.LateCancellationRateFormat = (byte)form.LateCancellationRateFormat;
            company.LateCancellationPolicy = form.LateCancellationPolicy;
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        #endregion
    }
}