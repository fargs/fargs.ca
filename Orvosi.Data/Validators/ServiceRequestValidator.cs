using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orvosi.Shared.Enums;

namespace Orvosi.Data
{
    public class ServiceRequestValidator : AbstractValidator<ServiceRequest>
    {
        public ServiceRequestValidator()
        {
            RuleFor(sr => sr.ClaimantName).NotEmpty().WithState(sr => ValidationTypeEnum.Error);
            RuleFor(sr => sr.PhysicianId).NotNull().WithState(sr => ValidationTypeEnum.Error);
            RuleFor(sr => sr.CompanyId).NotNull().WithState(sr => ValidationTypeEnum.Error);
            RuleFor(sr => sr.ServiceId).NotNull().WithState(sr => ValidationTypeEnum.Error);

            var addonServices = new short?[] { Services.Addendum, Services.PaperReview };

            // When service is not an addon
            //RuleFor(sr => sr.AvailableSlotId).NotNull().When(sr => !addonServices.Contains(sr.ServiceId));
            //RuleFor(sr => sr.AppointmentDate).NotNull().When(sr => !addonServices.Contains(sr.ServiceId));

            // When service is an addon
            RuleFor(sr => sr.DueDate).NotNull().When(sr => addonServices.Contains(sr.ServiceId));
        }
    }
}
