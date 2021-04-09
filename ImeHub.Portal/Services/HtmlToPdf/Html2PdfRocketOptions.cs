using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Services.HtmlToPdf
{
    public class Html2PdfRocketOptions
    {
        public const string SectionName = "Html2Pdf:Html2PdfRocket";
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
