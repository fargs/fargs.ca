using data = Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.ServiceRequestModels2
{
    public static class ServiceRequestMapper2
    {
        public static IEnumerable<DayFolder> MapToDueDates(Guid serviceProviderId, DateTime now, string requestUrl)
        {
            using (var context = new data.OrvosiDbContext())
            {
                // read all the data in from the database.
                var source = context.ServiceRequests
                    .Where(d => d.DueDate.HasValue
                        && d.ServiceRequestTasks.Any(srt => srt.AssignedTo == serviceProviderId)
                        && !d.IsClosed)
                    .Select(sr => new ServiceRequest
                    {
                        Id = sr.Id,
                        ClaimantName = sr.ClaimantName,
                        DueDate = sr.DueDate,
                        AppointmentDate = sr.AppointmentDate,
                        Now = now,
                        StartTime = sr.StartTime,
                        CancelledDate = sr.CancelledDate,
                        IsClosed = sr.IsClosed,
                        BoxCaseFolderId = sr.BoxCaseFolderId,
                        Company = new Company
                        {
                            Id = sr.Company.Id,
                            Name = sr.Company.Name
                        },
                        Service = new Service
                        {
                            Id = sr.Service.Id,
                            Name = sr.Service.Name,
                            Code = sr.Service.Code
                        },
                        ServiceRequestTasks = sr.ServiceRequestTasks.Select(srt => new ServiceRequestTask
                        {
                            Id = srt.Id,
                            ServiceRequestId = srt.ServiceRequestId,
                            AppointmentDate = sr.AppointmentDate,
                            Now = now,
                            ProcessTask = new ProcessTask
                            {
                                Id = srt.OTask.Id,
                                Name = srt.OTask.Name,
                                Sequence = srt.OTask.Sequence.Value
                            },
                            AssignedTo = new Person
                            {
                                Id = srt.AspNetUser == null ? (Guid?)null : srt.AspNetUser.Id,
                                Title = srt.AspNetUser.Title,
                                FirstName = srt.AspNetUser.FirstName,
                                LastName = srt.AspNetUser.LastName,
                                ColorCode = srt.AspNetUser.ColorCode,
                                Role = srt.AspNetUser.AspNetUserRoles.Select(r => new UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            },
                            CompletedDate = srt.CompletedDate,
                            IsObsolete = srt.IsObsolete,
                            Dependencies = srt.Child.Select(c => new ServiceRequestTaskDependent
                            {
                                TaskId = c.TaskId.Value,
                                CompletedDate = c.CompletedDate,
                                IsObsolete = c.IsObsolete
                            })
                        }),
                        People = sr.ServiceRequestTasks.Where(srt => srt.AssignedTo != null)
                            .Select(srt => new Person
                            {
                                Id = srt.AspNetUser == null ? (Guid?)null : srt.AspNetUser.Id,
                                Title = srt.AspNetUser.Title,
                                FirstName = srt.AspNetUser.FirstName,
                                LastName = srt.AspNetUser.LastName,
                                ColorCode = srt.AspNetUser.ColorCode,
                                Role = srt.AspNetUser.AspNetUserRoles.Select(r => new UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            }).Distinct()
                    }).ToList();

                // apply filters that use computed fields
                var filtered = source.Where(s => !s.IsDoneTheirPart(serviceProviderId, SystemTime.Now()));

                filtered = filtered.Where(s => s.IsAppointmentComplete.HasValue ? s.IsAppointmentComplete.Value : true);

                // group into hierarchy
                return filtered // this filters out the days
                    .GroupBy(d => new { d.DueDate })
                    .Select(d => new DayFolder
                    {
                        Day = d.Key.DueDate.Value,
                        ServiceRequests = filtered.Where(s => s.DueDate == d.Key.DueDate)
                    });
            }
        }
        public static ServiceRequest MapToServiceRequest(int serviceRequestId, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            using (var context = new data.OrvosiDbContext())
            {
                return context.ServiceRequests
                    .Where(d => d.Id == serviceRequestId)
                    .Select(sr => new ServiceRequest
                    {
                        Id = sr.Id,
                        DueDate = sr.DueDate,
                        AppointmentDate = sr.AppointmentDate,
                        StartTime = sr.StartTime,
                        Now = now,
                        CancelledDate = sr.CancelledDate,
                        IsClosed = sr.IsClosed,
                        BoxCaseFolderId = sr.BoxCaseFolderId,
                        Company = new Company
                        {
                            Id = sr.Company.Id,
                            Name = sr.Company.Name
                        },
                        Service = new Service
                        {
                            Id = sr.Service.Id,
                            Name = sr.Service.Name,
                            Code = sr.Service.Code
                        },
                        ServiceRequestTasks = sr.ServiceRequestTasks.Select(srt => new ServiceRequestTask
                        {
                            Id = srt.Id,
                            ServiceRequestId = srt.ServiceRequestId,
                            AppointmentDate = sr.AppointmentDate,
                            Now = now,
                            ProcessTask = new ProcessTask
                            {
                                Id = srt.OTask.Id,
                                Name = srt.OTask.Name,
                                Sequence = srt.OTask.Sequence.Value,
                                Workload = srt.Workload
                            },
                            AssignedTo = new Person
                            {
                                Id = srt.AspNetUser == null ? (Guid?)null : srt.AspNetUser.Id,
                                Title = srt.AspNetUser.Title,
                                FirstName = srt.AspNetUser.FirstName,
                                LastName = srt.AspNetUser.LastName,
                                ColorCode = srt.AspNetUser.ColorCode
                            },
                            CompletedDate = srt.CompletedDate,
                            IsObsolete = srt.IsObsolete,
                            Dependencies = srt.Child.Select(c => new ServiceRequestTaskDependent
                            {
                                TaskId = c.TaskId.Value,
                                CompletedDate = c.CompletedDate,
                                IsObsolete = c.IsObsolete
                            })
                        })
                    }).First();
            }
        }
    }
}