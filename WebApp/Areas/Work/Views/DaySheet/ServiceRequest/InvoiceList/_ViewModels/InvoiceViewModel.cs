using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.Models;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Terms { get; set; }
        public string PaymentDueDate { get; set; }
        public string SubTotal { get; set; }
        public string TaxRateHst { get; set; }
        public string Hst { get; set; }
        public string Total { get; set; }
        public string SentDate { get; set; }
        public string PaymentReceivedDate { get; set; }
        public Guid InvoiceGuid { get; set; }
        public int? ServiceRequestId { get; set; }
        public string AmountPaid { get; set; }
        public string OutstandingBalance { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSent { get; set; }
        public bool IsPartiallyPaid { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedDate { get; set; }

        public ServiceProviderViewModel ServiceProvider { get; set; }
        public CustomerViewModel Customer { get; set; }

        public IEnumerable<InvoiceDetailViewModel> InvoiceDetails { get; set; }
        public int InvoiceDetailCount { get; set; }

        public IEnumerable<ReceiptViewModel> Receipts { get; set; }
        public IEnumerable<InvoiceSentLogViewModel> SentLog { get; set; }

        public static Expression<Func<InvoiceDto, InvoiceViewModel>> FromInvoiceDto = i => new InvoiceViewModel
        {
            Id = i.Id,
            InvoiceNumber = i.InvoiceNumber,
            InvoiceDate = i.InvoiceDate.ToOrvosiDateFormat(),
            Terms = i.Terms,
            SubTotal = i.SubTotal.GetValueOrDefault(0.0M).ToString("C2"),
            TaxRateHst = i.TaxRateHst.HasValue ? i.TaxRateHst.Value.ToString("0%") : string.Empty,
            Hst = i.Hst.GetValueOrDefault(0.0M).ToString("C2"),
            Total = i.Total.GetValueOrDefault(0.0M).ToString("C2"),
            SentDate = i.SentDate.ToOrvosiDateFormat(),
            PaymentReceivedDate = i.PaymentReceivedDate.ToOrvosiDateFormat(),
            InvoiceGuid = i.InvoiceGuid,
            ServiceRequestId = i.ServiceRequestId,
            AmountPaid = i.AmountPaid.ToString("C2"),
            OutstandingBalance = i.OutstandingBalance.ToString("C2"),
            IsPaid = i.IsPaid,
            IsSent = i.IsSent,
            IsPartiallyPaid = i.IsPartiallyPaid,
            IsDeleted = i.IsDeleted,
            CreatedDate = i.CreatedDate.ToOrvosiDateFormat(),

            ServiceProvider = ServiceProviderViewModel.FromServiceProviderDto.Compile().Invoke(i.ServiceProvider),
            Customer = CustomerViewModel.FromCustomerDto.Compile().Invoke(i.Customer),

            InvoiceDetails = i.InvoiceDetails.Select(InvoiceDetailViewModel.FromInvoiceDetailDto.Compile()),
            InvoiceDetailCount = i.InvoiceDetailCount,

            Receipts = i.Receipts.Select(ReceiptViewModel.FromReceiptDto.Compile()),
            SentLog = i.SentLog.Select(InvoiceSentLogViewModel.FromInvoiceSentLogDto.Compile())
        };

        public static Expression<Func<InvoiceDto, InvoiceViewModel>> FromInvoiceDtoForInvoiceMenu = i => new InvoiceViewModel
        {
            Id = i.Id,
            InvoiceGuid = i.InvoiceGuid,
            SentDate = i.SentDate.ToOrvosiDateFormat(),
            Total = i.Total.GetValueOrDefault(0.0M).ToString("C2"),
            IsPaid = i.IsPaid,
            IsSent = i.IsSent,
            IsPartiallyPaid = i.IsPartiallyPaid,
            Receipts = i.Receipts.Select(ReceiptViewModel.FromReceiptDto.Compile()),
            InvoiceDetails = i.InvoiceDetails.Select(InvoiceDetailViewModel.FromInvoiceDetailDto.Compile())
        };
    }

}