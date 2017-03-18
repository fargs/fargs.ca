using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.ServiceRequestTaskViewModels
{
    public enum TaskListViewModelFilter
    {
        AllTasks,
        MyTasks,
        PrimaryRolesOnly,
        CriticalPathOnly
    }
    public enum TaskListViewOptions
    {
        Dashboard,
        Details
    }
}