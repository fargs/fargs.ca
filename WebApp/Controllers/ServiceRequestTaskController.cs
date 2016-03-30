using Model;
using Model.Enums;
using WebApp.ViewModels.ServiceRequestTaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebApp.Library;
using MoreLinq;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceRequestTaskController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();
        // GET: ServiceRequestTasks
        public ActionResult Index(FilterArgs filterArgs)
        {
            // get the user
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            var sr = db.ServiceRequestTasks.Where(srt => !srt.IsObsolete).AsQueryable<ServiceRequestTask>();

            if (user.RoleId == Roles.SuperAdmin && filterArgs.ShowAll.Value) { }
            else
            {
                sr.Where(c => c.AssignedTo == user.Id);
            }

            var vm = new IndexViewModel()
            {
                User = user,
                Tasks = sr.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddRespondToQACommentsTask(int serviceRequestId)
        {
            var st = db.ServiceTasks.Single(t => t.Id == Tasks.RespondToQAComments);
            var task = new ServiceRequestTask();
            task.ServiceRequestId = serviceRequestId;
            task.TaskId = Tasks.RespondToQAComments;
            task.TaskName = st.TaskName;
            task.TaskPhaseId = st.TaskPhaseId;
            task.TaskPhaseName = st.TaskPhaseName;
            task.ResponsibleRoleId = st.ResponsibleRoleId;
            task.ResponsibleRoleName = st.ResponsibleRoleName;
            task.Sequence = st.Sequence;
            task.AssignedTo = db.ServiceRequestTasks.First(sr => sr.ResponsibleRoleId == st.ResponsibleRoleId).AssignedTo;
            task.IsBillable = st.IsBillable.Value;
            task.HourlyRate = st.HourlyRate;
            task.EstimatedHours = st.EstimatedHours;
            task.DependsOn = st.DependsOn;
            task.DueDateBase = st.DueDateBase;
            task.DueDateDiff = st.DueDateDiff;
            task.Guidance = st.Guidance;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = User.Identity.Name;

            db.ServiceRequestTasks.Add(task);

            var obtainFinalReportCompanyTask = db.ServiceRequestTasks.Single(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.ObtainFinalReportCompany);
            obtainFinalReportCompanyTask.DependsOn = Tasks.RespondToQAComments.ToString();
            db.SaveChanges();
            
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult TaskList(int? serviceRequestId, bool hideCaseCoordinator = false, bool useShortName = false, bool myTasksOnly = true, bool collapsed = false)
        {
            ViewBag.HideCaseCoordinator = hideCaseCoordinator;
            ViewBag.UseShortName = useShortName;
            ViewBag.MyTasksOnly = myTasksOnly;
            ViewBag.Collapsed = collapsed;

            // get the user
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            if (user.RoleId != Roles.SuperAdmin && user.RoleId != Roles.CaseCoordinator && !db.ServiceRequestTasks.Any(srt => srt.AssignedTo == user.Id && !srt.IsObsolete))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var serviceRequest = db.ServiceRequests.Single(c => c.Id == serviceRequestId);

            var dueDate = serviceRequest.DueDate.HasValue ? serviceRequest.DueDate.Value : serviceRequest.AppointmentDate.Value.AddDays(5);

            var tasks = db.ServiceRequestTasks.Where(t => t.ServiceRequestId == serviceRequestId && !t.IsObsolete)
                .Select(t => new TaskViewModel()
                {
                    Id = t.Id,
                    ServiceRequestId = t.ServiceRequestId,
                    TaskId = t.TaskId,
                    Name = t.TaskName,
                    ShortName = t.ShortName,
                    CompletedDate = t.CompletedDate,
                    AssignedToDisplayName = t.AssignedToDisplayName,
                    AssignedTo = t.AssignedTo,
                    AssignedToRoleId = t.ResponsibleRoleId,
                    AssignedToColorCode = t.AssignedToColorCode,
                    Initials = t.AssignedToInitials,
                    DueDateBase = t.DueDateBase,
                    DueDateDiff = t.DueDateDiff,
                    ExamDate = serviceRequest.AppointmentDate.Value,
                    ReportDate = dueDate,
                    DependsOn = t.DependsOn,
                    Sequence = t.Sequence
                })
                .OrderBy(t => t.Sequence)
                .ToList();

            var closeOutTask = tasks.Single(t => t.TaskId == Tasks.CloseCase);

            BuildDependencies(closeOutTask, null, tasks);

            if (myTasksOnly)
            {
                tasks = tasks.Where(t => (t.AssignedTo ?? string.Empty).ToLower() == user.Id.ToLower()).ToList();
            }

            //var tasks = db.ServiceRequestTasks.Where(srt => srt.ServiceRequestId == serviceRequestId)
            //    .GroupBy(srt => new
            //    {
            //        srt.AssignedToDisplayName,
            //        srt.AssignedTo,
            //        srt.TaskId,
            //        srt.TaskName
            //    })
            //    .Select(c => new
            //    {
            //        c.Key.AssignedToDisplayName,
            //        c.Key.AssignedTo,
            //        c.Key.TaskId,
            //        c.Key.TaskName,
            //        CompletedTaskCount = c.Count(t => t.CompletedDate == null),
            //        ActiveTaskCount = c.Count(t => t.CompletedDate != null)
            //    });

            return PartialView("TaskList", tasks);
        }

        public ActionResult NextTaskList(int? serviceRequestId)
        {
            // get the user
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            if (user.RoleId != Roles.SuperAdmin && user.RoleId != Roles.CaseCoordinator && !db.ServiceRequestTasks.Any(srt => srt.AssignedTo == user.Id && !srt.IsObsolete))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.User = user;

            var serviceRequest = db.ServiceRequests.Single(c => c.Id == serviceRequestId);

            var dueDate = serviceRequest.DueDate.HasValue ? serviceRequest.DueDate.Value : serviceRequest.AppointmentDate.Value.AddDays(5);

            var tasks = db.ServiceRequestTasks.Where(t => t.ServiceRequestId == serviceRequestId && !t.IsObsolete)
                .Select(t => new TaskViewModel()
                {
                    Id = t.Id,
                    ServiceRequestId = t.ServiceRequestId,
                    TaskId = t.TaskId,
                    Name = t.TaskName,
                    ShortName = t.ShortName,
                    CompletedDate = t.CompletedDate,
                    AssignedToDisplayName = t.AssignedToDisplayName,
                    AssignedTo = t.AssignedTo,
                    AssignedToRoleId = t.ResponsibleRoleId,
                    AssignedToColorCode = t.AssignedToColorCode,
                    Initials = t.AssignedToInitials,
                    DueDateBase = t.DueDateBase,
                    DueDateDiff = t.DueDateDiff,
                    ExamDate = serviceRequest.AppointmentDate.Value,
                    ReportDate = dueDate,
                    DependsOn = t.DependsOn,
                    Sequence = t.Sequence
                })
                .OrderBy(t => t.Sequence)
                .ToList();

            tasks.Select(t => {
                if (t.DueDateBase.HasValue && t.DueDateDiff.HasValue)
                {
                    var reportDueDate = serviceRequest.DueDate.HasValue ? serviceRequest.DueDate.Value : serviceRequest.AppointmentDate.Value.AddDays(7);
                    t.DueDate = t.DueDateBase == 1 ? serviceRequest.AppointmentDate.Value.AddDays(t.DueDateDiff.Value) : reportDueDate.AddDays(t.DueDateDiff.Value);
                }
                return t;
            }).ToList();

            var closeOutTask = tasks.Single(t => t.TaskId == Tasks.CloseCase);

            BuildDependencies(closeOutTask, null, tasks);

            var nextTasks = tasks
                        .Where(c => c.Status.Id != WebApp.Library.Enums.TaskStatuses.Completed)
                        .OrderBy(o => o.DueDate)
                        .GroupBy(g => (string.IsNullOrEmpty(g.AssignedTo) ? string.Empty : g.AssignedTo.ToLower()))
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

            //var tasks = db.ServiceRequestTasks.Where(srt => srt.ServiceRequestId == serviceRequestId)
            //    .GroupBy(srt => new
            //    {
            //        srt.AssignedToDisplayName,
            //        srt.AssignedTo,
            //        srt.TaskId,
            //        srt.TaskName
            //    })
            //    .Select(c => new
            //    {
            //        c.Key.AssignedToDisplayName,
            //        c.Key.AssignedTo,
            //        c.Key.TaskId,
            //        c.Key.TaskName,
            //        CompletedTaskCount = c.Count(t => t.CompletedDate == null),
            //        ActiveTaskCount = c.Count(t => t.CompletedDate != null)
            //    });

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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAsComplete(int id)
        {
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(id);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = SystemTime.Now();
            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAsIncomplete(int id)
        {
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(id);
            if (serviceRequestTask == null)
            {
                return HttpNotFound();
            }
            serviceRequestTask.CompletedDate = null;
            serviceRequestTask.ModifiedDate = SystemTime.Now();
            serviceRequestTask.ModifiedUser = User.Identity.Name;
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