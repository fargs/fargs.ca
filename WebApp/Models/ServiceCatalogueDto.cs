using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ServiceCatalogueDto
    {
        public Guid PhysicianId { get; set; }
        public short? CityId { get; set; }
        public CityDto City { get; set; }
        public short? ServiceId { get; set; }
        public LookupDto<short> Service { get; set; }
        public short? CompanyId { get; set; }
        public LookupDto<short> Company { get; set; }
        public decimal Price { get; set; }
    }
}