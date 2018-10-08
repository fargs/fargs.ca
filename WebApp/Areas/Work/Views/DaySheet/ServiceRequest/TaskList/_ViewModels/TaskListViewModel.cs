﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Orvosi.Shared.Enums;
using WebApp.Library;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.TaskList
{
    public enum TaskListViewModelFilter
    {
        AllTasks,
        MyTasks,
        PrimaryRolesOnly,
        CriticalPathOnly,
        CriticalPathOrAssignedToUser,
        MyActiveTasks
    }

    public enum ViewTarget
    {
        DaySheet,
        DueDates,
        Schedule,
        Additionals,
        Details,
        Modal
    }

    public class TaskListViewModel : ViewModelBase
    {
        public TaskListViewModel(ServiceRequestDto serviceRequest, TaskListViewModelFilter filter, IList<PersonDto> teamMembers, IIdentity identity, DateTime now) : base(identity, now)
        {
            ServiceRequestId = serviceRequest.Id;
            var query = serviceRequest.Tasks;
            
            switch (filter)
            {
                case TaskListViewModelFilter.CriticalPathOrAssignedToUser:
                    query = query.AreOnCriticalPathOrAssignedToUser(LoggedInUserId);
                    break;
                case TaskListViewModelFilter.CriticalPathOnly:
                    query = query.AreOnCriticalPath();
                    break;
                case TaskListViewModelFilter.PrimaryRolesOnly:
                    var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };
                    query = query.AreAssignedToUserOrRoles(LoggedInUserId, rolesThatShouldBeSeen);
                    break;
                case TaskListViewModelFilter.MyTasks:
                    query = query.AreAssignedToUser(LoggedInUserId);
                    break;
                case TaskListViewModelFilter.MyActiveTasks:
                    query = query
                        .AreActive()
                        .AreAssignedToUser(LoggedInUserId);
                    break;
                default: // default to all tasks
                    break;
            }

            query = query
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Sequence);

            // Create the lookup for the team member list, each task will remove the selected item
            Tasks = query.Select(t => new TaskViewModel(t, serviceRequest.Physician, teamMembers, identity, now));
        }

        public int ServiceRequestId { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}