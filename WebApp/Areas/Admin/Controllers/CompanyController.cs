using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.CompanyViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        OrvosiEntities db = new OrvosiEntities();
        // GET: Admin/Company
        public ActionResult Index(Nullable<byte> parentId)
        {
            var companies = db.Companies.Where(c => c.ParentId == parentId && c.Id != ParentCompanies.Examworks && c.Id != ParentCompanies.SCM).ToList(); // exclude examworks and scm
            var companyContacts = db.Users.Where(c => c.RoleId == Roles.Company).ToList();

            var vm = new IndexViewModel()
            {
                Companies = companies,
                CompanyContacts = companyContacts
            };

            return View(vm);
        }
    }
}