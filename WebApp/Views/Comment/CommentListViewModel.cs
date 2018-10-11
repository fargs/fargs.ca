using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.Comment
{
    public class CommentListViewModel : ViewModelBase
    {
        public CommentListViewModel(ServiceRequestDto serviceRequest)
        {
            ServiceRequestId = serviceRequest.Id;
            Comments = serviceRequest.Comments
                .Select(c => CommentViewModel.FromCommentDto(c));
        }
        public CommentListViewModel(int serviceRequestId, OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var dto = db.ServiceRequestComments
                .CanAccess(LoggedInUserId) //this filter is duplicated in the ServiceRequestDto.FromServiceRequestEntity projection
                .Where(c => c.ServiceRequestId == serviceRequestId)
                .Select(CommentDto.FromServiceRequestCommentEntity.Expand())
                .ToList();

            ServiceRequestId = serviceRequestId;
            Comments = dto.Select(c => CommentViewModel.FromCommentDto(c));
        }
        public int ServiceRequestId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}