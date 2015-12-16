using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Admin.ViewModels.PhysicianInsuranceViewModels
{
    public class IndexViewModel
    {
        public User User { get; set; }
        public List<PhysicianInsurance> Insurance { get; set; }
    }
}