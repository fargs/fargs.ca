using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Work.DaySheet.ServiceRequest
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class EditFormModel
    {
        public int  ServiceRequestId { get; set; }
        public string SourceCompany { get; set; }
        public int? CompanyId { get; set; }

        public static Func<ServiceRequestDto, EditFormModel> FromServiceRequestDto = dto => dto == null ? null : new EditFormModel
        {
            ServiceRequestId = dto.Id,
            SourceCompany = dto.SourceCompany,
            CompanyId = dto.CompanyId
        };
    }
}