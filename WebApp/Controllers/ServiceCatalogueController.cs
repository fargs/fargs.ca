using Model;
using Model.Enums;
using WebApp.Library.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.ServiceCatalogueViewModels;

namespace WebApp.Controllers
{
    public class ServiceCatalogueController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Admin/ServiceCatalogue
        public async System.Threading.Tasks.Task<ActionResult> Index(FilterArgs args, byte mode = FormModes.ReadOnly)
        {
            ViewBag.FormMode = mode;

            var vm = new IndexViewModel();

            vm.FilterArgs = args;

            vm.CurrentUser = db.Users.Single(u => u.UserName == User.Identity.Name);

            vm.SelectedUser = await db.Users.SingleOrDefaultAsync(u => u.Id == args.UserId);
            if (vm.SelectedUser == null)
            {
                vm.SelectedUser = await db.Users.FirstAsync(c => c.RoleId == Roles.Physician);
            }

            vm.SelectedCompany = await db.Companies.SingleOrDefaultAsync(c => c.Id == args.CompanyId);
            if (vm.SelectedCompany == null)
            {
                vm.ServiceCatalogues = db.GetServiceCatalogueForCompany(args.UserId, args.CompanyId).OrderBy(c => c.LocationName).ToList();
            }
            else
            {
                vm.ServiceCatalogues = db.GetServiceCatalogue(args.UserId).OrderBy(c => c.LocationName).ToList();
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