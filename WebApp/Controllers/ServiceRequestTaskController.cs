﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.ViewModels.ServiceRequestTaskViewModels;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Models;
using MoreLinq;
using LinqKit;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewDataModels;
using WebApp.ViewModels;
using NinjaNye.SearchExtensions;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestTaskController : BaseController
    {
        public ActionResult TaskGrid(TaskListArgs args)
        {
            var dto = db.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .AreActive()
                .Where(srt => srt.DueDate.HasValue)
                .OrderBy(srt => srt.DueDate).ThenBy(srt => srt.ServiceRequestId).ThenBy(srt => srt.Sequence)
                .Select(TaskDto.FromServiceRequestTaskAndServiceRequestEntity.Expand())
                .ToList();

            var query = dto.AsQueryable();
            if (!string.IsNullOrEmpty(args.searchTerms))
            {
                var search = args.searchTerms.Split(' ');
                query = query
                    .Search(i => i.ServiceRequest.Id.ToString(),
                        i => i.ServiceRequest.ClaimantName,
                        i => i.Name,
                        i => i.ServiceRequest.Company.Name,
                        i => i.ServiceRequest.Company.Code)
                    .ContainingAll(search);
            }

            if (!string.IsNullOrEmpty(args.sort))
            {
                switch (args.sort)
                {
                    case "dueDate":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.DueDate) :
                            query.OrderByDescending(i => i.DueDate);
                        break;
                    case "claimantName":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.ClaimantName) :
                            query.OrderByDescending(i => i.ServiceRequest.ClaimantName);
                        break;
                    case "physician":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.Physician.DisplayName) :
                            query.OrderByDescending(i => i.ServiceRequest.Physician.DisplayName);
                        break;
                    case "appointmentDate":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.AppointmentDate) :
                            query.OrderByDescending(i => i.ServiceRequest.AppointmentDate);
                        break;
                    default:
                        break;
                }
            }

            var data = query
                .Skip(args.take * args.skip)
                .Take(args.take)
                .ToList();

            var total = query.Count();
            var pageCount = total / args.take;


            var taskViewModels = query
                .Select(TaskWithCaseViewModel.FromTaskDto.Expand())
                .ToList();

            var viewModel = new TaskGridViewModel
            {
                Data = taskViewModels,
                Total = total,
                Args = args,
                Pager = new PagerViewModel
                {
                    PageCount = pageCount,
                    CurrentPage = args.skip,
                    NextPage = args.skip >= pageCount ? pageCount : args.skip + 1,
                    PreviousPage = args.skip <= 0 ? 0 : args.skip - 1
                }
            };
            
            return PartialView("TaskGrid", viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]

        public ActionResult TaskGridRow(int serviceRequestTaskId)
        {
            var dto = db.ServiceRequestTasks
                .WithId(serviceRequestTaskId)
                .Select(TaskDto.FromServiceRequestTaskAndServiceRequestEntity.Expand())
                .Single();

            var viewModel = TaskWithCaseViewModel.FromTaskDto.Invoke(dto);

            return PartialView("Task", viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        public ActionResult TaskList(TaskListArgs args)
        {

            var viewModel = db.ServiceRequests
                .WithId(args.ServiceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntity.Expand())
                .ToList() // to dto -> execute the query against the database
                .AsQueryable()
                .Select(TaskListViewModel.FromServiceRequestDtoWithNoTasks.Expand())
                .Single(); // to view model

            // I could embed this into the projection on TaskListViewModel but for clarity I like to keep the filter on tasks in here as it is the primary focus of this controller action.
            var tasksDto = db.ServiceRequestTasks
                .WithServiceRequestId(args.ServiceRequestId)
                .Select(TaskDto.FromServiceRequestTaskEntity.Expand())
                .OrderBy(t => t.Sequence)
                .ToList();

            IEnumerable<TaskDto> query = tasksDto;
            switch (args.ViewFilter)
            {
                case TaskListViewModelFilter.CriticalPathOrAssignedToUser:
                    query = tasksDto.AreOnCriticalPathOrAssignedToUser(userId);
                    break;
                case TaskListViewModelFilter.CriticalPathOnly:
                    query = tasksDto.AreOnCriticalPath();
                    break;
                case TaskListViewModelFilter.PrimaryRolesOnly:
                    var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };
                    query = tasksDto.AreAssignedToUserOrRoles(userId, rolesThatShouldBeSeen);
                    break;
                case TaskListViewModelFilter.MyTasks:
                    query = tasksDto.AreAssignedToUser(userId);
                    break;
                case TaskListViewModelFilter.MyActiveTasks:
                    query = tasksDto
                        .AreActive()
                        .AreAssignedToUser(userId);
                    break;
                default: // default to all tasks
                    break;
            }

            if (args.DateRange != null)
            {
                query = query.AreDueBetween(args.DateRange);
            }

            // role based filters that override everything
            if (roleId == AspNetRoles.IntakeAssistant || roleId == AspNetRoles.DocumentReviewer)
            {
                query = query
                    .AreOnCriticalPathOrAssignedToUser(userId)
                    .ExcludeSubmitInvoice()
                    .ToList();
            }

            viewModel.Tasks = query.AsQueryable().Select(ViewModels.TaskViewModel.FromTaskDto.Expand()).ToList();

            ViewData.TaskListArgs_Set(args);

            return PartialView(viewModel);
        }

        //[ChildActionOnlyOrAjax]
        //[AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        //public ActionResult NextTaskList(ServiceRequestView serviceRequest, IEnumerable<ServiceRequestTask> serviceRequestTasks)
        //{
        //    // get the user
        //    var userId = User.Identity.GetGuidUserId();
        //    var roleId = User.Identity.GetRoleId();
        //    if (roleId != AspNetRoles.SuperAdmin && roleId != AspNetRoles.CaseCoordinator && !db.ServiceRequestTasks.Any(srt => srt.AssignedTo == userId && !srt.IsObsolete))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        //    }

        //    //var serviceRequest = db.ServiceRequests.Single(c => c.Id == serviceRequestId);

        //    var tasks = serviceRequestTasks
        //        .Select(t => new WebApp.ViewModels.ServiceRequestTaskViewModels.TaskViewModel()
        //        {
        //            Id = t.Id,
        //            ServiceRequestId = t.ServiceRequestId,
        //            TaskId = t.TaskId,
        //            Name = t.TaskName,
        //            ShortName = t.ShortName,
        //            CompletedDate = t.CompletedDate,
        //            AssignedToDisplayName = t.AspNetUser_AssignedTo?.GetDisplayName(),
        //            AssignedTo = t.AssignedTo,
        //            AssignedToRoleId = t.ResponsibleRoleId,
        //            AssignedToColorCode = t.AspNetUser_AssignedTo?.ColorCode,
        //            Initials = t.AspNetUser_AssignedTo?.GetInitials(),
        //            DueDateBase = t.DueDateBase,
        //            DueDateDiff = t.DueDateDiff,
        //            AppointmentDate = serviceRequest.AppointmentDate,
        //            ReportDate = serviceRequest.DueDate,
        //            DependsOn = t.DependsOn,
        //            Sequence = t.Sequence
        //        })
        //        .OrderBy(t => t.Sequence)
        //        .ToList();

        //    var closeOutTask = tasks.Single(t => t.TaskId == Tasks.CloseCase);

        //    tasks = tasks
        //        .Where(t => !t.IsObsolete)
        //        .Select(t => {
        //            if (t.DueDateBase.HasValue && t.DueDateDiff.HasValue && serviceRequest.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
        //            {
        //                var reportDueDate = serviceRequest.DueDate.HasValue ? serviceRequest.DueDate.Value : serviceRequest.AppointmentDate.Value.AddDays(3);
        //                t.DueDate = t.DueDateBase == 1 ? serviceRequest.AppointmentDate.Value.AddDays(t.DueDateDiff.Value) : reportDueDate.AddDays(t.DueDateDiff.Value);
        //            }
        //            return t;
        //        }).ToList();

        //    //BuildDependencies(closeOutTask, null, tasks);

        //    var nextTasks = tasks
        //                .Where(c => c.Status.Id != TaskStatuses.Done)
        //                .OrderBy(o => o.Sequence)
        //                .GroupBy(g => g.AssignedTo)
        //                .Select(s => new { s, Count = s.Count() })
        //                .SelectMany(sm => sm.s.Select(s => s)
        //                                  .Zip(Enumerable.Range(1, sm.Count), (task, index)
        //                                      => new NextTaskViewModel() {
        //                                          RowNum = index,
        //                                          Id = task.Id,
        //                                          ServiceRequestId = task.ServiceRequestId,
        //                                          DueDate = task.DueDate,
        //                                          ShortName = task.ShortName,
        //                                          TaskId = task.TaskId,
        //                                          Name = task.Name,
        //                                          Initials = task.Initials,
        //                                          AssignedToRoleId = task.AssignedToRoleId,
        //                                          AssignedToColorCode = task.AssignedToColorCode,
        //                                          AssignedTo = task.AssignedTo,
        //                                          AssignedToDisplayName = task.AssignedToDisplayName,
        //                                          StatusId = task.Status.Id,
        //                                          StatusName = task.Status.Name })
        //                            ).Where(t => t.RowNum == 1)
        //                            .ToList();

        //    return PartialView("NextTaskList", nextTasks);
        //}

        //private TaskViewModel BuildDependencies(TaskViewModel task, TaskViewModel parent, List<TaskViewModel> tasks)
        //{
        //    if (!string.IsNullOrEmpty(task.DependsOn) && task.DependsOn != "ExamDate")
        //    {
        //        var depends = task.DependsOn.Split(',');
        //        foreach (var item in depends)
        //        {
        //            var id = int.Parse(item);
        //            var depTask = tasks.SingleOrDefault(t => t.TaskId == id); // Obsolete tasks can be referenced by the DependsOn but will not be returned. Need a null check.
        //            if (depTask != null)
        //            {
        //                depTask.Parent = parent;
        //                task.Dependencies.Add(BuildDependencies(depTask, task, tasks));
        //            }
        //        }
        //    }
        //    return task;
        //}

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.AddTask)]
        public async Task<ActionResult> AddTask(int serviceRequestId, byte taskId)
        {
            var task = await service.AddTask(serviceRequestId, taskId);

            return Json(new
            {
                id = task.Id,
                serviceRequestId = serviceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.DeleteTask)]
        public async Task<ActionResult> Delete(int serviceRequestTaskId)
        {
            var serviceRequestId = (await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId)).ServiceRequestId;

            await service.DeleteTask(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = serviceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<ActionResult> ToggleTaskStatus(int serviceRequestTaskId, short taskStatusId)
        {
            await service.ToggleTaskStatus(serviceRequestTaskId, taskStatusId);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            //await service.UpdateServiceRequestStatus(srt.ServiceRequestId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<ActionResult> ToggleObsolete(int serviceRequestTaskId)
        {
            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            if (srt == null)
            {
                return HttpNotFound();
            }
            srt.IsObsolete = !srt.IsObsolete;
            srt.ModifiedDate = SystemTime.Now();
            srt.ModifiedUser = User.Identity.Name;
            srt.ServiceRequest.UpdateIsClosed();
            await db.SaveChangesAsync();
            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<ActionResult> ToggleTaskCompleted([Required] int serviceRequestTaskId, [Required] string isChecked)
        {
            var taskStatusId = isChecked == "on" ? TaskStatuses.Done : TaskStatuses.ToDo;

            await service.ChangeTaskStatus(serviceRequestTaskId, taskStatusId);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            //await service.UpdateServiceRequestStatus(srt.ServiceRequestId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.PickupTask)]
        public async Task<ActionResult> PickUp(int serviceRequestTaskId)
        {
            await service.PickUpTask(serviceRequestTaskId);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.AssignTask)]
        public async Task<ActionResult> AssignTo(int serviceRequestTaskId, Guid? assignedTo)
        {
            await service.AssignTaskTo(serviceRequestTaskId, assignedTo);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }
    }

}