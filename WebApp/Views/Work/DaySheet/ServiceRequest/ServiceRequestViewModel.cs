using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;
using WebApp.Views.Comment;
using WebApp.Views.ServiceRequestMessage;

namespace WebApp.Views.Work.DaySheet.ServiceRequest
{
    public class ServiceRequestViewModel
    {
        public int ServiceRequestId { get; set; }
        public SummaryViewModel Summary { get; set; }
        public EditFormModel Edit { get; set; }
        // Box
        public string BoxCaseFolderURL { get; private set; }

        public ActionMenuViewModel ActionMenu { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
        public DiscussionViewModel Discussion { get; set; }
        public MessageFormModel PostMessage { get; set; }

        public static Func<ServiceRequestDto, ServiceRequestViewModel> FromServiceRequestDto = dto => new ServiceRequestViewModel
        {
            ServiceRequestId = dto.Id,

            Summary = SummaryViewModel.FromServiceRequestDto.Invoke(dto),
            ActionMenu = ActionMenuViewModel.FromServiceRequestDto.Invoke(dto),
            Edit = EditFormModel.FromServiceRequestDto(dto),

            // Box
            BoxCaseFolderURL = dto.BoxCaseFolderURL,

            Comments = dto.Comments.Select(CommentViewModel.FromCommentDto),
            Discussion = DiscussionViewModel.FromServiceRequestDto(dto),
            PostMessage = MessageFormModel.FromServiceRequestDto(dto)
        };
    }
}