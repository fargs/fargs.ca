using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.FormModels
{
    public class NewTaskForm
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        [Range(typeof(DateTime), "1970-01-01", "2050-01-01")]
        public DateTime DueDate { get; set; }
        public string TaskName { get; set; }
        public Guid? AssignedTo { get; set; }

    }
}