using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class RequiredResourcesForm
    {
        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public short? ServiceRequestTemplateId { get; set; }
        [Required]
        public int ServiceRequestId { get; set; }
        public IEnumerable<RequiredResourceForm> Resources { get; set; }
    }
}