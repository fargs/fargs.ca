using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.ServiceRequestMessage
{
    public class DiscussionViewModel
    {
        public int ServiceRequestId { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }

        public static Func<ServiceRequestDto, DiscussionViewModel> FromServiceRequestDto = dto => dto == null ? null : new DiscussionViewModel
        {
            ServiceRequestId = dto.Id,
            Messages = dto.Messages.Select(MessageViewModel.FromMessageDto)
        };
    }
}