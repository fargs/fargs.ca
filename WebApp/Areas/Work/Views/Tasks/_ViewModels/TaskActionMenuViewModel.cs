using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Tasks
{
    public class TaskActionMenuViewModel : ViewModelBase
    {
        public TaskActionMenuViewModel()
        {

        }
        public TaskActionMenuViewModel(TaskDto task, IIdentity identity, DateTime now) : base(identity, now)
        {
            Id = task.Id;
            Physician = new PhysicianViewModel(task.ServiceRequest.Physician);
            ServiceRequestId = task.ServiceRequestId;
            TaskStatusId = task.TaskStatusId;
            IsAppointment = task.IsAppointment;
            AssignedTo = LookupViewModel<Guid>.FromPersonDto(task.AssignedTo);
            // this deep copies the team members into a new collection which we will then remove the AssignedTo from
            AssignedToSelectList = task.ServiceRequest.Physician.TeamMembers.Select(a => new SelectListItem
            {
                Text = a.DisplayName,
                Value = a.Id.ToString()
            }).ToList();
            if (task.AssignedToId.HasValue)
            {
                AssignedToSelectList.Remove(AssignedToSelectList.Single(m => m.Value == task.AssignedToId.Value.ToString()));
            }
        }
        public long Id { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public int ServiceRequestId { get; set; }
        public short TaskStatusId { get; set; }
        public bool IsAppointment { get; set; } = false;
        public LookupViewModel<Guid> AssignedTo { get; set; }
        public IList<SelectListItem> AssignedToSelectList { get; set; }
    }

}