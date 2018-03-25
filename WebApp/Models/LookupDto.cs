using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class LookupDto<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public static Expression<Func<ServiceRequestStatu, LookupDto<short>>> FromServiceRequestStatusEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<TaskStatu, LookupDto<short>>> FromTaskStatusEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = "",
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Service, LookupDto<short>>> FromServiceEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Company, LookupDto<short>>> FromCompanyEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = ""
        };

        public static Expression<Func<City, LookupDto<short>>> FromCityEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = ""
        };

        public static Expression<Func<AspNetRole, LookupDto<Guid>>> FromAspNetRoleEntity = e => e == null ? null : new LookupDto<Guid>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Name.Substring(0, 2),
            ColorCode = ""
        };

        public static Expression<Func<Province, LookupDto<short>>> FromProvinceEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.ProvinceName,
            Code = e.ProvinceCode,
            ColorCode = ""
        };

        public static Expression<Func<CommentType, LookupDto<byte>>> FromCommentTypeEntity = e => e == null ? null : new LookupDto<byte>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<OTask, LookupDto<short>>> FromTaskEntity = e => e == null ? null : new LookupDto<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.ShortName,
            ColorCode = ""
        };

        public static Expression<Func<TeleconferenceResult, LookupDto<byte>>> FromTeleconferenceResultEntity = e => e == null ? null : new LookupDto<byte>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<MedicolegalType, LookupDto<byte>>> FromMedicolegalTypeEntity = e => e == null ? null : new LookupDto<byte>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };
    }
}