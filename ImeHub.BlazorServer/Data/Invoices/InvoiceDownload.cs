using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.BlazorServer.Data.Invoices
{
    public partial class InvoiceDownload
    {
        public int Id { get; set; }
        public int InvoiceDownloadLinkId { get; set; }
        public DateTime DownloadDate { get; set; }
        public string DownloadedBy { get; set; }
        public string EmailSentTo { get; set; }
    }
}
