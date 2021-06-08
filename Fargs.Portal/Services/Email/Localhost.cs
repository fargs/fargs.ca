using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Email
{
    public class Localhost : IEmailService
    {
        public const string Host = "localhost";
        public LocalhostOptions Options { get; set; }
        public Localhost(IOptions<LocalhostOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage(Options.FromAddress, email, subject, message);

            await Execute(mailMessage);
        }

        public async Task SendEmailAsync(MailMessage message)
        {
            await Execute(message);
        }

        private async Task Execute(MailMessage mailMessage)
        {
            // Plug in your email service here to send an email.
            var client = new SmtpClient
            {
                Host = "localhost",
                Port = 25
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
