using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Data;

namespace WebApp.Areas.Admin.ViewModels.PhysicianLicenceViewModels
{
    public class IndexViewModel
    {
        public AspNetUser User { get; set; }
        public List<PhysicianLicense> Licences { get; set; }
    }
}