using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.FormModels
{
    public class ServiceCatalogueForm
    {
        [Required]
        public short? CompanyId { get; set; }
        public short? ServiceId { get; set; }
        public short? LocationId { get; set; }
        public decimal? Price { get; set; }
        public bool IsLocationRequired { get; set; } = false;
    }
}