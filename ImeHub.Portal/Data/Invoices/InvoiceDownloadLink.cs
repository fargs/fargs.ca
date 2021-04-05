using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Invoices
{
    public partial class InvoiceDownloadLink
    {
        public int Id { get; set; }
        public Guid ObjectGuid { get; set; }
        public int InvoiceId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AllowedAttempts { get; set; }

        public virtual ICollection<InvoiceDownload> InvoiceDownloads { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
