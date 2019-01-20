using LinqKit;
using ImeHub.Data;
using ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceFormModel
    {
        public ServiceFormModel()
        {
        }
        public ServiceFormModel(ServiceModel service, ImeHubDbContext db)
        {
            SetProperties(service);
        }

        private void SetProperties(ServiceModel service)
        {
            Id = service.Id;
            CompanyId = service.CompanyId;
            Name = service.Name;
            Price = service.Price;
        }

        public ServiceFormModel(Guid companyId, ImeHubDbContext db)
        {
            CompanyId = companyId;
        }
        public ServiceFormModel(Guid companyId, Guid serviceId, ImeHubDbContext db)
        {
            var entity = db.Services.Single(cs => cs.Id == serviceId);
            var service = ServiceModel.FromServiceEntity.Invoke(entity);
            SetProperties(service);
        }
        public Guid? Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; private set; }
        public decimal Price { get; set; }
    }
}