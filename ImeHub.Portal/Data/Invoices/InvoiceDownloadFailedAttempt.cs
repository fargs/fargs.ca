using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Invoices
{
    public class InvoiceDownloadFailedAttempt
    {
        public int Id { get; set; }
        public Guid? InvoiceObjectGuid { get; set; }
        public Guid? InvoiceDownloadLinkObjectGuid { get; set; }
        public DateTime? FailedAttemptDate { get; set; }
        public string DownloadBy { get; set; }
        public string EmailSentTo { get; set; }
        public string ErrorMessage { get; set; }
    }
}
