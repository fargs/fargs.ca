using Orvosi.Data;
using System;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Tasks
{
    public partial class TasksViewModel : ViewModelBase
    {
        public TasksViewModel(OrvosiDbContext db, HttpRequestBase request, IIdentity identity, DateTime now) : base(identity, now)
        {

            TaskFilter = new TaskFilterViewModel(db, request, identity, now);
        }

        public TaskFilterViewModel TaskFilter { get; private set; }
    }
}