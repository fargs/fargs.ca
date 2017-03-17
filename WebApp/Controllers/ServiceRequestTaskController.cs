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

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestTaskController : BaseController
    {

        // GET: ServiceRequestTasks
        public ActionResult Index(FilterArgs filterArgs)
        {
            // get the user
            var sr = db.ServiceRequestTasks.Where(srt => !srt.IsObsolete).AsQueryable<ServiceRequestTask>();

            if (User.Identity.GetRoleId() == AspNetRoles.SuperAdmin && filterArgs.ShowAll.Value) { }
            else
            {
                sr.Where(c => c.AssignedTo == User.Identity.GetGuidUserId());
            }

            var vm = new WebApp.ViewModels.ServiceRequestTaskViewModels.IndexViewModel() // namespace is needed because Models/ManageViewModels didn't come with a namespace.
            {
                Tasks = sr.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.AddTask)]
        public ActionResult AddTask(int serviceRequestId, byte taskId)
        {
            var request = db.ServiceRequests.Find(serviceRequestId);

            var st = db.OTasks.Single(t => t.Id == taskId);
            var task = new ServiceRequestTask();
            task.ServiceRequestId = serviceRequestId;
            task.TaskId = taskId;
            task.TaskName = st.Name;
            task.TaskPhaseId = st.TaskPhaseId;
            task.TaskPhaseName = st.TaskPhase.Name;
            task.ResponsibleRoleId = st.ResponsibleRoleId;
            task.ResponsibleRoleName = st.AspNetRole.Name;
            task.Sequence = st.Sequence;
            task.AssignedTo = db.ServiceRequestTasks.FirstOrDefault(sr => sr.ServiceRequestId == serviceRequestId && sr.ResponsibleRoleId == st.ResponsibleRoleId).AssignedTo;
            task.IsBillable = st.IsBillable.Value;
            task.HourlyRate = st.HourlyRate;
            task.EstimatedHours = st.EstimatedHours;
            task.DependsOn = st.DependsOn;
            task.DueDateBase = st.DueDateBase;
            task.DueDateDiff = st.DueDateDiff;
            task.Guidance = st.Guidance;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = User.Identity.Name;

            // this is currently hard coded but should be made editable by the user adding the task
            if (taskId == Tasks.AdditionalEdits)
            {
                var approveReport = db.ServiceRequestTasks.FirstOrDefault(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.ApproveReport);
                if (approveReport != null)
                {
                    approveReport.CompletedDate = null;
                    approveReport.CompletedBy = null;
                    approveReport.ModifiedDate = SystemTime.UtcNow();
                    approveReport.ModifiedUser = User.Identity.GetGuidUserId().ToString();

                    // make this task dependent on additional edits
                    task.Parent.Add(approveReport);

                    // make the task display before the approve report task
                    task.Sequence = (short)(approveReport.Sequence.Value - 2);
                }
            }

            if (taskId == Tasks.PaymentReceived) // THIS IS CURRENTLY SPECIFIC TO HAWKESWOOD
            {
                var submitReport = db.ServiceRequestTasks.FirstOrDefault(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.SubmitReport);
                if (submitReport != null)
                { 
                    task.Parent.Add(submitReport);
                    task.Sequence = (short)(submitReport.Sequence.Value - 2);
                }
                var submitInvoice = db.ServiceRequestTasks.FirstOrDefault(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.SubmitInvoice);
                if (submitInvoice != null)
                    task.Child.Add(submitInvoice);
            }

            if (taskId == Tasks.RespondToQAComments)
            {
                var closeCase = db.ServiceRequestTasks.FirstOrDefault(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.CloseCase);
                if (closeCase != null)
                {
                    closeCase.CompletedDate = null;
                    closeCase.CompletedBy = null;
                    closeCase.ModifiedDate = SystemTime.UtcNow();
                    closeCase.ModifiedUser = User.Identity.GetGuidUserId().ToString();

                    task.Parent.Add(closeCase);
                    task.Sequence = (short)(closeCase.Sequence.Value - 2);
                }
            }

            request.ServiceRequestTasks.Add(task);

            request.UpdateIsClosed();

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ChildActionOnly]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        public ActionResult MyTaskList(Guid serviceProviderId, byte serviceRequestCategoryId)
        {
            return PartialView("MyTaskList");
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        public ActionResult TaskList(int serviceRequestId, TaskListViewModelOptions options = TaskListViewModelOptions.CriticalPath)
        {
            var viewModel = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntity.Expand())
                .ToList() // to dto -> execute the query against the database
                .AsQueryable()
                .Select(ViewModels.TaskListViewModel.FromServiceRequestDtoWithNoTasks.Expand())
                .Single(); // to view model

            // I could embed this into the projection on TaskListViewModel but for clarity I like to keep in filter login on tasks in here as it is the primary focus of this controller action.
            var query = db.ServiceRequestTasks
                .WithServiceRequestId(serviceRequestId);

            switch (options)
            {
                case TaskListViewModelOptions.MyTasks:
                    query = query.AreAssignedToUser(userId);
                    break;
                case TaskListViewModelOptions.CriticalPath:
                    query = query.AreOnCriticalPath();
                    break;
                default: // default to all tasks
                    break;
            }

            var tasks = query.Select(TaskDto.FromServiceRequestTaskEntity.Expand())
                .OrderBy(t => t.Sequence)
                .ToList()
                .AsQueryable()
                .Select(ViewModels.TaskViewModel.FromTaskDto.Expand());

            viewModel.Tasks = tasks;

            return PartialView("TaskList", viewModel);
        }

        [ChildActionOnly]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        public ActionResult NextTaskList(ServiceRequestView serviceRequest, IEnumerable<ServiceRequestTask> serviceRequestTasks)
        {
            // get the user
            var userId = User.Identity.GetGuidUserId();
            var roleId = User.Identity.GetRoleId();
            if (roleId != AspNetRoles.SuperAdmin && roleId != AspNetRoles.CaseCoordinator && !db.ServiceRequestTasks.Any(srt => srt.AssignedTo == userId && !srt.IsObsolete))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //var serviceRequest = db.ServiceRequests.Single(c => c.Id == serviceRequestId);

            var tasks = serviceRequestTasks
                .Select(t => new TaskViewModel()
                {
                    Id = t.Id,
                    ServiceRequestId = t.ServiceRequestId,
                    TaskId = t.TaskId,
                    Name = t.TaskName,
                    ShortName = t.ShortName,
                    CompletedDate = t.CompletedDate,
                    AssignedToDisplayName = t.AspNetUser_AssignedTo?.GetDisplayName(),
                    AssignedTo = t.AssignedTo,
                    AssignedToRoleId = t.ResponsibleRoleId,
                    AssignedToColorCode = t.AspNetUser_AssignedTo?.ColorCode,
                    Initials = t.AspNetUser_AssignedTo?.GetInitials(),
                    DueDateBase = t.DueDateBase,
                    DueDateDiff = t.DueDateDiff,
                    AppointmentDate = serviceRequest.AppointmentDate,
                    ReportDate = serviceRequest.DueDate,
                    DependsOn = t.DependsOn,
                    Sequence = t.Sequence
                })
                .OrderBy(t => t.Sequence)
                .ToList();

            var closeOutTask = tasks.Single(t => t.TaskId == Tasks.CloseCase);

            tasks = tasks
                .Where(t => !t.IsObsolete)
                .Select(t => {
                    if (t.DueDateBase.HasValue && t.DueDateDiff.HasValue && serviceRequest.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
                    {
                        var reportDueDate = serviceRequest.DueDate.HasValue ? serviceRequest.DueDate.Value : serviceRequest.AppointmentDate.Value.AddDays(3);
                        t.DueDate = t.DueDateBase == 1 ? serviceRequest.AppointmentDate.Value.AddDays(t.DueDateDiff.Value) : reportDueDate.AddDays(t.DueDateDiff.Value);
                    }
                    return t;
                }).ToList();

            BuildDependencies(closeOutTask, null, tasks);

            var nextTasks = tasks
                        .Where(c => c.Status.Id != TaskStatuses.Done)
                        .OrderBy(o => o.Sequence)
                        .GroupBy(g => g.AssignedTo)
                        .Select(s => new { s, Count = s.Count() })
                        .SelectMany(sm => sm.s.Select(s => s)
                                          .Zip(Enumerable.Range(1, sm.Count), (task, index)
                                              => new NextTaskViewModel() {
                                                  RowNum = index,
                                                  Id = task.Id,
                                                  ServiceRequestId = task.ServiceRequestId,
                                                  DueDate = task.DueDate,
                                                  ShortName = task.ShortName,
                                                  TaskId = task.TaskId,
                                                  Name = task.Name,
                                                  Initials = task.Initials,
                                                  AssignedToRoleId = task.AssignedToRoleId,
                                                  AssignedToColorCode = task.AssignedToColorCode,
                                                  AssignedTo = task.AssignedTo,
                                                  AssignedToDisplayName = task.AssignedToDisplayName,
                                                  StatusId = task.Status.Id,
                                                  StatusName = task.Status.Name })
                                    ).Where(t => t.RowNum == 1)
                                    .ToList();

            return PartialView("NextTaskList", nextTasks);
        }

        private TaskViewModel BuildDependencies(TaskViewModel task, TaskViewModel parent, List<TaskViewModel> tasks)
        {
            if (!string.IsNullOrEmpty(task.DependsOn) && task.DependsOn != "ExamDate")
            {
                var depends = task.DependsOn.Split(',');
                foreach (var item in depends)
                {
                    var id = int.Parse(item);
                    var depTask = tasks.SingleOrDefault(t => t.TaskId == id); // Obsolete tasks can be referenced by the DependsOn but will not be returned. Need a null check.
                    if (depTask != null)
                    {
                        depTask.Parent = parent;
                        task.Dependencies.Add(BuildDependencies(depTask, task, tasks));
                    }
                }
            }
            return task;
        }
        
        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.DeleteTask)]
        public async Task<ActionResult> Delete(int serviceRequestTaskId)
        {
            var  task = db.ServiceRequestTasks.Single(srt => srt.Id == serviceRequestTaskId);
            foreach (var item in task.Child.ToList())
            {
                task.Child.Remove(item);
            }
            foreach (var item in task.Parent.ToList())
            {
                task.Parent.Remove(item);
            }
            // Parents are removed using referential integrity at the database level.
            db.ServiceRequestTasks.Remove(task);
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
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
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<ActionResult> ToggleObsolete(int serviceRequestTaskId)
        {
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.IsObsolete = !serviceRequestTask.IsObsolete;
            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            serviceRequestTask.ServiceRequest.UpdateIsClosed();
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
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
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.PickupTask)]
        public async Task<ActionResult> PickUp(int serviceRequestTaskId)
        {
            var task = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            var userId = User.Identity.GetGuidUserId();
            task.AssignedTo = userId;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.AssignTask)]
        public async Task<ActionResult> AssignTo(int serviceRequestTaskId, Guid? assignedTo)
        {
            var task = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            var userId = User.Identity.GetGuidUserId();
            task.AssignedTo = assignedTo;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}