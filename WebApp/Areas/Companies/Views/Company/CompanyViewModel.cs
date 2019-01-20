using System;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class CompanyViewModel : LookupViewModel<Guid>
    {
        public CompanyViewModel(CompanyModel company)
        {
            Id = company.Id;
            Name = company.Name;
            Code = company.Code;
            ColorCode = company.ColorCode;
            Description = company.Description;
        }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}