﻿using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Team;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity.Owin;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.ViewModels.DiagnosticViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class DiagnosticsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CodeFirstContextTests()
        {
            var db = new Orvosi.Data.OrvosiDbContext();
            return View(db.Invoices.First());
        }
        public ActionResult ConnectToBox()
        {
            return PartialView();
        }
        public ActionResult CreateBoxFolder()
        {
            //var box = new BoxManager();
            //using (var db = new OrvosiDbContext(User.Identity.Name))
            //{
            //    var request = db.ServiceRequests.Single(sr => sr.Id == 131);
            //    var caseFolder = box.CreateCaseFolder("7027883033", request.ProvinceName, request.AppointmentDate.Value, request.Title, );
            //    return PartialView(caseFolder);
            //}
            return new HttpNotFoundResult();
        }
        // GET: Diagnostics
        public async Task<ActionResult> SendEmail(string to)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            await userManager.SendEmailAsync(user.Id, "Test Email Subject", "Test Email Body");
            return PartialView();
        }
        public async Task<ActionResult> SendActivationEmail(string email)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(email);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendActivationEmail(user.Email, user.UserName, callbackUrl, AspNetRoles.Company);

            return PartialView("SendEmail");
        }
        public async Task<ActionResult> SendResetPasswordEmail(string email)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByEmailAsync(email);
            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var messenger = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.DnsSafeHost);
            await messenger.SendResetPasswordEmail(user.Email, user.UserName, callbackUrl);

            return PartialView("SendEmail");
        }
        public async Task<ActionResult> DropboxViewer()
        {
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();
            var teamInfo = await dropbox.TeamClient.Team.GetInfoAsync();
            ViewBag.MemberPolicy = teamInfo.Policies.Sharing.SharedFolderMemberPolicy;
            ViewBag.JoinPolicy = teamInfo.Policies.Sharing.SharedFolderJoinPolicy;
            ViewBag.Path = "/test folder 1";
            ListFolderResult model = await client.Files.ListFolderAsync(ViewBag.Path);
            return View(model);
        }
        public async Task<ActionResult> DropboxCreateFolder(string Path, string FolderName)
        {
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();
            var args = new CreateFolderArg(string.Join("/", Path, FolderName));
            var model = await client.Files.CreateFolderAsync(args);
            return RedirectToAction("DropboxViewer");
        }
        public async Task<ActionResult> DropboxShareFolder(string Path)
        {
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();
            var args = new ShareFolderArg(Path, MemberPolicy.Team.Instance);
            await client.Sharing.ShareFolderAsync(args);
            
            return RedirectToAction("DropboxViewer");
        }
        public async Task<ActionResult> DropboxFolderDetails(string Path)
        {
            var dropbox = new OrvosiDropbox();
            var client = await dropbox.GetServiceAccountClientAsync();

            // Get the folder
            var folder = await client.Files.GetMetadataAsync(
                new GetMetadataArg(Path)
            );

            // Get the members for the shared folder
            var sharedMembers = await client.Sharing.ListFolderMembersAsync(
                new ListFolderMembersArgs(folder.AsFolder.SharingInfo.SharedFolderId)
            );
            
            // Get the full member entities of the shared members
            var args = new List<UserSelectorArg>();
            foreach (var m in sharedMembers.Users)
            {
                args.Add(new UserSelectorArg.TeamMemberId(m.User.TeamMemberId));
            }
            var members = await dropbox.TeamClient.Team.MembersGetInfoAsync(args);

            var model = new DropboxFolderDetails()
            {
                Folder = folder,
                Members = members
            };

            return View(model);
        }
        public async Task<ActionResult> DashboardPerformanceTest()
        {
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var context = new OrvosiDbContext();
                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
                var now = SystemTime.Now();
                var userId = new Guid("8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9");

                var requests = new List<GetAssignedServiceRequestsReturnModel>(); // await context.API_GetAssignedServiceRequestsAsync(userId, now);
                //var vm = new WebApp.ViewModels.DashboardViewModels.IndexViewModel(requests, now, userId, this.ControllerContext);
                System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
                return View(requests);
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
            }
            return View();
            
        }
    }
}