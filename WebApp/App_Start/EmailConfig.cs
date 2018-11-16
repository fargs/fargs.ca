using Microsoft.AspNet.Identity;
using SendGrid;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System;
using Orvosi.Shared.Enums;
using WebApp.Library.Extensions;
using WebApp.Library;

namespace WebApp
{

    public class MessagingService
    {
        public IEmailService service;
        private string _templateFolder;
        private string _logoPath;
        private const string FromEmail = "support@imehub.com";
        private const string BccEmail = "lesliefarago@gmail.com";

        public MessagingService(string templateFolder, string host)
        {
            _templateFolder = templateFolder;
            _logoPath = "/Content/images/OrvosiBranding/logo-orvosi-md.png";
            // not in debug mode and not local request => we are in production
            service = MessagingService.CreateService();
        }

        private static IEmailService CreateService()
        {
            var mailSystem = ConfigurationManager.AppSettings["MailSystem"];
            if (mailSystem == "Gmail")
            {
                return new GoogleServices();
            }
            return new LocalEmailService();

        }

        public static IEmailService GetService()
        {
            return CreateService();
        }

        public async Task<bool> SendActivationEmail(string email, string userName, string callbackUrl)
        {
            var message = new MailMessage();
            message.To.Add(email);
            message.From = new MailAddress(FromEmail);
            message.Subject = "ImeHub - Confidential Account Activation";
            message.IsBodyHtml = true;
            message.Bcc.Add(BccEmail);

            var templatePath = Path.Combine(_templateFolder, "PhysicianAccountActivation.html");
            
            StreamReader sr = File.OpenText(templatePath);
            while (sr.Peek() >= 0)
            {
                message.Body += sr.ReadLine().Replace("%URL%", callbackUrl).Replace("%USERNAME%", userName).Replace("%logo%", _logoPath);
            }
            sr.Close();

            await service.SendEmailAsync(message);
            return true;
        }

        public async Task<bool> SendResetPasswordEmail(string email, string userName, string callbackUrl)
        {
            var message = new MailMessage();
            message.To.Add(email);
            message.From = new MailAddress(FromEmail);
            message.Subject = "ImeHub - Confidential Password Reset";
            message.IsBodyHtml = true;
            message.Bcc.Add(BccEmail);

            var templatePath = Path.Combine(_templateFolder, "ResetPassword.html");

            StreamReader sr = File.OpenText(templatePath);
            while (sr.Peek() >= 0)
            {
                message.Body += sr.ReadLine().Replace("%URL%", callbackUrl).Replace("%USERNAME%", userName).Replace("%logo%", _logoPath);
            }
            sr.Close();

            await service.SendEmailAsync(message);
            return true;
        }

        public async Task<bool> SendInvoice(Orvosi.Data.Invoice invoice, string callbackUrlBase)
        {
            var message = new MailMessage();
            message.To.Add(invoice.CustomerEmail);
            message.From = new MailAddress(invoice.ServiceProviderEmail);
            message.Subject = string.Format("Invoice {0} - {1} - Payment Due {2}", invoice.InvoiceNumber, invoice.ServiceProviderName, invoice.DueDate.Value.ToOrvosiDateFormat());
            message.IsBodyHtml = true;
            message.Bcc.Add(BccEmail);

            var templatePath = Path.Combine(_templateFolder, "Invoice.html");


            var url = string.Format("{0}/Invoice/Download/{1}", callbackUrlBase, invoice.ObjectGuid.ToString());
            StreamReader sr = File.OpenText(templatePath);
            while (sr.Peek() >= 0)
            {
                message.Body += sr.ReadLine().Replace("%DOWNLOADLINK%", url).Replace("%SERVICEPROVIDERNAME%", invoice.ServiceProviderName);
            }
            sr.Close();

            await service.SendEmailAsync(message);
            return true;
        }

        public async Task<bool> SendTestEmail(string email)
        {
            var message = new MailMessage();

            message.To.Add(email);
            message.From = new MailAddress(FromEmail);
            message.Subject = "ImeHub - Test Email";
            message.IsBodyHtml = true;
            message.Body = "<h1>Hello World</h1>";

            await service.SendEmailAsync(message);
            return true;
        }

        public class SendGridEmailService : IEmailService
        {
            public Task SendEmailAsync(MailMessage message)
            {
                // Plug in your email service here to send an email.
                var myMessage = new SendGridMessage();

                foreach (var item in message.To)
                {
                    myMessage.AddTo(item.Address);
                }
                myMessage.From = new MailAddress(FromEmail);
                myMessage.Subject = message.Subject;
                myMessage.Html = message.Body;

                var credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["SendGridUserName"],
                    ConfigurationManager.AppSettings["SendGridPassword"]
                );

                // Create a Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email.
                if (transportWeb != null)
                {
                    return transportWeb.DeliverAsync(myMessage);
                }
                else
                {
                    //Trace.TraceError("Failed to create Web transport.");
                    return Task.FromResult(0);
                }
            }
        }

        public class LocalEmailService : IEmailService
        {
            public async Task SendEmailAsync(MailMessage message)
            {
                // Plug in your email service here to send an email.
                var client = new SmtpClient
                {
                    Host = "localhost",
                    Port = 25
                };

                await client.SendMailAsync(message);
            }
        }

        public class GmailEmailService : IEmailService
        {
            public async Task SendEmailAsync(MailMessage message)
            {
                // Plug in your email service here to send an email.
                var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["GmailUserName"],
                        ConfigurationManager.AppSettings["GmailPassword"]
                    ),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                await client.SendMailAsync(message);
            }
        }
    }

    public class SendGridEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress("support@imehub.com");
            myMessage.Subject = message.Subject;
            //myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["SendGridUserName"],
                ConfigurationManager.AppSettings["SendGridPassword"]
            );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                return Task.FromResult(0);
            }
        }
    }

    public class LocalEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var client = new SmtpClient
            {
                Host = "localhost",
                Port = 25
            };

            var @from = new MailAddress("support@imehub.com");
            var to = new MailAddress(message.Destination);

            var mail = new MailMessage(@from, to)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true,
            };

            await client.SendMailAsync(mail);
        }
    }

}