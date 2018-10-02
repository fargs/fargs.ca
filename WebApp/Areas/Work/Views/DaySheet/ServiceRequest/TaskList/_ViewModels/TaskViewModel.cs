using System;
using System.Collections.Generic;
using System.Security.Principal;
using Enums = Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.TaskList
{
    public class TaskViewModel
    {
        public TaskViewModel(TaskDto task, PersonDto physician, IList<PersonDto> teamMembers, IIdentity identity, DateTime now)
        {
            Id = task.Id;
            TaskId = task.TaskId;
            ServiceRequestId = task.ServiceRequestId;
            Name = task.Name;
            AssignedTo = LookupViewModel<Guid>.FromPersonDto(task.AssignedTo);
            TaskStatusChangedBy = LookupViewModel<Guid>.FromPersonDto(task.TaskStatusChangedBy);
            TaskStatusChangedDate = task.TaskStatusChangedDate;
            StatusId = task.TaskStatusId;
            Status = LookupViewModel<short>.FromTaskStatusDto(task.TaskStatus);
            DueDate = task.DueDate.ToOrvosiDateFormat();
            IsAppointment = task.IsAppointment;

            IsOverdue = task.IsOverdue(task.DueDate, task.TaskStatusId, now);
            IsDueToday = task.IsDueToday(task.DueDate, task.TaskStatusId, now);

            // filter out the assigned to user from the team member select list.
            ActionMenu = new TaskActionMenuViewModel(task, physician, teamMembers, identity, now);

            // Handle nulls when passed into partial views
            AssignedTo = AssignedTo == null ? new LookupViewModel<Guid>() : AssignedTo;

            // Html values
            IsCheckedValue = StatusId == Enums.TaskStatuses.Done || StatusId == Enums.TaskStatuses.Archive;
            IsCheckedChecked = StatusId == Enums.TaskStatuses.Done || StatusId == Enums.TaskStatuses.Archive ? "checked" : "";
            TaskStatusMessage = "Last changed to " + Status.Name + " by " + (TaskStatusChangedDate.HasValue ? TaskStatusChangedBy.Name + " on " + TaskStatusChangedDate.Value.ToString("ddd, MMM dd") + " at " + TaskStatusChangedDate.Value.ToShortTimeString() : "was not recorded");

            // Styles
            Style_TaskName = TaskId == Enums.Tasks.AssessmentDay || TaskId == Enums.Tasks.SubmitReport ? "font-size:20px;" : "";
            Style_TaskDueDate = TaskId == Enums.Tasks.AssessmentDay || TaskId == Enums.Tasks.SubmitReport ? "font-size:20px;" : "";
        }
        public int Id { get; set; }
        public short TaskId { get; set; }
        public int ServiceRequestId { get; set; }
        public string Name { get; set; }
        public DateTime? TaskStatusChangedDate { get; set; }
        public LookupViewModel<Guid> TaskStatusChangedBy { get; set; }
        public short StatusId { get; set; }
        public LookupViewModel<short> Status { get; set; }
        public string DueDate { get; set; }
        public LookupViewModel<Guid> AssignedTo { get; set; }
        public bool IsCheckedValue { get; }
        public string IsCheckedChecked { get; }
        public string TaskStatusMessage { get; }
        public string Style_TaskName { get; }
        public string Style_TaskDueDate { get; }
        public bool IsAppointment { get; set; } = false;
        public bool IsOverdue { get; }
        public bool IsDueToday { get; }
        public TaskActionMenuViewModel ActionMenu { get; set; }
    }
}