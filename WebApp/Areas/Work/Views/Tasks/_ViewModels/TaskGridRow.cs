using System;
using System.Linq.Expressions;
using System.Security.Principal;
using LinqKit;
using Enums = Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApp.Areas.Work.Views.Tasks
{
    public class TaskGridRow : ViewModelBase
    {
        public TaskGridRow()
        {

        }
        public TaskGridRow(TaskDto task, IIdentity identity, DateTime now) : base(identity, now)
        {
            Id = task.Id;
            AssignedTo = LookupViewModel<Guid>.FromPersonDto(task.AssignedTo);
            TaskId = task.TaskId;
            TaskShortName = task.ShortName;
            TaskName = task.Name;
            TaskStatusId = task.TaskStatusId;
            TaskStatusName = task.TaskStatus.Name;
            IsDone = task.TaskStatusId == Enums.TaskStatuses.Done;
            DueDate = task.DueDate;
            DueDateFormatted = task.DueDate.HasValue ? task.DueDate.Value.ToOrvosiDateFormat() : "ASAP";
            TaskStatusChangedDate = task.TaskStatusChangedDate;
            TaskStatusChangedBy = LookupViewModel<Guid>.FromPersonDto(task.TaskStatusChangedBy);
            ServiceRequestId = task.ServiceRequest.Id;
            ClaimantName = task.ServiceRequest.ClaimantName;
            HasNotes = !string.IsNullOrEmpty(task.ServiceRequest.Notes);
            AppointmentDateAndStartTime = task.ServiceRequest.AppointmentDateAndStartTime;
            Company = task.ServiceRequest.Company.Code;
            Service = task.ServiceRequest.Service.Code;
            City = task.ServiceRequest.Address != null ? task.ServiceRequest.Address.CityCode : "";
            Physician = LookupViewModel<Guid>.FromPersonDto(task.ServiceRequest.Physician);
            PhysicianSortColumn = task.ServiceRequest.Physician.LastName;

            IsOverdue = task.IsOverdue(task.DueDate, task.TaskStatusId, now);
            IsDueToday = task.IsDueToday(task.DueDate, task.TaskStatusId, now);

            // filter out the assigned to user from the team member select list.
            ActionMenu = new TaskActionMenuViewModel(task, identity, now);

            // Handle nulls when passed into partial views
            AssignedTo = AssignedTo == null ? new LookupViewModel<Guid>() : AssignedTo;

            // Html values
            IsCheckedValue = TaskStatusId == Enums.TaskStatuses.Done || TaskStatusId == Enums.TaskStatuses.Archive;
            IsCheckedChecked = TaskStatusId == Enums.TaskStatuses.Done || TaskStatusId == Enums.TaskStatuses.Archive ? "checked" : "";
            TaskStatusMessage = "Last changed to " + TaskStatusName + " by " + (TaskStatusChangedDate.HasValue ? TaskStatusChangedBy.Name + " on " + TaskStatusChangedDate.Value.ToString("ddd, MMM dd") + " at " + TaskStatusChangedDate.Value.ToShortTimeString() : "was not recorded");
        }
        public int Id { get; set; }
        public LookupViewModel<Guid> AssignedTo { get; set; }
        public bool IsCheckedValue { get; set; }
        public string IsCheckedChecked { get; set; }
        public string TaskStatusMessage { get; set; }
        public short TaskId { get; set; }
        public string TaskShortName { get; set; }
        public string TaskName { get; set; }
        public short TaskStatusId { get; set; }
        public string TaskStatusName { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public LookupViewModel<Guid> TaskStatusChangedBy { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public string DueDateFormatted { get; set; }
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
        public TaskActionMenuViewModel ActionMenu { get; set; }
    }
}