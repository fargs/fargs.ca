﻿using System;
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

namespace WebApp.Library
{
    public class WorkService
    {
        IOrvosiDbContext db;
        IIdentity identity;
        DateTime now;
        Guid userId;
        Guid physicianId;
        UserContextViewModel userContext;

        public WorkService(IOrvosiDbContext db, IIdentity identity)
        {
            this.db = db;
            this.identity = identity;
            userId = identity.GetGuidUserId();
            userContext = identity.GetUserContext();
            physicianId = userContext.Id;
            this.now = SystemTime.Now();
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
                .Where(srt => srt.TaskStatusId != TaskStatuses.Done);

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
                .Where(srt => srt.TaskStatusId != TaskStatuses.Done);

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
        public async Task NoShow(NoShowForm form)
        {
            var sr = await db.ServiceRequests.FindAsync(form.ServiceRequestId);

            sr.IsNoShow = form.IsNoShow;

            byte newTaskStatus = form.IsNoShow ? TaskStatuses.Obsolete : TaskStatuses.ToDo;

            foreach (var task in sr.ServiceRequestTasks
                .Where(srt => srt.TaskStatusId != TaskStatuses.Done)
                .Where(srt => srt.TaskId != Tasks.SubmitInvoice))
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

            foreach (var task in sr.ServiceRequestTasks.Where(srt => srt.TaskStatusId != TaskStatuses.Done))
            {
                await SaveTaskStatusChange(task, newTaskStatus);
            }
            await SaveDependentTaskStatusChanges(sr);

            // update the request status
            var newServiceRequestStatusId = form.IsOnHold ? ServiceRequestStatuses.OnHold : CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, ServiceRequestStatuses.Active);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }


        public async Task AddTask(int serviceRequestId, byte taskId)
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
            task.DueDateBase = st.DueDateBase;
            task.DueDateDiff = st.DueDateDiff;
            task.Guidance = st.Guidance;
            task.IsCriticalPath = true;
            task.ModifiedDate = SystemTime.Now();
            task.ModifiedUser = identity.Name;
            task.TaskStatusId = TaskStatuses.ToDo;
            task.TaskStatusChangedBy = identity.GetGuidUserId();
            task.TaskStatusChangedDate = now;

            // this is currently hard coded but should be made editable by the user adding the task
            if (taskId == Tasks.AdditionalEdits)
            {
                var approveReport = await db.ServiceRequestTasks.FirstOrDefaultAsync(srt => srt.ServiceRequestId == serviceRequestId && srt.TaskId == Tasks.ApproveReport);
                if (approveReport != null)
                {
                    approveReport.CompletedDate = null;
                    approveReport.CompletedBy = null;
                    approveReport.ModifiedDate = SystemTime.UtcNow();
                    approveReport.ModifiedUser = identity.GetGuidUserId().ToString();

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
                    closeCase.ModifiedDate = SystemTime.UtcNow();
                    closeCase.ModifiedUser = identity.GetGuidUserId().ToString();

                    task.Parent.Add(closeCase);
                    task.Sequence = (short)(closeCase.Sequence.Value - 2);
                }
            }

            request.ServiceRequestTasks.Add(task);

            await db.SaveChangesAsync();

            await SaveDependentTaskStatusChanges(request);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(request.ServiceRequestTasks, request.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(request, newServiceRequestStatusId);
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

            foreach (var item in srt.Child.ToList())
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

            await SaveTaskStatusChange(srt, newTaskStatusId);

            var sr = srt.ServiceRequest;
            await SaveDependentTaskStatusChanges(sr);

            var newServiceRequestStatusId = CalculateNewServiceRequestStatus(sr.ServiceRequestTasks, sr.ServiceRequestStatusId);
            await SaveServiceRequestStatusChange(sr, newServiceRequestStatusId);
        }


        public async Task UpdateAssessmentDayTaskStatus(int serviceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null)
            {
                throw new ArgumentNullException($"Service request with Id {serviceRequestId} not found.");
            }

            var appointmentDate = serviceRequest.AppointmentDate;
            var now = SystemTime.Now();
            var tasks = serviceRequest.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.AssessmentDay);
            foreach (var task in tasks)
            {
                if (appointmentDate >= now)
                {
                    await ChangeTaskStatus(task.Id, TaskStatuses.Waiting);
                }
                else
                {
                    await ChangeTaskStatus(task.Id, TaskStatuses.Done);
                }
            }
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


        private async Task SaveTaskStatusChange(ServiceRequestTask srt, short newTaskStatusId)
        {
            srt.TaskStatusId = newTaskStatusId;
            srt.TaskStatusChangedBy = identity.GetGuidUserId();
            srt.TaskStatusChangedDate = now;
            srt.ModifiedDate = now;
            srt.ModifiedUser = identity.GetGuidUserId().ToString();

            await db.SaveChangesAsync();
        }
        private async Task SaveDependentTaskStatusChanges(ServiceRequest serviceRequest)
        {
            foreach (var srt in serviceRequest.ServiceRequestTasks)
            {
                if (srt.TaskId != Tasks.AssessmentDay)
                {
                    var isWaiting = srt.Child.Any(d => d.TaskStatusId == TaskStatuses.ToDo || d.TaskStatusId == TaskStatuses.Waiting || d.TaskStatusId == TaskStatuses.OnHold);

                    if (srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting)
                    {
                        srt.TaskStatusId = isWaiting ? TaskStatuses.Waiting : TaskStatuses.ToDo;
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
            if (tasks.Any(srt => srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting || srt.TaskStatusId == TaskStatuses.OnHold))
            {
                // return on hold if it was explicitely set, otherwise return active
                return currentStatus == ServiceRequestStatuses.OnHold ? ServiceRequestStatuses.OnHold : ServiceRequestStatuses.Active;
            }
            else
            {
                return ServiceRequestStatuses.Closed;
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
    }
}