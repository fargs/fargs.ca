using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceFormModel
    {
        public ServiceFormModel()
        {
        }
        public ServiceFormModel(CompanyServiceDto service, OrvosiDbContext db)
        {
            SetProperties(service);
        }

        private void SetProperties(CompanyServiceDto service)
        {
            Id = service.Id;
            CompanyId = service.CompanyId;
            ServiceId = service.ServiceId;
            Service = new ServiceV2ViewModel(service.Service);
            Name = service.Name;
            Price = service.Price;
        }

        public ServiceFormModel(Guid companyId, OrvosiDbContext db)
        {
            CompanyId = companyId;
        }
        public ServiceFormModel(Guid companyId, Guid companyServiceId, OrvosiDbContext db)
        {
            var entity = db.CompanyServices.Single(cs => cs.Id == companyServiceId);
            var service = CompanyServiceDto.FromCompanyServiceEntity.Invoke(entity);
            SetProperties(service);
        }
        public Guid? Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public ServiceV2ViewModel Service { get; set; }
        public string Name { get; private set; }
        public decimal Price { get; set; }
    }
}