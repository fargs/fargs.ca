
using LinqKit;
using MoreLinq;
using NinjaNye.SearchExtensions;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Components.Grid;
using WebApp.FormModels;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Models;
using MoreLinq;
using LinqKit;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewDataModels;
using WebApp.ViewModels;
using WebApp.ViewModels.ServiceRequestTaskViewModels;
using Features = Orvosi.Shared.Enums.Features;
using NinjaNye.SearchExtensions;
using WebApp.Components.Grid;
using System.Security.Principal;
using WebApp.FormModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestTaskController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;

        public ServiceRequestTaskController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
        }
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public PartialViewResult TaskGrid(TaskListArgs args)
        {
            var dto = db.ServiceRequestTasks
                .AreAssignedToUser(loggedInUserId)
                .AreActiveOrDone()
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
                    case "duedate":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.DueDate) :
                            query.OrderByDescending(i => i.DueDate);
                        break;
                    case "claimantname":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.ClaimantName) :
                            query.OrderByDescending(i => i.ServiceRequest.ClaimantName);
                        break;
                    case "physician":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.Physician.DisplayName) :
                            query.OrderByDescending(i => i.ServiceRequest.Physician.DisplayName);
                        break;
                    case "appointmentdateandstarttime":
                        query = args.sortDir.ToLower() == "desc" ?
                            query.OrderBy(i => i.ServiceRequest.AppointmentDateAndStartTime) :
                            query.OrderByDescending(i => i.ServiceRequest.AppointmentDateAndStartTime);
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

            var taskViewModels = data
                .AsQueryable()
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

            viewModel.Columns = new List<GridColumn>
            {
                new GridColumn { Name = "Actions" },
                new GridColumn { Name = "StatusToggle" },
                new GridColumn { Name = "AssignedTo" },
                new GridColumn { DisplayName = "Status", Name = "TaskStatusName" },
                new GridColumn { DisplayName = "Due", Name = "DueDate", IsSortable = true  },
                new GridColumn { DisplayName = "Task", Name = "ShortName" },
                new GridColumn { DisplayName = "Claimant", Name = "ClaimaintName" },
                new GridColumn { DisplayName = "Exam Date", Name = "AppointmentDateAndStartTime" },
                new GridColumn { DisplayName = "File Type", Name = "ServiceName" },
                new GridColumn { DisplayName = "Company", Name = "Company" },
                new GridColumn { DisplayName = "City", Name = "CityCode" },
                new GridColumn { DisplayName = "Physician", Name = "PhysicianName" }
            };

            return PartialView("TaskGrid", viewModel);
        }

        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.ServiceRequest.ViewTaskList)]
        public PartialViewResult TaskGridRow(int serviceRequestTaskId)
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
        public PartialViewResult TaskList(TaskListArgs args)
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
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Sequence)
                .ToList();

            IEnumerable<TaskDto> query = tasksDto;
            switch (args.ViewFilter)
            {
                case TaskListViewModelFilter.CriticalPathOrAssignedToUser:
                    query = tasksDto.AreOnCriticalPathOrAssignedToUser(loggedInUserId);
                    break;
                case TaskListViewModelFilter.CriticalPathOnly:
                    query = tasksDto.AreOnCriticalPath();
                    break;
                case TaskListViewModelFilter.PrimaryRolesOnly:
                    var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };
                    query = tasksDto.AreAssignedToUserOrRoles(loggedInUserId, rolesThatShouldBeSeen);
                    break;
                case TaskListViewModelFilter.MyTasks:
                    query = tasksDto.AreAssignedToUser(loggedInUserId);
                    break;
                case TaskListViewModelFilter.MyActiveTasks:
                    query = tasksDto
                        .AreActive()
                        .AreAssignedToUser(loggedInUserId);
                    break;
                default: // default to all tasks
                    break;
            }

            if (args.DateRange != null)
            {
                query = query.AreDueBetween(args.DateRange);
            }

            // role based filters that override everything
            if (loggedInRoleId == AspNetRoles.IntakeAssistant || loggedInRoleId == AspNetRoles.DocumentReviewer)
            {
                query = query
                    .AreOnCriticalPathOrAssignedToUser(loggedInUserId)
                    .ExcludeSubmitInvoice()
                    .ToList();
            }

            viewModel.Tasks = query.AsQueryable().Select(ViewModels.TaskViewModel.FromTaskDto.Expand()).ToList();

            ViewData.TaskListArgs_Set(args);

            return PartialView(viewModel);
        }

        public JsonResult GetRelatedServiceRequestId(int serviceRequestTaskId)
        {
            var task = db.ServiceRequestTasks.Find(serviceRequestTaskId);
            return Json(new
            {
                serviceRequestId = task.ServiceRequestId,
                taskId = task.TaskId
            }, JsonRequestBehavior.AllowGet);
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
        public async Task<JsonResult> AddTask(int serviceRequestId, byte taskId)
        {
            var task = await service.AddTask(serviceRequestId, taskId);

            return Json(new
            {
                id = task.Id,
                serviceRequestId = serviceRequestId
            });
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public PartialViewResult ShowNewTaskForm(int serviceRequestId)
        {
            var data = db.ServiceRequestTasks.WithId(serviceRequestId).FirstOrDefault();
            if (data == null)
            {
                return PartialView("NotFound");
            }

            var viewModel = new TaskViewModel()
            {
                ServiceRequestId = serviceRequestId
            };

            return PartialView("NewTaskForm", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public async Task<ActionResult> CreateAsync(NewTaskForm form)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = await service.AddTask(form);

            return Json(new
            {
                serviceRequestId = task.ServiceRequestId,
                serviceRequestTaskId = task.Id
            });
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public PartialViewResult ShowEditTaskForm(int serviceRequestTaskId)
        {
            var data = db.ServiceRequestTasks.WithId(serviceRequestTaskId).FirstOrDefault();
            if (data == null)
            {
                return PartialView("NotFound");
            }

            var dto = TaskDto.FromServiceRequestTaskEntity.Invoke(data);

            var viewModel = EditTaskForm.FromTaskDto.Invoke(dto);

            return PartialView("EditTaskModalForm", viewModel);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public PartialViewResult ShowTaskTools(int serviceRequestTaskId)
        {
            var data = db.ServiceRequestTasks.WithId(serviceRequestTaskId).FirstOrDefault();
            if (data == null)
            {
                return PartialView("NotFound");
            }

            var dto = TaskDto.FromServiceRequestTaskEntity.Invoke(data);

            var viewModel = TaskViewModel.FromTaskDto.Invoke(dto);

            return PartialView("TaskTools", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageTasks)]
        public async Task<ActionResult> UpdateTaskDueDate(int serviceRequestTaskId, DateTime? dueDate)
        {
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            if (serviceRequestTask == null)
            {
                return new HttpNotFoundResult();
            }

            await service.UpdateTaskDueDate(serviceRequestTaskId, dueDate);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = serviceRequestTask.ServiceRequestId
            });
        }

        [HttpGet]
        public PartialViewResult ShowDeleteRequest(int serviceRequestTaskId)
        {
            var model = db.ServiceRequestTasks
                .WithId(serviceRequestTaskId)
                .Select(TaskDto.FromServiceRequestTaskEntityForSummary.Expand())
                .SingleOrDefault();

            var viewModel = TaskViewModel.FromTaskDto.Invoke(model);

            return PartialView("DeleteModalForm", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.DeleteTask)]
        public async Task<JsonResult> Delete(int serviceRequestTaskId)
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
        public async Task<HttpStatusCodeResult> ArchiveCompleted()
        {
            await service.ArchiveCompletedTasksForCurrentUser();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<HttpStatusCodeResult> ArchiveTask(int serviceRequestTaskId)
        {
            await service.ArchiveTask(serviceRequestTaskId);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<JsonResult> ToggleTaskStatus(int serviceRequestTaskId, short taskStatusId)
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
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
        public async Task<JsonResult> ToggleTaskCompleted([Required] int serviceRequestTaskId, [Required] string isChecked)
        {
            var taskStatusId = isChecked == "on" ? TaskStatuses.Done : TaskStatuses.ToDo;

            await service.ChangeTaskStatus(serviceRequestTaskId, taskStatusId);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.PickupTask)]
        public async Task<JsonResult> PickUp(int serviceRequestTaskId)
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
        public async Task<JsonResult> AssignTo(int serviceRequestTaskId, Guid? assignedTo)
        {
            await service.AssignTaskTo(serviceRequestTaskId, assignedTo);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.AssignTask)]
        public ActionResult ShowPickupTasksAssignedToRoleForm(PickupTasksAssignedToRoleForm form)
        {
            return PartialView("~/Views/ServiceRequestTask/_PickupTasksAssignedToRoleModalForm.cshtml", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.AssignTask)]
        public async Task<ActionResult> PickupTasksAssignedToRole(PickupTasksAssignedToRoleForm form)
        {
            await service.PickupTasksAssignedToRole(form);

            return Json(form);
        }

        [HttpGet]
        public ActionResult UrgentTaskCount()
        {
            var count = db.ServiceRequestTasks
                .AreAssignedToUser(loggedInUserId)
                .Where(srt => srt.TaskStatusId == TaskStatuses.ToDo)
                .Count();

            return PartialView("~/Views/Dashboard/_TaskHeading.cshtml", count);
        }

        public JsonResult GetAssessmentDayTasks()
        {
            var shouldBeDone = db.ServiceRequestTasks
                .Where(srt => srt.ServiceRequest.AppointmentDate.HasValue && srt.ServiceRequest.AppointmentDate <= now.Date)
                .WithTaskId(Tasks.AssessmentDay)
                .AreActive()
                .Select(srt => new
                {
                    id = srt.Id,
                    newTaskStatusId = TaskStatuses.Done
                });

            var shouldBeWaiting = db.ServiceRequestTasks
                .Where(srt => srt.ServiceRequest.AppointmentDate.HasValue && srt.ServiceRequest.AppointmentDate > now.Date)
                .WithTaskId(Tasks.AssessmentDay)
                .Where(t => t.TaskStatusId == TaskStatuses.ToDo || t.TaskStatusId == TaskStatuses.Done || t.TaskStatusId == TaskStatuses.Archive)
                .Select(srt => new
                {
                    id = srt.Id,
                    newTaskStatusId = TaskStatuses.Waiting
                });

            var result = shouldBeDone.Concat(shouldBeWaiting).OrderBy(id => id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.UpdateTaskStatus)]
        public async Task<JsonResult> UpdateAssessmentDayTaskStatuses(int serviceRequestTaskId, byte newTaskStatusId)
        {
            await service.ChangeTaskStatus(serviceRequestTaskId, newTaskStatusId);

            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            return Json(new
            {
                id = serviceRequestTaskId,
                serviceRequestId = srt.ServiceRequestId
            });
        }

        public PartialViewResult BulkUpdateDueDateForm(int serviceRequestId)
        {
            var viewModel = new BulkUpdateDueDateForm();
            viewModel.ServiceRequestId = serviceRequestId;
            var tasks = db.ServiceRequestTasks
                .WithServiceRequestId(serviceRequestId)
                .Select(TaskDto.FromServiceRequestTaskEntityAndTemplate.Expand())
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Sequence)
                .ToList();


            //TODO: set the NewDueDate property on each task
            var serviceRequest = db.ServiceRequests.WithId(serviceRequestId).First();

            foreach (var task in tasks)
            {
                var bulkUpdateViewModel = BulkUpdateDueDateViewModel.FromTaskDto.Invoke(task);

                if (task.TaskTemplateId.HasValue)
                {
                    bulkUpdateViewModel.NewDueDate = GetTaskDueDate(serviceRequest.AppointmentDate, serviceRequest.DueDate, task.TaskTemplate);
                }
                else
                {
                    bulkUpdateViewModel.NewDueDate = task.DueDate;
                }
                
                viewModel.Tasks.Add(bulkUpdateViewModel);
            }

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult BulkUpdateDueDates()
        {
            // the form is submitted with 2 arrays of values that need to be attached together. Zip is used for this.
            var form = this.Request.Form[0].Split(',').Zip(this.Request.Form[1].Split(','), (first, second) =>
            {
                DateTime d;
                if (DateTime.TryParse(second, out d))
                {
                    return new BulkUpdateDueDateFormModel()
                    {
                        ServiceRequestTaskId = int.Parse(first),
                        NewDueDate = d
                    };
                }
                else
                {
                    return new BulkUpdateDueDateFormModel()
                    {
                        ServiceRequestTaskId = int.Parse(first),
                        NewDueDate = null
                    };
                }
            });

            var ids = form.Select(f => f.ServiceRequestTaskId);
            var tasks = db.ServiceRequestTasks.Where(srt => ids.Contains(srt.Id));
            foreach (var task in tasks)
            {
                var d = form.First(f => f.ServiceRequestTaskId == task.Id);
                task.DueDate = d.NewDueDate;
            }
            db.SaveChanges();

            int serviceRequestId = tasks.First().ServiceRequestId;
            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }

        private DateTime? GetTaskDueDate(DateTime? appointmentDate, DateTime? reportDueDate, ServiceRequestTemplateTaskDto taskTemplate)
        {
            DateTime? taskDueDate;
            if (appointmentDate.HasValue && taskTemplate.TaskId == Tasks.AssessmentDay) // assessment day task is set to the appointment date
            {
                taskDueDate = appointmentDate.Value;
            }
            else if (reportDueDate.HasValue && taskTemplate.TaskId == Tasks.SubmitReport) // submit report task is set to the report due date
            {
                taskDueDate = reportDueDate.Value;
            }
            else // for all other tasks, we calculate the due date accordingly
            {
                if (!taskTemplate.DueDateDurationFromBaseline.HasValue) // If there is no duration, return null (ASAP) NOTE: 0 must be set explicitly to have it match the baseline date.
                {
                    taskDueDate = null;
                }
                else
                {
                    if (taskTemplate.DueDateTypeTrimmed == DueDateTypes.AppointmentDate)
                    {
                        taskDueDate = appointmentDate.Value.AddDays(taskTemplate.DueDateDurationFromBaseline.Value);
                    }
                    else if (taskTemplate.DueDateTypeTrimmed == DueDateTypes.ReportDueDate)
                    {
                        taskDueDate = reportDueDate.Value.AddDays(taskTemplate.DueDateDurationFromBaseline.Value);
                        if (appointmentDate.HasValue && taskDueDate < appointmentDate)
                        {
                            taskDueDate = appointmentDate;
                        }
                    }
                    else
                    {
                        taskDueDate = null;
                    }
                }
            }

            return taskDueDate;
        }

    }

}