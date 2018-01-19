using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CommentListViewModel
    {
        public int ServiceRequestId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<CaseViewModel, CommentListViewModel>> FromCaseViewModel = dto => dto == null ? null : new CommentListViewModel
        {
            ServiceRequestId = dto.ServiceRequestId,
            Comments = dto.Comments
        };
    }
}