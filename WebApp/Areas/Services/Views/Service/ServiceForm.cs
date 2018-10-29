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

namespace WebApp.Areas.Services.Views.Service
{
    public class ServiceForm : ViewModelBase
    {
        public ServiceForm() { }
        public ServiceForm(IIdentity identity, DateTime now) : base(identity, now)
        {
            PhysicianId = PhysicianId;
        }
        public ServiceForm(Guid serviceId, OrvosiDbContext db, IIdentity identity, DateTime now) : this(identity, now)
        {
            var service = db.ServiceV2.
                Single(s => s.Id == serviceId);

            ServiceId = serviceId;
            Name = service.Name;
            Description = service.Description;
            Code = service.Code;
            ColorCode = service.ColorCode;
            Price = service.Price;
        }

        public Guid? ServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(2)]
        public string Code { get; set; }
        [Required]
        public string ColorCode { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}