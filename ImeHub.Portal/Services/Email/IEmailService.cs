using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email
{
    public interface IEmailService : IEmailSender
    {
        Task SendEmailAsync(MailMessage message);
    }
}
