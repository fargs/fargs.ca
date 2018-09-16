using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Work.DaySheet.ServiceRequest
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class ActionMenuViewModel
    {
        public int ServiceRequestId { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public bool IsNoShow { get; set; }
        public bool CanBeNoShow { get; set; }

        public static Func<ServiceRequestDto, ActionMenuViewModel> FromServiceRequestDto = dto => dto == null ? null : new ActionMenuViewModel
        {
            ServiceRequestId = dto.Id,
            BoxCaseFolderURL = dto.BoxCaseFolderURL,
            CanBeNoShow = dto.CanBeNoShow(dto.AppointmentDate),
            IsNoShow = dto.IsNoShow
        };
    }
}