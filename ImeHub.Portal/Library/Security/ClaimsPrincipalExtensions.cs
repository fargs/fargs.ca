using System.Collections.Generic;
using System.Security.Claims;

namespace ImeHub.Portal.Library.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.UserId) ?? string.Empty;
        public static string DisplayName(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.DisplayName) ?? string.Empty;
        public static string Initials(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.Initials) ?? string.Empty;
        public static string ColorCode(this ClaimsPrincipal obj)
            => obj.FindFirstValue(AuthorizationClaimTypes.ColorCode) ?? string.Empty;
    }
}
