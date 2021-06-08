using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Email
{
    public class LocalhostOptions
    {
        public const string SectionName = "Email:Localhost";
        public string Port { get; set; } = "25";
        public string FromAddress { get; set; } = "no-reply@localhost.ca";
    }
}
