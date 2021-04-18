using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email.CompanyUserInvitation
{
    public class CompanyUserInvitationTemplateData
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string InviteUrl { get; set; }
    }
}
