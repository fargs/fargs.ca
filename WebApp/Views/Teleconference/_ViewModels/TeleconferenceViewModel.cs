using System;
using System.Linq.Expressions;
using LinqKit;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.Teleconference
{
    public class TeleconferenceViewModel
    {
        public Guid Id { get; set; }
        public int ServiceRequestId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public string ClaimantName { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public string SourceCompany { get; set; }
        public LookupViewModel<short> Service { get; set; }
        public byte? MedicolegalTypeId { get; set; }
        public LookupViewModel<byte> MedicolegalType { get; set; }
        public int CommentCount { get; set; }
        public DateTime? ResultSentDate { get; set; }
        public byte? ResultTypeId { get; set; }
        public LookupViewModel<byte> ResultType { get; set; }

        public static Expression<Func<TeleconferenceDto, TeleconferenceViewModel>> FromTeleconferenceDto = dto => dto == null ? null : new TeleconferenceViewModel
        {
            Id = dto.Id,
            ServiceRequestId = dto.ServiceRequestId,
            AppointmentDate = dto.AppointmentDate,
            StartTime = dto.StartTime,
            ResultSentDate = dto.ResultSentDate,
            ResultTypeId = dto.ResultTypeId,
            ResultType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.ResultType)
        };

        public static Func<TeleconferenceDto, TeleconferenceViewModel> FromTeleconferenceDtoForDaySheet = dto => dto == null ? null : new TeleconferenceViewModel
        {
            Id = dto.Id,
            ServiceRequestId = dto.ServiceRequestId,
            AppointmentDate = dto.AppointmentDate,
            StartTime = dto.StartTime,
            ClaimantName = dto.ClaimantName,
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            SourceCompany = dto.SourceCompany,
            Service = LookupViewModel<short>.FromServiceDtoExpr.Invoke(dto.Service),
            MedicolegalTypeId = dto.MedicolegalTypeId,
            MedicolegalType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.MedicolegalType),
            CommentCount = dto.CommentCount,
            ResultSentDate = dto.ResultSentDate,
            ResultTypeId = dto.ResultTypeId,
            ResultType = LookupViewModel<byte>.FromLookupDto.Invoke(dto.ResultType)
        };
    }
}