using Microsoft.AspNet.Identity;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApp
{

    public class MessagingService
    {
        public static IEmailService GetService()
        {
            // not in debug mode and not local request => we are in production
            if (ConfigurationManager.AppSettings["MailSystem"] == "SendGrid")
            {
                return new SendGridEmailService();
            }
            return new LocalEmailService();
        }

        public interface IEmailService
        {
            Task SendAsync(MailMessage message);
        }

        public class SendGridEmailService : IEmailService
        {
            public Task SendAsync(MailMessage message)
            {
                // Plug in your email service here to send an email.
                var myMessage = new SendGridMessage();
                foreach (var item in message.To)
                {
                    myMessage.AddTo(item.Address);
                }
                myMessage.From = new MailAddress("security@orvosi.ca");
                myMessage.Subject = message.Subject;
                myMessage.Text = message.Body;
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
            public async Task SendAsync(MailMessage message)
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
    }

    public class SendGridEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress("security@orvosi.ca");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
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

            var @from = new MailAddress("security@orvosi.ca");
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