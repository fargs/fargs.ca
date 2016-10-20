using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using WebApp.Library.Extensions;
using System;
using System.Threading.Tasks;
using Orvosi.Data;
using MimeKit;

namespace WebApp.Library
{
    public class GoogleServices : IEmailService
    {
        public CalendarService GetCalendarService(string userId)
        {
            string[] scopes = new string[] {
                CalendarService.Scope.Calendar,  // Manage your calendars
                CalendarService.Scope.CalendarReadonly, // View your Calendars
            };

            var credential = GetServiceAccountCredential(userId, scopes);

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Orvosi"
            });

            return service;
        }
        public GmailService GetGmailService(string userId)
        {
            string[] scopes = new string[] {
                GmailService.Scope.GmailCompose,
                GmailService.Scope.GmailSend
            };

            var credential = GetServiceAccountCredential(userId, scopes);

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Orvosi"
            });

            return service;
        }

        public GoogleServices()
        {
            this.configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orvosi-b54037435bbf.json");
            this.settings = ServiceAccountJsonSettings.Load(configFile);
        }

        public async Task SendEmailAsync(MailMessage message)
        {
            var mimeMessage = MimeMessage.CreateFromMailMessage(message);
            byte[] encodedBytes = Encoding.UTF8.GetBytes(mimeMessage.ToString());
            string base64EncodedText = HttpServerUtility.UrlTokenEncode(encodedBytes);

            var googleMessage = new Google.Apis.Gmail.v1.Data.Message();
            googleMessage.Raw = base64EncodedText;

            // Create the service.
            var gmail = GetGmailService(message.From.Address);
            var request = gmail.Users.Messages.Send(googleMessage, message.From.Address);
            await request.ExecuteAsync();
        }

        private string configFile;
        private ServiceAccountJsonSettings settings;
        private ServiceAccountCredential GetServiceAccountCredential(string userId, string[] scopes)
        {
            return new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(settings.client_id)
                {
                    User = userId,
                    Scopes = scopes
                }.FromPrivateKey(settings.private_key));
        }
    }
    public class ServiceAccountJsonSettings
    {
        public static ServiceAccountJsonSettings Load(string keyFilePath)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, keyFilePath);
            var settings = JObject.Parse(System.IO.File.ReadAllText(filePath));
            return new ServiceAccountJsonSettings()
            {
                type = settings["type"].ToString(),
                project_id = settings["project_id"].ToString(),
                private_key_id = settings["private_key_id"].ToString(),
                private_key = settings["private_key"].ToString(),
                client_email = settings["client_email"].ToString(),
                client_id = settings["client_id"].ToString(),
                auth_uri = settings["auth_uri"].ToString(),
                token_uri = settings["token_uri"].ToString(),
                auth_provider_x509_cert_url = settings["auth_provider_x509_cert_url"].ToString(),
                client_x509_cert_url = settings["client_x509_cert_url"].ToString()
            };
        }
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
    }
}