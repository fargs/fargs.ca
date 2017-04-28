using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public short TaskId { get; set; }
        public int ServiceRequestId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public LookupViewModel<Guid> TaskStatusChangedBy { get; set; }
        public short Sequence { get; set; }
        public short StatusId { get; set; }
        public LookupViewModel<short> Status { get; set; }
        public DateTime? DueDate { get; set; }
        public LookupViewModel<Guid> AssignedTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsOverdue { get; set; } = false;
        public bool IsDueToday { get; set; } = false;

        public static Expression<Func<TaskDto, TaskViewModel>> FromTaskDto = dto => dto == null ? null : new TaskViewModel
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
            IsOverdue = TaskDto.IsOverdueExp.Invoke(dto.DueDate, DateTime.Now), // This must be DateTime.Now and not SystemTime.Now() because it is getting translated to sql
            IsDueToday = TaskDto.IsDueTodayExp.Invoke(dto.DueDate, DateTime.Now)
        };
    }
}