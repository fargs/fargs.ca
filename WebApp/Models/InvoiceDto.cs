using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class InvoiceDto
    {
        public InvoiceDto()
        {
            InvoiceDetails = new List<InvoiceDetailDto>();
            Receipts = new List<ReceiptDto>();
        }
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
        public bool IsDeleted { get; set; }
        public ServiceProviderDto ServiceProvider { get; set; }
        public CustomerDto Customer { get; set; }
        public Guid InvoiceGuid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<InvoiceDetailDto> InvoiceDetails { get; set; }
        public IEnumerable<ReceiptDto> Receipts { get; set; }
        public IEnumerable<InvoiceSentLogDto> SentLog { get; set; }
        public TaskDto SubmitInvoiceTask
        {
            get
            {
                if (ServiceRequestId.HasValue)
                {
                    var invoiceDetail = InvoiceDetails.FirstOrDefault(id => id.ServiceRequest.Tasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice));
                    return invoiceDetail == null ? null : invoiceDetail.ServiceRequest.Tasks.FirstOrDefault(srt => srt.TaskId == Tasks.SubmitInvoice);
                }
                return null;
            }
        }
        public int? ServiceRequestId
        {
            get
            {
                if (InvoiceDetails.Any(id => id.ServiceRequestId.HasValue))
                {
                    return InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequestId;
                }
                return (int?)null;
            }
        }
        private string _terms;
        public string Terms
        {
            get
            {
                return string.IsNullOrEmpty(_terms) ? "30" : _terms;
            }
            set
            {
                _terms = value;
            }
        }
        public decimal AmountPaid
        {
            get
            {
                return Receipts.Sum(r => r.Amount);
            }
        }
        public decimal OutstandingBalance
        {
            get
            {
                return Total.Value - AmountPaid;
            }
        }
        public bool IsPaid
        {
            get
            {
                return Total != 0 && OutstandingBalance <= 0 && InvoiceDetailCount > 0;
            }
        }
        public bool IsSent
        {
            get
            {
                return SentDate.HasValue;// || SubmitInvoiceTask == null ? false : SubmitInvoiceTask.TaskStatusId == TaskStatuses.Done;
            }
        }

        public bool IsPartiallyPaid
        {
            get
            {
                return OutstandingBalance > 0 && OutstandingBalance < Total.Value;
            }
        }
        public int InvoiceDetailCount
        {
            get
            {
                return InvoiceDetails.Count();
            }
        }

        public static Expression<Func<Invoice, InvoiceDto>> FromInvoiceEntity = i => new InvoiceDto
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
            IsDeleted = i.IsDeleted,
            PaymentReceivedDate = i.PaymentReceivedDate,
            InvoiceGuid = i.ObjectGuid,
            CreatedDate = i.CreatedDate,
            ServiceProvider = new ServiceProviderDto
            {
                Id = i.ServiceProviderGuid,
                Name = i.ServiceProviderName,
                Email = i.ServiceProviderEmail,
                Province = i.ServiceProviderProvince
            },
            Customer = new CustomerDto
            {
                Id = i.CustomerGuid,
                Name = i.CustomerName,
                BillingEmail = i.CustomerEmail,
                City = i.CustomerCity,
                Province = i.CustomerProvince
            },
            InvoiceDetails = i.InvoiceDetails.AsQueryable().Select(InvoiceDetailDto.FromInvoiceDetailEntity.Expand()),
            Receipts = i.Receipts.AsQueryable().Select(ReceiptDto.FromReceiptEntity.Expand()),
            SentLog = i.InvoiceSentLogs.AsQueryable().Select(InvoiceSentLogDto.FromInvoiceSentLogEntity.Expand())
        };

        public static Expression<Func<Invoice, InvoiceDto>> FromInvoiceEntityForInvoiceMenu = i => new InvoiceDto
        {
            Id = i.Id,
            InvoiceGuid = i.ObjectGuid,
            SentDate = i.SentDate,
            Total = i.Total,
            InvoiceDetails = i.InvoiceDetails.AsQueryable().Select(InvoiceDetailDto.FromInvoiceDetailEntity.Expand()),
            Receipts = i.Receipts.AsQueryable().Select(ReceiptDto.FromReceiptEntity.Expand())
        };
    }
}