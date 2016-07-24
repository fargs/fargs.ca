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
            var addOns = model.Where(m => m.ServiceCategoryId == ServiceCategories.AddOn);

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
                                                                 group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.ClaimantName, o.ServiceName } into sr
                                                                 where sr.Key.AppointmentDate == days.Key
                                                                 select new Assessment
                                                                 {
                                                                     Id = sr.Key.ServiceRequestId,
                                                                     ClaimantName = sr.Key.ClaimantName,
                                                                     StartTime = sr.Key.StartTime.Value,
                                                                     Service = sr.Key.ServiceName,
                                                                     Title = $"{sr.Key.StartTime} - {sr.Key.ClaimantName}",
                                                                     URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                                                                     BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                                                     ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 102),
                                                                     WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == 100),
                                                                     People = from o in assessments
                                                                              group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials } into at
                                                                              where at.Key.AppointmentDate == days.Key && at.Key.ServiceRequestId == sr.Key.ServiceRequestId
                                                                              select new Person
                                                                              {
                                                                                  Id = at.Key.AssignedTo.Value,
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
                                                                                              AssignedTo = o.AssignedTo,
                                                                                              IsComplete = o.TaskStatusId.Value == TaskStatuses.Done
                                                                                          }
                                                                              }
                                                                 }
                                               }
                              };

            var past = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Past);
            var present = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Present);
            var future = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Future);

            WeekFolders = weekFolders;
        }

        public IEnumerable<WeekFolder> WeekFolders { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        public Guid? SelectedUserId { get; set; }

    }

    public class TaskViewModel
    {
        public Task Task { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
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
        public string Service { get; set; }
        public TimeSpan StartTime { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public IEnumerable<Task> MyTasks { get; set; }
        public IEnumerable<Person> People { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string ColorCode { get; set; }
        public string Initials { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Sequence { get; set; }  
        public byte StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CompletedDate { get; set; }
        public Guid? AssignedTo { get; set; }
        public bool IsComplete { get; set; }
        public IEnumerable<Person> WaitingOn { get; set; }
        public string DependsOnCSV { get; internal set; }
    }
}