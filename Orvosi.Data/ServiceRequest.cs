using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    [FluentValidation.Attributes.Validator(typeof(ServiceRequestValidator))]
    public partial class ServiceRequest
    {
    }
}
