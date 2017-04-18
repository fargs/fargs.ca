using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class ServiceRequestProjections
    {
        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> MinimalInfo()
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ServiceRequestTasks = sr.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new Orvosi.Shared.Model.ServiceRequestTask
                {
                    Id = srt.Id,
                    ProcessTask = new Orvosi.Shared.Model.ProcessTask
                    {
                        Id = srt.TaskId.Value
                    },
                    IsObsolete = srt.IsObsolete,
                    CompletedDate = srt.CompletedDate
                })
            };
        }
        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> BasicInfo(Guid serviceProviderId, DateTime now)
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                IsClosed = sr.IsClosed,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                ServiceCataloguePrice = sr.ServiceCataloguePrice,
                NoShowRate = sr.NoShowRate,
                LateCancellationRate = sr.LateCancellationRate,
                Notes = sr.Notes,
                PhysicianId = sr.PhysicianId,
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                ServiceRequestTasks = sr.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new Orvosi.Shared.Model.ServiceRequestTask
                {
                    Id = srt.Id,
                    ProcessTask = new Orvosi.Shared.Model.ProcessTask
                    {
                        Id = srt.TaskId.Value
                    },
                    IsObsolete = srt.IsObsolete,
                    CompletedDate = srt.CompletedDate
                }),
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code
                },
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Address = sr.Address == null ? null : new Orvosi.Shared.Model.Address
                {
                    Id = sr.Address.Id,
                    Name = sr.Address.Name,
                    City = sr.Address.City_CityId.Name,
                    ProvinceCode = sr.Address.Province.ProvinceCode
                }
            };
        }
        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> DetailsWithInvoices(Guid serviceProviderId, DateTime now)
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                IsClosed = sr.IsClosed,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                ServiceCataloguePrice = sr.ServiceCataloguePrice,
                NoShowRate = sr.NoShowRate,
                LateCancellationRate = sr.LateCancellationRate,
                Notes = sr.Notes,
                PhysicianId = sr.PhysicianId,
                ServiceRequestTasks = sr.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new Orvosi.Shared.Model.ServiceRequestTask
                {
                    Id = srt.Id,
                    ProcessTask = new Orvosi.Shared.Model.ProcessTask
                    {
                        Id = srt.TaskId.Value
                    },
                    IsObsolete = srt.IsObsolete,
                    CompletedDate = srt.CompletedDate
                }),
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code
                },
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Address = sr.Address == null ? null : new Orvosi.Shared.Model.Address
                {
                    Id = sr.Address.Id,
                    Name = sr.Address.Name,
                    City = sr.Address.City_CityId.Name,
                    ProvinceCode = sr.Address.Province.ProvinceCode
                },
                InvoiceDetails = sr.InvoiceDetails.Where(id => !id.IsDeleted && id.Invoice.ServiceProviderGuid == serviceProviderId).Select(id => new Orvosi.Shared.Model.InvoiceDetail
                {
                    Id = id.Id,
                    Description = id.Description,
                    DiscountDescription = id.DiscountDescription,
                    AdditionalNotes = id.AdditionalNotes,
                    Rate = id.Rate.Value,
                    Amount = id.Amount.Value,
                    Total = id.Total.Value,
                    Invoice = new Orvosi.Shared.Model.Invoice
                    {
                        Id = id.Invoice.Id,
                        InvoiceNumber = id.Invoice.InvoiceNumber,
                        InvoiceDate = id.Invoice.InvoiceDate,
                        SubTotal = id.Invoice.SubTotal,
                        TaxRateHst = id.Invoice.TaxRateHst,
                        Hst = id.Invoice.Hst,
                        Total = id.Invoice.Total.Value,
                        SentDate = id.Invoice.SentDate,
                        PaymentReceivedDate = id.Invoice.PaymentReceivedDate,
                        InvoiceGuid = id.Invoice.ObjectGuid,
                        Customer = new Orvosi.Shared.Model.Customer
                        {
                            Id = id.Invoice.CustomerGuid,
                            Name = id.Invoice.CustomerName,
                            BillingEmail = id.Invoice.CustomerEmail,
                            City = id.Invoice.CustomerCity,
                            Province = id.Invoice.CustomerProvince
                        }
                    }
                })
            };
        }
        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> AllDetails(Guid serviceProviderId, DateTime now)
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                Now = now,
                StartTime = sr.StartTime,
                CancelledDate = sr.CancelledDate,
                IsLateCancellation = sr.IsLateCancellation,
                IsNoShow = sr.IsNoShow,
                IsClosed = sr.IsClosed,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                ServiceCataloguePrice = sr.ServiceCataloguePrice,
                NoShowRate = sr.NoShowRate,
                LateCancellationRate = sr.LateCancellationRate,
                Notes = sr.Notes,
                PhysicianId = sr.PhysicianId,
                Address = sr.Address == null ? null : new Orvosi.Shared.Model.Address
                {
                    Id = sr.Address.Id,
                    Name = sr.Address.Name,
                    City = sr.Address.City_CityId.Name,
                    CityCode = sr.Address.City_CityId.Code,
                    PostalCode = sr.Address.PostalCode,
                    Address1 = sr.Address.Address1,
                    ProvinceCode = sr.Address.Province.ProvinceCode,
                    TimeZone = sr.Address.TimeZone.Name
                },
                Company = new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Name = sr.Service.Name,
                    Code = sr.Service.Code,
                    ServiceCategoryId = sr.Service.ServiceCategoryId.Value
                },
                CaseCoordinator = sr.CaseCoordinator == null ? null : new Orvosi.Shared.Model.Person
                {
                    Id = sr.CaseCoordinator.Id,
                    Title = sr.CaseCoordinator.Title,
                    FirstName = sr.CaseCoordinator.FirstName,
                    LastName = sr.CaseCoordinator.LastName,
                    Email = sr.CaseCoordinator.Email,
                    ColorCode = sr.CaseCoordinator.ColorCode,
                    Role = sr.CaseCoordinator.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                    {
                        Id = AspNetRoles.CaseCoordinator,
                        Name = AspNetRoles.CaseCoordinatorName
                    }).FirstOrDefault()
                },
                DocumentReviewer = sr.DocumentReviewer == null ? null : new Orvosi.Shared.Model.Person
                {
                    Id = sr.DocumentReviewer.Id,
                    Title = sr.DocumentReviewer.Title,
                    FirstName = sr.DocumentReviewer.FirstName,
                    LastName = sr.DocumentReviewer.LastName,
                    Email = sr.DocumentReviewer.Email,
                    ColorCode = sr.DocumentReviewer.ColorCode,
                    Role = sr.DocumentReviewer.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                    {
                        Id = AspNetRoles.DocumentReviewer,
                        Name = AspNetRoles.DocumentReviewerName
                    }).FirstOrDefault()
                },
                IntakeAssistant = sr.IntakeAssistant == null ? null : new Orvosi.Shared.Model.Person
                {
                    Id = sr.IntakeAssistant.Id,
                    Title = sr.IntakeAssistant.Title,
                    FirstName = sr.IntakeAssistant.FirstName,
                    LastName = sr.IntakeAssistant.LastName,
                    Email = sr.IntakeAssistant.Email,
                    ColorCode = sr.IntakeAssistant.ColorCode,
                    Role = sr.IntakeAssistant.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                    {
                        Id = AspNetRoles.IntakeAssistant,
                        Name = AspNetRoles.IntakeAssistantName
                    }).FirstOrDefault()
                },
                Physician = sr.Physician == null ? null : new Orvosi.Shared.Model.Person
                {
                    Id = sr.Physician.Id,
                    Title = sr.Physician.AspNetUser.Title,
                    FirstName = sr.Physician.AspNetUser.FirstName,
                    LastName = sr.Physician.AspNetUser.LastName,
                    Email = sr.Physician.AspNetUser.Email,
                    ColorCode = sr.Physician.AspNetUser.ColorCode,
                    Role = sr.Physician.AspNetUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                    {
                        Id = AspNetRoles.Physician,
                        Name = AspNetRoles.PhysicianName
                    }).FirstOrDefault()
                },
                ServiceRequestMessages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new Orvosi.Shared.Model.ServiceRequestMessage
                {
                    Id = srm.Id,
                    TimeZone = srm.ServiceRequest.Address == null ? TimeZones.EasternStandardTime : srm.ServiceRequest.Address.TimeZone.Name,
                    Message = srm.Message,
                    PostedDate = srm.PostedDate,
                    PostedBy = new Orvosi.Shared.Model.Person
                    {
                        Id = srm.AspNetUser.Id,
                        Title = srm.AspNetUser.Title,
                        FirstName = srm.AspNetUser.FirstName,
                        LastName = srm.AspNetUser.LastName,
                        Email = srm.AspNetUser.Email,
                        ColorCode = srm.AspNetUser.ColorCode,
                        Role = srm.AspNetUser.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                        {
                            Id = r.AspNetRole.Id,
                            Name = r.AspNetRole.Name
                        }).FirstOrDefault()
                    }
                }),
                ServiceRequestTasks = sr.ServiceRequestTasks
                    .OrderBy(srt => srt.Sequence)
                    .Select(t => new Orvosi.Shared.Model.ServiceRequestTask
                    {
                        Id = t.Id,
                        ServiceRequestId = t.ServiceRequestId,
                        AppointmentDate = sr.AppointmentDate,
                        DueDate = t.DueDate,
                        Now = now,
                        ProcessTask = new Orvosi.Shared.Model.ProcessTask
                        {
                            Id = t.OTask.Id,
                            Name = t.OTask.Name,
                            Sequence = t.OTask.Sequence.Value,
                            ResponsibleRole = t.OTask.AspNetRole == null ? null : new Orvosi.Shared.Model.UserRole
                            {
                                Id = t.OTask.AspNetRole.Id,
                                Name = t.OTask.AspNetRole.Name
                            }
                        },
                        AssignedTo = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                            Title = t.AspNetUser_AssignedTo.Title,
                            FirstName = t.AspNetUser_AssignedTo.FirstName,
                            LastName = t.AspNetUser_AssignedTo.LastName,
                            Email = t.AspNetUser_AssignedTo.Email,
                            ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                            Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedBy = new Orvosi.Shared.Model.Person
                        {
                            Id = t.AspNetUser_TaskStatusChangedBy == null ? (Guid?)null : t.AspNetUser_TaskStatusChangedBy.Id,
                            Title = t.AspNetUser_TaskStatusChangedBy.Title,
                            FirstName = t.AspNetUser_TaskStatusChangedBy.FirstName,
                            LastName = t.AspNetUser_TaskStatusChangedBy.LastName,
                            Email = t.AspNetUser_TaskStatusChangedBy.Email,
                            ColorCode = t.AspNetUser_TaskStatusChangedBy.ColorCode,
                            Role = t.AspNetUser_TaskStatusChangedBy.AspNetUserRoles.Select(r => new Orvosi.Shared.Model.UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        },
                        CompletedDate = t.CompletedDate,
                        IsObsolete = t.IsObsolete,
                        Dependencies = t.Child.Select(c => new Orvosi.Shared.Model.ServiceRequestTaskDependent
                        {
                            TaskId = c.TaskId.Value,
                            CompletedDate = c.CompletedDate,
                            IsObsolete = c.IsObsolete
                        })
                    }),
                InvoiceDetails = sr.InvoiceDetails.Where(id => !id.IsDeleted).Select(id => new Orvosi.Shared.Model.InvoiceDetail
                {
                    Id = id.Id,
                    Description = id.Description,
                    DiscountDescription = id.DiscountDescription,
                    AdditionalNotes = id.AdditionalNotes,
                    Rate = id.Rate.Value,
                    Amount = id.Amount.Value,
                    Total = id.Total.Value,
                    Invoice = new Orvosi.Shared.Model.Invoice
                    {
                        Id = id.Invoice.Id,
                        InvoiceNumber = id.Invoice.InvoiceNumber,
                        InvoiceDate = id.Invoice.InvoiceDate,
                        SubTotal = id.Invoice.SubTotal,
                        TaxRateHst = id.Invoice.TaxRateHst,
                        Hst = id.Invoice.Hst,
                        Total = id.Invoice.Total.Value,
                        SentDate = id.Invoice.SentDate,
                        PaymentReceivedDate = id.Invoice.PaymentReceivedDate,
                        InvoiceGuid = id.Invoice.ObjectGuid,
                        Customer = new Orvosi.Shared.Model.Customer
                        {
                            Id = id.Invoice.CustomerGuid,
                            Name = id.Invoice.CustomerName,
                            BillingEmail = id.Invoice.CustomerEmail,
                            City = id.Invoice.CustomerCity,
                            Province = id.Invoice.CustomerProvince
                        }
                    }
                })
            };
        }
        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> ForBoxManager()
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                ClaimantName = sr.ClaimantName,
                CalendarEventId = sr.CalendarEventId,
                Physician = new Orvosi.Shared.Model.Person
                {
                    Id = sr.Physician.Id,
                    Email = sr.Physician.AspNetUser.Email,
                    BoxFolderId = sr.Physician.AspNetUser.BoxFolderId,
                    BoxCaseTemplateFolderId = sr.Physician.BoxCaseTemplateFolderId,
                    BoxAddOnTemplateFolderId = sr.Physician.BoxAddOnTemplateFolderId
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Code = sr.Service.Code,
                    ServiceCategoryId = sr.Service.ServiceCategoryId.Value
                },
                Company = sr.Company == null ? null : new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name,
                    Code = sr.Company.Code
                },
                Address = sr.Address == null ? null : new Orvosi.Shared.Model.Address
                {
                    Id = sr.AddressId.Value,
                    CityCode = sr.Address.City_CityId.Code,
                    ProvinceId = sr.Address.ProvinceId
                }
            };
        }

        public static Expression<Func<Orvosi.Data.ServiceRequest, Orvosi.Shared.Model.ServiceRequest>> ForGoogleCalendar(string baseUrl)
        {
            return sr => new Orvosi.Shared.Model.ServiceRequest
            {
                Id = sr.Id,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                DueDate = sr.DueDate,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                EndTime = sr.EndTime,
                ClaimantName = sr.ClaimantName,
                CalendarEventId = sr.CalendarEventId,
                URL = baseUrl + "/ServiceRequest/Details/" + sr.Id,
                Physician = new Orvosi.Shared.Model.Person
                {
                    Id = sr.Physician.Id,
                    Email = sr.Physician.AspNetUser.Email,
                    BoxFolderId = sr.Physician.AspNetUser.BoxFolderId,
                    BoxCaseTemplateFolderId = sr.Physician.BoxCaseTemplateFolderId,
                    BoxAddOnTemplateFolderId = sr.Physician.BoxAddOnTemplateFolderId
                },
                Service = new Orvosi.Shared.Model.Service
                {
                    Id = sr.Service.Id,
                    Code = sr.Service.Code,
                    ServiceCategoryId = sr.Service.ServiceCategoryId.Value
                },
                Company = sr.Company == null ? null : new Orvosi.Shared.Model.Company
                {
                    Id = sr.Company.Id,
                    Name = sr.Company.Name,
                    Code = sr.Company.Code
                },
                Address = sr.Address == null ? null : new Orvosi.Shared.Model.Address
                {
                    Id = sr.AddressId.Value,
                    Name = sr.Address.Name,
                    Address1 = sr.Address.Address1,
                    City = sr.Address.City_CityId.Name,
                    CityCode = sr.Address.City_CityId.Code,
                    ProvinceId = sr.Address.ProvinceId,
                    ProvinceCode = sr.Address.Province.ProvinceCode,
                    PostalCode = sr.Address.PostalCode,
                    TimeZoneIana = sr.Address.TimeZone.Iana,
                    TimeZone = sr.Address.TimeZone.Name
                }
            };
        }
    }
}