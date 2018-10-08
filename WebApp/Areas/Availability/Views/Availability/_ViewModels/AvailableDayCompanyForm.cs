using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayCompanyForm : ViewModelBase
    {
        public AvailableDayCompanyForm()
        {
        }
        public AvailableDayCompanyForm(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);   
        }
        [Required]
        public short AvailableDayId { get; set; }
        public short? CompanyId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            private Guid physicianId;
            public ViewDataModel(OrvosiDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                Companies = GetPhysicianCompanySelectList();
            }
            public IEnumerable<SelectListItem> Companies { get; set; }

            private List<SelectListItem> GetPhysicianCompanySelectList()
            {
                var companies = db.PhysicianCompanies
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(c => c.Company)
                    .Select(LookupDto<short>.FromCompanyEntity.Expand())
                    .ToList();

                return companies
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
        }
    }
}