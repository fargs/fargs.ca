using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public partial class AvailableSlot
    {
        public string GetTitle()
        {
            var sr = ServiceRequests.FirstOrDefault();
            if (sr == null)
            {
                return "Available";
            }
            else
            {
                return $"{sr.Service?.Name ?? "[Service Not Set]"} - {sr.Company?.Name ?? "[Company Not Set]"} - {sr.Address?.Name ?? "[Address Not Set]"} ";
            }
        }
    }
}
