using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Physicians.Views.Physician.Address
{
    public class AddressListViewModel
    {
        public AddressListViewModel(PhysicianModel physician)
        {
            PhysicianId = physician.Id;
            Addresses = physician.Addresses.Select(a => new AddressViewModel(a));
        }
        public Guid PhysicianId { get; set; }
        
        public IEnumerable<AddressViewModel> Addresses { get; set; }
       
    }
}