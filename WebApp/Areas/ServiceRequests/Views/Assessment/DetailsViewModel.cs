using ImeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.ServiceRequests.Views.Assessment
{
    public class DetailsViewModel : ViewModelBase
    {
        public DetailsViewModel(ServiceRequestModel assessment, IIdentity identity, DateTime now) : base(identity, now)
        {
            var a = assessment;
            Id = a.Id;
            ClaimantName = a.ClaimantName;
        }
        public Guid Id { get; set; }
        public string ClaimantName { get; set; }
    }
}