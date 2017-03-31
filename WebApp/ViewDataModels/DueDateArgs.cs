using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewDataModels
{
    public class DueDateArgs
    {
        public ViewTarget ViewTarget { get; set; } = ViewTarget.DueDates;
        public TaskListArgs TaskListArgs { get; set; }
        public bool UseGridView { get; set; }
    }
}