using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TeleconferenceListViewModel
    {
        public int ServiceRequestId { get; set; }
        public IEnumerable<TeleconferenceViewModel> Teleconferences { get; set; }
        public IEnumerable<CommentViewModel> Notes { get; set; }

        public static Expression<Func<CaseViewModel, TeleconferenceListViewModel>> FromCaseViewModel = dto => dto == null ? null : new TeleconferenceListViewModel
        {
            ServiceRequestId = dto.ServiceRequestId,
            Teleconferences = dto.Teleconferences,
            Notes = dto.Comments.Where(c => c.CommentTypeId == CommentTypes.Teleconference)
        };
    }
}