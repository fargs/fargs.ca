using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.InvoiceViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.UnsentInvoices = new List<Orvosi.Shared.Model.UnsentInvoiceDayFolder>();
        }
        public IEnumerable<Orvosi.Shared.Model.UnsentInvoiceDayFolder> UnsentInvoices { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class DashboardViewModel
    {
        public IEnumerable<decimal?> ExpensesByMonth { get; internal set; }
        public IEnumerable<decimal?> HstByMonth { get; internal set; }
        public IEnumerable<decimal?> NetIncomeByMonth { get; internal set; }
        public decimal? Expenses { get; internal set; }
        public decimal? Hst { get; internal set; }
        public decimal? NetIncome { get; internal set; }
        public AspNetUser User { get; set; }
        public FilterArgs FilterArgs { get; set; }
        public int InvoiceCount { get; internal set; }
        public List<Orvosi.Shared.Model.Invoice> Invoices { get; set; }
        public IEnumerable<decimal?> NetIncomeByCompany { get; internal set; }
        public IEnumerable<string> Months { get; internal set; }
        public IEnumerable<string> Companies { get; internal set; }
    }

    public class FilterArgs
    {
        public Guid? ServiceProviderId { get; set; }
        public Guid? CustomerId { get; set; }
        public string SearchText { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool ShowSubmitted { get; set; } = false;
        public int? InvoiceId { get; set; }
    }

    public class EditInvoiceDetailForm
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string InvoiceDate { get; set; }
        public string ClaimantName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Rate { get; set; }
        public string AdditionalNotes { get; set; }
        public int ServiceRequestId { get; internal set; }
    }

    public class CreateInvoiceForm
    {
        public Guid ServiceProviderGuid { get; set; }
        public Guid CustomerGuid { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}