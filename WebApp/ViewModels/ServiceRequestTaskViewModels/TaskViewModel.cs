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
        public string Name { get; set; }
        public bool IsObsolete { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public LookupViewModel<Guid> TaskStatusChangedBy { get; set; }
        public short Sequence { get; set; }
        public short StatusId { get; set; }
        public DateTime? DueDate { get; set; }
        public LookupViewModel<Guid> AssignedTo { get; set; }

        public static Expression<Func<TaskDto, TaskViewModel>> FromTaskDto = dto => dto == null ? null : new TaskViewModel
        {
            Id = dto.Id,
            TaskId = dto.TaskId,
            Name = dto.Name,
            IsObsolete = dto.IsObsolete,
            Sequence = dto.Sequence,
            AssignedTo = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.AssignedTo),
            TaskStatusChangedBy = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.TaskStatusChangedBy),
            TaskStatusChangedDate = dto.TaskStatusChangedDate,
            StatusId = dto.TaskStatusId,
            DueDate = dto.DueDate
        };
    }
}