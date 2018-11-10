using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Data;
using System.Security.Principal;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApp.FormModels;
using MoreLinq;
using Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.ViewModels;
using Orvosi.Data.Filters;
using FluentValidation;
using WebApp.Models;
using LinqKit;
using WebApp.Views.Comment;

namespace WebApp.Library
{
    public class WorkService : IDisposable
    {
        IOrvosiDbContext db;
        IIdentity identity;
        DateTime now;
        Guid userId;
        Guid physicianId;
        UserContextViewModel userContext;

        public WorkService(IOrvosiDbContext db, IPrincipal principal)
        {
            this.db = db;
            this.identity = principal.Identity;
            userId = identity.GetGuidUserId();
            userContext = identity.GetUserContext();
            physicianId = userContext.Id;
            this.now = SystemTime.Now();
        }


        public async Task<int> BookAssessment(BookingForm form)
        {
            var sr = new ServiceRequest();
            var slot = await db.AvailableSlots.FindAsync(form.AvailableSlotId);
            sr.ServiceRequestStatusId = ServiceRequestStatuses.Active;
            sr.ServiceRequestStatusChangedBy = userId;
            sr.ServiceRequestStatusChangedDate = now;
            sr.ServiceRequestTemplateId = form.ServiceRequestTemplateId;
            sr.RequestedDate = now;
            sr.ServiceId = form.ServiceId;
            sr.PhysicianId = physicianId;
            sr.CompanyId = form.CompanyId;
            sr.AddressId = form.AddressId;
            sr.AppointmentDate = form.AppointmentDate;
            sr.AvailableSlotId = (short)form.AvailableSlotId;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.DueDate = form.DueDate;
            sr.CompanyReferenceId = form.CompanyReferenceId;
            sr.ClaimantName = form.ClaimantName;
            sr.SourceCompany = form.SourceCompany;
            sr.ModifiedUser = userId.ToString();
            sr.ModifiedDate = now;
            sr.CreatedUser = userId.ToString();
            sr.CreatedDate = now;
            
            // clone the workflow template tasks
            var requestTemplate = await db.ServiceRequestTemplates.FindAsync(sr.ServiceRequestTemplateId);
            foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted().Select(ServiceRequestTemplateTaskDto.FromEntity.Expand()))
            {
                var st = new Orvosi.Data.ServiceRequestTask();
                st.Guidance = null;
                st.ObjectGuid = Guid.NewGuid();
                st.ResponsibleRoleId = template.ResponsibleRoleId;
                st.Sequence = template.Sequence;
                st.ShortName = template.ShortName;
                st.TaskId = template.TaskId;
                st.TaskName = template.TaskName;
                st.ModifiedDate = now;
                st.ModifiedUser = userId.ToString();
                st.CreatedDate = now;
                st.CreatedUser = userId.ToString();
                // Assign tasks to physician and case coordinator to start
                st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
                st.ServiceRequestTemplateTaskId = template.Id;
                st.TaskType = template.DueDateType;
                st.Workload = null;
                st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
                st.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, template);
                st.TaskStatusId = TaskStatuses.ToDo;
                st.TaskStatusChangedBy = userId;
                st.TaskStatusChangedDate = now;
                st.IsCriticalPath = template.IsCriticalPath;
                st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
                st.EffectiveDate = GetEffectiveDate(sr.AppointmentDate, template);

                sr.ServiceRequestTasks.Add(st);
            }

            sr.UpdateIsClosed();
            db.ServiceRequests.Add(sr);

            await db.SaveChangesAsync();

            // Clone the task dependencies
            foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
            {
                foreach (var dependentTemplate in taskTemplate.Child)
                {
                    var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
                    var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
                    task.Child.Add(dependent);
                }
            }

            await db.SaveChangesAsync();

            await UpdateDependentTaskStatuses(sr.Id);
            await UpdateServiceRequestStatus(sr.Id);

            return sr.Id; 
        }

        public async Task UpdateServiceRequestStatus(int serviceRequestId)
        {
            var sr = await db.ServiceRequests.FindAsync(serviceRequestId);

            if (sr == null) throw new ArgumentNullException($"Service Request with Id {serviceRequestId} not found.");

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);

            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task ChangeServiceRequestStatus(int serviceRequestId, short newServiceRequestStatusId)
        {
            var sr = await db.ServiceRequests.FindAsync(serviceRequestId);

            if (sr == null) throw new ArgumentNullException($"Service Request with Id {serviceRequestId} not found.");

            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task CancelRequest(CancellationForm form)
        {
            var sr = await db.ServiceRequests.FindAsync(form.ServiceRequestId);

            sr.CancelledDate = form.CancelledDate;
            sr.IsLateCancellation = form.IsLate == "on" ? true : false;

            var query = sr.ServiceRequestTasks
                .AsQueryable()
                .AreActive()
                .Where(srt => srt.TaskId != Tasks.CloseCase);

            if (sr.IsLateCancellation)
            {
                query = query.Where(srt => srt.TaskId != Tasks.SubmitInvoice);
            }
            foreach (var task in query)
            {
                await SaveTaskStatusChange(task, TaskStatuses.Obsolete);
            }
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task CancelRequestUndo(int serviceRequestId)
        {
            var sr = await db.ServiceRequests.FindAsync(serviceRequestId);

            sr.CancelledDate = null;
            sr.IsLateCancellation = false;

            var query = sr.ServiceRequestTasks
                .AsQueryable()
                .AreCancelled();

            if (sr.IsLateCancellation)
            {
                query = query.Where(srt => srt.TaskId != Tasks.SubmitInvoice);
            }
            foreach (var task in query)
            {
                await SaveTaskStatusChange(task, TaskStatuses.ToDo);
            }
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task DeleteRequest(ServiceRequest sr)
        {
            sr.IsDeleted = true;
            sr.ModifiedDate = now;
            sr.ModifiedUser = userId.ToString();
            await db.SaveChangesAsync();
        }
        public async Task NoShow(NoShowForm form)
        {
            var sr = await db.ServiceRequests.FindAsync(form.ServiceRequestId);

            sr.IsNoShow = form.IsNoShow;

            byte newTaskStatus = form.IsNoShow ? TaskStatuses.Obsolete : TaskStatuses.ToDo;

            IEnumerable<ServiceRequestTask> tasks = null;
            if (form.IsNoShow)
            {
                tasks = sr.ServiceRequestTasks
                    .AsQueryable()
                    .AreActive()
                    .Where(srt => srt.TaskId != Tasks.SubmitInvoice && srt.TaskId != Tasks.CloseCase);
            }
            else
            {
                tasks = sr.ServiceRequestTasks
                    .AsQueryable()
                    .AreCancelled();
            }

            foreach (var task in tasks)
            {
                await SaveTaskStatusChange(task, newTaskStatus);
            }
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task OnHold(OnHoldForm form)
        {
            var sr = await db.ServiceRequests.FindAsync(form.ServiceRequestId);

            sr.IsOnHold = form.IsOnHold;

            byte newTaskStatus = form.IsOnHold ? TaskStatuses.OnHold : TaskStatuses.ToDo;

            IEnumerable<ServiceRequestTask> tasks = null;
            if (form.IsOnHold)
            {
                tasks = sr.ServiceRequestTasks
                    .AsQueryable()
                    .AreActive();
            }
            else
            {
                tasks = sr.ServiceRequestTasks
                    .AsQueryable()
                    .AreOnHold();
            }

            foreach (var task in tasks)
            {
                await SaveTaskStatusChange(task, newTaskStatus);
            }
            await SaveDependentTaskStatusChanges(sr);

            // update the request status
            var newServiceRequestStatusId = form.IsOnHold ? ServiceRequestStatuses.OnHold : CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, ServiceRequestStatuses.Active);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task Reschedule(RescheduleForm form)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == form.ServiceRequestId);
            // cache the service requests to ensure validation results are cleared properly
            var originalSlot = sr.AvailableSlot;
            var originalRelatedRequests = originalSlot.ServiceRequests;
            originalSlot.ServiceRequests.Remove(sr);

            var newSlot = db.AvailableSlots.Single(c => c.Id == form.AvailableSlotId);

            newSlot.ServiceRequests.Add(sr);
            sr.AvailableSlot = newSlot;
            sr.AppointmentDate = newSlot.AvailableDay.Day;
            sr.StartTime = newSlot.StartTime;
            sr.EndTime = newSlot.EndTime;

            sr.ModifiedDate = now;
            sr.ModifiedUser = identity.Name;

            // validate the modified request
            var validator = new ServiceRequestValidator();
            foreach (var item in originalSlot.ServiceRequests)
            {
                var results = validator.Validate(item);
                item.HasErrors = results.Errors.Any(error => error.CustomState.ToString() == ValidationTypeEnum.Error.ToString());
                item.HasWarnings = results.Errors.Any(error => error.CustomState.ToString() == ValidationTypeEnum.Warning.ToString());
            }
            foreach (var item in newSlot.ServiceRequests)
            {
                var results = validator.Validate(item);
                item.HasErrors = results.Errors.Any(error => error.CustomState.ToString() == ValidationTypeEnum.Error.ToString());
                item.HasWarnings = results.Errors.Any(error => error.CustomState.ToString() == ValidationTypeEnum.Warning.ToString());
            }

            var task = db.ServiceRequestTasks.Single(srt => srt.Id == form.ServiceRequestTaskId);
            task.DueDate = form.AppointmentDate;

            await db.SaveChangesAsync();
        }
        public async Task ChangeCompany(ChangeCompanyForm form)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == form.ServiceRequestId);

            sr.CompanyId = form.CompanyId;
            sr.SourceCompany = form.SourceCompany;

            sr.ModifiedDate = now;
            sr.ModifiedUser = identity.Name;

            await db.SaveChangesAsync();
        }
        public async Task ChangeService(ChangeServiceForm form)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == form.ServiceRequestId);

            sr.ServiceId = form.ServiceId;
            sr.MedicolegalTypeId = form.MedicolegalTypeId;

            sr.ModifiedDate = now;
            sr.ModifiedUser = identity.Name;

            await db.SaveChangesAsync();
        }
        public async Task ChangeAddress(ChangeAddressForm form)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == form.ServiceRequestId);

            sr.AddressId = form.AddressId;

            sr.ModifiedDate = now;
            sr.ModifiedUser = identity.Name;

            await db.SaveChangesAsync();
        }
        public async Task ChangeClaimant(ChangeClaimantForm form)
        {
            var sr = db.ServiceRequests.Single(c => c.Id == form.ServiceRequestId);

            sr.ClaimantName = form.ClaimantName;

            sr.ModifiedDate = now;
            sr.ModifiedUser = identity.Name;

            await db.SaveChangesAsync();
        }

        public async Task<ServiceRequestTask> AddTask(NewTaskForm form)
        {
            var task = db.OTasks.SingleOrDefault(t => t.Name == form.TaskName);
            if (task == null)
            {
                task = db.OTasks.SingleOrDefault(t => t.Id == Tasks.General);
            }

            var newTask = new ServiceRequestTask()
            {
                ServiceRequestId = form.ServiceRequestId,
                DueDate = form.DueDate,
                AssignedTo = form.AssignedTo,
                TaskName = form.TaskName,
                IsCriticalPath = task.IsCriticalPath,
                ModifiedDate = now,
                ModifiedUser = userId.ToString(),
                TaskStatusId = TaskStatuses.ToDo,
                TaskStatusChangedBy = userId,
                TaskStatusChangedDate = now,
                CreatedDate = now,
                CreatedUser = userId.ToString(),
                TaskId = task.Id,
                ShortName = task.ShortName,
                ResponsibleRoleId = task.ResponsibleRoleId,
                ResponsibleRoleName = task.AspNetRole.Name,
                TaskPhaseId = task.TaskPhaseId,
                TaskPhaseName = task.TaskPhase.Name,
                Sequence = task.Sequence,
                IsBillable = task.IsBillable.Value,
                HourlyRate = task.HourlyRate,
                EstimatedHours = task.EstimatedHours,
                DependsOn = task.DependsOn,
                Guidance = task.Guidance
            };

            var request = db.ServiceRequests.Single(sr => sr.Id == newTask.ServiceRequestId);
            request.ServiceRequestTasks.Add(newTask);

            await db.SaveChangesAsync();

            await SaveDependentTaskStatusChanges(request);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(request.ServiceRequestTasks, request.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(request, newServiceRequestStatusId);

            await (db as OrvosiDbContext).Entry(task).ReloadAsync();

            return newTask;
        }

        public async Task<ServiceRequestTask> AddTask(int serviceRequestId, byte taskId)
        {
            var request = await db.ServiceRequests.FindAsync(serviceRequestId);

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
            task.AssignedTo = (await db.ServiceRequestTasks.FirstOrDefaultAsync(sr => sr.ServiceRequestId == serviceRequestId && sr.ResponsibleRoleId == st.ResponsibleRoleId)).AssignedTo;
            task.IsBillable = st.IsBillable.Value;
            task.HourlyRate = st.HourlyRate;
            task.EstimatedHours = st.EstimatedHours;
            task.DependsOn = st.DependsOn;
            task.Guidance = st.Guidance;
            task.IsCriticalPath = true;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = identity.Name;
            task.TaskStatusId = TaskStatuses.ToDo;
            task.TaskStatusChangedBy = identity.GetGuidUserId();
            task.TaskStatusChangedDate = now;
            task.CreatedDate = now;
            task.CreatedUser = userId.ToString();

            // this is currently hard coded but should be made editable by the user adding the task
            if (taskId == Tasks.AdditionalEdits)
            {
                var approveReport = await db.ServiceRequestTasks.FirstOrDefaultAsync(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.ApproveReport);
                if (approveReport != null)
                {
                    approveReport.CompletedDate = null;
                    approveReport.CompletedBy = null;
                    approveReport.ModifiedDate = now;
                    approveReport.ModifiedUser = userId.ToString();

                    // make this task dependent on additional edits
                    task.Parent.Add(approveReport);

                    // make the task display before the approve report task
                    task.Sequence = (short)(approveReport.Sequence.Value - 2);
                }
            }

            if (taskId == Tasks.PaymentReceived) // THIS IS CURRENTLY SPECIFIC TO HAWKESWOOD
            {
                var submitReport = await db.ServiceRequestTasks.FirstOrDefaultAsync(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.SubmitReport);
                if (submitReport != null)
                {
                    task.Parent.Add(submitReport);
                    task.Sequence = (short)(submitReport.Sequence.Value - 2);
                }
                var submitInvoice = await db.ServiceRequestTasks.FirstOrDefaultAsync(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.SubmitInvoice);
                if (submitInvoice != null)
                    task.Child.Add(submitInvoice);
            }

            if (taskId == Tasks.RespondToQAComments)
            {
                var closeCase = await db.ServiceRequestTasks.FirstOrDefaultAsync(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.CloseCase);
                if (closeCase != null)
                {
                    closeCase.CompletedDate = null;
                    closeCase.CompletedBy = null;
                    closeCase.ModifiedDate = now;
                    closeCase.ModifiedUser = userId.ToString();

                    task.Parent.Add(closeCase);
                    task.Sequence = (short)(closeCase.Sequence.Value - 2);
                }
            }

            request.ServiceRequestTasks.Add(task);

            await db.SaveChangesAsync();

            await SaveDependentTaskStatusChanges(request);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(request.ServiceRequestTasks, request.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(request, newServiceRequestStatusId);

            await (db as OrvosiDbContext).Entry(task).ReloadAsync();

            return task;
        }
        public async Task PickUpTask(int serviceRequestTaskId)
        {
            var task = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            task.AssignedTo = userId;
            task.ModifiedDate = now;
            task.ModifiedUser = userId.ToString();
            await db.SaveChangesAsync();
        }
        public async Task AssignTaskTo(int serviceRequestTaskId, Guid? assignedTo)
        {
            var task = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            task.AssignedTo = assignedTo;
            task.ModifiedDate = now;
            task.ModifiedUser = userId.ToString();
            await db.SaveChangesAsync();
        }
        public async Task PickupTasksAssignedToRole(PickupTasksAssignedToRoleForm form)
        {
            var sr = await db.ServiceRequests.Include(c => c.ServiceRequestTasks).SingleAsync(c => c.Id == form.ServiceRequestId);
            
            foreach (var task in sr.ServiceRequestTasks.Where(t => t.ResponsibleRoleId == form.RoleId))
            {
                task.AssignedTo = form.UserId;
                task.ModifiedDate = now;
                task.ModifiedUser = userId.ToString();
            }

            await db.SaveChangesAsync();
        }

        public async Task ChangeTaskStatus(int serviceRequestTaskId, short newTaskStatusId)
        {
            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            if (srt == null) throw new ArgumentNullException($"Task with Id {serviceRequestTaskId} not found.");

            await SaveTaskStatusChange(srt, newTaskStatusId);

            var sr = srt.ServiceRequest;
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task DeleteTask(int serviceRequestTaskId)
        {
            var srt = db.ServiceRequestTasks.Single(t => t.Id == serviceRequestTaskId);

            foreach (var item in srt.Child.AsQueryable().ToList())
            {
                srt.Child.Remove(item);
            }
            foreach (var item in srt.Parent.ToList())
            {
                srt.Parent.Remove(item);
            }
            // Parents are removed using referential integrity at the database level.
            db.ServiceRequestTasks.Remove(srt);

            await db.SaveChangesAsync();

            var sr = db.ServiceRequests.Single(s => s.Id == srt.ServiceRequestId);
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task ToggleTaskStatus(int serviceRequestTaskId, short currentTaskStatusId)
        {
            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            if (srt == null) throw new ArgumentNullException($"Task with Id {serviceRequestTaskId} not found.");

            var newTaskStatusId = srt.TaskStatusId == currentTaskStatusId ? TaskStatuses.ToDo : currentTaskStatusId;

            if (srt.TaskStatusId == TaskStatuses.Archive)
            {
                newTaskStatusId = TaskStatuses.Done;
            }

            await SaveTaskStatusChange(srt, newTaskStatusId);

            var sr = srt.ServiceRequest;
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }
        public async Task UpdateTaskDueDate(int serviceRequestTaskId, DateTime? dueDate)
        {
            var data = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            data.DueDate = dueDate;
            data.ModifiedDate = now;
            data.ModifiedUser = userId.ToString();

            if (data.TaskId == Tasks.SubmitReport)
            {
                data.ServiceRequest.DueDate = dueDate;
            }
            else if (data.TaskId == Tasks.AssessmentDay)
            {
                data.ServiceRequest.AppointmentDate = dueDate;
            }

            await db.SaveChangesAsync();
        }
        public async Task UpdateTaskDependencies(EditTaskDependenciesForm form)
        {
            var serviceRequestTask = await db.ServiceRequestTasks.FindAsync(form.ServiceRequestTaskId);
            serviceRequestTask.ModifiedDate = now;
            serviceRequestTask.ModifiedUser = userId.ToString();

            serviceRequestTask.Child.Clear();
            foreach (var dependent in form.Dependencies)
            {
                var child = db.ServiceRequestTasks.Find(dependent);
                serviceRequestTask.Child.Add(child);
            }
            await db.SaveChangesAsync();

            var sr = serviceRequestTask.ServiceRequest;
            await SaveDependentTaskStatusChanges(sr);
        }

        public async Task ArchiveTask(int serviceRequestTaskId)
        {
            var srt = await db.ServiceRequestTasks
                .WithId(serviceRequestTaskId)
                .SingleAsync();

            ChangeTaskStatus(srt, TaskStatuses.Archive);

            await db.SaveChangesAsync();
        }
        public async Task ArchiveCompletedTasksForCurrentUser()
        {
            var tasks = db.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .WithTaskStatus(TaskStatuses.Done);

            foreach (var srt in tasks)
            {
                ChangeTaskStatus(srt, TaskStatuses.Archive);
            }

            await db.SaveChangesAsync();
        }

        public async Task UpdateAssessmentDayTaskStatus(int serviceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                throw new ArgumentNullException($"Service request with Id {serviceRequestId} not found.");
            }
            await SaveDependentTaskStatusChanges(serviceRequest);
        }
        public async Task UpdateDependentTaskStatuses(int serviceRequestId)
        {
            //TODO: insert to a log table
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                throw new ArgumentNullException($"Service request with Id {serviceRequestId} not found.");
            }

            await SaveDependentTaskStatusChanges(serviceRequest);
        }


        private ServiceRequestTask ChangeTaskStatus(ServiceRequestTask srt, short newTaskStatusId)
        {
            srt.TaskStatusId = newTaskStatusId;
            srt.TaskStatusChangedBy = userId;
            srt.TaskStatusChangedDate = now;
            srt.ModifiedDate = now;
            srt.ModifiedUser = userId.ToString();

            if (srt.TaskStatusId == TaskStatuses.Done)
            {
                srt.CompletedBy = userId;
                srt.CompletedDate = now;
            }
            return srt;
        }
        private async Task SaveTaskStatusChange(ServiceRequestTask srt, short newTaskStatusId)
        {
            var task = ChangeTaskStatus(srt, newTaskStatusId);
            await db.SaveChangesAsync();
        }
        private async Task SaveDependentTaskStatusChanges(ServiceRequest serviceRequest)
        {
            foreach (var srt in serviceRequest.ServiceRequestTasks.OrderBy(srt => srt.DueDate).ThenBy(srt => srt.Sequence))
            {
                if (srt.TaskId != Tasks.AssessmentDay)
                {
                    var isWaiting = srt.Child.AsQueryable().AreActive().Any();

                    if (srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting)
                    {
                        srt.TaskStatusId = isWaiting ? TaskStatuses.Waiting : TaskStatuses.ToDo;
                    }
                }
                else if (srt.TaskId == Tasks.AssessmentDay && serviceRequest.AppointmentDate.HasValue && (srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting))
                {
                    var appointmentDate = serviceRequest.AppointmentDate.Value;
                    if (appointmentDate >= now)
                    {
                        srt.TaskStatusId = TaskStatuses.Waiting;
                    }
                    else
                    {
                        srt.TaskStatusId = TaskStatuses.Done;
                    }
                }
            }
            await db.SaveChangesAsync();
        }
        private async Task SaveServiceRequestStatusChange(ServiceRequest sr, short newServiceRequestStatusId)
        {
            sr.ServiceRequestStatusId = newServiceRequestStatusId;
            sr.ServiceRequestStatusChangedBy = userId;
            sr.ServiceRequestStatusChangedDate = now;
            sr.ModifiedDate = now;
            sr.ModifiedUser = userId.ToString();

            await db.SaveChangesAsync();
        }

        private short CalculateNewServiceRequestStatus(IEnumerable<ServiceRequestTask> tasks, short currentStatus)
        {
            //TODO: insert to a log table
            if (tasks.AsQueryable().AreActive().Any())
            {
                // return on hold if it was explicitely set, otherwise return active
                return currentStatus == ServiceRequestStatuses.OnHold ? ServiceRequestStatuses.OnHold : ServiceRequestStatuses.Active;
            }
            else
            {
                return ServiceRequestStatuses.Closed;
            }
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
        private DateTime GetEffectiveDate(DateTime? baselineDate, ServiceRequestTemplateTaskDto taskTemplate)
        {
            if (!baselineDate.HasValue)
            {
                throw new Exception("Baseline Date is required to calculate Due Dates and Effective Dates.");
            }

            if (taskTemplate.IsBaselineDate) // BASELINE
            {
                return baselineDate.Value;
            }
            else if (taskTemplate.EffectiveDateDurationFromBaseline.HasValue) // HAS A DURATION FROM BASELINE
            {
                return baselineDate.Value.AddDays(taskTemplate.EffectiveDateDurationFromBaseline.Value);
            }
            else // ASAP
            {
                return now;
            }
        }

        private DateTime? GetTaskDueDate(string dueDateType, DateTime? appointmentDate, DateTime? dueDate)
        {
            switch (dueDateType)
            {
                case DueDateTypes.AppointmentDate:
                    return appointmentDate;
                case DueDateTypes.ReportDueDate:
                    return dueDate;
                default:
                    return null;
            }
        }
        private Guid? GetTaskAssignment(Guid? responsibleRoleId, Guid physicianId, Guid? caseCoordinatorId, Guid? intakeAssistantId, Guid? documentReviewerId)
        {
            if (responsibleRoleId == AspNetRoles.Physician)
            {
                return physicianId;
            }
            else if (responsibleRoleId == AspNetRoles.CaseCoordinator)
            {
                return caseCoordinatorId;
            }
            else if (responsibleRoleId == AspNetRoles.IntakeAssistant)
            {
                return intakeAssistantId;
            }
            else if (responsibleRoleId == AspNetRoles.DocumentReviewer)
            {
                return documentReviewerId;
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<Guid> CreateAdditionalResource(AdditionalResourceForm form)
        {
            var resource = new ServiceRequestResource
            {
                Id = Guid.NewGuid(),
                ServiceRequestId = form.ServiceRequestId,
                UserId = form.UserId,
                CreatedDate = now,
                CreatedUser = identity.GetGuidUserId().ToString(),
                ModifiedDate = now,
                ModifiedUser = identity.GetGuidUserId().ToString()
            };
            db.ServiceRequestResources.Add(resource);

            await db.SaveChangesAsync();

            return resource.Id;
        }

        public async Task<Guid> RemoveAdditionalResource(Guid resourceId)
        {
            var resource = await db.ServiceRequestResources.FindAsync(resourceId);

            db.ServiceRequestResources.Remove(resource);

            await db.SaveChangesAsync();

            return resourceId;
        }

        public void SaveRequiredResources(RequiredResourceForm formItem, int serviceRequestId, ICollection<ServiceRequestResource> existingResources)
        {
            // if does not exist then Add
            var existingItem = existingResources.FirstOrDefault(dbItem => dbItem.RoleId == formItem.RoleId);
            if (existingItem == null && formItem.UserId.HasValue)
            {
                var newResource = new ServiceRequestResource
                {
                    Id = Guid.NewGuid(),
                    ServiceRequestId = serviceRequestId,
                    UserId = formItem.UserId.Value,
                    RoleId = formItem.RoleId.Value,
                    CreatedDate = now,
                    CreatedUser = userId.ToString(),
                    ModifiedDate = now,
                    ModifiedUser = userId.ToString()
                };
                db.ServiceRequestResources.Add(newResource);
            }
            else if (existingItem != null && formItem.UserId.HasValue && existingItem.UserId != formItem.UserId)
            {
                existingItem.UserId = formItem.UserId.Value;
                existingItem.ModifiedDate = now;
                existingItem.ModifiedUser = userId.ToString();
            }
            else if (existingItem != null && !formItem.UserId.HasValue)
            {
                db.ServiceRequestResources.Remove(existingItem);
            }
        }

        public async Task DeleteResource(Guid resourceId)
        {
            var resource = await db.ServiceRequestResources.FindAsync(resourceId);
            db.ServiceRequestResources.Remove(resource);
            await db.SaveChangesAsync();
        }

        internal async Task AssignRequiredResourcesToTasks(int serviceRequestId)
        {
            var sr = await db.ServiceRequests.Include(c => c.ServiceRequestResources).Include(c => c.ServiceRequestTasks).WithId(serviceRequestId).SingleAsync(); ;

            sr.ServiceRequestTasks.Where(t => t.ResponsibleRoleId != AspNetRoles.Physician).ForEach(t =>
            {
                var resource = sr.ServiceRequestResources.FirstOrDefault(r => r.RoleId == t.ResponsibleRoleId);
                if (resource == null)
                {
                    t.AssignedTo = null;
                }
                else if (resource.UserId != t.AssignedTo)
                {
                    t.AssignedTo = resource.UserId;
                }
            });

            await db.SaveChangesAsync();
        }

        public async Task<Guid> SaveComment(CommentForm form)
        {
            Guid commentId = form.CommentId.HasValue ? form.CommentId.Value : Guid.NewGuid();

            if (!form.CommentId.HasValue)
            {
                var comment = new ServiceRequestComment
                {
                    Id = commentId,
                    ServiceRequestId = form.ServiceRequestId,
                    Comment = form.Message,
                    IsPrivate = form.IsPrivate,
                    CommentTypeId = form.CommentTypeId,
                    PostedDate = now,
                    UserId = identity.GetGuidUserId(),
                    CreatedDate = now,
                    CreatedUser = identity.GetGuidUserId().ToString(),
                    ModifiedDate = now,
                    ModifiedUser = identity.GetGuidUserId().ToString()
                };
                foreach (var access in form.AccessList)
                {
                    var newAccess = new ServiceRequestCommentAccess
                    {
                        Id = Guid.NewGuid(),
                        ServiceRequestCommentId = commentId,
                        UserId = access,
                        CreatedDate = now,
                        CreatedUser = identity.GetGuidUserId().ToString(),
                        ModifiedDate = now,
                        ModifiedUser = identity.GetGuidUserId().ToString()
                    };
                    db.ServiceRequestCommentAccesses.Add(newAccess);
                }
                db.ServiceRequestComments.Add(comment);
            }
            else
            {
                var comment = await db.ServiceRequestComments.FindAsync(form.CommentId);
                comment.Comment = form.Message;
                comment.IsPrivate = form.IsPrivate;
                comment.CommentTypeId = form.CommentTypeId;
                comment.UserId = identity.GetGuidUserId();
                comment.ModifiedDate = now;
                comment.ModifiedUser = identity.GetGuidUserId().ToString();

                comment.ServiceRequestCommentAccesses.ToList()
                    .ForEach(x => db.ServiceRequestCommentAccesses.Remove(x));

                foreach (var access in form.AccessList)
                {
                    var newAccess = new ServiceRequestCommentAccess
                    {
                        Id = Guid.NewGuid(),
                        UserId = access,
                        ServiceRequestCommentId = commentId,
                        CreatedDate = now,
                        CreatedUser = identity.GetGuidUserId().ToString(),
                        ModifiedDate = now,
                        ModifiedUser = identity.GetGuidUserId().ToString()
                    };
                    db.ServiceRequestCommentAccesses.Add(newAccess);
                }
            }

            await db.SaveChangesAsync();

            return commentId;
        }
        public async Task DeleteComment(Guid commentId)
        {
            var comment = await db.ServiceRequestComments.FindAsync(commentId);
            db.ServiceRequestComments.Remove(comment);
            await db.SaveChangesAsync();
        }


        public async Task<Guid> SaveTeleconference(TeleconferenceForm form)
        {
            Guid teleconferenceId = form.TeleconferenceId.HasValue ? form.TeleconferenceId.Value : Guid.NewGuid();

            if (!form.TeleconferenceId.HasValue)
            {
                var teleconference = new Teleconference
                {
                    Id = teleconferenceId,
                    ServiceRequestId = form.ServiceRequestId,
                    AppointmentDate = form.AppointmentDate,
                    StartTime = form.StartTime,
                    LastModifiedBy = identity.GetGuidUserId().ToString()
                };
                db.Teleconferences.Add(teleconference);
            }
            else
            {
                var teleconference = await db.Teleconferences.FindAsync(form.TeleconferenceId);
                teleconference.ServiceRequestId = form.ServiceRequestId;
                teleconference.AppointmentDate = form.AppointmentDate;
                teleconference.StartTime = form.StartTime;
                teleconference.LastModifiedBy = identity.GetGuidUserId().ToString();
            }

            await db.SaveChangesAsync();

            return teleconferenceId;
        }

        public void SaveTeleconferenceResultForm(TeleconferenceForm form)
        {
            
        }

        public async Task DeleteTeleconference(Guid teleconferenceId)
        {
            var teleconference = await db.Teleconferences.FindAsync(teleconferenceId);
            db.Teleconferences.Remove(teleconference);
            await db.SaveChangesAsync();
        }

    }
}