using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Library
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailMessage message);
    }
}
