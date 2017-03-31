using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TaskWithCaseViewModel
    {
        public TaskViewModel Task { get; set; }
        public CaseViewModel Case { get; set; }

        public static Expression<Func<TaskDto, TaskWithCaseViewModel>> FromTaskDto = dto => dto == null ? null : new TaskWithCaseViewModel
        {
            Task = TaskViewModel.FromTaskDto.Invoke(dto),
            Case = CaseViewModel.FromServiceRequestDto.Invoke(dto.ServiceRequest)
        };
    }
}