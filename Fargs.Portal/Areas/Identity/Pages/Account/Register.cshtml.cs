using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Fargs.Portal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Fargs.Portal.Library.Security;
using Microsoft.EntityFrameworkCore;
using Fargs.Portal.Services.DateTimeService;
using Fargs.Portal.Data.Companies;

namespace Fargs.Portal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IDateTime _dateTime;

        public RegisterModel(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDateTime dateTime)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dateTime = dateTime;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [StringLength(10)]
            [Display(Name = "Title")]
            public string Title { get; set; }

            [Required]
            [StringLength(128)]
            [Display(Name = "Given Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(128)]
            [Display(Name = "Surname")]
            public string LastName { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Invitation Id")]
            public Guid InvitationId { get; set; }

            [Display(Name = "Invitation Code")]
            public string ConfirmInviteCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(Guid invitationId, string returnUrl = null)
        {
            var invitation = await _dbContext.CompanyUserInvitations
                .Where(c => c.ObjectGuid == invitationId)
                .SingleOrDefaultAsync();

            if (invitation == null)
            {
                return Redirect(AnonymousRoutes.NotFound);
            }

            Input = new InputModel
            {
                Title = invitation.Title,
                FirstName = invitation.FirstName,
                LastName = invitation.LastName,
                Email = invitation.Email,
                ConfirmInviteCode = invitation.InviteCode,
                InvitationId = invitation.ObjectGuid
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var invitation = await _dbContext.CompanyUserInvitations
                .Where(c => c.ObjectGuid == Input.InvitationId)
                .SingleOrDefaultAsync();

            if (invitation.InviteCode != Input.ConfirmInviteCode)
            {
                ModelState.AddModelError(nameof(Input.ConfirmInviteCode), "Invitation code is not valid");
                return Page();
            }

            var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                _logger.LogInformation("The user was not created.");
                return Page();
            }

            _logger.LogInformation("User created a new account with password.");

            user.Title = Input.Title;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            var companyRole = await _dbContext.CompanyRoles
                .Where(c => c.Id == invitation.CompanyRoleId)
                .SingleAsync();

            var companyAccess = new CompanyAccess()
            {
                CompanyRole = companyRole,
                ObjectGuid = Guid.NewGuid(),
                UserId = user.Id,
                ModifiedBy = user.Id
            };

            _dbContext.CompanyAccesses.Add(companyAccess);

            invitation.UserId = user.Id;
            invitation.AcceptedDate = _dateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl ??= "~/");
        }
    }
}
