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
        public bool CanBeRescheduled(DateTime now)
        {
            return this.Service.HasAppointment;
        }

        public bool CanBeCancelled()
        {
            return !this.IsLateCancellation && !this.CancelledDate.HasValue && !this.IsNoShow;
        }

        public bool CanBeUncancelled()
        {
            return this.IsLateCancellation || this.CancelledDate.HasValue;
        }

        public bool CanBeNoShow()
        {
            if (Service.HasAppointment)
            {
                return !CancelledDate.HasValue && !IsLateCancellation && !IsNoShow;
            }
            return false; 
        }

        public bool CanNoShowBeUndone()
        {
            if (Service.HasAppointment)
            {
                return IsNoShow;
            }
            return false;
        }

        public bool IsReportSubmitted()
        {
            if (!Service.HasReportDeliverable())
                throw new Exception("Submitting a report is not applicable to this service.");

            return ServiceRequestTasks.First(c => c.TaskId == Tasks.SubmitReport || c.TaskId == 36).IsComplete();
        }

        public bool ResponseToQAIsRequired()
        {
            return IsReportSubmitted() && !ServiceRequestTasks.First(srt => srt.TaskId == Tasks.RespondToQAComments).IsComplete();
        }

        public string GetCalendarEventTitle()
        {
            return $"{this.Address.City_CityId.Code}: {ClaimantName} ({Service.Code}) {Company.Code}-{Id}";
        }

        public byte ServiceStatusId {
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
                return $"{this.DueDate.Value.ToString("yy-MM-dd")} {ClaimantName} ({Service.Code}-{Physician.AspNetUser.UserName}) {Company.Code}-{Id}";
            }
            else
            {
                return $"{this.AppointmentDate.Value.ToString("yy-MM-dd")}({this.StartTime.Value.ToString(@"hh\:mm")}) {ClaimantName} ({Service.Code}-{Physician.AspNetUser.UserName}) {Company.Code}-{Id}";
            }
        }
    }
}
