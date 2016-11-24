using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModels
{
    public class ChangeCompanyFormViewModel
    {
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public short? ServiceId { get; set; }
        public short? CompanyId { get; set; }
        public bool HasInvoices { get; internal set; }
    }
    public class ChangeCompanyViewModel : ChangeCompanyFormViewModel
    {
        public IEnumerable<SelectListItem> AddressSelectList { get; internal set; }
        public IEnumerable<SelectListItem> CompanySelectList { get; set; }
        public IEnumerable<SelectListItem> ServiceSelectList { get; internal set; }
    }
}