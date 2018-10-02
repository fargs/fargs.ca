using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class InvoiceDetailViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public bool HasDiscount { get; set; }
        public string Discount { get; set; }
        public string DiscountDescription { get; set; }
        public string Total { get; set; }
        public string AdditionalNotes { get; set; }
        public int InvoiceId { get; set; }
        public int? ServiceRequestId { get; set; }

        public static Expression<Func<InvoiceDetailDto, InvoiceDetailViewModel>> FromInvoiceDetailDto = id => new InvoiceDetailViewModel
        {
            Id = id.Id,
            Description = id.Description,
            Amount = id.Amount.GetValueOrDefault(0).ToString("C2"),
            Rate = id.Rate.ToString("0%"),
            Total = id.Total.GetValueOrDefault(0).ToString("C2"),
            HasDiscount = id.HasDiscount,
            DiscountDescription = id.DiscountDescription,
            AdditionalNotes = id.AdditionalNotes,
            InvoiceId = id.InvoiceId,
            ServiceRequestId = id.ServiceRequestId
        };
    }
}