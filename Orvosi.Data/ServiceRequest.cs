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
        public byte ServiceStatusId
        {
            get
            {
                if (IsLateCancellation)
                {
                    return ServiceStatus.LateCancellation;
                }
                else if (CancelledDate.HasValue)
                {
                    return ServiceStatus.Cancellation;
                }
                else if (IsNoShow)
                {
                    return ServiceStatus.NoShow;
                }
                else
                {
                    return ServiceStatus.Active;
                }
            }
        }

        public string GetCaseFolderName()
        {
            if (ServiceId == Services.Addendum || ServiceId == Services.PaperReview)
            {
                return $"{this.DueDate.Value.ToString("yyyy-MM-dd")} {ClaimantName} ({Service.Code}-{Physician.AspNetUser.UserName}) {Company.Code}-{Id}";
            }
            else
            {
                return $"{this.AppointmentDate.Value.ToString("yyyy-MM-dd")}({this.StartTime.Value.ToString(@"hhmm")})-{Address.City_CityId.Code}-{ClaimantName} ({Service.Code}-{Physician.AspNetUser.UserName}) {Company.Code}-{Id}";
            }
        }
    }
}
