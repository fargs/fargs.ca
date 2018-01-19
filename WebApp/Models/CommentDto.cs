using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace WebApp.Models
{
    public class CommentDto : MessageDto
    {
        public byte CommentTypeId { get; set; }
        public LookupDto<byte> CommentType { get; set; }
        public bool IsPrivate { get; set; }
        public IEnumerable<ContactDto> AccessList { get; set; }
        public ContactDto Physician { get; set; }

        public static Expression<Func<ServiceRequestComment, CommentDto>> FromServiceRequestCommentEntity = e => new CommentDto
        {
            Id = e.Id,
            ServiceRequestId = e.ServiceRequestId,
            TimeZone = e.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : e.ServiceRequest.Address.TimeZone.Name,
            Message = e.Comment,
            IsPrivate = e.IsPrivate,
            CommentTypeId = e.CommentTypeId,
            CommentType = LookupDto<byte>.FromCommentTypeEntity.Invoke(e.CommentType),
            PostedDate = e.PostedDate,
            PostedBy = ContactDto.FromAspNetUserEntity.Invoke(e.AspNetUser),
            AccessList = e.ServiceRequestCommentAccesses.AsQueryable().Select(ContactDto.FromServiceRequestCommentAccessEntity.Expand())
        };

        public static Expression<Func<ServiceRequestComment, CommentDto>> ForCommentForm = e => new CommentDto
        {
            Id = e.Id,
            ServiceRequestId = e.ServiceRequestId,
            Message = e.Comment,
            CommentTypeId = e.CommentTypeId,
            IsPrivate = e.IsPrivate,
            Physician = ContactDto.FromAspNetUserEntity.Invoke(e.ServiceRequest.Physician.AspNetUser),
            AccessList = e.ServiceRequestCommentAccesses.AsQueryable().Select(ContactDto.FromServiceRequestCommentAccessEntity.Expand())
        };
    }
}