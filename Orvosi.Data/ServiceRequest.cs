﻿using Orvosi.Shared.Enums;
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
            return Service.HasAppointment() && CanBeCancelled(now);
        }

        public bool CanBeCancelled(DateTime now)
        {
            Console.WriteLine($"Service Request ID: {Id}");
            if (Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam || Service.ServiceCategoryId == ServiceCategories.MedicalConsultation)
            {
                var status = GetExaminationStatusId(now);
                return status == ServiceStatus.Pending;
            }
            else if (Service.ServiceCategoryId == ServiceCategories.AddOn)
            {
                return !IsReportSubmitted();
            }
            return false;
        }

        public bool CanBeUncancelled(DateTime now)
        {
            if (Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
            {
                var status = GetExaminationStatusId(now);
                return status == ServiceStatus.Cancellation || status == ServiceStatus.LateCancellation;
            }
            return false;
        }

        public bool CanBeNoShow(DateTime now)
        {
            if (Service.HasAppointment())
            {
                var status = GetExaminationStatusId(now);
                return status != ServiceStatus.Cancellation && status != ServiceStatus.LateCancellation && status != ServiceStatus.NoShow;
            }
            return false; 
        }

        public bool CanNoShowBeUndone(DateTime now)
        {
            if (Service.HasAppointment())
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
                return ServiceStatus.Pending;
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

        public bool IsClosed()
        {
            if (ServiceRequestTasks.All(srt => srt.CompletedDate.HasValue || srt.IsObsolete))
                return true;

            return false;
        }

        public string GetCalendarEventTitle()
        {
            return $"{Address.City_CityId.Code}: {ClaimantName} ({Service.Code}) {Company.Code}-{Id})";
        }
    }
}