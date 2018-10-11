using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList;
using WebApp.Areas.Work.Views.DaySheet.ServiceRequest.TaskList;
using WebApp.Models;
using WebApp.Views.Comment;
using WebApp.Views.ServiceRequestMessage;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest
{
    public class ServiceRequestViewModel : ViewModelBase
    {
        public ServiceRequestViewModel(ServiceRequestDto dto, IEnumerable<InvoiceDto> invoices, IList<PersonDto> teamMembers, IIdentity identity, DateTime now) : base(identity, now)
        {
            ServiceRequestId = dto.Id;

            Summary = SummaryViewModel.FromServiceRequestDto.Invoke(dto);
            Edit = EditFormModel.FromServiceRequestDto(dto);
            ActionMenu = new ActionMenuViewModel(dto, identity, now);

            Comments = dto.Comments.Select(CommentViewModel.FromCommentDto);
            CommentCount = dto.Comments.Count();
            Discussion = DiscussionViewModel.FromServiceRequestDto(dto);
            PostMessage = MessageFormModel.FromServiceRequestDto(dto);
            TaskList = new TaskListViewModel(dto, TaskListViewModelFilter.CriticalPathOrAssignedToUser, teamMembers, identity, Now);

            InvoiceList = new InvoiceListViewModel(invoices.Where(i => i.ServiceRequestId == dto.Id), identity, now);
        }
        public ServiceRequestViewModel(int serviceRequestId, DateTime selectedDate, OrvosiDbContext db, IIdentity identity, DateTime now)
        {
            var physician = PersonDto.FromAspNetUserEntity.Invoke(db.AspNetUsers.Single(a => a.Id == PhysicianId));
            var teamMembers = db.Collaborators
                .ForPhysician(physician.Id)
                .Select(PersonDto.FromCollaboratorEntity.Expand())
                .ToList();
            teamMembers.Add(physician);

            var serviceRequest = db.ServiceRequests
                .AsNoTracking()
                .AsExpandable()
                .WithId(serviceRequestId)
                .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForDaySheet(LoggedInUserId))
                .Single();

            var invoiceIds = serviceRequest.InvoiceDetails.Select(id => id.InvoiceId).ToArray();
            var invoices = db.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(i => invoiceIds.Contains(i.Id))
                .Select(InvoiceDto.FromInvoiceEntity)
                .AsEnumerable();

            ServiceRequestId = serviceRequest.Id;

            Summary = SummaryViewModel.FromServiceRequestDto.Invoke(serviceRequest);
            Edit = EditFormModel.FromServiceRequestDto(serviceRequest);
            ActionMenu = new ActionMenuViewModel(serviceRequest, identity, now);

            Comments = serviceRequest.Comments.Select(CommentViewModel.FromCommentDto);
            CommentCount = serviceRequest.Comments.Count();
            Discussion = DiscussionViewModel.FromServiceRequestDto(serviceRequest);
            PostMessage = MessageFormModel.FromServiceRequestDto(serviceRequest);
            TaskList = new TaskListViewModel(serviceRequest, TaskListViewModelFilter.CriticalPathOrAssignedToUser, teamMembers, identity, Now);

            InvoiceList = new InvoiceListViewModel(invoices.Where(i => i.ServiceRequestId == serviceRequest.Id), identity, now);

        }
        public int ServiceRequestId { get; set; }
        public SummaryViewModel Summary { get; set; }
        public EditFormModel Edit { get; set; }
        public ActionMenuViewModel ActionMenu { get; set; }
        public TaskListViewModel TaskList { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public int CommentCount { get; set; }
        public DiscussionViewModel Discussion { get; set; }
        public MessageFormModel PostMessage { get; set; }
        public InvoiceListViewModel InvoiceList { get; set; }
    }
}