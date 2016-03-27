using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Library.Enums;

namespace WebApp.ViewModels.ServiceRequestTaskViewModels
{
    public class IndexViewModel
    {
        public User User { get; set; }
        public List<ServiceRequestTask> Tasks { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }


    public class TaskViewModel
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public short? TaskId { get; set; }
        public string Name { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToRoleId { get; set; }
        public string Initials { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsObsolete { get; set; } = false;
        public byte? DueDateBase { get; set; }
        public short? DueDateDiff { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime ReportDate { get; set; }
        public string DependsOn { get; set; }
        public short? Sequence { get; set; }
        public List<TaskViewModel> Dependencies { get; set; }
        public TaskStatusViewModel Status
        {
            get
            {
                if (this.CompletedDate.HasValue)
                {
                    return new TaskStatusViewModel(TaskStatuses.Completed);
                }
                else if (this.IsObsolete == true)
                {
                    return new TaskStatusViewModel(TaskStatuses.Obsolete);
                }
                else if (this.DependsOn == "ExamDate")
                {
                    if (DateTime.Now >= this.ExamDate)
                    {
                        return new TaskStatusViewModel(TaskStatuses.Active);
                    }
                    return new TaskStatusViewModel(TaskStatuses.Waiting);
                }
                else if (string.IsNullOrEmpty(this.DependsOn))
                {
                    return new TaskStatusViewModel(TaskStatuses.Active);
                }
                else
                {
                    var status = new TaskStatusViewModel(TaskStatuses.Active);
                    foreach (var item in Dependencies.Where(d => !d.CompletedDate.HasValue && d.IsObsolete == false))
                    {
                        status.Id = TaskStatuses.Waiting;
                        status.Name = "Waiting On";
                        status.WaitingOn.Add(item);
                    }
                    return status;
                }
            }
        }

        public TaskViewModel()
        {
            Dependencies = new List<TaskViewModel>();
        }
    }


    public class TaskStatusViewModel
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public List<TaskViewModel> WaitingOn { get; set; }

        public TaskStatusViewModel(byte TaskStatusId)
        {
            WaitingOn = new List<TaskViewModel>();

            if (TaskStatusId == TaskStatuses.Active)
            {
                Id = TaskStatuses.Active;
                Name = "Active";
            }
            else if (TaskStatusId == TaskStatuses.Completed)
            {
                Id = TaskStatuses.Completed;
                Name = "Completed";
            }
            else if (TaskStatusId == TaskStatuses.Waiting)
            {
                Id = TaskStatuses.Waiting;
                Name = "Waiting On";
            }
            else if (TaskStatusId == TaskStatuses.Obsolete)
            {
                Id = TaskStatuses.Obsolete;
                Name = "Obsolete";
            }
        }
    }

    public class UserTaskViewModel
    {
        public string Initials { get; set; }
        public Guid UserId { get; set; }
        public string ColorCode { get; set; }
        public int ActiveTaskCount { get; set; }
        public string ActiveColorCode { get; set; }
        public int WaitingTaskCount { get; set; }
        public string WaitingColorCode { get; set; }
        public int CompletedTaskCount { get; set; }
        public string CompletedColorCode { get; set; }
        public IEnumerable<ServiceRequestTask> Tasks { get; set; }
    }

    public class FilterArgs
    {
        public string Sort { get; set; }
        public byte? DateRange { get; set; }
        public byte? StatusId { get; set; }
        public string Ids { get; set; }
        public string ClaimantName { get; set; }
        public string PhysicianId { get; set; }
        public bool? ShowAll { get; set; }
    }
}