using data = Orvosi.Data;
using Orvosi.Data.Filters;
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
        public static DayFolder MapToToday(Guid serviceProviderId, DateTime day, DateTime now, Guid loggedInUserId, string baseUrl)
        {
            using (var context = new data.OrvosiDbContext())
            {
                // read all the data in from the database.
                var source = context.ServiceRequests
                    .AreAssignedToServiceProvider(serviceProviderId)
                    .AreScheduledThisDay(day)
                    .Select(sr => new Assessment
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
                        IsNoShow = sr.IsNoShow,
                        IsLateCancellation = sr.IsLateCancellation,
                        Notes = sr.Notes,
                        Address = new Address
                        {
                            Id = sr.Address.Id,
                            Name = sr.Address.Name,
                            City = sr.Address.City_CityId.Name,
                            ProvinceCode = sr.Address.Province.ProvinceCode
                        },
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
                                ResponsibleRole = srt.OTask.AspNetRole == null ? null : new UserRole
                                {
                                    Id = srt.OTask.AspNetRole.Id,
                                    Name = srt.OTask.AspNetRole.Name
                                }
                            },
                            AssignedTo = new Person
                            {
                                Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id,
                                Title = srt.AspNetUser_AssignedTo.Title,
                                FirstName = srt.AspNetUser_AssignedTo.FirstName,
                                LastName = srt.AspNetUser_AssignedTo.LastName,
                                ColorCode = srt.AspNetUser_AssignedTo.ColorCode,
                                Role = srt.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new UserRole
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
                                Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id,
                                Title = srt.AspNetUser_AssignedTo.Title,
                                FirstName = srt.AspNetUser_AssignedTo.FirstName,
                                LastName = srt.AspNetUser_AssignedTo.LastName,
                                ColorCode = srt.AspNetUser_AssignedTo.ColorCode,
                                Role = srt.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            }).Distinct()
                    }).ToList();

                // group into hierarchy
                return source
                    .Where(sr => sr.ServiceStatusId.GetValueOrDefault(0) != ServiceStatus.Cancellation)
                    .GroupBy(d => new { d.AppointmentDate, d.Address })
                    .Select(d => new DayFolder
                    {
                        Day = d.Key.AppointmentDate.Value,
                        Address = d.Key.Address,
                        Assessments = source.Where(s => s.AppointmentDate == d.Key.AppointmentDate && s.ServiceStatusId.GetValueOrDefault(0) != ServiceStatus.Cancellation)
                    }).FirstOrDefault();
            }
        }
        public static int ScheduleThisDayCount(Guid serviceProviderId, DateTime day)
        {
            using (var context = new data.OrvosiDbContext())
            {
                // read all the data in from the database.
                var source = context.ServiceRequests
                    .AreAssignedToServiceProvider(serviceProviderId)
                    .AreScheduledThisDay(day)
                    .Select(sr => new ServiceRequest
                    {
                        Id = sr.Id,
                        IsLateCancellation = sr.IsLateCancellation,
                        CancelledDate = sr.CancelledDate,
                        IsNoShow = sr.IsNoShow
                    })
                    .ToList();

                return source
                    .Where(sr => sr.ServiceStatusId.GetValueOrDefault(0) != ServiceStatus.Cancellation)
                    .Count();
            }
        }
        public static IEnumerable<DayFolder> MapToDueDates(Guid serviceProviderId, DateTime now, string requestUrl)
        {
            using (var context = new data.OrvosiDbContext())
            {
                // read all the data in from the database.
                var source = context.ServiceRequests
                    .AreAssignedToServiceProvider(serviceProviderId)
                    .HasDueDate()
                    .AreOpen()
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
                        PhysicianId = sr.PhysicianId,
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
                                Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id,
                                Title = srt.AspNetUser_AssignedTo.Title,
                                FirstName = srt.AspNetUser_AssignedTo.FirstName,
                                LastName = srt.AspNetUser_AssignedTo.LastName,
                                ColorCode = srt.AspNetUser_AssignedTo.ColorCode,
                                Role = srt.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new UserRole
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
                                Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id,
                                Title = srt.AspNetUser_AssignedTo.Title,
                                FirstName = srt.AspNetUser_AssignedTo.FirstName,
                                LastName = srt.AspNetUser_AssignedTo.LastName,
                                ColorCode = srt.AspNetUser_AssignedTo.ColorCode,
                                Role = srt.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            }).Distinct()
                    }).ToList();

                // apply filters that use computed fields
                var filtered = source.Where(s => !s.IsDoneTheirPart(serviceProviderId));

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
        internal static int DueDatesCount(Guid serviceProviderId, DateTime now)
        {
            using (var context = new data.OrvosiDbContext())
            {
                var source = context
                    .ServiceRequests
                        .AreAssignedToServiceProvider(serviceProviderId)
                        .HasDueDate()
                        .AreOpen()
                        .Select(sr => new ServiceRequest
                        {
                            Id = sr.Id,
                            DueDate = sr.DueDate,
                            AppointmentDate = sr.AppointmentDate,
                            Now = now,
                            StartTime = sr.StartTime,
                            ServiceRequestTasks = sr.ServiceRequestTasks.Select(srt => new ServiceRequestTask
                            {
                                Id = srt.Id,
                                AppointmentDate = sr.AppointmentDate,
                                Now = now,
                                AssignedTo = new Person
                                {
                                    Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id
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
                        }).ToList();

                // apply filters that use computed fields
                var filtered = source.Where(s => !s.IsDoneTheirPart(serviceProviderId));

                filtered = filtered.Where(s => s.IsAppointmentComplete.HasValue ? s.IsAppointmentComplete.Value : true);

                return filtered.Count();
            }
        }
        internal static int AdditionalsCount(Guid serviceProviderIdOrDefault)
        {
            using (var context = new data.OrvosiDbContext())
            {
                return context
                    .ServiceRequests
                        .AreAddOns()
                        .AreAssignedToServiceProvider(serviceProviderIdOrDefault)
                        .AreOpen()
                        .Count();
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
                        IsLateCancellation = sr.IsLateCancellation,
                        IsNoShow = sr.IsNoShow,
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
                                Workload = srt.Workload,
                                ResponsibleRole = srt.OTask.AspNetRole == null ? null : new UserRole
                                {
                                    Id = srt.OTask.AspNetRole.Id,
                                    Name = srt.OTask.AspNetRole.Name
                                }
                            },
                            AssignedTo = new Person
                            {
                                Id = srt.AspNetUser_AssignedTo == null ? (Guid?)null : srt.AspNetUser_AssignedTo.Id,
                                Title = srt.AspNetUser_AssignedTo.Title,
                                FirstName = srt.AspNetUser_AssignedTo.FirstName,
                                LastName = srt.AspNetUser_AssignedTo.LastName,
                                ColorCode = srt.AspNetUser_AssignedTo.ColorCode
                            },
                            CompletedDate = srt.CompletedDate,
                            CompletedBy = new Person
                            {
                                Id = srt.AspNetUser_CompletedBy == null ? (Guid?)null : srt.AspNetUser_CompletedBy.Id,
                                Title = srt.AspNetUser_CompletedBy.Title,
                                FirstName = srt.AspNetUser_CompletedBy.FirstName,
                                LastName = srt.AspNetUser_CompletedBy.LastName,
                                ColorCode = srt.AspNetUser_CompletedBy.ColorCode
                            },
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