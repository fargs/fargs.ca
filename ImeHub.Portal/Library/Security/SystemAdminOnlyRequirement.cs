using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImeHub.Portal.Library.Security
{
    public class IsSystemAdminRequirement : ClaimsAuthorizationRequirement
    {
        public IsSystemAdminRequirement() : base(ClaimTypes.Email, new[] { "lesliefarago@gmail.com", "lfarago@orvosi.ca", "leslie.farago@fargs.ca" })
        {

        }
    }
}
