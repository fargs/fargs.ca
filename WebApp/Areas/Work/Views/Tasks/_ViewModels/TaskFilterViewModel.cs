using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Tasks
{
    public class TaskFilterViewModel : ViewModelBase
    {
        private OrvosiDbContext db;
        public TaskFilterViewModel(OrvosiDbContext db, HttpRequestBase request, IIdentity identity, DateTime now) : base(identity, now)
        {
            this.db = db;
            Tasks = GetTasksSelectList(LoggedInUserId);
            TaskStatuses = GetTaskStatusesSelectList();

            // get the query string 
            SelectedTaskIds = new short[] { };
            SelectedTaskStatusIds = new short[] { };

        }
        public IEnumerable<SelectListItem> Tasks { get; set; }
        public short[] SelectedTaskIds { get; set; }
        public IEnumerable<SelectListItem> TaskStatuses { get; set; }
        public short[] SelectedTaskStatusIds { get; set; }
        
        private const string taskIdKey = "taskIds-key";
        private IEnumerable<LookupViewModel<short>> GetTasks(Guid userId)
        {
            var item = HttpContext.Current.Items[taskIdKey] as IEnumerable<LookupViewModel<short>>;
            if (item == null)
            {
                var dto = db.ServiceRequestTasks
                    .AreActiveOrDone()
                    .AreAssignedToUser(userId)
                    .Select(srt => srt.OTask)
                    .Distinct()
                    .Select(LookupDto<short>.FromTaskEntity)
                    .ToList();

                var viewModel = dto.Select(LookupViewModel<short>.FromLookupDto);

                HttpContext.Current.Items[taskIdKey] = item = viewModel;
            }
            return item;
        }
        private IEnumerable<SelectListItem> GetTasksSelectList(Guid userId)
        {
            var statuses = GetTasks(userId);
            return statuses.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        private const string taskStatusIdsKey = "task-status-ids-key";
        private IEnumerable<LookupViewModel<short>> GetTaskStatuses()
        {
            var item = HttpContext.Current.Items[taskStatusIdsKey] as IEnumerable<LookupViewModel<short>>;
            if (item == null)
            {
                var dto = db.TaskStatus
                    .OrderBy(c => c.Name)
                    .Select(LookupDto<short>.FromTaskStatusEntity.Expand())
                    .ToList();

                var viewModel = dto
                    .Select(LookupViewModel<short>.FromLookupDto)
                    .ToList();

                HttpContext.Current.Items[taskStatusIdsKey] = item = viewModel;
            }
            return item;
        }
        private IEnumerable<SelectListItem> GetTaskStatusesSelectList()
        {
            var statuses = GetTaskStatuses();
            return statuses.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

    }
}