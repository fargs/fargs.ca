using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.HtmlToPdf
{
    public class Html2PdfRocketOptions
    {
        public const string SectionName = "HtmlToPdf:Html2PdfRocket";
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
