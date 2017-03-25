using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class TaskStatusSummaryViewModel
    {
        public int Count { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public int DoneCount { get; set; }
        public int ObsoleteCount { get; set; }
        public int OnHoldCount { get; set; }

        public static Expression<Func<IGrouping<DateTime, CaseViewModel>, TaskStatusSummaryViewModel>> FromCaseViewModelGrouping = c => new TaskStatusSummaryViewModel
        {
            Count = c.Count(),
            ToDoCount = c.Count(sr => sr.ServiceRequestStatusId == TaskStatuses.ToDo),
            WaitingCount = c.Count(sr => sr.ServiceRequestStatusId == TaskStatuses.Waiting),
            DoneCount = c.Count(sr => sr.ServiceRequestStatusId == TaskStatuses.Done),
            ObsoleteCount = c.Count(sr => sr.ServiceRequestStatusId == TaskStatuses.Obsolete),
            OnHoldCount = c.Count(sr => sr.ServiceRequestStatusId == TaskStatuses.OnHold)
        };

        public static Expression<Func<IGrouping<DateTime, ServiceRequestTask>, TaskStatusSummaryViewModel>> FromServiceRequestTaskEntityGrouping = c => new TaskStatusSummaryViewModel
        {
            Count = c.Count(),
            ToDoCount = c.Count(sr => sr.TaskStatusId == TaskStatuses.ToDo),
            WaitingCount = c.Count(sr => sr.TaskStatusId == TaskStatuses.Waiting),
            DoneCount = c.Count(sr => sr.TaskStatusId == TaskStatuses.Done),
            ObsoleteCount = c.Count(sr => sr.TaskStatusId == TaskStatuses.Obsolete),
            OnHoldCount = c.Count(sr => sr.TaskStatusId == TaskStatuses.OnHold)
        };
    }
}