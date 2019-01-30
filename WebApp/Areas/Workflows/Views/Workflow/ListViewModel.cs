using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using ImeHub.Data;
using System.Security.Principal;
using System;
using System.Linq;
using ImeHub.Models;
using LinqKit;

namespace WebApp.Areas.Workflows.Views.Workflow
{
    public class ListViewModel : ViewModelBase
    {
        private ImeHubDbContext db;
        public ListViewModel(Guid? workflowId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            this.db = db;

            Workflows = db.Workflows
                .AsNoTracking()
                .AsExpandable()
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(w => new LookupViewModel<Guid>
                {
                    Id = w.Id,
                    Name = w.Name
                })
                .ToList();

            if (workflowId.HasValue)
            {
                SelectedWorkflowId = workflowId;
                SelectedWorkflow = new WorkflowViewModel(workflowId.Value, db, identity, now);
            }
        }
        public IEnumerable<LookupViewModel<Guid>> Workflows { get; set; }
        public int WorkflowCount { get; set; }
        public Guid? SelectedWorkflowId { get; private set; }
        public WorkflowViewModel SelectedWorkflow { get; private set; }
    }
}