using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orvosi.Shared.Enums;

namespace Orvosi.Shared.Model
{
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
        public Company Company { get; set; }
        public Address Address { get; set; }
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
        public bool IsLateCancellation { get; set; }
        public bool IsNoShow { get; set; }
        public bool IsClosed { get; set; }
        public decimal? ServiceCataloguePrice { get; set; }
        public decimal? NoShowRate { get; set; }
        public decimal? LateCancellationRate { get; set; }
        public string Notes { get; set; }
        public string City { get; set; }

        // references
        public Service Service { get; set; }
        public Company Company { get; set; }
        public IEnumerable<ServiceRequestTask> ServiceRequestTasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<ServiceRequestMessage> Messages { get; set; }
        public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }
        public Address Address { get; set; }

        // computeds
        public int CommentCount { get; set; } = 0;
        public int ToDoCount { get; set; }
        public int WaitingCount { get; set; }
        public byte ServiceRequestStatusId { get; internal set; }
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
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public bool HasHighWorkload { get; internal set; }
        public bool? IsAppointmentComplete
        {
            get
            {
                if (AppointmentDate.HasValue)
                {
                    return Now >= new DateTime(AppointmentDate.Value.Ticks + StartTime.Value.Ticks);
                }
                return null;
            }
        }
        public string BoxCaseFolderURL
        {
            get
            {
                return $"https://orvosi.app.box.com/files/0/f/{BoxCaseFolderId}";
            }
        }

        // methods
        public bool IsDoneTheirPart(Guid? userId)
        {
            return this.ServiceRequestTasks
                .Where(srt => srt.AssignedTo?.Id == userId)
                .All(srt => srt.Status.Id == TaskStatuses.Done || srt.Status.Id == TaskStatuses.Obsolete);
        }
    }

    public class Assessment : ServiceRequest
    {
        public bool CanBeRescheduled
        {
            get
            {
                return !IsCancelled && AppointmentDate > Now;
            }
        }
        public bool CanBeCancelled
        {
            get
            {
                return !IsCancelled && AppointmentDate > Now;
            }

        }
        public bool CanBeUncancelled
        {
            get
            {
                return IsCancelled || IsLateCancellation && AppointmentDate > Now;
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
        public string BoxUserId { get; set; }
        public string Email { get; set; }
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
                var isWaiting = Dependencies.Any(d => (!d.CompletedDate.HasValue && !d.IsObsolete && d.TaskId != 133) || (d.TaskId == 133 && AppointmentDate.Value.Date > Now));

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
        public byte? Workload { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string ProvinceCode { get; set; }
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TaxRateHst { get; set; }
        public decimal? Hst { get; set; }
        public decimal? Total { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }
        public Customer Customer { get; set; }
        public Guid InvoiceGuid { get; set; }
    }

    public class InvoiceDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Total { get; set; }
        public string AdditionalNotes { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BillingEmail { get; set; }

    }

    public class ServiceRequestMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public Person PostedBy { get; set; }

    }
}
