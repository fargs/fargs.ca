using ImeHub.Models;
using System;

namespace WebApp.Areas.Availability.Views.Shared
{
    public class PhysicianViewModel
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public PhysicianViewModel(PhysicianModel physician)
        {
            Id = physician.Id;
            CompanyName = physician.CompanyName;
            Code = physician.Code;
            ColorCode = physician.ColorCode;
        }
    }
}