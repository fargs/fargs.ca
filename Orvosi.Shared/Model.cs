using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Filters;

namespace Orvosi.Shared.Model
{
    public class WeekFolder
    {
        public string WeekFolderName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<DayFolder> DayFolders { get; set; }

        public int ToDoCount(Guid userId)
        {
            return DayFolders.Sum(d => d.ToDoCount(userId));
        }
        public int WaitingCount(Guid userId)
        {
            return DayFolders.Sum(d => d.WaitingCount(userId));
        }
        public int ServiceRequestCount(Guid userId)
        {
            return DayFolders.Sum(d => d.ServiceRequests.Count());
        }
        public long StartDateTicks
        {
            get
            {
                return StartDate.Ticks;
            }
        }
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

    public class DayFolderBase
    {
        public DateTime? DayAndTime { get; set; }
        public string Company { get; set; }
        public DateTime? Day
        {
            get
            {
                return DayAndTime.HasValue ? DayAndTime.Value.Date : (DateTime?)null;
            }
        }
        public long DayTicks
        {
            get
            {
                return DayAndTime.HasValue ? DayAndTime.Value.Date.Ticks : 0;
            }
        }
        public string DayFormatted_ddd
        {
            get
            {
                return DayAndTime.HasValue ? DayAndTime.Value.Date.ToString("ddd") : string.Empty;
            }
        }
        public string DayFormatted_dddd
        {
            get
            {
                return DayAndTime.HasValue ? DayAndTime.Value.Date.ToString("dddd") : string.Empty;
            }
        }
        public string DayFormatted_MMMdd
        {
            get
            {
                return DayAndTime.HasValue ? DayAndTime.Value.Date.ToString("MMMdd") : string.Empty;
            }
        }
        public IEnumerable<ServiceRequest> ServiceRequests { get; set; }
        public int ToDoCount(Guid userId)
        {
            return ServiceRequests.GroupBy(sr => new
            {
                ServiceRequestId = sr.Id,
                StatusOfNextTaskAssignedtoUser = sr.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .AreActive()
                .OrderBy(srt => srt.ProcessTask.Sequence)
                .First()
                .Status.Id
            })
            .Count(srt => srt.Key.StatusOfNextTaskAssignedtoUser == TaskStatuses.ToDo);
        }
        public int WaitingCount(Guid userId)
        {
            return ServiceRequests.GroupBy(sr => new
            {
                ServiceRequestId = sr.Id,
                StatusOfNextTaskAssignedtoUser = sr.ServiceRequestTasks
                .AreAssignedToUser(userId)
                .AreActive()
                .OrderBy(srt => srt.ProcessTask.Sequence)
                .First()
                .Status.Id
            })
            .Count(srt => srt.Key.StatusOfNextTaskAssignedtoUser == TaskStatuses.Waiting);
        }

    }

    public class DayFolder : DayFolderBase
    {
        public DayFolder()
        {
            Assessments = new List<Assessment>();
        }
        public string City { get; set; }
        public Address Address { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public IEnumerable<Assessment> Assessments { get; set; }
    }

    public class UnsentInvoiceDayFolder : DayFolderBase
    {
        public UnsentInvoiceDayFolder()
        {
            UnsentInvoices = new List<UnsentInvoice>();
        }
        public IEnumerable<UnsentInvoice> UnsentInvoices { get; set; }
    }

    public class DueDateDayFolder : DayFolderBase
    {
        public DueDateDayFolder()
        {
            ServiceRequests = new List<Assessment>();
        }
    }

    public class Service
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public short ServiceCategoryId { get; set; }
        public bool HasReportDeliverable
        {
            get
            {
                return ServiceCategoryId == ServiceCategories.AddOn || ServiceCategoryId == ServiceCategories.IndependentMedicalExam;
            }
        }
        public bool CanBeRescheduled
        {
            get
            {
                return ServiceCategoryId == ServiceCategories.IndependentMedicalExam || ServiceCategoryId == ServiceCategories.MedicalConsultation;
            }
        }
        public bool HasAppointment
        {
            get
            {
                return ServiceCategoryId == ServiceCategories.IndependentMedicalExam || ServiceCategoryId == ServiceCategories.MedicalConsultation;
            }
        }
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
        public Guid PhysicianId { get; set; }
        public string CalendarEventId { get; set; }

        // references
        public Service Service { get; set; }
        public Company Company { get; set; }
        public IEnumerable<ServiceRequestTask> ServiceRequestTasks { get; set; }
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<ServiceRequestMessage> ServiceRequestMessages { get; set; }
        public Invoice Invoice { get; set; }  // used on Unsent Invoice for Traceability
        public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }
        public Address Address { get; set; }

        // computeds
        public int CommentCount { get; set; } = 0;
        public byte ServiceRequestStatusId(Guid userId)
        {
            var query = ServiceRequestTasks
                .AreAssignedToUser(userId)
                .AreActive();

            if (query.Any(srt => srt.Status.Id == TaskStatuses.ToDo)) return TaskStatuses.ToDo;

            if (query.Any(srt => srt.Status.Id == TaskStatuses.Waiting)) return TaskStatuses.Waiting;

            if (IsNoShow) return TaskStatuses.Obsolete;

            return TaskStatuses.Done;


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
        public bool IsCancelled
        {
            get
            {
                return CancelledDate.HasValue;
            }
        }
        public bool IsCancellation
        {
            get
            {
                return CancelledDate.HasValue && !IsLateCancellation;
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
        public DateTime EffectiveDate
        {
            get
            {
                return AppointmentDate.HasValue ? AppointmentDate.Value : DueDate.Value;
            }
        }

        public Person CaseCoordinator { get; set; }
        public Person DocumentReviewer { get; set; }
        public Person IntakeAssistant { get; set; }
        public Person Physician { get; set; }

        // methods
        public bool IsDoneTheirPart(Guid? userId)
        {
            return this.ServiceRequestTasks
                .Where(srt => (srt.AssignedTo == null ? null : srt.AssignedTo.Id) == userId)
                .All(srt => srt.Status.Id == TaskStatuses.Done || srt.Status.Id == TaskStatuses.Obsolete);
        }

        public string CaseFolderName
        {
            get
            {
                if (Service.ServiceCategoryId == ServiceCategories.AddOn)
                {
                    return $"{this.DueDate.Value.ToString("yyyy-MM-dd")} {ClaimantName} ({Service.Code}-{Physician.UserName}) {Company.Code}-{Id}";
                }
                else
                {
                    return $"{this.AppointmentDate.Value.ToString("yyyy-MM-dd")}({this.StartTime.Value.ToString(@"hhmm")})-{Address.CityCode}-{ClaimantName} ({Service.Code}-{Physician.UserName}) {Company.Code}-{Id}";
                }
            }
        }
        public string CalendarEventTitle
        {
            get
            {
                return $"{this.Address.CityCode}: {ClaimantName} ({Service.Code}) {Company.Code}-{Id}";
            }
        }

        public bool CanBeRescheduled
        {
            get
            {
                return this.Service.HasAppointment;
            }
        }

        public bool CanBeCancelled
        {
            get
            {
                return !this.IsLateCancellation && !this.CancelledDate.HasValue && !this.IsNoShow;
            }
        }

        public bool CanBeUncancelled
        {
            get
            {
                return this.IsLateCancellation || this.CancelledDate.HasValue;
            }
        }

        public bool CanBeNoShow
        {
            get
            {
                if (Service.HasAppointment)
                {
                    return !CancelledDate.HasValue && !IsLateCancellation && !IsNoShow;
                }
                return false;
            }
        }

        public bool CanNoShowBeUndone
        {
            get
            {
                if (Service.HasAppointment)
                {
                    return IsNoShow;
                }
                return false;
            }
        }

        public bool IsReportSubmitted
        {
            get
            {
                if (!Service.HasReportDeliverable)
                    throw new Exception("Submitting a report is not applicable to this service.");

                return ServiceRequestTasks.First(c => c.ProcessTask.Id == Tasks.SubmitReport || c.ProcessTask.Id == 36).CompletedDate.HasValue;
            }
        }

        public TimeSpan? EndTime { get; set; }
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
        public string Notes { get; set; }

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
        public string UserName
        {
            get
            {
                return Email.Split('@')[0];
            }
        }

        public string BoxFolderId { get; set; }
        public string BoxCaseTemplateFolderId { get; set; }
        public string BoxAddOnTemplateFolderId { get; set; }
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
                var isWaiting = Dependencies.Any(d => (!d.CompletedDate.HasValue && !d.IsObsolete && d.TaskId != 133) || (d.TaskId == 133 && AppointmentDate.HasValue && AppointmentDate.Value.Date > Now));

                if (IsObsolete)
                {
                    return new ServiceRequestTaskStatus
                    {
                        Id = TaskStatuses.Obsolete,
                        Name = "Obsolete"
                    };
                }
                else if (CompletedDate.HasValue || (ProcessTask.Id == 133 && AppointmentDate.HasValue && AppointmentDate.Value.Date <= Now))
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

        public DateTime? DueDate { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
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
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string ProvinceCode { get; set; }
        public string TimeZone { get; set; }
        public short? ProvinceId { get; set; }
        public string TimeZoneIana { get; set; }

        public override string ToString()
        {
            return $"{Address1}, {City} {ProvinceCode}, {PostalCode}, {Name}";
        }
    }

    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new List<InvoiceDetail>();
            Receipts = new List<Receipt>();
        }
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        private string _terms;
        public string Terms { get
            {
                return string.IsNullOrEmpty(_terms) ? "90" : _terms;
            } set
            {
                _terms = value;
            }
        }
        public DateTime? PaymentDueDate { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TaxRateHst { get; set; }
        public decimal? Hst { get; set; }
        public decimal? Total { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public Customer Customer { get; set; }
        public Guid InvoiceGuid { get; set; }
        public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }
        public int InvoiceDetailCount { get; set; }
        public int? ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }
        public decimal AmountPaid
        {
            get
            {
                return Receipts.Sum(r => r.Amount);
            }
        }
        public decimal OutstandingBalance
        {
            get
            {
                return Total.Value - AmountPaid;
            }
        }
        public bool IsPaid
        {
            get
            {
                return OutstandingBalance <= 0 && InvoiceDetailCount > 0;
            }
        }
        public bool IsSent
        {
            get
            {
                return SentDate.HasValue || InvoiceDetails.Any(id => id.ServiceRequest == null ? false : id.ServiceRequest.ServiceRequestTasks.Any(srt => srt.ProcessTask.Id == Tasks.SubmitInvoice && srt.CompletedDate.HasValue));
            }
        }

        public bool IsPartiallyPaid
        {
            get
            {
                return OutstandingBalance > 0 && OutstandingBalance < Total.Value;
            }
        }
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
        public int? ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }

    public class Receipt
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
    }

    public class UnsentInvoice
    {
        public ServiceRequest ServiceRequest { get; set; }
        public Invoice Invoice { get; set; }
        public DateTime Day
        {
            get
            {
                if (ServiceRequest == null)
                {
                    return Invoice.InvoiceDate;
                }
                else if (Invoice == null)
                {
                    return ServiceRequest.EffectiveDate;
                }
                else
                {
                    return ServiceRequest.EffectiveDate;
                    throw new Exception($"Invoice {Invoice.Id} Date nor Service Request {ServiceRequest.Id} Date exist.");
                }
            }
        }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BillingEmail { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }

    public class ServiceProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }

    public class ServiceRequestMessage
    {
        public Guid Id { get; set; }
        public string TimeZone { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; }
        public Person PostedBy { get; set; }
        public DateTime PostedDateLocal
        {
            get
            {
                return PostedDate.ToLocalTimeZone(TimeZone);
            }
        }
    }
}
