using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels.ServiceRequestTaskViewModels;

namespace WebApp.ViewDataModels
{
    public class TaskListFilterArgs
    {
        public TaskListFilterArgs()
        {
        }
        public int ServiceRequestId { get; set; }
        public short[] TaskIds { get; set; }
        public short[] TaskStatusIds { get; set; }
        public Guid[] AssignedTo { get; set; }
        public DateFilterArgs DateRange { get; set; }

        // eventually replace these with the above params
        public TaskListViewOptions ViewOptions { get; set; }
        public TaskListViewModelFilter Options { get; set; }
    }

    public static class TaskListFilterArgsExtensions
    {
        public static TaskListFilterArgs TaskListFilterArgs_Get(this ViewDataDictionary viewData)
        {
            return viewData["args"] as TaskListFilterArgs;
        }
        public static void TaskListFilterArgs_Set(this ViewDataDictionary viewData, TaskListFilterArgs value)
        {
            viewData.Add("args", value);
        }
    }
}