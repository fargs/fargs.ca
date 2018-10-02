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