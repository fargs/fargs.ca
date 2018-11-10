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
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class PhysicianFormModel
    {
        public PhysicianFormModel() { }
        public PhysicianFormModel(Guid physicianId, ImeHubDbContext db)
        {
            var physician = db.Physicians
                .Single(s => s.Id == physicianId);

            PhysicianId = physicianId;
            CompanyName = physician.CompanyName;
            Code = physician.Code;
            ColorCode = physician.ColorCode;
        }

        [Required]
        public Guid PhysicianId { get; set; }
        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public string Email { get; set; }
    }
}