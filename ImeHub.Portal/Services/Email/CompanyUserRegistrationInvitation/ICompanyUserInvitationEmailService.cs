using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.CompanyUserRegistrationInvitation
{
    public interface ICompanyUserRegistrationInvitationEmailService
    {
        Task SendEmailAsync(string toAddress, CompanyUserRegistrationInvitationTemplateData templateData);
    }
}
