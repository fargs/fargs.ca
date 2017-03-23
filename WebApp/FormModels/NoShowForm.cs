using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.FormModels
{
    public class NoShowForm
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        public bool IsNoShow { get; set; }
    }
}