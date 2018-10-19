using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class EditInvoiceItemFormModel
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string InvoiceDate { get; set; }
        public string ClaimantName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Rate { get; set; }
        public string AdditionalNotes { get; set; }
        public int ServiceRequestId { get; internal set; }
    }
}