using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.FormModels
{
    public class EditTaskForm
    {
        [Required]
        public long ServiceRequestTaskId { get; set; }
        [Range(typeof(DateTime), "1970-01-01", "2050-01-01")]
        public DateTime? DueDate { get; set; }
        [Required]
        public string TaskName { get; set; }

        public static Expression<Func<TaskDto, EditTaskForm>> FromTaskDto = dto => dto == null ? null : new EditTaskForm
        {
            ServiceRequestTaskId = dto.Id,
            TaskName = dto.Name,
            DueDate = dto.DueDate
        };

    }
}