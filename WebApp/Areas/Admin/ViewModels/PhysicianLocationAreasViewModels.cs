using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Data;

namespace WebApp.Areas.Admin.ViewModels.PhysicianLocationAreasViewModels
{
    public class IndexViewModel
    {
        public AspNetUser User { get; set; }
        public List<PhysicianLocationArea> PhysicianLocationAreas { get; set; }
        public List<LocationArea> LocationAreas { get; set; }
    }
}