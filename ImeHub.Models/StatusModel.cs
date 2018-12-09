using ImeHub.Data;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class StatusModel<T> : LookupModel<T>
    {
        public DateTime ChangedDate { get; set; }
        public Guid ChangedById { get; set; }
        public ContactModel ChangedBy { get; set; }

        public static Expression<Func<UserSetupWorkflow, StatusModel<Enums.WorkflowStatus>>> FromUserSetupWorkflow = e => e == null ? null : new StatusModel<Enums.WorkflowStatus>
        {
            Id = (Enums.WorkflowStatus)e.StatusId,
            Name = e.Name,
            Code = null,
            ColorCode = null,
            ChangedDate = e.StatusChangedDate,
            ChangedById = e.StatusChangedById,
            ChangedBy = ContactModel.FromUser.Invoke(e.StatusChangedBy)
        };
        public static Expression<Func<UserSetupWorkItem, StatusModel<Enums.WorkItemStatus>>> FromUserSetupWorkItem = e => e == null ? null : new StatusModel<Enums.WorkItemStatus>
        {
            Id = (Enums.WorkItemStatus)e.StatusId,
            Name = e.Name,
            Code = null,
            ColorCode = null,
            ChangedDate = e.StatusChangedDate,
            ChangedById = e.StatusChangedById,
            ChangedBy = ContactModel.FromUser.Invoke(e.User)
        };
    }
}