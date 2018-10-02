using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TaskListViewModel
    {
        public LookupViewModel<Guid> Physician { get; set; }
        public CaseViewModel ServiceRequest { get; set; }
        public int ServiceRequestId { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }

        public static Expression<Func<ServiceRequestDto, TaskListViewModel>> FromServiceRequestDtoWithNoTasks = dto => new TaskListViewModel
        {
            ServiceRequestId = dto.Id,
            Physician = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.Physician)
        };
    }
}