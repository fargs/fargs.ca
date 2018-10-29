using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Services.Views.Service;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.Views.Shared;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Services.Controllers
{
    public class ServiceController : BaseController
    {
        private OrvosiDbContext db;

        public ServiceController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.Services.Search)]
        public ViewResult Index()
        {
            var viewModel = new ListViewModel(db, identity, now);
            return View(viewModel);
        }


        [AuthorizeRole(Feature = Features.Services.Search)]
        public ViewResult Details()
        {
            return View();
        }
        #region Views

        [AuthorizeRole(Feature = Features.Services.Search)]
        public PartialViewResult List()
        {
            var viewModel = new ListViewModel(db, identity, now);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.Services.Manage)]
        public PartialViewResult ShowNewServiceForm()
        {
            var formModel = new ServiceForm(identity, now);

            return PartialView("ServiceForm", formModel);
        }
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public PartialViewResult ShowEditServiceForm(Guid serviceId)
        {
            var formModel = new ServiceForm(serviceId, db, identity, now);

            return PartialView("ServiceForm", formModel);
        }
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public PartialViewResult ShowDeleteServiceConfirmation(Guid serviceId)
        {
            var formModel = new ServiceForm(serviceId, db, identity, now);

            return PartialView("DeleteServiceConfirmation", formModel);
        }

        #endregion

        #region API

        [HttpPost]
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public async Task<ActionResult> SaveNewServiceForm(ServiceForm form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ServiceForm", form);
            }

            var service = new ServiceV2
            {
                Id = Guid.NewGuid(),
                PhysicianId = form.PhysicianId,
                Name = form.Name,
                Description = form.Description,
                Code = form.Code,
                ColorCode = form.ColorCode,
                Price = form.Price
            };
            db.ServiceV2.Add(service);
            await db.SaveChangesAsync();

            return Json(new
            {
                service.Id
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public async Task<ActionResult> SaveEditServiceForm(ServiceForm form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ServiceForm", form);
            }

            var service = db.ServiceV2.Single(s => s.Id == form.ServiceId);
            service.Name = form.Name;
            service.Description = form.Description;
            service.Code = form.Code;
            service.ColorCode = form.ColorCode;
            service.Price = form.Price;

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Services.Manage)]
        public ActionResult Remove(Guid serviceId)
        {
            var entity = db.ServiceV2.Single(c => c.Id == serviceId);
            db.ServiceV2.Remove(entity);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion
    }
}