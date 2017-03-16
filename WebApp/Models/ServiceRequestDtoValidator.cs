using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Shared.Enums;

namespace WebApp.Models
{
    public class ServiceRequestDtoValidator : AbstractValidator<ServiceRequestDto>
    {
        //public ServiceRequestDtoValidator()
        //{
        //    AvailableSlotDtoValidator availableSlotDtoValidator = new AvailableSlotDtoValidator();

        //    RuleFor(sr => sr.ClaimantName).NotEmpty().WithState(sr => ValidationTypeEnum.Error);
        //    RuleFor(sr => sr.Physician).NotNull().WithState(sr => ValidationTypeEnum.Error);
        //    RuleFor(sr => sr.Company).NotNull().WithState(sr => ValidationTypeEnum.Warning);
        //    RuleFor(sr => sr.Service).NotNull().WithState(sr => ValidationTypeEnum.Warning);

        //    AssessmentRules();

        //    RequiredResourcesRules();

        //    //RuleFor(sr => sr.InvoiceDetails).NotNull().WithState(sr => ValidationTypeEnum.Error); ;
        //    //RuleFor(sr => sr.InvoiceDetails).NotEmpty().WithState(sr => ValidationTypeEnum.Warning); ;
        //    //RuleFor(sr => sr.InvoiceDetails).Must(sr => sr.Any())
        //    //    .WithState(sr => ValidationTypeEnum.Warning)
        //    //    .WithMessage("Service request has not been invoiced.");

        //    //RuleFor(sr => sr.AvailableSlotDto).SetValidator(availableSlotDtoValidator);
        //}

        //private void AssessmentRules()
        //{
        //    When(sr => sr.AppointmentDate.HasValue, () =>
        //    {
        //        RuleFor(sr => sr.Address).NotNull()
        //            .WithMessage("Address is missing")
        //            .WithState(sr => ValidationTypeEnum.Warning);
        //    });
        //}

        //private void RequiredResourcesRules()
        //{
        //    // When the task list requires the case coordinator role
        //    When(sr => sr.Tasks.Where(t => t.ResponsibleRoleId == AspNetRoles.CaseCoordinator).Distinct().Any(), () =>
        //    {
        //        RuleFor(sr => sr.Resources)
        //            .Cascade(CascadeMode.StopOnFirstFailure)
        //            .Must(list => list.Any(item => item.RoleId == AspNetRoles.CaseCoordinator))
        //                .WithMessage("Case requires a Case Coordinator.")
        //                .WithState(sr => ValidationTypeEnum.Error)
        //            .Must(list => list.Where(item => item.RoleId == AspNetRoles.CaseCoordinator).Count() == 1)
        //                .WithMessage("Multiple case coordinators have been assigned to the case.")
        //                .WithState(sr => ValidationTypeEnum.Warning);
        //    });

        //    // When the task list requires the document reviewer role
        //    When(sr => sr.Tasks.Where(t => t.ResponsibleRoleId == AspNetRoles.DocumentReviewer).Distinct().Any(), () =>
        //    {
        //        RuleFor(sr => sr.Resources)
        //            .Cascade(CascadeMode.StopOnFirstFailure)
        //            .Must(list => list.Any(item => item.RoleId == AspNetRoles.DocumentReviewer))
        //                .WithMessage("Case requires a Document Reviewer.")
        //                .WithState(sr => ValidationTypeEnum.Error)
        //            .Must(list => list.Where(item => item.RoleId == AspNetRoles.DocumentReviewer).Count() == 1)
        //                .WithMessage("Multiple document reviewers have been assigned to the case.")
        //                .WithState(sr => ValidationTypeEnum.Warning);
        //    });

        //    // When the task list requires the intake associate role
        //    When(sr => sr.Tasks.Where(t => t.ResponsibleRoleId == AspNetRoles.IntakeAssistant).Distinct().Any(), () =>
        //    {
        //        RuleFor(sr => sr.Resources)
        //            .Cascade(CascadeMode.StopOnFirstFailure)
        //            .Must(list => list.Any(item => item.RoleId == AspNetRoles.IntakeAssistant))
        //                .WithMessage("Case requires a Intake Associate.")
        //                .WithState(sr => ValidationTypeEnum.Error)
        //            .Must(list => list.Where(item => item.RoleId == AspNetRoles.IntakeAssistant).Count() == 1)
        //                .WithMessage("Multiple intake associates have been assigned to the case.")
        //                .WithState(sr => ValidationTypeEnum.Warning);
        //        });
        //}

        //private bool ResourcesAreAssigned(ServiceRequestDto sr)
        //{
        //    // fail if any required role is not in the assigned role.
        //    return sr.RequiredRoles(sr.Tasks).Intersect(sr.AssignedRoles(sr.Resources)).Count() == sr.RequiredRoles(sr.Tasks).Count();
        //}

        //private bool AllTasksAreAssigned(IEnumerable<TaskDto> tasks)
        //{
        //    // fail if any required role is not in the assigned role.
        //    return tasks.Any() && tasks.Where(t => t.TaskId != Tasks.AssessmentDay).All(t => t.AssignedToId.HasValue);
        //}
    }
}