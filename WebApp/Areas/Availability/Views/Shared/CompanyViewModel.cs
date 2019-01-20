using ImeHub.Models;
using System;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Shared
{
    public class CompanyViewModel : LookupViewModel<Guid>
    {
        public CompanyViewModel(CompanyModel company)
        {
            if (company != null)
            {
                Id = company.Id;
                Name = company.Name;
                Code = company.Code;
                ColorCode = company.ColorCode;
                Description = company.Description;
            }
        }
        public CompanyViewModel(LookupModel<Guid> company)
        {
            if (company != null)
            {
                Id = company.Id;
                Name = company.Name;
                Code = company.Code;
            }
        }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}