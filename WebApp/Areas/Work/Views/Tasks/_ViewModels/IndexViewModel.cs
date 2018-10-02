using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Views.Calendar;

namespace WebApp.Areas.Work.Views.Tasks
{
    public class IndexViewModel
    {
        public IndexViewModel(TasksViewModel tasks)
        {
            Tasks = tasks;
        }
        public TasksViewModel Tasks { get; private set; }
    }
}