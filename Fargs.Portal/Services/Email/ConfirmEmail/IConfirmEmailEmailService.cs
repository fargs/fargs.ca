using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Email.ConfirmEmail
{
    public interface IConfirmEmailEmailService
    {
        Task SendEmailAsync(string toAddress, ConfirmEmailTemplateData templateData);
    }
}
