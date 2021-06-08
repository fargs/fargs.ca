using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fargs.Portal.Library.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid UserId(this ClaimsPrincipal obj)
            => new Guid(obj.FindFirstValue(AuthorizationClaimTypes.UserId));
        public static string Email(this ClaimsPrincipal obj)
            => obj.FindFirstValue(ClaimTypes.Email);
        public static string DisplayName(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.DisplayName) ?? string.Empty;
        public static string Initials(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.Initials) ?? string.Empty;
        public static string ColorCode(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.ColorCode) ?? string.Empty;
    }
}
