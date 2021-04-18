﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.CompanyUserInvitation
{
    public interface ICompanyUserInvitationEmailService
    {
        Task SendEmailAsync(string toAddress, CompanyUserInvitationTemplateData templateData);
    }
}
