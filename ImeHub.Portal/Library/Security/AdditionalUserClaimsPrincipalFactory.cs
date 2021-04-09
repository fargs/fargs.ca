using ImeHub.Portal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImeHub.Portal.Library.Security
{
	public class AdditionalUserClaimsPrincipalFactory
		: UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
	{
		public AdditionalUserClaimsPrincipalFactory(
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			IOptions<IdentityOptions> optionsAccessor)
			: base(userManager, roleManager, optionsAccessor)
		{ }

		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);
			var identity = (ClaimsIdentity)principal.Identity;

			var claims = new List<Claim>();

			claims.Add(new Claim(AuthorizationClaimTypes.UserId, user.Id.ToString()));

            if (user.DisplayName != null) claims.Add(new Claim(AuthorizationClaimTypes.DisplayName, user.DisplayName));
			if (user.Initials != null) claims.Add(new Claim(AuthorizationClaimTypes.Initials, user.Initials));
			if (user.ColorCode != null) claims.Add(new Claim(AuthorizationClaimTypes.ColorCode, user.ColorCode));

			identity.AddClaims(claims);
			return principal;
		}
	}
}
