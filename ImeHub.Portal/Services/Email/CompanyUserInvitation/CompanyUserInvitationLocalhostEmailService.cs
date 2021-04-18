﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.CompanyUserInvitation
{
    public class CompanyUserInvitationLocalhostEmailService : ICompanyUserInvitationEmailService
    {
        public const string Host = "localhost";
        public LocalhostOptions Options { get; set; }
        public CompanyUserInvitationLocalhostEmailService(IOptions<LocalhostOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public async Task SendEmailAsync(string email, CompanyUserInvitationTemplateData templateData)
        {
            var subject = "Invitation to IME HUB";
            var message = $"Hi {templateData.Name}, {templateData.CompanyName} has invited you to IME HUB. Click {templateData.InviteUrl} to sign in. Welcome!";
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
