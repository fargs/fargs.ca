using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public static class ServiceRequestExtensions
    {
        public static void UpdateIsClosed(this ServiceRequest serviceRequest)
        {
            serviceRequest.IsClosed =
                serviceRequest
                    .ServiceRequestTasks
                    .Where(srt => srt.TaskType == null)
                    .All(srt => srt.CompletedDate.HasValue || srt.IsObsolete);
        }

        public static void UpdateToDoTasksToObsolete(this ServiceRequest serviceRequest)
        {
            foreach (var task in serviceRequest.ServiceRequestTasks)
            {
                task.IsObsolete = !task.CompletedDate.HasValue ? true : false;

                if ((serviceRequest.IsLateCancellation || serviceRequest.IsNoShow) && task.TaskId == Tasks.SubmitInvoice)
                    task.IsObsolete = false;

                if (task.TaskId == Tasks.CloseCase)
                    task.IsObsolete = false;
            }
        }

        public static void UpdateObsoleteTasksToToDo(this ServiceRequest serviceRequest)
        {
            foreach (var task in serviceRequest.ServiceRequestTasks)
            {
                task.IsObsolete = false;

                if (task.TaskId == Tasks.CloseCase)
                    task.CompletedDate = null;
            }
        }
    }
}
