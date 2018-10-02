using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Cancellation
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class CancellationStatusViewModel
    {
        public bool IsNoShow { get; set; }
        public bool IsLateCancellation { get; set; }
        public bool IsCancelled { get; set; }
        
        public static Func<ServiceRequestDto, CancellationStatusViewModel> FromServiceRequestDto = dto => FromServiceRequestDtoExpr.Compile().Invoke(dto);
        public static Expression<Func<ServiceRequestDto, CancellationStatusViewModel>> FromServiceRequestDtoExpr = dto => dto == null ? null : new CancellationStatusViewModel
        {
            IsNoShow = dto.IsNoShow,
            IsLateCancellation = dto.IsLateCancellation,
            IsCancelled = dto.IsCancelled
        };
    }
}