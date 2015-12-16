using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Admin.ViewModels.PhysicianLicenceViewModels
{
    public class IndexViewModel
    {
        public User User { get; set; }
        public List<PhysicianLicense> Licences { get; set; }
    }
}