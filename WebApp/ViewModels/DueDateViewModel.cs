using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.ViewDataModels;

namespace WebApp.ViewModels
{
    public class DueDateViewModel
    {
        public DueDateArgs DueDateArgs { get; set; }
        public IEnumerable<CaseLinkArgs> CaseLinkArgs { get; set; }
        public IEnumerable<TaskListArgs> TaskListArgs { get; set; }
    }
}