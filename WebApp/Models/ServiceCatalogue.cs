using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ServiceCatalogue
    {
        public short Id { get; set; }
        public string PhysicianId { get; set; }
        public short ServiceId { get; set; }
        public short CompanyId { get; set; }
    }
}