using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class TaskDependentDto
    {
        public short TaskId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsObsolete { get; set; }
    }
}