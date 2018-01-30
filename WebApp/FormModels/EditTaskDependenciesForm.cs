using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.FormModels
{
    public class EditTaskDependenciesForm
    {
        [Required]
        public long ServiceRequestTaskId { get; set; }

        public string TaskName { get; set; }

        public int[] Dependencies { get; set; }

        public static Expression<Func<ServiceRequestTask, EditTaskDependenciesForm>> FromTaskEntity = dto => dto == null ? null : new EditTaskDependenciesForm
        {
            ServiceRequestTaskId = dto.Id,
            Dependencies = dto.Child.Select(c => c.Id).ToArray()
        };

    }
}