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
                    .AreAssignedToUser(serviceProviderId)
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
                        DayAndTime = d.Key.AppointmentDate.Value,
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
                    .AreAssignedToUser(serviceProviderId)
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
        public static IEnumerable<DueDateDayFolder> MapToDueDates(Guid userId, DateTime now, string requestUrl)
        {
            using (var context = new data.OrvosiDbContext())
            {
                var roleId = context.AspNetUsers.Single(u => u.Id == userId).AspNetUserRoles.First().RoleId;
                // read all the data in from the database.
                var source = context.ServiceRequestTasks
                    .Where(srt => srt.AssignedTo == userId)
                    .AreActive()
                    .Select(srt => new DayIndex
                    {
                        DueDate = srt.DueDate,
                        ServiceRequest = new ServiceRequest
                        {
                            Id = srt.ServiceRequest.Id,
                            ClaimantName = srt.ServiceRequest.ClaimantName,
                            DueDate = srt.ServiceRequest.DueDate,
                            AppointmentDate = srt.ServiceRequest.AppointmentDate,
                            Now = now,
                            StartTime = srt.ServiceRequest.StartTime,
                            CancelledDate = srt.ServiceRequest.CancelledDate,
                            IsClosed = srt.ServiceRequest.IsClosed,
                            BoxCaseFolderId = srt.ServiceRequest.BoxCaseFolderId,
                            PhysicianId = srt.ServiceRequest.PhysicianId,
                            Company = new Company
                            {
                                Id = srt.ServiceRequest.Company.Id,
                                Name = srt.ServiceRequest.Company.Name
                            },
                            Service = new Service
                            {
                                Id = srt.ServiceRequest.Service.Id,
                                Name = srt.ServiceRequest.Service.Name,
                                Code = srt.ServiceRequest.Service.Code
                            },
                            ServiceRequestMessages = srt.ServiceRequest.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new ServiceRequestMessage
                            {
                                Id = srm.Id,
                                TimeZone = srm.ServiceRequest.Address.TimeZone.Name,
                                Message = srm.Message,
                                PostedDate = srm.PostedDate,
                                PostedBy = new Person
                                {
                                    Id = srm.AspNetUser.Id,
                                    Title = srm.AspNetUser.Title,
                                    FirstName = srm.AspNetUser.FirstName,
                                    LastName = srm.AspNetUser.LastName,
                                    Email = srm.AspNetUser.Email,
                                    ColorCode = srm.AspNetUser.ColorCode,
                                    Role = srm.AspNetUser.AspNetUserRoles.Select(r => new UserRole
                                    {
                                        Id = r.AspNetRole.Id,
                                        Name = r.AspNetRole.Name
                                    }).FirstOrDefault()
                                }
                            }),
                            ServiceRequestTasks = srt.ServiceRequest.ServiceRequestTasks.Select(t => new ServiceRequestTask
                            {
                                Id = t.Id,
                                ServiceRequestId = t.ServiceRequestId,
                                AppointmentDate = srt.ServiceRequest.AppointmentDate,
                                DueDate = t.DueDate,
                                Now = now,
                                ProcessTask = new ProcessTask
                                {
                                    Id = t.OTask.Id,
                                    Name = t.OTask.Name,
                                    Sequence = t.OTask.Sequence.Value,
                                    ResponsibleRole = t.OTask.AspNetRole == null ? null : new UserRole
                                    {
                                        Id = t.OTask.AspNetRole.Id,
                                        Name = t.OTask.AspNetRole.Name
                                    }
                                },
                                AssignedTo = new Person
                                {
                                    Id = t.AspNetUser_AssignedTo == null ? (Guid?)null : t.AspNetUser_AssignedTo.Id,
                                    Title = t.AspNetUser_AssignedTo.Title,
                                    FirstName = t.AspNetUser_AssignedTo.FirstName,
                                    LastName = t.AspNetUser_AssignedTo.LastName,
                                    Email = t.AspNetUser_AssignedTo.Email,
                                    ColorCode = t.AspNetUser_AssignedTo.ColorCode,
                                    Role = t.AspNetUser_AssignedTo.AspNetUserRoles.Select(r => new UserRole
                                    {
                                        Id = r.AspNetRole.Id,
                                        Name = r.AspNetRole.Name
                                    }).FirstOrDefault()
                                },
                                CompletedDate = t.CompletedDate,
                                IsObsolete = t.IsObsolete,
                                Dependencies = t.Child.Select(c => new ServiceRequestTaskDependent
                                {
                                    TaskId = c.TaskId.Value,
                                    CompletedDate = c.CompletedDate,
                                    IsObsolete = c.IsObsolete
                                }),
                            })
                        }
                    })
                    .ToList();

                // group into hierarchy
                return source // this filters out the days
                    .GroupBy(srt => new { DueDate = srt.DueDate.HasValue ? srt.DueDate.Value : (DateTime?)null })
                    .Select(d => new DueDateDayFolder
                    {
                        DayAndTime = d.Key.DueDate,
                        ServiceRequests = source
                            .Where(srt => srt.DueDate == d.Key.DueDate)
                            .GroupBy(srt => new { srt.ServiceRequest })
                            .Select(sr => new ServiceRequest
                            {
                                Id = sr.Key.ServiceRequest.Id,
                                ClaimantName = sr.Key.ServiceRequest.ClaimantName,
                                DueDate = sr.Key.ServiceRequest.DueDate,
                                AppointmentDate = sr.Key.ServiceRequest.AppointmentDate,
                                Now = now,
                                StartTime = sr.Key.ServiceRequest.StartTime,
                                CancelledDate = sr.Key.ServiceRequest.CancelledDate,
                                IsClosed = sr.Key.ServiceRequest.IsClosed,
                                BoxCaseFolderId = sr.Key.ServiceRequest.BoxCaseFolderId,
                                PhysicianId = sr.Key.ServiceRequest.PhysicianId,
                                Company = sr.Key.ServiceRequest.Company,
                                Service = sr.Key.ServiceRequest.Service,
                                ServiceRequestMessages = sr.Key.ServiceRequest.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new ServiceRequestMessage
                                {
                                    Id = srm.Id,
                                    TimeZone = srm.TimeZone,
                                    Message = srm.Message,
                                    PostedDate = srm.PostedDate,
                                    PostedBy = srm.PostedBy
                                }),
                                ServiceRequestTasks = sr.Key.ServiceRequest.ServiceRequestTasks
                                    .Where(t => t.DueDate == d.Key.DueDate)
                                    .ShouldBeDisplayedToUser(roleId)
                                    .Select(t => new ServiceRequestTask
                                    {
                                        Id = t.Id,
                                        ServiceRequestId = t.ServiceRequestId,
                                        AppointmentDate = t.AppointmentDate,
                                        DueDate = t.DueDate,
                                        Now = now,
                                        ProcessTask = t.ProcessTask,
                                        AssignedTo = t.AssignedTo,
                                        CompletedDate = t.CompletedDate,
                                        IsObsolete = t.IsObsolete,
                                        Dependencies = t.Dependencies,
                                    })
                            })
                    });
            }
        }
        internal static int DueDatesCount(Guid serviceProviderId, DateTime now)
        {
            using (var context = new data.OrvosiDbContext())
            {
                var source = context
                    .ServiceRequests
                        .AreAssignedToUser(serviceProviderId)
                        .AreNotClosed()
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
                                ProcessTask = new ProcessTask
                                {
                                    Id = srt.OTask.Id
                                },
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
                        .AreAssignedToUser(serviceProviderIdOrDefault)
                        .AreNotClosed()
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
                                Id = srt.AspNetUser_TaskStatusChangedBy == null ? (Guid?)null : srt.AspNetUser_TaskStatusChangedBy.Id,
                                Title = srt.AspNetUser_TaskStatusChangedBy.Title,
                                FirstName = srt.AspNetUser_TaskStatusChangedBy.FirstName,
                                LastName = srt.AspNetUser_TaskStatusChangedBy.LastName,
                                ColorCode = srt.AspNetUser_TaskStatusChangedBy.ColorCode
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

        // Filters
        public static IEnumerable<ServiceRequestTask> ShouldBeDisplayedToUser(this IEnumerable<ServiceRequestTask> serviceRequestTasks, Guid roleId)
        {
            var rolesThatShouldSeeEverything = new Guid[2] { AspNetRoles.SuperAdmin, AspNetRoles.CaseCoordinator };
            var rolesThatShouldBeSeen = new Guid?[3] { AspNetRoles.Physician, AspNetRoles.IntakeAssistant, AspNetRoles.DocumentReviewer };
            if (!rolesThatShouldSeeEverything.Contains(roleId))
            {
                return serviceRequestTasks.Where(srt => rolesThatShouldBeSeen.Contains(srt.ProcessTask?.ResponsibleRole?.Id));
            }
            return serviceRequestTasks;
        }
    }

    internal class DayIndex
    {
        public DateTime? DueDate { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
    
}