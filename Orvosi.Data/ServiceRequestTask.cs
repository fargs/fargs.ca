using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orvosi.Shared.Enums;

namespace Orvosi.Data
{
    public partial class ServiceRequestTask
    {
        public int GetStatus(DateTime now)
        {
            if (CompletedDate.HasValue)
                return TaskStatuses.Done;
            else if (IsObsolete)
                return TaskStatuses.Obsolete;
            else if (IsDependentOnExamDate.Value && now < ServiceRequest.AppointmentDate)
                return TaskStatuses.Waiting;
            else if (GetDependents().Any(t => !t.CompletedDate.HasValue && !t.IsObsolete))
                return TaskStatuses.Waiting;
            else
                return TaskStatuses.ToDo;

        }

        public bool IsActionable(DateTime now)
        {
            var status = GetStatus(now);
            if (status == TaskStatuses.ToDo)
                return true;

            return false;
        }

        public bool IsComplete()
        {
            return CompletedDate.HasValue;
        }

        public IEnumerable<ServiceRequestTask> GetDependents()
        {
            return ServiceRequest.ServiceRequestTasks
                .Where(srt => !srt.IsObsolete)
                .Where(srt => (DependsOn ?? string.Empty).Split(',').Contains(srt.TaskId.ToString()));
        }
    }
}
