using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;

namespace WebApp.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public short TaskId { get; set; }
        public string Name { get; set; }
        public bool IsObsolete { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public PersonDto TaskStatusChangedBy { get; set; }
        public IEnumerable<TaskDependentDto> Dependencies { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? AppointmentDate { get; set; } // This is set from the parent service request to determine the status of the Assessment Day task
        public DateTime Now { get; set; }
        public short Sequence { get; set; }
        public Guid? AssignedToId { get; set; }
        public PersonDto AssignedTo { get; set; }
        public short TaskStatusId { get; set; }
        public Guid? ResponsibleRoleId { get; set; }
        public string ResponsibleRoleName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool IsCriticalPath { get; set; }

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntity = srt => srt == null ? null : new TaskDto()
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            Name = srt.TaskName,
            IsObsolete = srt.IsObsolete,
            TaskStatusChangedDate = srt.TaskStatusChangedDate,
            TaskStatusChangedBy = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_TaskStatusChangedBy),
            Sequence = srt.Sequence.Value,
            AssignedToId = srt.AssignedTo,
            AssignedTo = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_AssignedTo),
            TaskStatusId = srt.TaskStatusId.Value,
            ResponsibleRoleId = srt.ResponsibleRoleId,
            ResponsibleRoleName = srt.ResponsibleRoleName,
            IsCriticalPath = srt.IsCriticalPath,
            DueDate = srt.DueDate
        };

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntityForSummary = srt => srt == null ? null : new TaskDto()
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            Name = srt.OTask.Name,
            TaskStatusId = srt.TaskStatusId.Value,
            DueDate = srt.DueDate
        };
    }
}