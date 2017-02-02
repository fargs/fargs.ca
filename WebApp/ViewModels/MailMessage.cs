using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApp.ViewModels
{
    public class MailMessageViewModel
    {
        public int InvoiceId { get; set; }
        public int? ServiceRequestId { get; set; }
        public MailMessage Message { get; set; }
    }
}