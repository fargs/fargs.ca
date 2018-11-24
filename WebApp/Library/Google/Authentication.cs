using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApp.Library.GoogleHelpers;
using Google.Apis.Auth.OAuth2.Mvc;
using ImeHub.Data;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Gmail.v1;
using MimeKit;
using System.Net.Mail;

namespace WebApp.Library
{
    public class GoogleAuthentication
    {

         
        /// <summary>
        /// Authenticate to Google Using Oauth2
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
        /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
        /// <param name="userName">A string used to identify a user.</param>
        /// <returns></returns>
        public async Task<AuthorizationCodeWebApp.AuthResult> AuthenticateOauthAsync(Controller controller, IImeHubDbContext db, Guid userId, CancellationToken cancellationToken)
        {

            var flow = new AppFlowMetadata(db, userId);
            var app = new AuthorizationCodeMvcApp(controller, flow);
            return await app.AuthorizeAsync(cancellationToken);
        }

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="keyFilePath">Location of the Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns></returns>
        public static CalendarService AuthenticateServiceAccount(string serviceAccountEmail, string keyFilePath, string userNameToImpersonate = null)
        {

            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }

            string[] scopes = new string[] {
                CalendarService.Scope.Calendar  ,  // Manage your calendars
                CalendarService.Scope.CalendarReadonly    // View your Calendars
            };

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        User = userNameToImpersonate,
                        Scopes = scopes
                    }.FromCertificate(certificate));


                // Create the service.
                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }
        }

        public GmailService GetGmailService(ICredential credential)
        {
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ImeHub"
            });
            return service;
        }
        public async Task SendEmailAsync(GmailService service, MailMessage message)
        {
            var mimeMessage = MimeMessage.CreateFromMailMessage(message);
            var base64EncodedText = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(mimeMessage.ToString());
            var googleMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = base64EncodedText
            };

            // Create the service.
            var request = service.Users.Messages.Send(googleMessage, message.From.Address);
            await request.ExecuteAsync();
        }
    }
}