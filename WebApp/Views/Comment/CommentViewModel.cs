using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.ServiceRequestMessage;
using WebApp.Views.Shared;

namespace WebApp.Views.Comment
{
    public class CommentViewModel : MessageViewModel
    {
        public bool IsPrivate { get; set; }
        public byte CommentTypeId { get; set; }
        public LookupViewModel<byte> CommentType { get; set; }
        public IEnumerable<ContactViewModel<Guid>> AccessList { get; set; }

        public static Func<CommentDto, CommentViewModel> FromCommentDto = dto => dto == null ? null : new CommentViewModel
        {
            Id = dto.Id,
            TimeZone = dto.TimeZone,
            Message = dto.Message,
            PostedDate = dto.PostedDateLocal,
            PostedBy = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(dto.PostedBy),
            IsPrivate = dto.IsPrivate,
            CommentTypeId = dto.CommentTypeId,
            CommentType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.CommentType),
            AccessList = dto.AccessList.AsQueryable().Select(ContactViewModel<Guid>.FromContactDto)
        };
    }
}