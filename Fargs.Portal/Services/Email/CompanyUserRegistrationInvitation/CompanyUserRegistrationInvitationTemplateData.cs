using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Email.CompanyUserRegistrationInvitation
{
    public class CompanyUserRegistrationInvitationTemplateData
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string InviteUrl { get; set; }
        public string InviteCode { get; set; }
    }
}
