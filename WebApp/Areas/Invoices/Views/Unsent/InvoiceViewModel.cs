using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.ViewModels.CalendarViewModels;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TaxRateHst { get; set; }
        public decimal? Hst { get; set; }
        public decimal? Total { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }
        public ServiceProviderViewModel ServiceProvider { get; set; }
        public CustomerViewModel Customer { get; set; }
        public Guid InvoiceGuid { get; set; }
        public IEnumerable<InvoiceDetailViewModel> InvoiceDetails { get; set; }
        public IEnumerable<ReceiptViewModel> Receipts { get; set; }
        public IEnumerable<InvoiceSentLogViewModel> SentLog { get; set; }
        public int? ServiceRequestId { get; set; }
        public string Terms { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal OutstandingBalance { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSent { get; set; }
        public bool IsPartiallyPaid { get; set; }
        public int InvoiceDetailCount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public static Expression<Func<InvoiceDto, InvoiceViewModel>> FromInvoiceDtoExpr = i => FromInvoiceDto(i);
        public static Func<InvoiceDto, InvoiceViewModel> FromInvoiceDto = i => i == null ? null : new InvoiceViewModel
        {
            Id = i.Id,
            InvoiceNumber = i.InvoiceNumber,
            InvoiceDate = i.InvoiceDate,
            Terms = i.Terms,
            SubTotal = i.SubTotal,
            TaxRateHst = i.TaxRateHst,
            Hst = i.Hst,
            Total = i.Total,
            SentDate = i.SentDate,
            PaymentReceivedDate = i.PaymentReceivedDate,
            InvoiceGuid = i.InvoiceGuid,
            ServiceRequestId = i.ServiceRequestId,
            AmountPaid = i.AmountPaid,
            OutstandingBalance = i.OutstandingBalance,
            IsPaid = i.IsPaid,
            IsSent = i.IsSent,
            IsPartiallyPaid = i.IsPartiallyPaid,
            IsDeleted = i.IsDeleted,
            CreatedDate = i.CreatedDate,

            ServiceProvider = ServiceProviderViewModel.FromServiceProviderDto.Invoke(i.ServiceProvider),
            Customer = CustomerViewModel.FromCustomerDto.Invoke(i.Customer),

            InvoiceDetails = i.InvoiceDetails.AsQueryable().Select(InvoiceDetailViewModel.FromInvoiceDetailDto.Expand()),
            InvoiceDetailCount = i.InvoiceDetailCount,

            Receipts = i.Receipts.AsQueryable().Select(ReceiptViewModel.FromReceiptDto.Expand()),
            SentLog = i.SentLog.AsQueryable().Select(InvoiceSentLogViewModel.FromInvoiceSentLogDto.Expand())
        };

        public static Expression<Func<InvoiceDto, InvoiceViewModel>> FromInvoiceDtoForInvoiceMenu = i => new InvoiceViewModel
        {
            Id = i.Id,
            InvoiceGuid = i.InvoiceGuid,
            SentDate = i.SentDate,
            Total = i.Total,
            IsPaid = i.IsPaid,
            IsSent = i.IsSent,
            IsPartiallyPaid = i.IsPartiallyPaid,
            Receipts = i.Receipts.AsQueryable().Select(ReceiptViewModel.FromReceiptDto.Expand()),
            InvoiceDetails = i.InvoiceDetails.AsQueryable().Select(InvoiceDetailViewModel.FromInvoiceDetailDto.Expand())
        };
    }
    public class ServiceProviderViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public static Expression<Func<ServiceProviderDto, ServiceProviderViewModel>> FromServiceProviderDto = i => new ServiceProviderViewModel
        {

            Id = i.Id,
            Name = i.Name,
            Email = i.Email,
            Province = i.Province
        };
    }
    public class CustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BillingEmail { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public static Expression<Func<CustomerDto, CustomerViewModel>> FromCustomerDto = i => new CustomerViewModel
        {
            Id = i.Id,
            Name = i.Name,
            BillingEmail = i.BillingEmail,
            City = i.City,
            Province = i.Province
        };
    }
    public class InvoiceDetailViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Total { get; set; }
        public string AdditionalNotes { get; set; }
        public int InvoiceId { get; set; }
        public int? ServiceRequestId { get; set; }

        public static Expression<Func<InvoiceDetailDto, InvoiceDetailViewModel>> FromInvoiceDetailDto = id => new InvoiceDetailViewModel
        {
            Id = id.Id,
            Description = id.Description,
            Amount = id.Amount.GetValueOrDefault(0),
            Rate = id.Rate,
            Total = id.Total.GetValueOrDefault(0),
            DiscountDescription = id.DiscountDescription,
            AdditionalNotes = id.AdditionalNotes,
            InvoiceId = id.InvoiceId,
            ServiceRequestId = id.ServiceRequestId
        };
    }
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
    public class ReceiptViewModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceivedDate { get; set; }

        public static Expression<Func<ReceiptDto, ReceiptViewModel>> FromReceiptDto = r => new ReceiptViewModel
        {
            Id = r.Id,
            Amount = r.Amount,
            ReceivedDate = r.ReceivedDate
        };
    }

}