using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Dashboard.Views.Home;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.Library.GoogleHelpers;

namespace WebApp.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DateTime now;
        private IIdentity identity;
        private Guid userId;
        private ImeHubDbContext db;

        public HomeController(ImeHubDbContext db, IIdentity identity, DateTime now)
        {
            this.now = now;
            this.identity = identity;
            this.userId = identity.GetGuidUserId();
            this.db = db;
        }
        public ActionResult Index(Guid? physicianId)
        {

            var viewModel = new IndexViewModel(db, identity, now);

            if (Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> UseGmail()
        {
            //******* Google to Send Email *************
            //var emailProvider = new GoogleAuthentication(); // eventually I want to DI this into here
            var flow = new AppFlowMetadata(db, userId);
            var app = new AuthorizationCodeMvcApp(this, flow);
            var authResult = await app.AuthorizeAsync(new CancellationToken());

            // if access token and refresh token are expired
            if (authResult.Credential == null) return Redirect(authResult.RedirectUri);

            var entity = db.Users
                .Where(u => u.Id == userId)
                .Single();
            entity.EmailProvider = "Google";

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> GetProfile()
        {
            var user = db.Users
                .Where(u => u.Id == userId)
                .First();

            switch (user.EmailProvider)
            {
                case "Google":
                    //******* Google to Send Email *************
                    //var emailProvider = new GoogleAuthentication(); // eventually I want to DI this into here
                    var flow = new AppFlowMetadata(db, userId);
                    var app = new AuthorizationCodeMvcApp(this, flow);
                    var authResult = await app.AuthorizeAsync(new CancellationToken());

                    // if access token and refresh token are expired
                    if (authResult.Credential == null) return Redirect(authResult.RedirectUri);

                    var service = new GoogleOauthServiceWrapper(authResult.Credential);
                    var gProfile = await service.GetProfileAsync();
                    var profile = new GoogleProfileViewModel
                    {
                        Email = gProfile.Email,
                        Name = gProfile.Name
                    };
                    return PartialView("LinkedProfile", profile);
                default:
                    break;
            }
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
        public async Task<ActionResult> RevokeAccess()
        {
            var user = db.Users
                .Where(u => u.Id == userId)
                .First();

            switch (user.EmailProvider)
            {
                case "Google":
                    //******* Google to Send Email *************
                    //var emailProvider = new GoogleAuthentication(); // eventually I want to DI this into here
                    var flow = new AppFlowMetadata(db, userId);
                    var app = new AuthorizationCodeMvcApp(this, flow);
                    await app.Flow.DeleteTokenAsync(userId.ToString(), new CancellationToken());

                    user.EmailProvider = null;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                default:
                    break;
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> SendTestEmail(string to)
        {
            var user = db.Users
                .AsNoTracking()
                .AsExpandable()
                .Where(u => u.Id == userId)
                .Select(UserModel.FromUser)
                .First();

            switch (user.EmailProvider)
            {
                case "Google":
                    var flow = new AppFlowMetadata(db, userId);
                    var app = new AuthorizationCodeMvcApp(this, flow);
                    var authResult = await app.AuthorizeAsync(new CancellationToken());

                    // if access token and refresh token are expired
                    if (authResult.Credential == null) return Redirect(authResult.RedirectUri);

                    var service = new GmailServiceWrapper(authResult.Credential);
                    await service.SendEmailAsync(new System.Net.Mail.MailMessage(User.Identity.GetEmail(), to));
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                default:
                    break;
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}