using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Comment;

namespace WebApp.Views.Teleconference
{
    public class TeleconferenceListViewModel
    {
        public int ServiceRequestId { get; set; }
        public IEnumerable<TeleconferenceViewModel> Teleconferences { get; set; }
        public IEnumerable<CommentViewModel> Notes { get; set; }

        public static Func<ServiceRequestDto, TeleconferenceListViewModel> FromServiceRequestDto = dto => dto == null ? null : new TeleconferenceListViewModel
        {
            ServiceRequestId = dto.Id,
            Teleconferences = dto.Teleconferences.Select(TeleconferenceViewModel.FromTeleconferenceDtoForDaySheet),
            Notes = dto.Comments.Where(c => c.CommentTypeId == CommentTypes.Teleconference)
                .Select(CommentViewModel.FromCommentDto)
        };
    }

    public class TeleconferenceDayListViewModel
    {
        public TeleconferenceDayListViewModel()
        {

        }
        public TeleconferenceDayListViewModel(DateTime day, IEnumerable<TeleconferenceDto> teleconferences)
        {
            Day = day;
            Teleconferences = teleconferences.Select(TeleconferenceViewModel.FromTeleconferenceDtoForDaySheet);
        }
        public DateTime Day { get; set; }
        public IEnumerable<TeleconferenceViewModel> Teleconferences { get; set; }
    }
}