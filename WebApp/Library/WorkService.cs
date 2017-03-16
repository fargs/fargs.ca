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

namespace WebApp.Library
{
    public class WorkService : IDisposable
    {
        IOrvosiDbContext db;
        IIdentity identity;
        DateTime now;
        public WorkService(IOrvosiDbContext db, IIdentity identity)
        {
            this.db = db;
            this.identity = identity;
            this.now = SystemTime.Now();
        }


        public async Task ChangeTaskStatus(int serviceRequestTaskId, short newTaskStatusId)
        {
            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            if (srt == null) throw new ArgumentNullException($"Task with Id {serviceRequestTaskId} not found.");

            await UpdateTaskStatus(srt, newTaskStatusId);
        }
        public async Task ToggleTaskStatus(int serviceRequestTaskId, short currentTaskStatusId)
        {
            var srt = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);

            if (srt == null) throw new ArgumentNullException($"Task with Id {serviceRequestTaskId} not found.");

            var newTaskStatusId = srt.TaskStatusId == currentTaskStatusId ? TaskStatuses.ToDo : currentTaskStatusId;

            await UpdateTaskStatus(srt, newTaskStatusId);
        }

        private async Task UpdateTaskStatus(ServiceRequestTask srt, short newTaskStatusId)
        {
            srt.TaskStatusId = newTaskStatusId;
            srt.TaskStatusChangedBy = identity.GetGuidUserId();
            srt.TaskStatusChangedDate = now;
            srt.ModifiedDate = now;
            srt.ModifiedUser = identity.GetGuidUserId().ToString();

            await db.SaveChangesAsync();

            await UpdateDependentTaskStatuses(srt.ServiceRequestId);
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



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                this.identity = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}