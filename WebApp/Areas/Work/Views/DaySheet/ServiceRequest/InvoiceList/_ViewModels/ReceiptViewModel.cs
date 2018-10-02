using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class ReceiptViewModel
    {
        public Guid Id { get; set; }
        public string Amount { get; set; }
        public DateTime ReceivedDate { get; set; }

        public static Expression<Func<ReceiptDto, ReceiptViewModel>> FromReceiptDto = r => new ReceiptViewModel
        {
            Id = r.Id,
            Amount = r.Amount.ToString("C2"),
            ReceivedDate = r.ReceivedDate
        };
    }
}