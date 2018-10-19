using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class CreateInvoiceFormModel
    {
        public Guid ServiceProviderGuid { get; set; }
        public Guid CustomerGuid { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}