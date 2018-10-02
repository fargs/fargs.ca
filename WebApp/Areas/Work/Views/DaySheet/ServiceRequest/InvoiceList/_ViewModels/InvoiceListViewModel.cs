using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using LinqKit;
using Orvosi.Data;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet.ServiceRequest.InvoiceList
{
    public class InvoiceListViewModel : ViewModelBase
    {
        public InvoiceListViewModel(IEnumerable<InvoiceDto> invoices, IIdentity identity, DateTime now) : base(identity, now)
        {
            Invoices = invoices
                .Select(InvoiceViewModel.FromInvoiceDto.Compile());
        }
        public InvoiceListViewModel(OrvosiDbContext db, int serviceRequestId, IIdentity identity, DateTime now) : base(identity, now)
        {
            var invoiceIds = db.InvoiceDetails.Where(id => id.ServiceRequestId == serviceRequestId).Select(id => id.InvoiceId).ToArray();

            var invoices = db.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(i => invoiceIds.Contains(i.Id))
                .Select(InvoiceDto.FromInvoiceEntity)
                .AsEnumerable();

            Invoices = invoices
                .Select(InvoiceViewModel.FromInvoiceDto.Compile());
        }
        public IEnumerable<InvoiceViewModel> Invoices { get; set; }

    }
}