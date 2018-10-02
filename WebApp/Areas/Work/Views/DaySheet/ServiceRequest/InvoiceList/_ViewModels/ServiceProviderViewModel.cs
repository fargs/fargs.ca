using System;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class ServiceProviderViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public static Expression<Func<ServiceProviderDto, ServiceProviderViewModel>> FromServiceProviderDto = i => new ServiceProviderViewModel
        {
            Id = i.Id,
            Name = i.Name,
            Email = i.Email,
            Province = i.Province
        };
    }
}