using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Email
{
    public class SendGridOptions
    {
        public const string SectionName = "Email:SendGrid";
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string FromAddress { get; set; }
        public SendGridTemplates Templates { get; set; }

        public class SendGridTemplates
        {
            public string CompanyUserRegistrationInvitationId { get; set; }
            public string CompanyUserInvitationId { get; set; }
            public string ConfirmEmailId { get; set; }
        }
    }
}
