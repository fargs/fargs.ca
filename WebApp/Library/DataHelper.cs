using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Library
{
    public class DataHelper
    {
        public IEnumerable<AddressViewModel> LoadAddressesWithOwner(IOrvosiDbContext context)
        {
            var query1 = context.AspNetUsers
                .Where(a => a.AspNetUserRoles.FirstOrDefault().RoleId == AspNetRoles.Physician)
                .Select(a => new Orvosi.Shared.Model.Person { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName, Title = a.Title })
                .ToList()
                .Select(a => new OwnerViewModel { Id = a.Id, Name = a.DisplayName });

            // retrieve all the companies from the database
            var query2 = context.Companies
                .Select(c => new OwnerViewModel { Id = c.ObjectGuid, Name = c.Name }).ToList();

            // concat them into 1 list
            var entities = query1.Concat(query2);

            // join with addresses to get the owners
            return from a in context.Addresses.ToList()
                   join e in entities on a.OwnerGuid equals e.Id into ao
                   select new AddressViewModel
                   {
                       Address = a,
                       Owner = ao.FirstOrDefault() == null ? string.Empty : ao.FirstOrDefault().Name
                   };
        }
    }
}