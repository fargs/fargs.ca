using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayCompanyForm : ViewModelBase
    {
        public AvailableDayCompanyForm()
        {
        }
        public AvailableDayCompanyForm(ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);   
        }
        [Required]
        public Guid AvailableDayId { get; set; }
        public Guid? CompanyId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;
            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                Companies = GetPhysicianCompanySelectList();
            }
            public IEnumerable<SelectListItem> Companies { get; set; }

            private List<SelectListItem> GetPhysicianCompanySelectList()
            {
                var companies = db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(CompanyModel.FromCompany)
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