using data = Orvosi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using WebApp.Library;
using WebApp.Library.Extensions;
using Orvosi.Shared.Enums;
using WebApp.Models.ServiceRequestModels;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class IndexViewModel
    {   
        public IEnumerable<WeekFolder> WeekFolders { get; set; }
        public IEnumerable<AddOn> AddOns { get; set; }
        public Orvosi.Shared.Model.DayFolder Today { get; set; }
        public IEnumerable<Orvosi.Shared.Model.DayFolder> DueDates { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        public Guid? SelectedUserId { get; set; }
        public bool ShowClosed { get; set; }
        public DateTime Day { get; set; }
        public int TodayCount { get; internal set; }
        public int AddOnsSummaryCount { get; internal set; }
    }

    public class TaskListViewModel
    {
        public TaskListViewModel(List<data.GetAssignedServiceRequestsReturnModel> model, int? taskId, Guid loggedInUserRole)
        {
            this.Tasks = model.Select(o => new ServiceRequestTask
            {
                Id = o.Id,
                Name = o.TaskName,
                StatusId = o.TaskStatusId,
                Status = o.TaskStatusName,
                AssignedTo = o.AssignedTo,
                AssignedToColorCode = o.AssignedToColorCode,
                AssignedToDisplayName = o.AssignedToDisplayName,
                AssignedToInitials = o.AssignedToInitials,
                IsComplete = o.TaskStatusId == TaskStatuses.Done || o.TaskStatusId == TaskStatuses.Archive,
                CompletedDate = o.CompletedDate,
                Sequence = (byte)o.TaskSequence.Value,
                Parent = null,
                TaskId = o.TaskId.Value,
                DependsOnCSV = o.DependsOnCSV,
                TaskType = o.TaskType
            });

            var todo = model.Where(t => t.TaskStatusId == TaskStatuses.ToDo || (t.TaskStatusId == TaskStatuses.Waiting && t.TaskType == "EVENT"));

            if (loggedInUserRole == AspNetRoles.Physician || loggedInUserRole == AspNetRoles.IntakeAssistant || loggedInUserRole == AspNetRoles.DocumentReviewer)
                todo = todo.Where(srt => srt.ResponsibleRoleId == AspNetRoles.Physician || srt.ResponsibleRoleId == AspNetRoles.IntakeAssistant || srt.ResponsibleRoleId == AspNetRoles.DocumentReviewer || srt.TaskId == Orvosi.Shared.Enums.Tasks.SaveMedBrief || srt.TaskId == Orvosi.Shared.Enums.Tasks.AssessmentDay);

            this.People = from t in todo
                          group t by new { t.AssignedTo, t.AssignedToDisplayName, t.AssignedToColorCode, t.AssignedToInitials } into p
                          select new Person
                          {
                              Id = p.Key.AssignedTo,
                              DisplayName = p.Key.AssignedToDisplayName,
                              ColorCode = p.Key.AssignedToColorCode,
                              Initials = p.Key.AssignedToInitials,
                              Tasks = from t in todo
                                      where t.AssignedTo == p.Key.AssignedTo
                                      select new ServiceRequestTask
                                      {
                                          Id = t.Id,
                                          TaskId = t.TaskId.Value,
                                          Name = t.TaskName,
                                          TaskType = t.TaskType
                                      }
                          };

            //if (!taskId.HasValue)
            //{
            //    this.RootTask = this.Tasks.Single(t => t.TaskId == Orvosi.Shared.Enums.Tasks.CloseCase || t.TaskId == Orvosi.Shared.Enums.Tasks.CloseAddOn);
            //}
            //else
            //{
            //    this.RootTask = this.Tasks.Single(t => t.Id == taskId);
            //}

            //BuildDependencies(this.RootTask, this.Tasks);
        }

        public ServiceRequestTask RootTask { get; set; }
        public IEnumerable<ServiceRequestTask> Tasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        private ServiceRequestTask BuildDependencies(ServiceRequestTask task, IEnumerable<ServiceRequestTask> allTasks)
        {

            if (!string.IsNullOrEmpty(task.DependsOnCSV))
            {
                var depends = task.DependsOnCSV.Split(',');
                foreach (var item in depends)
                {
                    //if (task.DependsOnCSV != "ExamDate")
                    //{
                    var id = int.Parse(item);
                    var depTask = allTasks.SingleOrDefault(t => t.Id == id); // Obsolete tasks can be referenced by the DependsOn but will not be returned. Need a null check.
                    if (depTask != null)
                    {
                        depTask.Parent = task;
                        var filledTask = BuildDependencies(depTask, allTasks);
                        task.Dependencies.Add(filledTask);
                    }
                    //}
                    //else
                    //{
                    //    task.Dependencies.Add(new DashboardViewModels.Task
                    //    {
                    //        Name = $"Perform the assessment",
                    //        TaskId = Orvosi.Shared.Enums.Tasks.IntakeInterview,
                    //        StatusId = Orvosi.Shared.Enums.TaskStatuses.ToDo,
                    //        Status = "Wait for the assessment date"
                    //    });
                    //}
                }
            }

            return task;
        }
    }

    //public class Timeline
    //{
    //    public string Name { get; set; }
    //    public byte Sequence { get; set; }
    //    public int AssessmentCount { get; set; }
    //    public int ToDoCount { get; set; }
    //    public int WaitingCount { get; set; }
    //    public IEnumerable<WeekFolder> WeekFolders { get; set; }
    //}

    //public class WeekFolder
    //{
    //    public string WeekFolderName { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public long StartDateTicks { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public int AssessmentCount { get; set; } = 0;
    //    //public int AssessmentToDoCount { get; set; } = 0;
    //    //public int AssessmentWaitingCount { get; set; } = 0;
    //    //public int AssessmentObsoleteCount { get; set; } = 0;
    //    //public int AssessmentDoneCount { get; set; } = 0;
    //    public int AssessmentToDoCount
    //    {
    //        get
    //        {
    //            return DayFolders.Sum(a => a.AssessmentToDoCount);
    //        }
    //    }
    //    public int AssessmentWaitingCount
    //    {
    //        get
    //        {
    //            return DayFolders.Sum(a => a.AssessmentWaitingCount);
    //        }
    //    }
    //    public int AssessmentObsoleteCount
    //    {
    //        get
    //        {
    //            return DayFolders.Sum(a => a.AssessmentObsoleteCount);
    //        }
    //    }
    //    public int AssessmentDoneCount
    //    {
    //        get
    //        {
    //            return DayFolders.Sum(a => a.AssessmentDoneCount);
    //        }
    //    }
    //    public int ToDoCount { get; internal set; } = 0;
    //    public int WaitingCount { get; set; }
    //    public IEnumerable<DayFolder> DayFolders { get; set; }
    //    public byte GetTimeline(DateTime now)
    //    {
    //        byte result = Orvosi.Shared.Enums.Timeline.Future;
    //        if (StartDate <= now && EndDate >= now)
    //            result = Orvosi.Shared.Enums.Timeline.Present;
    //        else if (EndDate < now)
    //            result = Orvosi.Shared.Enums.Timeline.Past;
    //        return result;
    //    }
    //}

    //public class WeekFolderEquals : IEqualityComparer<WeekFolder>
    //{
    //    public bool Equals(WeekFolder left, WeekFolder right)
    //    {
    //        if ((object)left == null && (object)right == null)
    //        {
    //            return true;
    //        }
    //        if ((object)left == null || (object)right == null)
    //        {
    //            return false;
    //        }
    //        return left.WeekFolderName == right.WeekFolderName && left.StartDate == right.StartDate;
    //    }

    //    public int GetHashCode(WeekFolder weekFolder)
    //    {
    //        return (weekFolder.WeekFolderName + weekFolder.StartDate.ToString()).GetHashCode();
    //    }
    //}

    //public class DayFolder
    //{
    //    public DayFolder()
    //    {
    //        Assessments = new List<Assessment>();
    //    }
    //    public DateTime Day { get; set; }
    //    public string City { get; set; }
    //    public string Address { get; set; }
    //    public string Company { get; set; }
    //    public TimeSpan StartTime { get; set; }
    //    public TimeSpan EndTime { get; set; }
    //    public int AssessmentCount { get; set; }
    //    public int ToDoCount { get; set; }
    //    public int WaitingCount { get; set; }
    //    public IEnumerable<Assessment> Assessments { get; set; }
    //    public string DayFormatted_dddd { get; internal set; }
    //    public string DayFormatted_MMMdd { get; internal set; }
    //    public long DayTicks { get; internal set; }
    //    public int AssessmentToDoCount
    //    {
    //        get
    //        {
    //            return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.ToDo);
    //        }
    //    }
    //    public int AssessmentWaitingCount
    //    {
    //        get
    //        {
    //            return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Waiting);
    //        }
    //    }
    //    public int AssessmentObsoleteCount
    //    {
    //        get
    //        {
    //            return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Obsolete);
    //        }
    //    }
    //    public int AssessmentDoneCount
    //    {
    //        get
    //        {
    //            return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Done);
    //        }
    //    }
    //    public bool IsToday()
    //    {
    //        return SystemTime.Now().Date == Day;
    //    }

    //    public bool IsPast()
    //    {
    //        return SystemTime.Now().Date < Day;
    //    }
    //}

    //public class Assessment
    //{
    //    public int Id { get; set; }
    //    public string URL { get; set; }
    //    public string Title { get; set; }
    //    public string ClaimantName { get; set; }
    //    public bool IsLateCancellation { get; set; }
    //    public bool IsNoShow { get; set; }
    //    public DateTime? CancelledDate { get; set; }
    //    public string Service { get; set; }
    //    public string ServiceCode { get; set; }
    //    public string ServiceColorCode { get; set; }
    //    public TimeSpan StartTime { get; set; }
    //    public int CommentCount { get; set; } = 0;
    //    public int ToDoCount { get; set; }
    //    public int WaitingCount { get; set; }
    //    public string BoxCaseFolderURL { get; set; }
    //    public IEnumerable<Task> Tasks { get; set; }
    //    public IEnumerable<Person> People { get; set; }
    //    public bool IsCancelled
    //    {
    //        get
    //        {
    //            return CancelledDate.HasValue;
    //        }
    //    }
    //    public byte ServiceRequestStatusId { get; internal set; }
    //    public bool IsClosed { get; set; }
    //    public byte? ServiceStatusId
    //    {
    //        get
    //        {
    //            if (IsLateCancellation)
    //            {
    //                return ServiceStatus.LateCancellation;
    //            }
    //            else if (CancelledDate.HasValue)
    //            {
    //                return ServiceStatus.Cancellation;
    //            }
    //            else if (IsNoShow)
    //            {
    //                return ServiceStatus.NoShow;
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //    }
    //    public IEnumerable<ServiceRequestMessage> Messages { get; set; }
    //    public bool HasHighWorkload { get; internal set; }
    //}

    //public class AddOn
    //{
    //    public int Id { get; set; }
    //    public string URL { get; set; }
    //    public string Title { get; set; }
    //    public DateTime ReportDueDate { get; set; }
    //    public string ClaimantName { get; set; }
    //    public DateTime? CancelledDate { get; set; }
    //    public string Service { get; set; }
    //    public string ServiceCode { get; set; }
    //    public string ServiceColorCode { get; set; }
    //    public int CommentCount { get; set; } = 0;
    //    public int ToDoCount { get; set; }
    //    public int WaitingCount { get; set; }
    //    public string BoxCaseFolderURL { get; set; }
    //    public IEnumerable<Task> Tasks { get; set; }
    //    public IEnumerable<Person> People { get; set; }
    //    public bool IsCancelled
    //    {
    //        get
    //        {
    //            return CancelledDate.HasValue;
    //        }
    //    }
    //    public IEnumerable<ServiceRequestMessage> Messages { get; set; }
    //    public bool HasHighWorkload { get; internal set; }
    //    public byte ServiceRequestStatusId { get; internal set; }
    //    public bool IsClosed { get; set; }
    //}

    //public class Person
    //{
    //    public Guid? Id { get; set; }
    //    public string DisplayName { get; set; }
    //    public string ColorCode { get; set; }
    //    public string Initials { get; set; }
    //    public int ToDoCount { get; set; }
    //    public int WaitingCount { get; set; }
    //    public IEnumerable<Task> Tasks { get; set; }
    //    public Guid RoleId { get; internal set; }
    //    public string RoleName { get; set; }
    //}

    public class Task
    {
        public Task()
        {
            Dependencies = new List<DashboardViewModels.Task>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public short TaskId { get; set; }
        public byte Sequence { get; set; }
        public byte StatusId { get; set; }
        public string Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid? AssignedTo { get; set; }
        public bool IsComplete { get; set; }
        public IEnumerable<Person> WaitingOn { get; set; }
        public string DependsOnCSV { get; internal set; }
        public Task Parent { get; set; }
        public List<Task> Dependencies { get; set; }
        public string AssignedToDisplayName { get; internal set; }
        public string AssignedToColorCode { get; internal set; }
        public string AssignedToInitials { get; internal set; }
        public int ServiceRequestId { get; internal set; }
        public byte Workload { get; set; }
        public string TaskType { get; set; }
    }

}