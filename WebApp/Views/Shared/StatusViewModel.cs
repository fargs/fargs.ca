using ImeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Shared
{
    public class StatusViewModel<T> : LookupViewModel<T>
    {
        public DateTime ChangedDate { get; set; }
        public Guid ChangedById { get; set; }
        public ContactViewModel ChangedBy { get; set; }
        
        public static Func<StatusModel<T>, StatusViewModel<T>> FromStatusModel = e => e == null ? null : new StatusViewModel<T>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode,
            ChangedDate = e.ChangedDate,
            ChangedById = e.ChangedById,
            ChangedBy = ContactViewModel.FromContactModel.Invoke(e.ChangedBy)
        };
    }
}