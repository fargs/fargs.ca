using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Extensions
{
    public static class Extensions
    {
        public static bool IsValidToSubmitInvoice(this PhysicianToCompanyServiceRequestInvoicePreview obj, DateTime? IntakeInterviewCompletedDate)
        {
            return IntakeInterviewCompletedDate.HasValue ? true : false;
        }
    }
}
