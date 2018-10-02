using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TaskGridRow
    {
        public int Id { get; set; }
        public LookupViewModel<Guid> AssignedTo { get; set; }
        public short TaskId { get; set; }
        public string TaskShortName { get; set; }
        public string TaskName { get; set; }
        public short TaskStatusId { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public LookupViewModel<Guid> TaskStatusChangedBy { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public int ServiceRequestId { get; set; }
        public DateTime? AppointmentDateAndStartTime { get; set; }
        public string ClaimantName { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public string PhysicianSortColumn { get; set; }
        public string Company { get; set; }
        public string Service { get; set; }
        public string City { get; set; }
        public bool HasNotes { get; set; }
        public bool? IsOverdue { get; set; }
        public bool? IsDueToday { get; set; }

        public static Expression<Func<TaskDto, TaskGridRow>> FromTaskDto = dto => dto == null ? null : new TaskGridRow
        {
            Id = dto.Id,
            AssignedTo = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.AssignedTo),
            TaskId = dto.TaskId,
            TaskShortName = dto.ShortName,
            TaskName = dto.Name,
            TaskStatusId = dto.TaskStatusId,
            IsDone = dto.TaskStatusId == TaskStatuses.Done,
            DueDate = dto.DueDate,
            TaskStatusChangedDate = dto.TaskStatusChangedDate,
            TaskStatusChangedBy = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.TaskStatusChangedBy),
            ServiceRequestId = dto.ServiceRequest.Id,
            ClaimantName = dto.ServiceRequest.ClaimantName,
            HasNotes = !string.IsNullOrEmpty(dto.ServiceRequest.Notes),
            AppointmentDateAndStartTime = dto.ServiceRequest.AppointmentDateAndStartTime,
            Company = dto.ServiceRequest.Company.Code,
            Service = dto.ServiceRequest.Service.Code,
            City = dto.ServiceRequest.Address != null ? dto.ServiceRequest.Address.CityCode : "",
            Physician = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.ServiceRequest.Physician),
            PhysicianSortColumn = dto.ServiceRequest.Physician.LastName,
            IsOverdue = TaskDto.IsOverdueExp.Invoke(dto.DueDate, dto.TaskStatusId, DateTime.Now),
            IsDueToday = TaskDto.IsDueTodayExp.Invoke(dto.DueDate, dto.TaskStatusId, DateTime.Now)
        };
    }
}