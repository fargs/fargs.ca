﻿using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.ServiceRequestModels2
{
    public static class ServiceRequestMapper2
    {
        public static IEnumerable<DayFolder> MapToDueDates(Guid serviceProviderId, DateTime now, Guid loggedInUserId, string requestUrl)
        {
            using (var context = new OrvosiDbContext())
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
            using (var context = new OrvosiDbContext())
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
                                Sequence = srt.OTask.Sequence.Value
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

    public class WeekFolder
    {
        public string WeekFolderName { get; set; }
        public DateTime StartDate { get; set; }
        public long StartDateTicks { get; set; }
        public DateTime EndDate { get; set; }
        public int AssessmentCount { get; set; } = 0;
        public int ToDoCount { get; internal set; } = 0;
        public int WaitingCount { get; set; }
        public IEnumerable<DayFolder> DayFolders { get; set; }
        public byte GetTimeline(DateTime now)
        {
            byte result = Orvosi.Shared.Enums.Timeline.Future;
            if (StartDate <= now && EndDate >= now)
                result = Orvosi.Shared.Enums.Timeline.Present;
            else if (EndDate < now)
                result = Orvosi.Shared.Enums.Timeline.Past;
            return result;
        }
    }

    public class WeekFolderEquals : IEqualityComparer<WeekFolder>
    {
        public bool Equals(WeekFolder left, WeekFolder right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.WeekFolderName == right.WeekFolderName && left.StartDate == right.StartDate;
        }

        public int GetHashCode(WeekFolder weekFolder)
        {
            return (weekFolder.WeekFolderName + weekFolder.StartDate.ToString()).GetHashCode();
        }
    }

    public class DayFolder
    {
        public DayFolder()
        {
            Assessments = new List<Assessment>();
        }
        public DateTime Day { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AssessmentCount { get; set; }
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public IEnumerable<ServiceRequest> ServiceRequests { get; set; }
        public IEnumerable<Assessment> Assessments { get; set; }
        public IEnumerable<ServiceRequest> DueDates { get; set; }

        // computeds
        public string DayFormatted_dddd
        {
            get
            {
                return Day.ToString("dddd");
            }
        }
        public string DayFormatted_MMMdd
        {
            get
            {
                return Day.ToString("MMM dd");
            }
        }
        public long DayTicks { get; set; }

    }

    public class Service
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ServiceRequest
    {
        // properties
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime Now { get; set; }
        public string BoxCaseFolderId { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool IsClosed { get; set; }

        // references
        public Service Service { get; set; }
        public Company Company { get; set; }
        public IEnumerable<ServiceRequestTask> ServiceRequestTasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<ServiceRequestMessage> Messages { get; set; }

        // computeds
        public int CommentCount { get; set; } = 0;
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public byte ServiceRequestStatusId { get; internal set; }
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public bool HasHighWorkload { get; internal set; }

        // methods
        public bool IsDoneTheirPart(Guid? userId, DateTime now)
        {
            return this.ServiceRequestTasks
                .Where(srt => srt.AssignedTo?.Id == userId)
                .All(srt => srt.Status.Id == TaskStatuses.Done || srt.Status.Id == TaskStatuses.Obsolete);
        }

        public string BoxCaseFolderURL
        {
            get
            {
                return $"https://orvosi.app.box.com/files/0/f/{BoxCaseFolderId}";
            }
        }
    }

    public class Assessment : ServiceRequest
    {
        public bool IsLateCancellation { get; set; }
        public bool IsNoShow { get; set; }
        public bool CanBeRescheduled
        {
            get
            {
                return !IsCancelled && AppointmentDate > SystemTime.Now();
            }
        }
        public bool CanBeCancelled
        {
            get
            {
                return !IsCancelled && AppointmentDate > SystemTime.Now();
            }

        }
        public bool CanBeUncancelled
        {
            get
            {
                return IsCancelled || IsLateCancellation && AppointmentDate > SystemTime.Now();
            }

        }
        public byte? ServiceStatusId
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
                    return null;
                }
            }
        }
    }

    public class AddOn : ServiceRequest
    {

    }

    public class Person
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ColorCode { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<ServiceRequestTask> ServiceRequestTasks { get; set; }

        // computeds
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{(!string.IsNullOrEmpty(Title) ? Title + " " : "")}{FirstName} {LastName}";
            }
        }

        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
            }
        }

    }

    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceRequestTaskStatus
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceRequestTask
    {
        public ServiceRequestTask()
        {
            Dependencies = new List<ServiceRequestTaskDependent>();
            WaitingOn = new List<Person>();
        }
        public int Id { get; set; }
        public ProcessTask ProcessTask { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Person CompletedBy { get; set; }
        public bool IsObsolete { get; set; }
        public Person AssignedTo { get; set; }
        public IEnumerable<Person> WaitingOn { get; set; }
        public ServiceRequestTask Parent { get; set; }
        public IEnumerable<ServiceRequestTaskDependent> Dependencies { get; set; }
        public int ServiceRequestId { get; set; }
        public DateTime? AppointmentDate { get; set; } // required for determining status
        public DateTime Now { get; set; }

        // methods
        public ServiceRequestTaskStatus Status
        {
            get
            {
                var isWaiting = Dependencies.Any(d => (!d.CompletedDate.HasValue && !d.IsObsolete && d.TaskId != 133) || (d.TaskId == 133 && AppointmentDate > Now));

                if (IsObsolete)
                {
                    return new ServiceRequestTaskStatus
                    {
                        Id = TaskStatuses.Obsolete,
                        Name = "Obsolete"
                    };
                }
                else if (CompletedDate.HasValue)
                {
                    return new ServiceRequestTaskStatus
                    {
                        Id = TaskStatuses.Done,
                        Name = "Done"
                    };
                }
                else if (isWaiting)
                {
                    return new ServiceRequestTaskStatus
                    {
                        Id = TaskStatuses.Waiting,
                        Name = "Waiting"
                    };
                }
                else
                {
                    return new ServiceRequestTaskStatus
                    {
                        Id = TaskStatuses.ToDo,
                        Name = "To Do"
                    };
                }
            }
        }
    }

    public class ServiceRequestTaskDependent
    {
        public short TaskId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsObsolete { get; set; }
    }


    public class ProcessItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Sequence { get; set; }
    }

    public class ProcessTask : ProcessItem
    {
        public UserRole ResponsibleRole { get; set; }
        public byte Workload { get; set; }
    }

    public class ServiceRequestMessage
    {
        public Guid Id { get; set; } // Id (Primary key)
        public string Message { get; set; } // Message (length: 256)
        public DateTime PostedDate { get; set; } // PostedDate
        public string PostedByDisplayName { get; internal set; }
        public string PostedByColorCode { get; internal set; }
        public string PostedByInitials { get; internal set; }
    }

}