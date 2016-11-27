using data = Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using WebApp.ViewModels.InvoiceViewModels;
using WebApp.Library.Extensions;
using WebApp.Library;
using System.Security.Principal;
using System.Data.Entity;
using System.Threading.Tasks;
using Orvosi.Data.Filters;

namespace WebApp.Models.AccountingModel
{
    public class Mapper
    {
        private data.IOrvosiDbContext context;
        public Mapper(data.IOrvosiDbContext context)
        {
            this.context = context;
        }

        public EditInvoiceDetailForm MapToEditForm(int invoiceDetailId)
        {
            var source = context.InvoiceDetails.Select(id => new
            {
                Id = id.Id,
                CustomerEmail = id.Invoice.CustomerEmail,
                InvoiceDate = id.Invoice.InvoiceDate,
                Amount = id.Amount,
                Rate = id.Rate,
                AdditionalNotes = id.AdditionalNotes,
                ClaimantName = id.ServiceRequest.ClaimantName,
                InvoiceNumber = id.Invoice.InvoiceNumber,
                ServiceRequestId = id.ServiceRequestId.Value
            })
            .First(id => id.Id == invoiceDetailId);

            return new EditInvoiceDetailForm
            {
                Id = source.Id,
                To = source.CustomerEmail,
                InvoiceDate = source.InvoiceDate.ToOrvosiDateFormat(),
                Amount = source.Amount,
                Rate = source.Rate,
                AdditionalNotes = source.AdditionalNotes,
                ClaimantName = source.ClaimantName,
                InvoiceNumber = source.InvoiceNumber,
                ServiceRequestId = source.ServiceRequestId
            };
        }

        public void Create(int serviceRequestId, IPrincipal User)
        {
            var db = context;
            var serviceRequest =
                db.ServiceRequests
                    .Find(serviceRequestId);

            // check if the no show rates are set in the request. Migrate old records to use invoices.
            if (!serviceRequest.NoShowRate.HasValue || !serviceRequest.LateCancellationRate.HasValue)
            {
                var rates = db.GetServiceCatalogueRate(serviceRequest.PhysicianId, serviceRequest.Company.ObjectGuid).First();
                serviceRequest.NoShowRate = rates.NoShowRate;
                serviceRequest.LateCancellationRate = rates.LateCancellationRate;
            }

            var serviceProvider = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.PhysicianId);
            var customer = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.Company.ObjectGuid);

            // Gets new invoice number specific to the service provider except for Shariff, Zeeshan and Rajiv.
            var invoiceNumber = db.Invoices.GetNextInvoiceNumber(serviceProvider.EntityGuid.Value);
            if (serviceProvider.EntityId == "8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c" || serviceProvider.EntityId == "8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9" || serviceProvider.EntityId == "48f9d9fd-deb5-471f-9454-066430a510f1") // Shariff, Zeeshan, Rajiv will use old invoice number approach
            {
                var invoiceNumberStr = db.GetNextInvoiceNumber().First().NextInvoiceNumber;
                invoiceNumber = long.Parse(invoiceNumberStr);
            }

            var invoiceDate = SystemTime.Now();
            if (serviceRequest.Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
            {
                invoiceDate = serviceRequest.AppointmentDate.Value;
            }

            var invoice = new data.Invoice();
            invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, User.Identity.Name);
            
            var invoiceDetail = new data.InvoiceDetail();
            invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.CalculateTotal();

            db.Invoices.Add(invoice);
            
            db.SaveChanges();
        }

        

        
    }
}