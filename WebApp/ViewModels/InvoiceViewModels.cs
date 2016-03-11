using Model;
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
            this.Invoices = new List<Invoice>();
        }
        public User CurrentUser { get; set; }
        public BillableEntity SelectedServiceProvider { get; set; }
        public BillableEntity SelectedCustomer { get; set; }
        public List<Invoice> Invoices { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
        public DateTime Today { get; internal set; }
        public FilterArgs FilterArgs { get; set; }
        public string[] Months { get; set; }
        public decimal Money { get; set; }
        public decimal Expenses { get; set; }
    }

    public class DashboardViewModel
    {
        public IEnumerable<decimal?> Expenses { get; internal set; }
        public IEnumerable<decimal?> Hst { get; internal set; }
        public IEnumerable<decimal?> SubTotal { get; internal set; }
        public IEnumerable<decimal?> Total { get; internal set; }
        public User User { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class FilterArgs
    {
        public Guid? ServiceProviderId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime FilterDate { get; set; }
    }
}