using Orvosi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using WebApp.Library;
using WebApp.Library.Extensions;
using Orvosi.Shared.Enums;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(List<API_GetAssignedServiceRequestsReturnModel> model, DateTime now, Guid userId, ControllerContext context)
        {
            // outstanding | today | upcoming
            // each lists days
            // each day lists assessments
            // each assessment lists people
            // each person has tasks   

            var startOfWeek = now.Date.GetStartOfWeek();
            var endOfWeek = now.Date.GetEndOfWeek();
            var startOfNextWeek = now.Date.GetStartOfNextWeek();
            var endOfNextWeek = now.Date.GetEndOfNextWeek();

            var assessments = from m in model
                              where m.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || m.ServiceCategoryId == ServiceCategories.MedicalConsultation
                              orderby m.AppointmentDate, m.StartTime, m.TaskSequence
                              select m;
            var addOns = model.Where(m => m.ServiceCategoryId == ServiceCategories.AddOn).OrderBy(m => m.ReportDueDate).ThenBy(m => m.TaskSequence);

            var firstDate = assessments.Min(a => a.AppointmentDate);
            var lastDate = assessments.Max(a => a.AppointmentDate);

            var weeks = firstDate.GetValueOrDefault(SystemTime.Now()).GetDateRangeTo(lastDate.GetValueOrDefault(SystemTime.Now()))
                .Select(w => new WeekFolder
                {
                    WeekFolderName = w.ToWeekFolderName(),
                    StartDate = w.GetStartOfWeekWithinMonth(),
                    EndDate = w.GetEndOfWeekWithinMonth()
                }).Distinct(new WeekFolderEquals());

            var weekFolders = from w in weeks
                              from a in assessments
                              where a.AppointmentDate >= w.StartDate && a.AppointmentDate <= w.EndDate
                              group a by new { w.WeekFolderName, w.StartDate, w.EndDate } into wf
                              select new WeekFolder
                              {
                                  WeekFolderName = wf.Key.WeekFolderName,
                                  StartDate = wf.Key.StartDate,
                                  StartDateTicks = wf.Key.StartDate.Ticks,
                                  EndDate = wf.Key.EndDate,
                                  AssessmentCount = wf.Select(c => c.ServiceRequestId).Distinct().Count(),
                                  ToDoCount = wf.Count(c => c.AssignedTo == userId && c.TaskStatusId == 102),
                                  WaitingCount = wf.Count(c => c.AssignedTo == userId && c.TaskStatusId == 100),
                                  DayFolders = from o in assessments
                                               where o.AppointmentDate >= wf.Key.StartDate && o.AppointmentDate <= wf.Key.EndDate
                                               group o by o.AppointmentDate into days
                                               select new DayFolder
                                               {
                                                   Day = days.Key.Value.Date,
                                                   DayTicks = days.Key.Value.Date.Ticks,
                                                   DayFormatted_dddd = days.Key.Value.Date.ToString("dddd"),
                                                   DayFormatted_MMMdd = days.Key.Value.Date.ToString("MMM dd"),
                                                   StartTime = days.Min(c => c.StartTime.Value),
                                                   Company = days.Min(c => c.CompanyName),
                                                   Address = days.Min(c => c.AddressName),
                                                   City = days.Min(c => c.City),
                                                   AssessmentCount = days.Select(c => c.ServiceRequestId).Distinct().Count(),
                                                   ToDoCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == 102),
                                                   WaitingCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == 100),
                                                   Assessments = from o in assessments
                                                                 group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
                                                                 where sr.Key.AppointmentDate == days.Key
                                                                 select new Assessment
                                                                 {
                                                                     Id = sr.Key.ServiceRequestId,
                                                                     ClaimantName = sr.Key.ClaimantName,
                                                                     StartTime = sr.Key.StartTime.Value,
                                                                     Service = sr.Key.ServiceName,
                                                                     ServiceCode = sr.Key.ServiceCode,
                                                                     ServiceColorCode = sr.Key.ServiceColorCode,
                                                                     IsLateCancellation = sr.Key.IsLateCancellation.Value,
                                                                     CancelledDate = sr.Key.CancelledDate,
                                                                     IsNoShow = sr.Key.IsNoShow.Value,
                                                                     Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                                                     URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                                                                     BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                                                     ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 102),
                                                                     WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 100),
                                                                     Tasks = from o in assessments
                                                                             where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId
                                                                             select new Task
                                                                             {
                                                                                 Id = o.Id,
                                                                                 Name = o.TaskName,
                                                                                 StatusId = o.TaskStatusId.Value,
                                                                                 Status = o.TaskStatusName,
                                                                                 AssignedTo = o.AssignedTo,
                                                                                 AssignedToDisplayName = o.AssignedToDisplayName,
                                                                                 AssignedToColorCode = o.AssignedToColorCode,
                                                                                 AssignedToInitials = o.AssignedToInitials,
                                                                                 IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                                                                                 ServiceRequestId = o.ServiceRequestId
                                                                             },
                                                                     People = from o in assessments
                                                                              group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials } into at
                                                                              where at.Key.AppointmentDate == days.Key && at.Key.ServiceRequestId == sr.Key.ServiceRequestId
                                                                              select new Person
                                                                              {
                                                                                  Id = at.Key.AssignedTo,
                                                                                  DisplayName = at.Key.AssignedToDisplayName,
                                                                                  ColorCode = at.Key.AssignedToColorCode,
                                                                                  Initials = at.Key.AssignedToInitials,
                                                                                  ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 102),
                                                                                  WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 100),
                                                                                  Tasks = from o in assessments
                                                                                          where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId && o.AssignedTo == at.Key.AssignedTo
                                                                                          select new Task
                                                                                          {
                                                                                              Id = o.Id,
                                                                                              Name = o.TaskName,
                                                                                              StatusId = o.TaskStatusId.Value,
                                                                                              Status = o.TaskStatusName,
                                                                                              AssignedTo = at.Key.AssignedTo,
                                                                                              AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                                                                              AssignedToColorCode = at.Key.AssignedToColorCode,
                                                                                              AssignedToInitials = at.Key.AssignedToInitials,
                                                                                              IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                                                                                              ServiceRequestId = at.Key.ServiceRequestId
                                                                                          }
                                                                              }
                                                                 }
                                               }
                              };

            var ad = from a in addOns
                     group a by new { a.ServiceRequestId, a.ReportDueDate, a.CancelledDate, a.ClaimantName, a.ServiceName, a.ServiceCode, a.ServiceColorCode, a.ServiceId, a.BoxCaseFolderId } into sr
                     select new AddOn
                     {
                         Id = sr.Key.ServiceRequestId,
                         ReportDueDate = sr.Key.ReportDueDate.Value,
                         ClaimantName = sr.Key.ClaimantName,
                         Service = sr.Key.ServiceName,
                         ServiceCode = sr.Key.ServiceCode,
                         ServiceColorCode = sr.Key.ServiceColorCode,
                         CancelledDate = sr.Key.CancelledDate,
                         Title = $"{sr.Key.ReportDueDate.Value.ToShortDateString()} - {sr.Key.ClaimantName}",
                         URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                         BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                         ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                         WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                         Tasks = from o in addOns
                                 where o.ServiceRequestId == sr.Key.ServiceRequestId
                                 select new Task
                                 {
                                     Id = o.Id,
                                     Name = o.TaskName,
                                     StatusId = o.TaskStatusId.Value,
                                     Status = o.TaskStatusName,
                                     AssignedTo = o.AssignedTo,
                                     AssignedToDisplayName = o.AssignedToDisplayName,
                                     AssignedToColorCode = o.AssignedToColorCode,
                                     AssignedToInitials = o.AssignedToInitials,
                                     IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                                     ServiceRequestId = o.ServiceRequestId
                                 },
                         People = from o in addOns
                                  group o by new { o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials } into p
                                  where p.Key.ServiceRequestId == sr.Key.ServiceRequestId
                                  select new Person
                                  {
                                      Id = p.Key.AssignedTo,
                                      DisplayName = p.Key.AssignedToDisplayName,
                                      ColorCode = p.Key.AssignedToColorCode,
                                      Initials = p.Key.AssignedToInitials,
                                      ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                      WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                      Tasks = from o in addOns
                                              where o.ServiceRequestId == p.Key.ServiceRequestId && o.AssignedTo == p.Key.AssignedTo
                                              select new Task
                                              {
                                                  Id = o.Id,
                                                  Name = o.TaskName,
                                                  StatusId = o.TaskStatusId.Value,
                                                  Status = o.TaskStatusName,
                                                  AssignedTo = p.Key.AssignedTo,
                                                  AssignedToDisplayName = p.Key.AssignedToDisplayName,
                                                  AssignedToColorCode = p.Key.AssignedToColorCode,
                                                  AssignedToInitials = p.Key.AssignedToInitials,
                                                  IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                                                  ServiceRequestId = p.Key.ServiceRequestId
                                              }
                                  }
                     };

            var past = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Past);
            var present = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Present);
            var future = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Future);

            WeekFolders = weekFolders;
            AddOns = ad; 
        }

        public IEnumerable<WeekFolder> WeekFolders { get; set; }
        public IEnumerable<AddOn> AddOns { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        public Guid? SelectedUserId { get; set; }
    }

    public class TaskListViewModel
    {
        public TaskListViewModel(List<API_GetServiceRequestReturnModel> model, int? taskId)
        {
            this.Tasks = model.Select(o => new DashboardViewModels.Task
            {
                Id = o.Id,
                Name = o.TaskName,
                StatusId = o.TaskStatusId.Value,
                Status = o.TaskStatusName,
                AssignedTo = o.AssignedTo,
                AssignedToColorCode = o.AssignedToColorCode,
                AssignedToDisplayName = o.AssignedToDisplayName,
                AssignedToInitials = o.AssignedToInitials,
                IsComplete = o.TaskStatusId.Value == TaskStatuses.Done,
                CompletedDate = o.CompletedDate,
                DependsOnCSV = o.DependsOnCSV,
                Sequence = (byte)o.TaskSequence.Value,
                Parent = null,
                TaskId = o.TaskId.Value
            });

            var todo = model.Where(t => t.TaskStatusId == TaskStatuses.ToDo);
            this.People = todo
                .GroupBy(t => new
                {
                    t.AssignedTo,
                    t.AssignedToDisplayName,
                    t.AssignedToColorCode,
                    t.AssignedToInitials
                }).Select(p => new DashboardViewModels.Person
                {
                    Id = p.Key.AssignedTo,
                    DisplayName = p.Key.AssignedToDisplayName,
                    ColorCode = p.Key.AssignedToColorCode,
                    Initials = p.Key.AssignedToInitials,
                    ToDoCount = p.Count(t => t.TaskStatusId == TaskStatuses.ToDo),
                    WaitingCount = p.Count(t => t.TaskStatusId == TaskStatuses.Waiting),
                    Tasks = model.Where(m => m.AssignedTo == p.Key.AssignedTo)
                        .Select(t => new Task
                        {
                            Id = t.Id,
                            TaskId = t.TaskId.Value,
                            Name = t.TaskName
                        })
                })
                .ToList();


            if (!taskId.HasValue)
            {
                this.RootTask = this.Tasks.Single(t => t.TaskId == Orvosi.Shared.Enums.Tasks.CloseCase || t.TaskId == Orvosi.Shared.Enums.Tasks.CloseAddOn);
            }
            else
            {
                this.RootTask = this.Tasks.Single(t => t.Id == taskId);
            }

            BuildDependencies(this.RootTask, this.Tasks);
        }

        public Task RootTask { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        private Task BuildDependencies(Task task, IEnumerable<Task> allTasks)
        {

            if (!string.IsNullOrEmpty(task.DependsOnCSV))
            {
                var depends = task.DependsOnCSV.Split(',');
                foreach (var item in depends)
                {
                    //if (task.DependsOnCSV != "ExamDate")
                    //{
                    var id = int.Parse(item);
                    var depTask = allTasks.SingleOrDefault(t => t.TaskId == id); // Obsolete tasks can be referenced by the DependsOn but will not be returned. Need a null check.
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

    public class Timeline
    {
        public string Name { get; set; }
        public byte Sequence { get; set; }
        public int AssessmentCount { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<WeekFolder> WeekFolders { get; set; }
    }

    public class WeekFolder
    {
        public string WeekFolderName { get; set; }
        public DateTime StartDate { get; set; }
        public long StartDateTicks { get; set; }
        public DateTime EndDate { get; set; }
        public int AssessmentCount { get; set; } = 0;
        public int ToDoCount { get; set; } = 0;
        public int WaitingCount { get; set; }
        public IEnumerable<DayFolder> DayFolders { get; set; }

        public byte GetTimeline(DateTime now)
        {
            byte result = Orvosi.Shared.Enums.Timeline.Future;
            if (StartDate <= now && EndDate >= now)
                result = Orvosi.Shared.Enums.Timeline.Present;
            else if (EndDate < now)
                result = Orvosi.Shared.Enums.Timeline.Past;
            return result;
        }
    }

    public class WeekFolderEquals : IEqualityComparer<WeekFolder>
    {
        public bool Equals(WeekFolder left, WeekFolder right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.WeekFolderName == right.WeekFolderName && left.StartDate == right.StartDate;
        }

        public int GetHashCode(WeekFolder weekFolder)
        {
            return (weekFolder.WeekFolderName + weekFolder.StartDate.ToString()).GetHashCode();
        }
    }

    public class DayFolder
    {
        public DayFolder()
        {
            Assessments = new List<Assessment>();
        }
        public DateTime Day { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AssessmentCount { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<Assessment> Assessments { get; set; }
        public string DayFormatted_dddd { get; internal set; }
        public string DayFormatted_MMMdd { get; internal set; }
        public long DayTicks { get; internal set; }

        public bool IsToday()
        {
            return SystemTime.Now().Date == Day;
        }

        public bool IsPast()
        {
            return SystemTime.Now().Date < Day;
        }
    }

    public class Assessment
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string ClaimantName { get; set; }
        public bool IsLateCancellation { get; set; }
        public bool IsNoShow { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string Service { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceColorCode { get; set; }
        public TimeSpan StartTime { get; set; }
        public int CommentCount { get; set; } = 0;
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public byte ServiceRequestStatusId
        {
            get
            {
                if (Tasks.Any(c => c.StatusId == TaskStatuses.ToDo || c.StatusId == TaskStatuses.Waiting))
                {
                    return ServiceRequestStatus.Open;
                }
                else
                {
                    return ServiceRequestStatus.Closed;
                }
            }
        }
        public byte? ServiceStatusId
        {
            get
            {
                if (IsLateCancellation)
                {
                    return ServiceStatus.LateCancellation;
                }
                else if (CancelledDate.HasValue)
                {
                    return ServiceStatus.Cancellation;
                }
                else if (IsNoShow)
                {
                    return ServiceStatus.NoShow;
                }
                else
                {
                    return null;
                }
            }
        }
        public IEnumerable<ServiceRequestMessage> Messages { get; set; }
    }
    public class AddOn
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public DateTime ReportDueDate { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string Service { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceColorCode { get; set; }
        public int CommentCount { get; set; } = 0;
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public byte ServiceRequestStatusId
        {
            get
            {
                if (Tasks.Any(c => c.StatusId == TaskStatuses.ToDo || c.StatusId == TaskStatuses.Waiting))
                {
                    return ServiceRequestStatus.Open;
                }
                else
                {
                    return ServiceRequestStatus.Closed;
                }
            }
        }
        public IEnumerable<ServiceRequestMessage> Messages { get; set; }
    }
    public class Person
    {
        public Guid? Id { get; set; }
        public string DisplayName { get; set; }
        public string ColorCode { get; set; }
        public string Initials { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }

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
    }

}