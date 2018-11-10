using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImeHub.Models
{
    public partial class LookupModel<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public static Expression<Func<Data.Role, LookupModel<Guid>>> FromRole = e => e == null ? null : new LookupModel<Guid>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Data.Physician, LookupModel<Guid>>> FromPhysician = e => e == null ? null : new LookupModel<Guid>
        {
            Id = e.Id,
            Name = e.CompanyName,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<Data.PhysicianInviteAcceptanceStatu, LookupModel<byte>>> FromPhysicianInviteAcceptanceStatus = e => e == null ? null : new LookupModel<byte>
        {
            Id = e.Id,
            Name = e.Name,
            Code = null,
            ColorCode = null
        };

    }
}