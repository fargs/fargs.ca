using System;
using System.Collections.Generic;
using System.Linq;
using Orvosi.Shared.Enums;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Models.ServiceRequestModels
{
    public static class ServiceRequestMapper
    {
        public static Assessment MapToAssessment(IEnumerable<Orvosi.Data.GetAssignedServiceRequestsReturnModel> source, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            return (
                from o in source
                group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow, o.CompanyId, o.CompanyName } into sr
                select
                new Assessment
                {
                    Id = sr.Key.ServiceRequestId,
                    ClaimantName = sr.Key.ClaimantName,
                    AppointmentDate = sr.Key.AppointmentDate.Value,
                    StartTime = sr.Key.StartTime.Value,
                    CompanyId = sr.Key.CompanyId.Value,
                    CompanyName = sr.Key.CompanyName,
                    Service = sr.Key.ServiceName,
                    ServiceCode = sr.Key.ServiceCode,
                    ServiceColorCode = sr.Key.ServiceColorCode,
                    IsLateCancellation = sr.Key.IsLateCancellation.Value,
                    CancelledDate = sr.Key.CancelledDate,
                    IsNoShow = sr.Key.IsNoShow.Value,
                    IsClosed = sr.Key.IsClosed.Value,
                    Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                    URL = $"{requestUrl}{sr.Key.ServiceRequestId}",
                    BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                    HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                    ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                    ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                    WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                    Tasks = from o in source
                            select new ServiceRequestTask
                            {
                                Id = o.Id,
                                Name = o.TaskName,
                                StatusId = o.TaskStatusId,
                                Status = o.TaskStatusName,
                                AssignedTo = o.AssignedTo,
                                AssignedToDisplayName = o.AssignedToDisplayName,
                                AssignedToColorCode = o.AssignedToColorCode,
                                AssignedToInitials = o.AssignedToInitials,
                                IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                CompletedDate = o.CompletedDate,
                                ServiceRequestId = o.ServiceRequestId,
                                Workload = o.Workload.GetValueOrDefault(0),
                                TaskId = o.TaskId.Value,
                                TaskType = o.TaskType,
                                ResponsibleRoleId = string.IsNullOrEmpty(o.TaskType) ? o.ResponsibleRoleId.Value : (Guid?)null,
                                ResponsibleRoleName = o.ResponsibleRoleName
                            },
                    People = from o in source
                             where o.TaskType == null
                             group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.AssignedToRoleId, o.AssignedToRoleName, o.ResponsibleRoleId, o.ResponsibleRoleName } into at
                             select new Person
                             {
                                 Id = at.Key.AssignedTo,
                                 DisplayName = at.Key.AssignedToDisplayName,
                                 ColorCode = at.Key.AssignedToColorCode,
                                 Initials = at.Key.AssignedToInitials,
                                 RoleId = at.Key.AssignedToRoleId,
                                 RoleName = at.Key.AssignedToRoleName,
                                 //ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                 //WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                 Tasks = from o in source
                                         where o.AssignedTo == at.Key.AssignedTo
                                         select new ServiceRequestTask
                                         {
                                             Id = o.Id,
                                             Name = o.TaskName,
                                             StatusId = o.TaskStatusId,
                                             Status = o.TaskStatusName,
                                             AssignedTo = at.Key.AssignedTo,
                                             AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                             AssignedToColorCode = at.Key.AssignedToColorCode,
                                             AssignedToInitials = at.Key.AssignedToInitials,
                                             IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                             CompletedDate = o.CompletedDate,
                                             ServiceRequestId = at.Key.ServiceRequestId,
                                             Workload = o.Workload.GetValueOrDefault(0),
                                             TaskId = o.TaskId.Value,
                                             TaskType = o.TaskType,
                                             ResponsibleRoleId = string.IsNullOrEmpty(o.TaskType) ? o.ResponsibleRoleId.Value : (Guid?)null,
                                             ResponsibleRoleName = o.ResponsibleRoleName
                                         }
                             }
                }).First();
        }

        public static AddOn MapToAddOn(IEnumerable<Orvosi.Data.GetAssignedServiceRequestsReturnModel> source, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            return (
                from o in source
                group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow, o.CompanyId, o.CompanyName } into sr
                select
                new AddOn
                {
                    Id = sr.Key.ServiceRequestId,
                    ClaimantName = sr.Key.ClaimantName,
                    CompanyId = sr.Key.CompanyId.Value,
                    CompanyName = sr.Key.CompanyName,
                    Service = sr.Key.ServiceName,
                    ServiceCode = sr.Key.ServiceCode,
                    ServiceColorCode = sr.Key.ServiceColorCode,
                    CancelledDate = sr.Key.CancelledDate,
                    IsClosed = sr.Key.IsClosed.Value,
                    Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                    URL = $"{requestUrl}{sr.Key.ServiceRequestId}",
                    BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                    HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                    ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                    ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                    WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                    Tasks = from o in source
                            select new ServiceRequestTask
                            {
                                Id = o.Id,
                                Name = o.TaskName,
                                StatusId = o.TaskStatusId,
                                Status = o.TaskStatusName,
                                AssignedTo = o.AssignedTo,
                                AssignedToDisplayName = o.AssignedToDisplayName,
                                AssignedToColorCode = o.AssignedToColorCode,
                                AssignedToInitials = o.AssignedToInitials,
                                IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                CompletedDate = o.CompletedDate,
                                ServiceRequestId = o.ServiceRequestId,
                                Workload = o.Workload.GetValueOrDefault(0),
                                TaskType = o.TaskType,
                                TaskId = o.TaskId.Value,
                                ResponsibleRoleId = string.IsNullOrEmpty(o.TaskType) ? o.ResponsibleRoleId.Value : (Guid?)null,
                                ResponsibleRoleName = o.ResponsibleRoleName
                            },
                    People = from o in source
                             group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.AssignedToRoleId, o.AssignedToRoleName } into at
                             where at.Key.TaskType != "EVENT"
                             select new Person
                             {
                                 Id = at.Key.AssignedTo,
                                 DisplayName = at.Key.AssignedToDisplayName,
                                 ColorCode = at.Key.AssignedToColorCode,
                                 Initials = at.Key.AssignedToInitials,
                                 RoleId = at.Key.AssignedToRoleId,
                                 RoleName = at.Key.AssignedToRoleName,
                                 //ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                 //WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                 Tasks = from o in source
                                         where o.AssignedTo == at.Key.AssignedTo
                                         select new ServiceRequestTask
                                         {
                                             Id = o.Id,
                                             Name = o.TaskName,
                                             StatusId = o.TaskStatusId,
                                             Status = o.TaskStatusName,
                                             AssignedTo = at.Key.AssignedTo,
                                             AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                             AssignedToColorCode = at.Key.AssignedToColorCode,
                                             AssignedToInitials = at.Key.AssignedToInitials,
                                             IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                             CompletedDate = o.CompletedDate,
                                             ServiceRequestId = at.Key.ServiceRequestId,
                                             Workload = o.Workload.GetValueOrDefault(0),
                                             TaskType = o.TaskType,
                                             TaskId = o.TaskId.Value,
                                             ResponsibleRoleId = string.IsNullOrEmpty(o.TaskType) ? o.ResponsibleRoleId.Value : (Guid?)null,
                                             ResponsibleRoleName = o.ResponsibleRoleName
                                         }
                             }
                }).First();
        }

        public static IEnumerable<WeekFolder> MapToWeekFolders(List<Orvosi.Data.GetAssignedServiceRequestsReturnModel> model, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            var startOfWeek = now.Date.GetStartOfWeek();
            var endOfWeek = now.Date.GetEndOfWeek();
            var startOfNextWeek = now.Date.GetStartOfNextWeek();
            var endOfNextWeek = now.Date.GetEndOfNextWeek();

            var assessments = from m in model
                              where m.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || m.ServiceCategoryId == ServiceCategories.MedicalConsultation
                              orderby m.AppointmentDate, m.StartTime, m.TaskSequence
                              select m;

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
                                  ToDoCount = wf.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Done),
                                  WaitingCount = wf.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
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
                                                   ToDoCount = days.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                                                   WaitingCount = days.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                                                   Assessments = from o in assessments
                                                                 group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow, o.CompanyId, o.CompanyName } into sr
                                                                 where sr.Key.AppointmentDate == days.Key
                                                                 select new Assessment
                                                                 {
                                                                     Id = sr.Key.ServiceRequestId,
                                                                     ClaimantName = sr.Key.ClaimantName,
                                                                     AppointmentDate = sr.Key.AppointmentDate.Value,
                                                                     StartTime = sr.Key.StartTime.Value,
                                                                     CompanyId = sr.Key.CompanyId.Value,
                                                                     CompanyName = sr.Key.CompanyName,
                                                                     Service = sr.Key.ServiceName,
                                                                     ServiceCode = sr.Key.ServiceCode,
                                                                     ServiceColorCode = sr.Key.ServiceColorCode,
                                                                     IsLateCancellation = sr.Key.IsLateCancellation.Value,
                                                                     CancelledDate = sr.Key.CancelledDate,
                                                                     IsNoShow = sr.Key.IsNoShow.Value,
                                                                     IsClosed = sr.Key.IsClosed.Value,
                                                                     Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                                                     URL = $"{requestUrl}{sr.Key.ServiceRequestId}",
                                                                     BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                                                     HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                                                                     ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                                                     ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                                                                     WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                                                                     Tasks = from o in assessments
                                                                             where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId
                                                                             select new ServiceRequestTask
                                                                             {
                                                                                 Id = o.Id,
                                                                                 Name = o.TaskName,
                                                                                 StatusId = o.TaskStatusId,
                                                                                 Status = o.TaskStatusName,
                                                                                 AssignedTo = o.AssignedTo,
                                                                                 AssignedToDisplayName = o.AssignedToDisplayName,
                                                                                 AssignedToColorCode = o.AssignedToColorCode,
                                                                                 AssignedToInitials = o.AssignedToInitials,
                                                                                 IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                                                 CompletedDate = o.CompletedDate,
                                                                                 ServiceRequestId = o.ServiceRequestId,
                                                                                 Workload = o.Workload.GetValueOrDefault(0),
                                                                                 TaskType = o.TaskType,
                                                                                 TaskId = o.TaskId.Value,
                                                                             },
                                                                     People = from o in assessments
                                                                              group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.AssignedToRoleId, o.AssignedToRoleName } into at
                                                                              where at.Key.AppointmentDate == days.Key && at.Key.ServiceRequestId == sr.Key.ServiceRequestId && at.Key.TaskType != "EVENT"
                                                                              select new Person
                                                                              {
                                                                                  Id = at.Key.AssignedTo,
                                                                                  DisplayName = at.Key.AssignedToDisplayName,
                                                                                  ColorCode = at.Key.AssignedToColorCode,
                                                                                  Initials = at.Key.AssignedToInitials,
                                                                                  RoleId = at.Key.AssignedToRoleId,
                                                                                  RoleName = at.Key.AssignedToRoleName,
                                                                                  //ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                                                                  //WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                                                                  Tasks = from o in assessments
                                                                                          where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId && o.AssignedTo == at.Key.AssignedTo
                                                                                          select new ServiceRequestTask
                                                                                          {
                                                                                              Id = o.Id,
                                                                                              Name = o.TaskName,
                                                                                              StatusId = o.TaskStatusId,
                                                                                              Status = o.TaskStatusName,
                                                                                              AssignedTo = at.Key.AssignedTo,
                                                                                              AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                                                                              AssignedToColorCode = at.Key.AssignedToColorCode,
                                                                                              AssignedToInitials = at.Key.AssignedToInitials,
                                                                                              IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                                                              CompletedDate = o.CompletedDate,
                                                                                              ServiceRequestId = at.Key.ServiceRequestId,
                                                                                              Workload = o.Workload.GetValueOrDefault(0),
                                                                                              TaskType = o.TaskType,
                                                                                              TaskId = o.TaskId.Value,
                                                                                          }
                                                                              }
                                                                 }
                                               }
                              };


            var past = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Past);
            var present = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Present);
            var future = weekFolders.Where(c => c.GetTimeline(now) == Orvosi.Shared.Enums.Timeline.Future);

            return weekFolders;

        }

        public static IEnumerable<AddOn> MapToAddOns(List<Orvosi.Data.GetAssignedServiceRequestsReturnModel> model, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            var addOns = model.Where(m => m.ServiceCategoryId == ServiceCategories.AddOn).OrderBy(m => m.ReportDueDate).ThenBy(m => m.TaskSequence);

            var ad = from a in addOns
                     group a by new { a.ServiceRequestId, a.ServiceRequestStatusId, a.IsClosed, a.ReportDueDate, a.CancelledDate, a.ClaimantName, a.ServiceName, a.ServiceCode, a.ServiceColorCode, a.ServiceId, a.BoxCaseFolderId, a.CompanyId, a.CompanyName } into sr
                     select new AddOn
                     {
                         Id = sr.Key.ServiceRequestId,
                         ReportDueDate = sr.Key.ReportDueDate.Value,
                         ClaimantName = sr.Key.ClaimantName,
                         CompanyId = sr.Key.CompanyId.Value,
                         CompanyName = sr.Key.CompanyName,
                         Service = sr.Key.ServiceName,
                         ServiceCode = sr.Key.ServiceCode,
                         ServiceColorCode = sr.Key.ServiceColorCode,
                         CancelledDate = sr.Key.CancelledDate,
                         Title = $"{sr.Key.ReportDueDate.Value.ToShortDateString()} - {sr.Key.ClaimantName}",
                         URL = $"{requestUrl}{sr.Key.ServiceRequestId}",
                         BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                         HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                         ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                         IsClosed = sr.Key.IsClosed.Value,
                         ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                         WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                         Tasks = from o in addOns
                                 where o.ServiceRequestId == sr.Key.ServiceRequestId
                                 select new ServiceRequestTask
                                 {
                                     Id = o.Id,
                                     Name = o.TaskName,
                                     StatusId = o.TaskStatusId,
                                     Status = o.TaskStatusName,
                                     AssignedTo = o.AssignedTo,
                                     AssignedToDisplayName = o.AssignedToDisplayName,
                                     AssignedToColorCode = o.AssignedToColorCode,
                                     AssignedToInitials = o.AssignedToInitials,
                                     IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                     CompletedDate = o.CompletedDate,
                                     ServiceRequestId = o.ServiceRequestId,
                                     Workload = o.Workload.GetValueOrDefault(0),
                                     TaskType = o.TaskType,
                                     TaskId = o.TaskId.Value,
                                 },
                         People = from o in addOns
                                  group o by new { o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.AssignedToRoleId, o.AssignedToRoleName } into p
                                  where p.Key.ServiceRequestId == sr.Key.ServiceRequestId && p.Key.TaskType != "EVENT"
                                  select new Person
                                  {
                                      Id = p.Key.AssignedTo,
                                      DisplayName = p.Key.AssignedToDisplayName,
                                      ColorCode = p.Key.AssignedToColorCode,
                                      Initials = p.Key.AssignedToInitials,
                                      RoleId = p.Key.AssignedToRoleId,
                                      RoleName = p.Key.AssignedToRoleName,
                                      //ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                                      //WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                                      Tasks = from o in addOns
                                              where o.ServiceRequestId == p.Key.ServiceRequestId && o.AssignedTo == p.Key.AssignedTo
                                              select new ServiceRequestTask
                                              {
                                                  Id = o.Id,
                                                  Name = o.TaskName,
                                                  StatusId = o.TaskStatusId,
                                                  Status = o.TaskStatusName,
                                                  AssignedTo = p.Key.AssignedTo,
                                                  AssignedToDisplayName = p.Key.AssignedToDisplayName,
                                                  AssignedToColorCode = p.Key.AssignedToColorCode,
                                                  AssignedToInitials = p.Key.AssignedToInitials,
                                                  IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                  CompletedDate = o.CompletedDate,
                                                  ServiceRequestId = p.Key.ServiceRequestId,
                                                  Workload = o.Workload.GetValueOrDefault(0),
                                                  TaskType = o.TaskType,
                                                  TaskId = o.TaskId.Value,
                                              }
                                  }
                     };

            return ad;
        }

        public static DayFolder MapToToday(List<Orvosi.Data.GetAssignedServiceRequestsReturnModel> assessments, DateTime now, Guid loggedInUserId, string baseUrl)
        {
            return (from o in assessments
                    where o.AppointmentDate == SystemTime.Now()
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
                        ToDoCount = days.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                        WaitingCount = days.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                        Assessments = from o in assessments
                                      group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.CompanyName, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
                                      where sr.Key.AppointmentDate == days.Key
                                      orderby sr.Key.AppointmentDate, sr.Key.StartTime
                                      select new Assessment
                                      {
                                          Id = sr.Key.ServiceRequestId,
                                          ClaimantName = sr.Key.ClaimantName,
                                          CompanyName = sr.Key.CompanyName,
                                          StartTime = sr.Key.StartTime.Value,
                                          Service = sr.Key.ServiceName,
                                          ServiceCode = sr.Key.ServiceCode,
                                          ServiceColorCode = sr.Key.ServiceColorCode,
                                          IsLateCancellation = sr.Key.IsLateCancellation.Value,
                                          CancelledDate = sr.Key.CancelledDate,
                                          IsNoShow = sr.Key.IsNoShow.Value,
                                          IsClosed = sr.Key.IsClosed.Value,
                                          Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                          URL = $"{baseUrl}/ServiceRequest/Details/{sr.Key.ServiceRequestId}",
                                          BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                          HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                                          ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                          ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                                          WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                                          Tasks = from o in assessments
                                                  where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId
                                                  orderby o.TaskSequence
                                                  select new ServiceRequestTask
                                                  {
                                                      Id = o.Id,
                                                      Name = o.TaskName,
                                                      StatusId = o.TaskStatusId,
                                                      Status = o.TaskStatusName,
                                                      AssignedTo = o.AssignedTo,
                                                      AssignedToDisplayName = o.AssignedToDisplayName,
                                                      AssignedToColorCode = o.AssignedToColorCode,
                                                      AssignedToInitials = o.AssignedToInitials,
                                                      IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                      ServiceRequestId = o.ServiceRequestId,
                                                      Workload = o.Workload.GetValueOrDefault(0)
                                                  },
                                          People = from o in assessments
                                                   group o by new { o.AppointmentDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.ResponsibleRoleId, o.ResponsibleRoleName } into at
                                                   where at.Key.AppointmentDate == days.Key && at.Key.ServiceRequestId == sr.Key.ServiceRequestId && at.Key.TaskType != "EVENT"
                                                   select new Person
                                                   {
                                                       Id = at.Key.AssignedTo,
                                                       DisplayName = at.Key.AssignedToDisplayName,
                                                       ColorCode = at.Key.AssignedToColorCode,
                                                       Initials = at.Key.AssignedToInitials,
                                                       RoleId = at.Key.ResponsibleRoleId.Value,
                                                       RoleName = at.Key.ResponsibleRoleName,
                                                       //ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                                       //WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                                       Tasks = from o in assessments
                                                               where o.AppointmentDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId && o.AssignedTo == at.Key.AssignedTo
                                                               select new ServiceRequestTask
                                                               {
                                                                   Id = o.Id,
                                                                   Name = o.TaskName,
                                                                   StatusId = o.TaskStatusId,
                                                                   Status = o.TaskStatusName,
                                                                   AssignedTo = at.Key.AssignedTo,
                                                                   AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                                                   AssignedToColorCode = at.Key.AssignedToColorCode,
                                                                   AssignedToInitials = at.Key.AssignedToInitials,
                                                                   IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                                   ServiceRequestId = at.Key.ServiceRequestId,
                                                                   Workload = o.Workload.GetValueOrDefault(0)
                                                               }
                                                               
                                                   }
                                      }
                    }).FirstOrDefault();
        }

        public static IEnumerable<DayFolder> MapToDueDates(List<Orvosi.Data.GetAssignedServiceRequestsReturnModel> assessments, DateTime now, Guid loggedInUserId, string baseUrl)
        {
            var assessmentIds = 
                assessments
                    .Where(a => (a.TaskId == Tasks.ApproveReport && a.TaskStatusId == TaskStatuses.ToDo)
                        || (a.TaskId == Tasks.ApproveReport && a.TaskStatusId == TaskStatuses.Waiting &&
                            ((a.AppointmentDate < SystemTime.Now()) || !a.AppointmentDate.HasValue)))
                    .Select(a => a.ServiceRequestId)
                    .Distinct();
            var source = assessments.Where(a => assessmentIds.Contains(a.ServiceRequestId));

            return (from o in source
                    group o by o.DueDate into days
                    select new DayFolder
                    {
                        Day = days.Key.Value.Date,
                        DayTicks = days.Key.Value.Date.Ticks,
                        DayFormatted_dddd = days.Key.Value.Date.ToString("dddd"),
                        DayFormatted_MMMdd = days.Key.Value.Date.ToString("MMM dd"),
                        Company = days.Min(c => c.CompanyName),
                        Address = days.Min(c => c.AddressName),
                        City = days.Min(c => c.City),
                        AssessmentCount = days.Select(c => c.ServiceRequestId).Distinct().Count(),
                        Assessments = from o in source
                                      group o by new { o.DueDate, o.Title, o.BoxCaseFolderId, o.ServiceRequestId, o.CompanyName, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
                                      where sr.Key.DueDate == days.Key 
                                      orderby sr.Key.DueDate
                                      select new Assessment
                                      {
                                          Id = sr.Key.ServiceRequestId,
                                          ClaimantName = sr.Key.ClaimantName,
                                          CompanyName = sr.Key.CompanyName,
                                          Service = sr.Key.ServiceName,
                                          ServiceCode = sr.Key.ServiceCode,
                                          ServiceColorCode = sr.Key.ServiceColorCode,
                                          IsLateCancellation = sr.Key.IsLateCancellation.Value,
                                          CancelledDate = sr.Key.CancelledDate,
                                          IsNoShow = sr.Key.IsNoShow.Value,
                                          IsClosed = sr.Key.IsClosed.Value,
                                          URL = $"{baseUrl}/ServiceRequest/Details/{sr.Key.ServiceRequestId}",
                                          BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                          HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == loggedInUserId && c.Workload == Workload.High),
                                          ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                          ToDoCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.ToDo),
                                          WaitingCount = sr.Count(c => c.AssignedTo == loggedInUserId && c.TaskStatusId == TaskStatuses.Waiting),
                                          //Tasks = from o in source
                                          //        where o.DueDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId
                                          //        orderby o.TaskSequence
                                          //        select new ServiceRequestTask
                                          //        {
                                          //            Id = o.Id,
                                          //            Name = o.TaskName,
                                          //            StatusId = o.TaskStatusId,
                                          //            Status = o.TaskStatusName,
                                          //            AssignedTo = o.AssignedTo,
                                          //            AssignedToDisplayName = o.AssignedToDisplayName,
                                          //            AssignedToColorCode = o.AssignedToColorCode,
                                          //            AssignedToInitials = o.AssignedToInitials,
                                          //            IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                          //            ServiceRequestId = o.ServiceRequestId,
                                          //            Workload = o.Workload.GetValueOrDefault(0)
                                          //        },
                                          People = from o in source
                                                   group o by new { o.DueDate, o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.ResponsibleRoleId, o.ResponsibleRoleName } into at
                                                   where at.Key.DueDate == days.Key && at.Key.ServiceRequestId == sr.Key.ServiceRequestId && at.Key.TaskType != "EVENT"
                                                   select new Person
                                                   {
                                                       Id = at.Key.AssignedTo,
                                                       DisplayName = at.Key.AssignedToDisplayName,
                                                       ColorCode = at.Key.AssignedToColorCode,
                                                       Initials = at.Key.AssignedToInitials,
                                                       RoleId = at.Key.ResponsibleRoleId.Value,
                                                       RoleName = at.Key.ResponsibleRoleName,
                                                       //ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                                       //WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                                       Tasks = from o in source
                                                               where o.DueDate == days.Key && o.ServiceRequestId == sr.Key.ServiceRequestId && o.AssignedTo == at.Key.AssignedTo
                                                               select new ServiceRequestTask
                                                               {
                                                                   Id = o.Id,
                                                                   Name = o.TaskName,
                                                                   StatusId = o.TaskStatusId,
                                                                   Status = o.TaskStatusName,
                                                                   AssignedTo = at.Key.AssignedTo,
                                                                   AssignedToDisplayName = at.Key.AssignedToDisplayName,
                                                                   AssignedToColorCode = at.Key.AssignedToColorCode,
                                                                   AssignedToInitials = at.Key.AssignedToInitials,
                                                                   IsComplete = o.TaskStatusId == TaskStatuses.Done,
                                                                   ServiceRequestId = at.Key.ServiceRequestId,
                                                                   Workload = o.Workload.GetValueOrDefault(0)
                                                               }

                                                   }
                                      }
                    });
        }
    }

    public class WeekFolder
    {
        public string WeekFolderName { get; set; }
        public DateTime StartDate { get; set; }
        public long StartDateTicks { get; set; }
        public DateTime EndDate { get; set; }
        public int AssessmentCount { get; set; } = 0;
        public int AssessmentToDoCount
        {
            get
            {
                return DayFolders.Sum(a => a.AssessmentToDoCount);
            }
        }
        public int AssessmentWaitingCount
        {
            get
            {
                return DayFolders.Sum(a => a.AssessmentWaitingCount);
            }
        }
        public int AssessmentObsoleteCount
        {
            get
            {
                return DayFolders.Sum(a => a.AssessmentObsoleteCount);
            }
        }
        public int AssessmentDoneCount
        {
            get
            {
                return DayFolders.Sum(a => a.AssessmentDoneCount);
            }
        }
        public int ToDoCount { get; internal set; } = 0;
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
        public int AssessmentToDoCount
        {
            get
            {
                return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.ToDo);
            }
        }
        public int AssessmentWaitingCount
        {
            get
            {
                return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Waiting);
            }
        }
        public int AssessmentObsoleteCount
        {
            get
            {
                return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Obsolete);
            }
        }
        public int AssessmentDoneCount
        {
            get
            {
                return Assessments.Count(a => a.ServiceRequestStatusId == TaskStatuses.Done);
            }
        }
        public bool IsToday()
        {
            return SystemTime.Now().Date == Day;
        }

        public bool IsPast()
        {
            return SystemTime.Now().Date < Day;
        }
    }

    public class ServiceRequest
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public string Service { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceColorCode { get; set; }
        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CommentCount { get; set; } = 0;
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public IEnumerable<ServiceRequestTask> Tasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public byte ServiceRequestStatusId { get; internal set; }
        public bool IsClosed { get; set; }
        public IEnumerable<ServiceRequestMessage> Messages { get; set; }
        public bool HasHighWorkload { get; internal set; }
        //public string GetCalendarEventTitle()
        //{
        //    return $"{this.Address.City_CityId.Code}: {ClaimantName} ({Service.Code}) {Company.Code}-{Id})";
        //}
        public Person Physician
        {
            get
            {
                return People.First(c => c.RoleId == AspNetRoles.Physician);
            }
        }
    }

    public class Assessment : ServiceRequest
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool IsLateCancellation { get; set; }
        public bool IsNoShow { get; set; }
        public bool CanBeRescheduled
        {
            get
            {
                return !IsCancelled && AppointmentDate > SystemTime.Now();
            }
        }
        public bool CanBeCancelled
        {
            get
            {
                return !IsCancelled && AppointmentDate > SystemTime.Now();
            }

        }
        public bool CanBeUncancelled
        {
            get
            {
                return IsCancelled || IsLateCancellation && AppointmentDate > SystemTime.Now();
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
    }

    public class AddOn : ServiceRequest
    {
        public DateTime ReportDueDate { get; set; }
    }

    public class Person
    {
        public Guid? Id { get; set; }
        public string DisplayName { get; set; }
        public string ColorCode { get; set; }
        public string Initials { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<ServiceRequestTask> Tasks { get; set; }
        public bool IsEvent { get; internal set; }
        public Guid? RoleId { get; internal set; }
        public string RoleName { get; internal set; }
    }

    public class ServiceRequestTask
    {
        public ServiceRequestTask()
        {
            Dependencies = new List<ServiceRequestTask>();
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
        public ServiceRequestTask Parent { get; set; }
        public List<ServiceRequestTask> Dependencies { get; set; }
        public string AssignedToDisplayName { get; internal set; }
        public string AssignedToColorCode { get; internal set; }
        public string AssignedToInitials { get; internal set; }
        public int ServiceRequestId { get; internal set; }
        public byte Workload { get; set; }
        public string TaskType { get; set; }
        public Guid? ResponsibleRoleId { get; set; }
        public string ResponsibleRoleName { get; set; }
        public string DependsOnCSV { get; internal set; }
    }

    public class ServiceRequestMessage
    {
        public Guid Id { get; set; } // Id (Primary key)
        public string Message { get; set; } // Message (length: 256)
        public DateTime PostedDate { get; set; } // PostedDate
        public string PostedByDisplayName { get; internal set; }
        public string PostedByColorCode { get; internal set; }
        public string PostedByInitials { get; internal set; }
    }
}