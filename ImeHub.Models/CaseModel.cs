using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models
{
    public class CaseModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string AlternateKey { get; set; }
        public string ClaimantName { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyModel Company { get; set; }
        public IEnumerable<ServiceRequestModel> ServiceRequests { get; set; }
    }
}
