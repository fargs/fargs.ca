using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImeHub.Data;
using Enums = ImeHub.Models.Enums;

namespace ImeHub.Models
{
    public class WorkManager
    {
        private ImeHubDbContext db;
        private Guid userId;
        private DateTime now;
        public WorkManager(ImeHubDbContext db, Guid userId, DateTime now)
        {
            this.db = db;
        }
        public async Task<Guid> BookAssessment(ServiceRequestModel serviceRequest)
        {
            var sr = new ServiceRequest();

            var slot = await db.AvailableSlots.FindAsync(serviceRequest.AvailableSlotId);

            sr.StatusId = (byte)Enums.ServiceRequestStatus.Active;
            sr.StatusChangedById = userId;
            sr.StatusChangedDate = now;
            sr.RequestedBy = serviceRequest.RequestedBy;
            sr.RequestedDate = serviceRequest.RequestedDate ?? now;
            sr.ServiceId = serviceRequest.ServiceId;
            sr.PhysicianId = serviceRequest.PhysicianId;
            sr.AddressId = serviceRequest.AddressId;
            sr.AppointmentDate = serviceRequest.AppointmentDate;
            sr.AvailableSlotId = serviceRequest.AvailableSlotId;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.DueDate = serviceRequest.DueDate;
            sr.ClaimantName = serviceRequest.ClaimantName;
            sr.ReferralSource = serviceRequest.ReferralSource;

            //// clone the workflow template tasks
            //var requestTemplate = await db.Workflows.FindAsync(sr.ServiceRequestTemplateId);
            //foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted().Select(ServiceRequestTemplateTaskDto.FromEntity.Expand()))
            //{
            //    var st = new Orvosi.Data.ServiceRequestTask();
            //    st.Guidance = null;
            //    st.ObjectGuid = Guid.NewGuid();
            //    st.ResponsibleRoleId = template.ResponsibleRoleId;
            //    st.Sequence = template.Sequence;
            //    st.ShortName = template.ShortName;
            //    st.TaskId = template.TaskId;
            //    st.TaskName = template.TaskName;
            //    st.ModifiedDate = now;
            //    st.ModifiedUser = userId.ToString();
            //    st.CreatedDate = now;
            //    st.CreatedUser = userId.ToString();
            //    // Assign tasks to physician and case coordinator to start
            //    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
            //    st.ServiceRequestTemplateTaskId = template.Id;
            //    st.TaskType = template.DueDateType;
            //    st.Workload = null;
            //    st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
            //    st.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, template);
            //    st.TaskStatusId = TaskStatuses.ToDo;
            //    st.TaskStatusChangedBy = userId;
            //    st.TaskStatusChangedDate = now;
            //    st.IsCriticalPath = template.IsCriticalPath;
            //    st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
            //    st.EffectiveDate = GetEffectiveDate(sr.AppointmentDate, template);

            //    sr.ServiceRequestTasks.Add(st);
            //}

            db.ServiceRequests.Add(sr);

            await db.SaveChangesAsync();

            // Clone the task dependencies
            //foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
            //{
            //    foreach (var dependentTemplate in taskTemplate.Child)
            //    {
            //        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
            //        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
            //        task.Child.Add(dependent);
            //    }
            //}

            //await db.SaveChangesAsync();

            return sr.Id;
        }

    }
}
