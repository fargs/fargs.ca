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
        public string ClaimantName { get; set; }
        public LookupDto<short> Company { get; set; }
        public string SourceCompany { get; set; }
        public LookupDto<short> Service { get; set; }
        public byte? MedicolegalTypeId { get; set; }
        public LookupDto<byte> MedicolegalType { get; set; }
        public int CommentCount { get; set; }
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
        public static Expression<Func<Teleconference, TeleconferenceDto>> FromEntityForDaySheet = e => e == null ? null : new TeleconferenceDto
        {
            Id = e.Id,
            ServiceRequestId = e.ServiceRequestId,
            AppointmentDate = e.AppointmentDate,
            StartTime = e.StartTime,
            ClaimantName = e.ServiceRequest.ClaimantName,
            Company = LookupDto<short>.FromCompanyEntity.Invoke(e.ServiceRequest.Company),
            SourceCompany = e.ServiceRequest.SourceCompany,
            Service = LookupDto<short>.FromServiceEntity.Invoke(e.ServiceRequest.Service),
            MedicolegalTypeId = e.ServiceRequest.MedicolegalTypeId,
            MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(e.ServiceRequest.MedicolegalType),
            CommentCount = e.ServiceRequest.ServiceRequestComments.Where(c => c.CommentTypeId == CommentTypes.Teleconference).Count(),
            ResultSentDate = e.TeleconferenceResultSentDate,
            ResultTypeId = e.TeleconferenceResultId,
            ResultType = LookupDto<byte>.FromTeleconferenceResultEntity.Invoke(e.TeleconferenceResult)
        };
    }
}