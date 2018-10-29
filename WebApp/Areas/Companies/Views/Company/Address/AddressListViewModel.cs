using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class AddressListViewModel
    {
        public AddressListViewModel(CompanyV2Dto company)
        {
            CompanyId = company.Id;
            Addresses = company.Addresses.Select(a => new AddressViewModel(a));
        }
        public Guid? CompanyId { get; set; }
        
        public IEnumerable<AddressViewModel> Addresses { get; set; }
       
    }
}