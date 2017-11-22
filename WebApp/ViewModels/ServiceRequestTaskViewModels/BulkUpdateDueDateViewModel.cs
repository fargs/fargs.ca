using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels.ServiceRequestTaskViewModels
{
    public class BulkUpdateDueDateViewModel : TaskViewModel
    {
        public DateTime? NewDueDate { get; set; }

        public new static Expression<Func<TaskDto, BulkUpdateDueDateViewModel>> FromTaskDto = dto => dto == null ? null : new BulkUpdateDueDateViewModel
        {
            Id = dto.Id,
            TaskId = dto.TaskId,
            ServiceRequestId = dto.ServiceRequestId,
            Name = dto.Name,
            ShortName = dto.ShortName,
            CompletedDate = dto.CompletedDate,
            Sequence = dto.Sequence,
            AssignedTo = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.AssignedTo),
            TaskStatusChangedBy = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.TaskStatusChangedBy),
            TaskStatusChangedDate = dto.TaskStatusChangedDate,
            StatusId = dto.TaskStatusId,
            Status = LookupViewModel<short>.FromTaskStatusDto.Invoke(dto.TaskStatus),
            DueDate = dto.DueDate,
            IsActive = dto.IsActive,
            TaskTemplateId = dto.TaskTemplateId,
            IsOverdue = TaskDto.IsOverdueExp.Invoke(dto.DueDate, dto.TaskStatusId, DateTime.Now), // This must be DateTime.Now and not SystemTime.Now() because it is getting translated to sql
            IsDueToday = TaskDto.IsDueTodayExp.Invoke(dto.DueDate, dto.TaskStatusId, DateTime.Now)
        };
    }
}