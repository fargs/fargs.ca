using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BillingEmail { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public static Expression<Func<CustomerDto, CustomerViewModel>> FromCustomerDto = i => new CustomerViewModel
        {
            Id = i.Id,
            Name = i.Name,
            BillingEmail = i.BillingEmail,
            City = i.City,
            Province = i.Province
        };
    }

}