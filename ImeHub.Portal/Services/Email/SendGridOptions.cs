using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.Email
{
    public class SendGridOptions
    {
        public const string SectionName = "Email:SendGrid";
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string FromAddress { get; set; }
    }
}
