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
    public class TeleconferenceDto
    {
        public Guid Id { get; set; }
        public int ServiceRequestId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? ResultSentDate { get; set; }
        public byte? ResultTypeId { get; set; }
        public LookupDto<byte> ResultType { get; set; }

        public static Expression<Func<Teleconference, TeleconferenceDto>> FromEntity = e => e == null ? null : new TeleconferenceDto
        {
            Id = e.Id,
            ServiceRequestId = e.ServiceRequestId,
            AppointmentDate = e.AppointmentDate,
            StartTime = e.StartTime,
            ResultSentDate = e.TeleconferenceResultSentDate,
            ResultTypeId = e.TeleconferenceResultId,
            ResultType = LookupDto<byte>.FromTeleconferenceResultEntity.Invoke(e.TeleconferenceResult)
        };
    }
}