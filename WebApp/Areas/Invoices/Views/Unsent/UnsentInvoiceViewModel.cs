using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class UnsentInvoiceViewModel : ViewModelBase
    {
        public UnsentInvoiceViewModel(ServiceRequestDto serviceRequest, InvoiceDto invoice, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (serviceRequest != null)
            {
                ServiceRequest = new ServiceRequestViewModel(serviceRequest);
                ServiceRequestId = serviceRequest.Id;
                Day = serviceRequest.ExpectedInvoiceDate;
            }
            if(invoice != null)
            {
                Invoice = InvoiceViewModel.FromInvoiceDto(invoice);
                InvoiceId = invoice.Id;
                Day = invoice.InvoiceDate;
            }

        }
        public UnsentInvoiceViewModel(OrvosiDbContext db, int? serviceRequestId, int? invoiceId, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (serviceRequestId.HasValue)
            {
                var serviceRequest = ServiceRequestDto.FromServiceRequestEntityForInvoice.Invoke(db.ServiceRequests.Single(sr => sr.Id == serviceRequestId));
                ServiceRequest = new ServiceRequestViewModel(serviceRequest);
                ServiceRequestId = serviceRequest.Id;
                Day = serviceRequest.ExpectedInvoiceDate;
            }

            if(invoiceId.HasValue)
            {
                // when invoice is deleted, invoiceId did exist but the unsent invoice should not return it
                var invoiceEntity = db.Invoices
                    .AreOwnedBy(PhysicianId.Value)
                    .AreNotDeleted()
                    .AreNotSent()
                    .SingleOrDefault(sr => sr.Id == invoiceId);
                if (invoiceEntity != null)
                {
                    var invoice = InvoiceDto.FromInvoiceEntity.Invoke(invoiceEntity);
                    Invoice = InvoiceViewModel.FromInvoiceDto(invoice);
                    InvoiceId = invoice.Id;
                    Day = invoice.InvoiceDate;
                }
            }
        }
        public int? ServiceRequestId { get; set; }
        public ServiceRequestViewModel ServiceRequest { get; set; }
        public int? InvoiceId { get; set; }
        public InvoiceViewModel Invoice { get; set; }
        public DateTime Day { get; set; }
    }
}