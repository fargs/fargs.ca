using System;
using System.Linq;
using System.Security.Principal;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest
{
    //TODO: Move the CanBeCancelled, CanBeUncancelled, CanBeNoShow, CanNoShowBeUndone properties from CaseViewModel to here
    public class ActionMenuViewModel : ViewModelBase
    {
        private OrvosiDbContext db;
        public ActionMenuViewModel(ServiceRequestDto serviceRequest, IIdentity identity, DateTime now) : base(identity, now)
        {
            SetValues(serviceRequest);
        }
        public ActionMenuViewModel(OrvosiDbContext db, int serviceRequestId, IIdentity identity, DateTime now) : base(identity, now)
        {
            var serviceRequest = db.ServiceRequests
                .AsExpandable()
                .WithId(serviceRequestId)
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForDaySheet(LoggedInUserId))
                .SingleOrDefault();

            SetValues(serviceRequest);
        }
        private void SetValues(ServiceRequestDto serviceRequest)
        {
            ServiceRequestId = serviceRequest.Id;
            BoxCaseFolderURL = serviceRequest.BoxCaseFolderURL;
            IsNoShow = serviceRequest.IsNoShow;
            CanBeNoShow = serviceRequest.CanBeNoShow(serviceRequest.AppointmentDate);
        }
        public int ServiceRequestId { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public bool IsNoShow { get; set; }
        public bool CanBeNoShow { get; set; }
    }
}