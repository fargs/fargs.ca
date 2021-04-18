using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.ConfirmEmail
{
    public class ConfirmEmailLocalhostEmailService : IConfirmEmailEmailService
    {
        public const string Host = "localhost";
        public LocalhostOptions Options { get; set; }
        public ConfirmEmailLocalhostEmailService(IOptions<LocalhostOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public async Task SendEmailAsync(string email, ConfirmEmailTemplateData templateData)
        {
            var subject = "IME HUB - Confirm your email";
            var message = $"Hi {templateData.Name}, Confirm your email by <a href=\"{templateData.ConfirmEmailUrl}\">clicking this link</a>.";
            var mailMessage = new MailMessage(Options.FromAddress, email, subject, message);

            await Execute(mailMessage);
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
