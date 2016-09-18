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
            return this.Service.HasAppointment && CanBeCancelled(now);
        }

        public bool CanBeCancelled(DateTime now)
        {
            Console.WriteLine($"Service Request ID: {Id}");
            if (Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || Service.ServiceCategoryId == ServiceCategories.MedicalConsultation)
            {
                var status = GetExaminationStatusId(now);
                return status == ServiceStatus.Active;
            }
            else if (Service.ServiceCategoryId == ServiceCategories.AddOn)
            {
                return !IsReportSubmitted() && !CancelledDate.HasValue;
            }
            return false;
        }

        public bool CanBeUncancelled(DateTime now)
        {
            if (Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || Service.ServiceCategoryId == ServiceCategories.MedicalConsultation)
            {
                var status = GetExaminationStatusId(now);
                return status == ServiceStatus.Cancellation || status == ServiceStatus.LateCancellation;
            }
            else
            {
                return CancelledDate.HasValue;
            }
        }

        public bool CanBeNoShow(DateTime now)
        {
            if (Service.HasAppointment)
            {
                var status = GetExaminationStatusId(now);
                return status != ServiceStatus.Cancellation && status != ServiceStatus.LateCancellation && status != ServiceStatus.NoShow;
            }
            return false; 
        }

        public bool CanNoShowBeUndone(DateTime now)
        {
            if (Service.HasAppointment)
            {
                var status = GetExaminationStatusId(now);
                return status == ServiceStatus.NoShow;
            }
            return false;
        }

        public byte? GetExaminationStatusId(DateTime now) {
            if (Service.ServiceCategoryId != ServiceCategories.IndependentMedicalExam)
                throw new Exception("Examination status is only applicable to services under the IME category.");

            if (IsLateCancellation)
                return ServiceStatus.LateCancellation;
            else if (CancelledDate.HasValue)
                return ServiceStatus.Cancellation;
            else if (IsNoShow)
                return ServiceStatus.NoShow;
            else if (AppointmentDate <= now)
                return ServiceStatus.Complete;
            else
                return ServiceStatus.Active;
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
            return $"{this.Address.City_CityId.Code}: {ClaimantName} ({Service.Code}) {Company.Code}-{Id})";
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
    }
}
