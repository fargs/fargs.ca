using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class CancellationViewModel
    {
        public bool IsNoShow { get; set; }
        public bool IsLateCancellation { get; set; }
        public DateTime? CancelledDate { get; set; }

        public static Expression<Func<ServiceRequestDto, CancellationViewModel>> FromServiceRequestDto = dto => dto == null ? null : new CancellationViewModel
        {
            IsNoShow = dto.IsNoShow,
            IsLateCancellation = dto.IsLateCancellation,
            CancelledDate = dto.CancelledDate
        };
    }
}