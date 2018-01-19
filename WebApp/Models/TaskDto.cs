using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;
using Orvosi.Shared.Enums;

namespace WebApp.Models
{
    public class TaskDto
    {

        public int Id { get; set; }
        public short TaskId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public PersonDto TaskStatusChangedBy { get; set; }
        public IEnumerable<TaskDependentDto> Dependencies { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? AppointmentDate { get; set; } // This is set from the parent service request to determine the status of the Assessment Day task
        public short? Sequence { get; set; }
        public Guid? AssignedToId { get; set; }
        public PersonDto AssignedTo { get; set; }
        public short TaskStatusId { get; set; }
        public LookupDto<short> TaskStatus { get; set; }
        public Guid? ResponsibleRoleId { get; set; }
        public string ResponsibleRoleName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool IsCriticalPath { get; set; }
        public int ServiceRequestId { get; set; }
        public Guid? TaskTemplateId { get; set; }
        public ServiceRequestDto ServiceRequest { get; set; }
        public ServiceRequestTemplateTaskDto TaskTemplate { get; set; }

        public bool IsActive
        {
            get
            {
                return TaskStatusId == TaskStatuses.ToDo || TaskStatusId == TaskStatuses.Waiting || TaskStatusId == TaskStatuses.OnHold;
            }
        }

        public bool IsAppointment
        {
            get
            {
                return TaskId == Tasks.AssessmentDay;
            }
        }

        public static Expression<Func<DateTime?, short, DateTime, bool>> IsOverdueExp = (dueDate, status, now) => dueDate.HasValue && (status == TaskStatuses.ToDo || status == TaskStatuses.Waiting) ? dueDate.Value.Date < now.Date : false;

        public static Expression<Func<DateTime?, short, DateTime, bool>> IsDueTodayExp = (dueDate, status, now) => dueDate.HasValue && (status == TaskStatuses.ToDo || status == TaskStatuses.Waiting) ? dueDate.Value.Date == now.Date : false;


        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntity = srt => srt == null ? null : new TaskDto()
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            ServiceRequestId = srt.ServiceRequestId,
            Name = srt.TaskName,
            ShortName = srt.ShortName,
            CompletedDate = srt.CompletedDate,
            TaskStatusChangedDate = srt.TaskStatusChangedDate,
            TaskStatusChangedBy = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_TaskStatusChangedBy),
            Sequence = srt.Sequence,
            AssignedToId = srt.AssignedTo,
            AssignedTo = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_AssignedTo),
            TaskStatusId = srt.TaskStatusId,
            ResponsibleRoleId = srt.ResponsibleRoleId,
            ResponsibleRoleName = srt.ResponsibleRoleName ?? (srt.ServiceRequestTemplateTask == null || srt.ServiceRequestTemplateTask.AspNetRole == null ? "" : srt.ServiceRequestTemplateTask.AspNetRole.Name),
            IsCriticalPath = srt.IsCriticalPath,
            DueDate = srt.DueDate,
            TaskTemplateId = srt.ServiceRequestTemplateTaskId,
            TaskStatus = LookupDto<short>.FromTaskStatusEntity.Invoke(srt.TaskStatu)
        };

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskAndServiceRequestEntity = srt => srt == null ? null : new TaskDto()
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            Name = srt.TaskName,
            ShortName = srt.ShortName,
            CompletedDate = srt.CompletedDate,
            TaskStatusChangedDate = srt.TaskStatusChangedDate,
            TaskStatusChangedBy = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_TaskStatusChangedBy),
            Sequence = srt.Sequence,
            AssignedToId = srt.AssignedTo,
            AssignedTo = PersonDto.FromAspNetUserEntity.Invoke(srt.AspNetUser_AssignedTo),
            TaskStatusId = srt.TaskStatusId,
            ResponsibleRoleId = srt.ResponsibleRoleId,
            ResponsibleRoleName = srt.ResponsibleRoleName ?? (srt.ServiceRequestTemplateTask == null || srt.ServiceRequestTemplateTask.AspNetRole == null ? "" : srt.ServiceRequestTemplateTask.AspNetRole.Name),
            IsCriticalPath = srt.IsCriticalPath,
            DueDate = srt.DueDate,
            TaskStatus = LookupDto<short>.FromTaskStatusEntity.Invoke(srt.TaskStatu),
            ServiceRequest = ServiceRequestDto.FromServiceRequestEntityForCase.Invoke(srt.ServiceRequest)
        };

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntityForSummary = srt => srt == null ? null : new TaskDto()
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            Name = srt.TaskName,
            ShortName = srt.ShortName,
            TaskStatusId = srt.TaskStatusId,
            DueDate = srt.DueDate
        };

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntityForInvoiceDetail = srt => new TaskDto
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            TaskStatusId = srt.TaskStatusId,
            TaskStatusChangedDate = srt.TaskStatusChangedDate
        };

        public static Expression<Func<ServiceRequestTask, TaskDto>> FromServiceRequestTaskEntityAndTemplate = srt => new TaskDto
        {
            Id = srt.Id,
            TaskId = srt.TaskId.Value,
            Name = srt.TaskName,
            Sequence = srt.Sequence,
            DueDate = srt.DueDate,
            TaskTemplateId = srt.ServiceRequestTemplateTaskId,
            TaskTemplate = ServiceRequestTemplateTaskDto.FromEntity.Invoke(srt.ServiceRequestTemplateTask)
        };
    }
}