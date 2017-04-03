using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.ViewDataModels;
using WebApp.Components.Grid;
namespace WebApp.ViewModels
{
    public class TaskGridViewModel
    {
        public IEnumerable<GridColumn> Columns { get; set; }
        public IEnumerable<TaskWithCaseViewModel> Data { get; internal set; }
        public int Total { get; internal set; }
        public PagerViewModel Pager { get; set; }
        public TaskListArgs Args { get; set; }
    }
}