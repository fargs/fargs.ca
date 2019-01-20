using ImeHub.Data;
using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Physicians.Views.Physician;
using WebApp.Areas.Shared;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Library.Helpers;
using ImeHub.Models;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;
using Features = ImeHub.Models.Enums.Features.UserPortal;
using WebApp.Models;
using WebApp.Areas.Physicians.Views.Physician.Address;

namespace WebApp.Areas.Physicians.Controllers
{
    [AuthorizeRole(Feature = Features.Physicians.Manage)]
    public class PhysicianController : Controller
    {
        private DateTime now;
        private IIdentity identity;
        private ImeHubDbContext db;
        private IEmailService emailService;

        public PhysicianController(ImeHubDbContext db, IEmailService emailService, DateTime now, IPrincipal principal)
        {
            this.now = now;
            this.identity = principal.Identity;
            this.db = db;
            this.emailService = emailService;
        }

        public ActionResult Index(Guid? physicianId)
        {
            var list = new ListViewModel(physicianId, db, identity, now);

            ReadOnlyViewModel readOnly = null;
            if (physicianId.HasValue)
            {
                readOnly = new ReadOnlyViewModel(physicianId.Value, db, identity, now);
            }

            var viewModel = new IndexViewModel(list, readOnly, identity, now);

            if (Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }

        public ActionResult TermsAndConditions(Guid physicianId)
        {
            var owner = db.PhysicianOwners.SingleOrDefault(p => p.PhysicianId == physicianId);

            var formModel = new TermsAndConditionsFormModel(physicianId);
            return View("TermsAndConditions", formModel);
        }
        [HttpPost]
        public async Task<ActionResult> TermsAndConditionsAsync(TermsAndConditionsFormModel viewModel)
        {
            var owner = db.PhysicianOwners
                .SingleOrDefault(u => u.PhysicianId == viewModel.PhysicianId);
            owner.AcceptanceStatusId = (byte)Enums.AcceptanceStatus.Accepted;
            owner.UserId = identity.GetGuidUserId();
            owner.AcceptanceStatusChangedDate = now;

            var teamRole = new TeamRole
            {
                Id = Guid.NewGuid(),
                PhysicianId = owner.PhysicianId,
                Name = "Physician"
            };

            db.TeamRoles.Add(teamRole);

            await db.SaveChangesAsync();

            var teamMember = new TeamMember
            {
                Id = Guid.NewGuid(),
                PhysicianId = owner.PhysicianId,
                RoleId = teamRole.Id,
                UserId = owner.UserId.Value
            };

            db.TeamMembers.Add(teamMember);

            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }

        #region Views

        public PartialViewResult List(Guid? physicianId)
        {
            var viewModel = new ListViewModel(physicianId, db, identity, now);

            return PartialView(viewModel);
        }

        public PartialViewResult ReadOnly(Guid physicianId)
        {
            var readOnly = new ReadOnlyViewModel(physicianId, db, identity, now);

            return PartialView(readOnly);
        }

        public PartialViewResult ShowNewPhysicianForm()
        {
            var formModel = new NewPhysicianFormModel();

            return PartialView("NewPhysicianForm", formModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveNewPhysicianForm(NewPhysicianFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("NewPhysicianForm", form);
            }

            var physician = new Physician
            {
                Id = Guid.NewGuid(),
                CompanyName = form.CompanyName,
                ManagerId = identity.GetGuidUserId()
            };

            db.Physicians.Add(physician);

            await db.SaveChangesAsync();

            return Json(new
            {
                id = physician.Id
            });
        }

        public PartialViewResult OwnerInvitationForm(Guid physicianId)
        {
            var formModel = new OwnerInvitationFormModel(physicianId, identity);

            return PartialView(formModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOwnerInvitationForm(OwnerInvitationFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("OwnerInvitationForm", form);
            }

            var owner = new PhysicianOwner
            {
                PhysicianId = form.PhysicianId,
                Email = form.Email,
                Title = form.Title,
                FirstName = form.FirstName,
                LastName = form.LastName,
                AcceptanceStatusId = (byte)Enums.AcceptanceStatus.NotSent,
                AcceptanceStatusChangedDate = now
            };

            db.PhysicianOwners.Add(owner);

            await db.SaveChangesAsync();

            return Json(new
            {
                physicianId = owner.PhysicianId
            });
        }

        public PartialViewResult ShowDeleteConfirmation(Guid physicianId)
        {
            var formModel = new PhysicianFormModel(physicianId, db);

            return PartialView("DeleteConfirmation", formModel);
        }

        public async Task<ActionResult> SendOwnerInvitationEmail(Guid physicianId)
        {
            var physician = db.Physicians.AsNoTracking().AsExpandable().Select(PhysicianModel.FromPhysician)
                .Where(p => p.Id == physicianId)
                .Single();

            if (physician.Owner.AcceptanceStatusId == (byte)Enums.AcceptanceStatus.Accepted || physician.Owner.AcceptanceStatusId == (byte)Enums.AcceptanceStatus.Rejected)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = new PhysicianInviteViewModel(physician);

            var mailMessage = BuildOwnerInviteEmail(viewModel);

            //******* Google to Send Email *************
            var emailProvider = new GoogleAuthentication(); // eventually I want to DI this into here
            var emailProviderAuthResult = await emailProvider.AuthenticateOauthAsync(this, db, identity.GetGuidUserId(), new CancellationToken());
            // if access token and refresh token are expired
            if (emailProviderAuthResult.Credential == null) return Redirect(emailProviderAuthResult.RedirectUri);

            var emailService = emailProvider.GetGmailService(emailProviderAuthResult.Credential);
            await emailProvider.SendEmailAsync(emailService, mailMessage);
            //******* Google to Send Email *************

            var entity = db.PhysicianOwners
                .Where(pi => pi.PhysicianId == physician.Id)
                .Single();
            entity.AcceptanceStatusChangedDate = now;
            entity.AcceptanceStatusId = (byte)Enums.AcceptanceStatus.NotResponded;

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        private MailMessage BuildOwnerInviteEmail(PhysicianInviteViewModel invite)
        {
            var message = new MailMessage();
            message.To.Add(invite.To);
            message.From = new MailAddress("lesliefarago@gmail.com");
            message.Subject = string.Format("Invitation to ImeHub");
            message.IsBodyHtml = true;

            ViewData["BaseUrl"] = Url.Content("~"); //This is needed because the full address needs to be included in the email download link
            message.Body = HtmlHelpers.RenderPartialViewToString(this, "OwnerInvitationNotification", invite);

            return message;
        }

        [AuthorizeRole(Feature = Features.Physicians.Manage)]
        public PartialViewResult ShowNewAddressForm(Guid physicianId)
        {
            var formModel = new AddressFormModel(physicianId, db);

            return PartialView("Address/AddressForm", formModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Physicians.Manage)]
        public async Task<ActionResult> SaveNewAddressForm(AddressFormModel form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                form.ViewData = new AddressFormModel.ViewDataModel(db, form.PhysicianId);
                return PartialView("Address/AddressForm", form);
            }

            Guid cityId = Guid.NewGuid();

            // use the existing city if found, otherwise create a new city and use the Id.
            var city = db.Cities.FirstOrDefault(c => c.Name == form.City && c.ProvinceId == form.ProvinceId);
            if (city == null)
            {
                city = new City
                {
                    Id = cityId,
                    Name = form.City,
                    ProvinceId = form.ProvinceId,
                    PhysicianId = form.PhysicianId
                };
                db.Cities.Add(city);
            }
            else
            {
                cityId = city.Id;
            }

            var address = new Address
            {
                Id = Guid.NewGuid(),
                PhysicianId = form.PhysicianId,
                Name = form.Name,
                CityId = cityId,
                PostalCode = form.PostalCode,
                TimeZoneId = form.TimeZoneId,
                Address1 = form.Address1,
                Address2 = form.Address2,
                AddressTypeId = (byte)Enums.AddressType.CompanyAssessmentOffice,
                IsBillingAddress = form.IsBillingAddress
            };
            db.Addresses.Add(address);


            await db.SaveChangesAsync();

            return Json(new
            {
                id = address.Id
            });
        }

    }
}