using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library.Extensions;
using WebApp.Models;
using Orvosi.Data.Filters;
using WebApp.Views.Shared;
using WebApp.Areas.Work.Views.DaySheet.ServiceRequest;
using System.Security.Principal;
using WebApp.Library;
using WebApp.Views.Teleconference;

namespace WebApp.Areas.Work.Views.DaySheet
{
    public partial class DaySheetViewModel : ViewModelBase
    {
        public DaySheetViewModel(DateTime selectedDate, OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var physician = PersonDto.FromAspNetUserEntity.Invoke(db.AspNetUsers.Single(a => a.Id == PhysicianId));
            var teamMembers = db.Collaborators
                .ForPhysician(physician.Id)
                .Select(PersonDto.FromCollaboratorEntity.Expand())
                .ToList();
            teamMembers.Add(physician);

            var serviceRequests = db.ServiceRequests
                .AsNoTracking()
                .AsExpandable()
                .AreScheduledThisDay(selectedDate)
                .AreNotCancellations()
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForDaySheet(LoggedInUserId))
                .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
                .AsEnumerable();

            var invoiceIds = serviceRequests.SelectMany(sr => sr.InvoiceDetails.Select(id => id.InvoiceId)).ToArray();
            var invoices = db.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(i => invoiceIds.Contains(i.Id))
                .Select(InvoiceDto.FromInvoiceEntity)
                .AsEnumerable();

            var teleconferences = db.Teleconferences
                .AsNoTracking()
                .AsExpandable()
                .AreScheduledThisDay(selectedDate)
                .Select(TeleconferenceDto.FromEntityForDaySheet)
                .AsEnumerable();

            Day = selectedDate;
            DayName = selectedDate.ToOrvosiLongDateFormat();
            Companies = serviceRequests.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct();
            Addresses = serviceRequests.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray();
            ServiceRequests = serviceRequests.Select(sr => new ServiceRequestViewModel(sr, invoices, teamMembers, identity, now));
            Teleconferences = teleconferences.Select(TeleconferenceViewModel.FromTeleconferenceDtoForDaySheet);
        }

        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<string> Companies { get; set; }
        public IEnumerable<ServiceRequestViewModel> ServiceRequests { get; set; }
        public IEnumerable<TeleconferenceViewModel> Teleconferences { get; set; }
    }
}