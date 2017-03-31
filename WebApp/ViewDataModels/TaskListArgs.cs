using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.ServiceRequestTaskViewModels;

namespace WebApp.ViewDataModels
{
    public class TaskListArgs : GridArgs
    {
        public TaskListArgs()
        {
        }
        public int ServiceRequestId { get; set; }
        public short[] TaskIds { get; set; }
        public short[] TaskStatusIds { get; set; }
        public Guid[] AssignedTo { get; set; }
        public DateFilterArgs DateRange { get; set; }

        // eventually replace these with the above params
        public ViewTarget ViewTarget { get; set; }
        public TaskListViewModelFilter ViewFilter { get; set; }
    }

    public static class TaskListArgsExtensions
    {
        public static TaskListArgs TaskListArgs_Get(this ViewDataDictionary viewData)
        {
            return viewData["args"] as TaskListArgs;
        }
        public static void TaskListArgs_Set(this ViewDataDictionary viewData, TaskListArgs value)
        {
            viewData.Add("args", value);
        }

        public static TaskListArgs TaskListArgs_Get(this TempDataDictionary viewData)
        {
            return viewData["args"] as TaskListArgs;
        }
        public static void TaskListArgs_Set(this TempDataDictionary viewData, TaskListArgs value)
        {
            viewData.Add("args", value);
        }
    }

    public enum TaskListViewModelFilter
    {
        AllTasks,
        MyTasks,
        PrimaryRolesOnly,
        CriticalPathOnly,
        CriticalPathOrAssignedToUser,
        MyActiveTasks
    }
}