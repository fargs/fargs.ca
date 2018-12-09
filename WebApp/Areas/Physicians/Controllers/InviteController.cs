using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Physicians.Views.Invite;
using WebApp.Areas.Physicians.Views.Physician;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Physicians.Controllers
{
    [AllowAnonymous]
    public class InviteController : Controller
    {
        private DateTime now;
        private IIdentity identity;
        private ImeHubDbContext db;
        private ApplicationUserManager userManager;
        private ApplicationSignInManager signInManager;

        public InviteController(ImeHubDbContext db, ApplicationUserManager userManager, ApplicationSignInManager signInManager, DateTime now, IPrincipal principal)
        {
            this.now = now;
            this.identity = principal.Identity;
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public ActionResult AcceptOwnerInvite(Guid physicianId)
        {
            var owner = db.PhysicianOwners
                .SingleOrDefault(po => po.PhysicianId == physicianId);

            if (owner == null)
            {
                return View("InvalidOwnerInvite", Enums.AcceptanceStatus.NotSent);
            }
            else if (owner.AcceptanceStatusId != (byte)Enums.AcceptanceStatus.NotResponded)
            {
                return View("InvalidOwnerInvite", (Enums.AcceptanceStatus)owner.AcceptanceStatusId);
            }

            var viewModel = new AcceptOwnerInviteFormModel()
            {
                PhysicianId = physicianId,
                Email = owner.Email
            };
            return View(viewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AcceptOwnerInvite(AcceptOwnerInviteFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var owner = db.PhysicianOwners.SingleOrDefault(p => p.PhysicianId == form.PhysicianId);
            var user = db.Users.SingleOrDefault(u => u.Email == form.Email);

            if (user == null)
            {
                var formModel = new RegisterUserFormModel(form.Email, form.PhysicianId);
                return RedirectToAction("RegisterUser", formModel);
            }
            else
            {
                var formModel = new TermsAndConditionsFormModel(form.PhysicianId);
                return RedirectToAction("TermsAndConditions", "Physician", formModel);
            }
        }
        [AllowAnonymous]
        public ActionResult RegisterUser(Guid physicianId)
        {
            var owner = db.PhysicianOwners.SingleOrDefault(p => p.PhysicianId == physicianId);
            var formModel = new RegisterUserFormModel(owner.Email, owner.PhysicianId);
            formModel.Title = owner.Title;
            formModel.FirstName = owner.FirstName;
            formModel.LastName = owner.LastName;
            return View("RegisterUser", formModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUserAsync(RegisterUserFormModel form)
        {

            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                UserName = form.Email,
                Email = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Title = form.Title,
                RoleId = Enums.Role.Physician,
                ColorCode = "#123123"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            // Take advantage of password management features of ASP.NET Identity
            var result = await userManager.AddPasswordAsync(id, form.Password);

            //  Comment the following line to prevent log in until the user is confirmed.
            await signInManager.PasswordSignInAsync(user.UserName, form.Password, isPersistent: false, shouldLockout: false);

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

            // Uncomment to debug locally 
            //TempData["ViewBagLink"] = callbackUrl;

            //  Uncomment the following line to prevent log in until the user is confirmed.
            //ViewBag.Message = "Check your email and confirm your account, you must be confirmed before you can log in.";
            //return RedirectToAction("Index", "User", new { area = "Admin" });

            //  Comment the following line to prevent log in until the user is confirmed.
            //return RedirectToAction("Index", "Home");
            var formModel = new TermsAndConditionsFormModel(form.PhysicianId);
            return RedirectToAction("TermsAndConditions", "Physician", formModel);
        }
    }
}