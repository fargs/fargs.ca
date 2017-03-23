using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public string TimeZone { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public LookupViewModel<Guid> PostedBy { get; set; }

        public static Expression<Func<MessageDto, MessageViewModel>> FromMessageDto = dto => dto == null ? null : new MessageViewModel
        {
            Id = dto.Id,
            TimeZone = dto.TimeZone,
            Message = dto.Message,
            PostedDate = dto.PostedDateLocal,
            PostedBy = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.PostedBy)
        };
    }
}