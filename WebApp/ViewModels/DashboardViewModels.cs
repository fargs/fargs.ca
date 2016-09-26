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
        public IndexViewModel()
        {
        }

        //public IndexViewModel(List<DashboardServiceRequestSummaryReturnModel> model, DateTime now, Guid userId, ControllerContext context)
        //{
        //    var startOfWeek = now.Date.GetStartOfWeek();
        //    var endOfWeek = now.Date.GetEndOfWeek();
        //    var startOfNextWeek = now.Date.GetStartOfNextWeek();
        //    var endOfNextWeek = now.Date.GetEndOfNextWeek();

        //    var assessments =
        //        model
        //            .Where(m => m.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || m.ServiceCategoryId == ServiceCategories.MedicalConsultation)
        //            .OrderBy(m => m.AppointmentDate);

        //    var assessmentFirstDate = assessments.Min(a => a.AppointmentDate);
        //    var assessmentLastDate = assessments.Max(a => a.AppointmentDate);

        //    var assessmentWeeks =
        //        assessmentFirstDate
        //            .GetValueOrDefault(SystemTime.Now()).GetDateRangeTo(assessmentLastDate.GetValueOrDefault(SystemTime.Now()))
        //            .Select(w => new WeekFolder
        //            {
        //                WeekFolderName = w.ToWeekFolderName(),
        //                StartDate = w.GetStartOfWeekWithinMonth(),
        //                EndDate = w.GetEndOfWeekWithinMonth()
        //            })
        //            .Distinct(new WeekFolderEquals());

        //    WeekFolders = from w in assessmentWeeks
        //                  from a in assessments
        //                  where a.AppointmentDate >= w.StartDate && a.AppointmentDate <= w.EndDate
        //                  group a by new
        //                  {
        //                      w.WeekFolderName,
        //                      w.StartDate,
        //                      w.EndDate
        //                  } into wf
        //                  select new WeekFolder
        //                  {
        //                      WeekFolderName = wf.Key.WeekFolderName,
        //                      StartDate = wf.Key.StartDate,
        //                      StartDateTicks = wf.Key.StartDate.Ticks,
        //                      EndDate = wf.Key.EndDate,
        //                      AssessmentCount = wf.Count(),
        //                      AssessmentToDoCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.ToDo),
        //                      AssessmentWaitingCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Waiting),
        //                      AssessmentObsoleteCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Obsolete),
        //                      AssessmentDoneCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Done)
        //                  };

        //    var addOns =
        //        model
        //            .Where(m => m.ServiceCategoryId == ServiceCategories.AddOn);

        //    var addOnFirstDate = addOns.Min(a => a.AppointmentDate);
        //    var addOnLastDate = addOns.Max(a => a.AppointmentDate);

        //    var addOnWeeks =
        //        addOnFirstDate
        //            .GetValueOrDefault(SystemTime.Now()).GetDateRangeTo(addOnLastDate.GetValueOrDefault(SystemTime.Now()))
        //            .Select(w => new WeekFolder
        //            {
        //                WeekFolderName = w.ToWeekFolderName(),
        //                StartDate = w.GetStartOfWeekWithinMonth(),
        //                EndDate = w.GetEndOfWeekWithinMonth()
        //            })
        //            .Distinct(new WeekFolderEquals());

        //    AddOns = from w in addOnWeeks
        //                  from a in addOns
        //                  where a.AppointmentDate >= w.StartDate && a.AppointmentDate <= w.EndDate
        //                  group a by new
        //                  {
        //                      w.WeekFolderName,
        //                      w.StartDate,
        //                      w.EndDate
        //                  } into wf
        //                  select new WeekFolder
        //                  {
        //                      WeekFolderName = wf.Key.WeekFolderName,
        //                      StartDate = wf.Key.StartDate,
        //                      StartDateTicks = wf.Key.StartDate.Ticks,
        //                      EndDate = wf.Key.EndDate,
        //                      AssessmentCount = wf.Count(),
        //                      AssessmentToDoCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.ToDo),
        //                      AssessmentWaitingCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Waiting),
        //                      AssessmentObsoleteCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Obsolete),
        //                      AssessmentDoneCount = wf.Count(c => c.ServiceRequestStatusId == TaskStatuses.Done)
        //                  };
        //}

        public IndexViewModel(List<data.GetAssignedServiceRequestsReturnModel> model, DateTime now, Guid userId, ControllerContext context)
        {
            // outstanding | today | upcoming
            // each lists days
            // each day lists assessments
            // each assessment lists people
            // each person has tasks   
            this.WeekFolders = new List<WeekFolder>();
            this.AddOns = new List<AddOn>();

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
                                  ToDoCount = wf.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Done),
                                  WaitingCount = wf.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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
                                                   ToDoCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                                   WaitingCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                                                   Assessments = from o in assessments
                                                                 group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
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
                                                                     IsClosed = sr.Key.IsClosed.Value,
                                                                     Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                                                     URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                                                                     BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                                                     HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == userId && c.Workload == Workload.High),
                                                                     ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                                                     ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                                                     WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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
                                               }
                              };

            var ad = from a in addOns
                     group a by new { a.ServiceRequestId, a.ServiceRequestStatusId, a.IsClosed, a.ReportDueDate, a.CancelledDate, a.ClaimantName, a.ServiceName, a.ServiceCode, a.ServiceColorCode, a.ServiceId, a.BoxCaseFolderId } into sr
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
                         HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == userId && c.Workload == Workload.High),
                         ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                         IsClosed = sr.Key.IsClosed.Value,
                         ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                         WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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
                                     ServiceRequestId = o.ServiceRequestId,
                                     Workload = o.Workload.GetValueOrDefault(0)
                                 },
                         People = from o in addOns
                                  group o by new { o.ServiceRequestId, o.AssignedTo, o.AssignedToColorCode, o.AssignedToDisplayName, o.AssignedToInitials, o.TaskType, o.ResponsibleRoleId, o.ResponsibleRoleName } into p
                                  where p.Key.ServiceRequestId == sr.Key.ServiceRequestId && p.Key.TaskType != "EVENT"
                                  select new Person
                                  {
                                      Id = p.Key.AssignedTo,
                                      DisplayName = p.Key.AssignedToDisplayName,
                                      ColorCode = p.Key.AssignedToColorCode,
                                      Initials = p.Key.AssignedToInitials,
                                      RoleId = p.Key.ResponsibleRoleId.Value,
                                      RoleName = p.Key.ResponsibleRoleName,
                                      ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                      WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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
                                                  ServiceRequestId = p.Key.ServiceRequestId,
                                                  Workload = o.Workload.GetValueOrDefault(0)
                                              }
                                  }
                     };

            var today = (from o in assessments
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
                            ToDoCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                            WaitingCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                            Assessments = from o in assessments
                                          group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
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
                                              IsClosed = sr.Key.IsClosed.Value,
                                              Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                              URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                                              BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                              HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == userId && c.Workload == Workload.High),
                                              ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                              ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                              WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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

            var due   = from o in model
                        where o.DueDate <= SystemTime.Now().AddDays(7)
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
                            ToDoCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                            WaitingCount = days.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
                            Assessments = from o in assessments
                                          group o by new { o.AppointmentDate, o.Title, o.BoxCaseFolderId, o.StartTime, o.ServiceRequestId, o.IsClosed, o.ServiceRequestStatusId, o.ClaimantName, o.ServiceName, o.ServiceCode, o.ServiceColorCode, o.IsLateCancellation, o.CancelledDate, o.IsNoShow } into sr
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
                                              IsClosed = sr.Key.IsClosed.Value,
                                              Title = $"{sr.Key.StartTime.ToShortTimeSafe()} - {sr.Key.ClaimantName}",
                                              URL = $"{context.HttpContext.Server.MapPath("/ServiceRequest/Details/")}{sr.Key.ServiceRequestId}",
                                              BoxCaseFolderURL = $"https://orvosi.app.box.com/files/0/f/{sr.Key.BoxCaseFolderId}",
                                              HasHighWorkload = sr.Any(c => (c.TaskStatusId == TaskStatuses.ToDo || c.TaskStatusId == TaskStatuses.Waiting) && c.AssignedTo == userId && c.Workload == Workload.High),
                                              ServiceRequestStatusId = sr.Key.ServiceRequestStatusId.Value,
                                              ToDoCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.ToDo),
                                              WaitingCount = sr.Count(c => c.AssignedTo == userId && c.TaskStatusId == TaskStatuses.Waiting),
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
                        };

            WeekFolders = weekFolders;
            AddOns = ad;
            Today = today;
        }

        public IEnumerable<WeekFolder> WeekFolders { get; set; }
        public IEnumerable<AddOn> AddOns { get; set; }
        public DayFolder Today { get; set; }
        public IEnumerable<DayFolder> DueDates { get; set; }
        public IEnumerable<WebApp.Models.ServiceRequestModels2.DayFolder> DueDates2 { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        public Guid? SelectedUserId { get; set; }
        public bool ShowClosed { get; set; }
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
                IsComplete = o.TaskStatusId == TaskStatuses.Done,
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