using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Additionals
{
    public class AdditionalViewModel
    {
        public LookupViewModel<short> Service { get; set; }
        public string DueDate { get; set; }
        public string ClaimantName { get; set; }
        public IEnumerable<LookupViewModel<Guid>> Collaborators { get; set; }
        public int ServiceRequestId { get; internal set; }
        public LookupViewModel<Guid> Physician { get; set; }
    }
}