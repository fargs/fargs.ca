using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Additionals
{
    public class AdditionalsViewModel : ViewModelBase
    {
        public AdditionalsViewModel(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var dto = db.ServiceRequests
                .AsExpandable()
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .AreNotClosed()
                .HaveNoAppointment()
                .Select(sr => new ServiceRequestDto()
                {
                    Id = sr.Id,
                    Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
                    Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
                    ClaimantName = sr.ClaimantName,
                    DueDate = sr.DueDate,
                    Resources = sr.ServiceRequestResources.AsQueryable().Select(srr => new ResourceDto
                    {
                        Person = ContactDto.FromAspNetUserEntity.Invoke(srr.AspNetUser)
                    })
                })
                .ToList();

            Additionals = dto
                .Select(sr => new AdditionalViewModel()
                {
                    ServiceRequestId = sr.Id,
                    Physician = LookupViewModel<Guid>.FromPersonDto(sr.Physician),
                    Service = LookupViewModel<short>.FromServiceDto(sr.Service),
                    ClaimantName = sr.ClaimantName,
                    DueDate = sr.DueDate.ToOrvosiDateFormat(),
                    Collaborators = sr.Resources.Select(r => LookupViewModel<Guid>.FromPersonDto(r.Person))
                });
        }

        public IEnumerable<AdditionalViewModel> Additionals { get; set; }
    }
}