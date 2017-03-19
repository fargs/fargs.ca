using LinqKit;
using Orvosi.Data;
using Orvosi.Shared;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public int ServiceRequestId { get; set; }
        public string TimeZone { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public ContactDto PostedBy { get; set; }
        public DateTime PostedDateLocal
        {
            get
            {
                return PostedDate.ToLocalTimeZone(TimeZone);
            }
        }

        public static Expression<Func<ServiceRequestMessage, MessageDto>> FromServiceRequestMessageEntity = e => e == null ? null : new MessageDto
        {
            Id = e.Id,
            ServiceRequestId = e.ServiceRequestId,
            TimeZone = e.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : e.ServiceRequest.Address.TimeZone.Name,
            Message = e.Message,
            PostedDate = e.PostedDate,
            PostedBy = ContactDto.FromAspNetUserEntity.Invoke(e.AspNetUser),
        };
    }
}