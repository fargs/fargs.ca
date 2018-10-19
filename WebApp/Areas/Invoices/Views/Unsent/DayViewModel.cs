using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.ViewModels.WorkViewModels;
using WebApp.ViewModels.InvoiceViewModels;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class DayViewModel
    {
        public DayViewModel()
        {
            UnsentInvoices = new List<UnsentInvoiceViewModel>();
        }
        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public IEnumerable<UnsentInvoiceViewModel> UnsentInvoices { get; set; }
    }
}