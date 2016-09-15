using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Data;

namespace WebApp.Areas.Admin.ViewModels.PhysicianInsuranceViewModels
{
    public class IndexViewModel
    {
        public AspNetUser User { get; set; }
        public List<PhysicianInsurance> Insurance { get; set; }
    }
}