using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class LookupViewModel<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public static Expression<Func<LookupDto<T>, LookupViewModel<T>>> FromLookupDto = e => e == null ? null : new LookupViewModel<T>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<short>, LookupViewModel<short>>> FromServiceDto = e => e == null ? null : new LookupViewModel<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<short>, LookupViewModel<short>>> FromCompanyDto = e => e == null ? null : new LookupViewModel<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<Guid>, LookupViewModel<Guid>>> FromRoleDto = e => e == null ? null : new LookupViewModel<Guid>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<byte>, LookupViewModel<byte>>> FromCommentTypeDto = e => e == null ? null : new LookupViewModel<byte>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<short>, LookupViewModel<short>>> FromServiceRequestStatusDto = e => e == null ? null : new LookupViewModel<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<short>, LookupViewModel<short>>> FromNextTaskStatusForUserDto = e => e == null ? null : new LookupViewModel<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<LookupDto<short>, LookupViewModel<short>>> FromTaskStatusDto = e => e == null ? null : new LookupViewModel<short>
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode
        };

        public static Expression<Func<PersonDto, LookupViewModel<Guid>>> FromPersonDto = e => e == null ? null : new LookupViewModel<Guid>
        {
            Id = e.Id,
            Name = e.DisplayName,
            Code = e.Initials,
            ColorCode = e.ColorCode
        };

        public class LookupViewModelEquals : IEqualityComparer<LookupViewModel<T>>
        {
            public bool Equals(LookupViewModel<T> left, LookupViewModel<T> right)
            {
                if ((object)left == null && (object)right == null)
                {
                    return true;
                }
                if ((object)left == null || (object)right == null)
                {
                    return false;
                }
                return left.Id.ToString() == right.Id.ToString() && left.Name == right.Name;
            }

            public int GetHashCode(LookupViewModel<T> obj)
            {
                return (obj.Id.ToString() + obj.Name).GetHashCode();
            }
        }
    }
}