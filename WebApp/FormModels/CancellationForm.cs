using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class CancellationForm
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        [Range(typeof(DateTime), "1970-01-01", "2050-01-01")]
        public DateTime CancelledDate { get; set; }
        public string IsLate { get; set; }
        public string Notes { get; set; }
        public int? LateCancellationPolicy { get; set; }
        public bool IsLateCancellationPolicyViolated { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}