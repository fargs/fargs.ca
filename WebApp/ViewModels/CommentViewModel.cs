using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CommentViewModel : MessageViewModel
    {
        public bool IsPrivate { get; set; }
        public byte CommentTypeId { get; set; }
        public LookupViewModel<byte> CommentType { get; set; }
        public IEnumerable<ContactViewModel<Guid>> AccessList { get; set; }

        public static Expression<Func<CommentDto, CommentViewModel>> FromCommentDto = dto => dto == null ? null : new CommentViewModel
        {
            Id = dto.Id,
            TimeZone = dto.TimeZone,
            Message = dto.Message,
            PostedDate = dto.PostedDateLocal,
            PostedBy = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.PostedBy),
            IsPrivate = dto.IsPrivate,
            CommentTypeId = dto.CommentTypeId,
            CommentType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.CommentType),
            AccessList = dto.AccessList.AsQueryable().Select(ContactViewModel<Guid>.FromContactDto.Expand())
        };
    }
}