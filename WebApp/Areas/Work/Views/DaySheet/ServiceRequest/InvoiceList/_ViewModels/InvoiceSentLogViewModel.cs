using System;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class InvoiceSentLogViewModel
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public DateTime SentDate { get; set; }
        public string ModifiedUser { get; set; }

        public static Expression<Func<InvoiceSentLogDto, InvoiceSentLogViewModel>> FromInvoiceSentLogDto = r => new InvoiceSentLogViewModel
        {
            Id = r.Id,
            EmailTo = r.EmailTo,
            SentDate = r.SentDate,
            ModifiedUser = r.ModifiedUser
        };
    }
}