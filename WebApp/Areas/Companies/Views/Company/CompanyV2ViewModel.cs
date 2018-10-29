using System;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class CompanyV2ViewModel : LookupViewModel<Guid>
    {
        public CompanyV2ViewModel(CompanyV2Dto company)
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