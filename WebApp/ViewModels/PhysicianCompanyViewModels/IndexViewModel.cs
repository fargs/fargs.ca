using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.PhysicianCompanyViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public int CompanyCount { get; set; }
    }
}