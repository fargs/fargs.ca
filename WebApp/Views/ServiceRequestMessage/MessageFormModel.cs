using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.ServiceRequestMessage
{
    public class MessageFormModel
    {
        public int ServiceRequestId { get; set; }
        
        public static Func<ServiceRequestDto, MessageFormModel> FromServiceRequestDto = dto => FromServiceRequestDtoExpr.Compile().Invoke(dto);
        public static Expression<Func<ServiceRequestDto, MessageFormModel>> FromServiceRequestDtoExpr = dto => dto == null ? null : new MessageFormModel
        {
            ServiceRequestId = dto.Id
        };
    }
}