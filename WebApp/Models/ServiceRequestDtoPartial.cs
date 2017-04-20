using System;
using System.Collections.Generic;
using WebApp.Library.Extensions;
using System.Linq;
using Orvosi.Shared.Enums;

namespace WebApp.Models
{
    public partial class ServiceRequestDto
    {
        public DateTime? Year
        {
            get
            {
                return AppointmentDate.HasValue ? new DateTime(AppointmentDate.Value.Year, 1, 1) : (DateTime?)null;
            }
        }
        public string YearName
        {
            get
            {
                return AppointmentDate.HasValue ? AppointmentDate.Value.Year.ToString() : "No Appointment";
            }
        }
        public DateTime? Month
        {
            get
            {
                return AppointmentDate.HasValue ? new DateTime(AppointmentDate.Value.Year, AppointmentDate.Value.Month, 1) : (DateTime?)null;
            }
        }
        public string MonthName
        {
            get
            {
                return Month.HasValue ? Month.Value.ToString("MMM") : "No Appointment";
            }
        }

        public DateTime FirstDayOfWeekByAppointment
        {
            get
            {
                return AppointmentDate.HasValue ? AppointmentDate.Value.GetStartOfWeek() : DateTime.MinValue;
            }
        }
        public string WeekNameByAppointment
        {
            get
            {
                return AppointmentDate.HasValue ? AppointmentDate.Value.ToWeekFolderName() : "No Appointment Date";
            }
        }
        public DateTime FirstDayOfWeekByDueDate
        {
            get
            {
                return DueDate.HasValue ? DueDate.Value.GetStartOfWeek() : DateTime.MinValue;
            }
        }
        public string WeekNameByDueDate
        {
            get
            {
                return DueDate.HasValue ? DueDate.Value.ToWeekFolderName() : "No Due Date";
            }
        }

        public string BoxCaseFolderURL
        {
            get
            {
                return $"https://orvosi.app.box.com/files/0/f/{BoxCaseFolderId}";
            }
        }
        public bool HasAppointment
        {
            get
            {
                return this.AppointmentDate.HasValue;
            }
        }
        public bool HasReportDeliverable
        {
            get
            {
                return this.DueDate.HasValue;
            }
        }

        public bool CanBeRescheduled(DateTime? appointmentDate)
        {
            return this.AppointmentDate.HasValue;
        }
        public bool CanBeCancelled(bool isNoShow, bool isLateCancellation, DateTime? cancelledDate)
        {
            return !this.IsLateCancellation && !this.CancelledDate.HasValue && !this.IsNoShow;
        }
        public bool CanBeUncancelled(bool isLateCancellation, DateTime? cancelledDate)
        {
            return this.IsLateCancellation || this.CancelledDate.HasValue;
        }
        public bool CanBeNoShow(DateTime? appointmentDate)
        {
            return AppointmentDate.HasValue && !CancelledDate.HasValue;
        }

        public IEnumerable<LookupDto<Guid>> RequiredRoles(IEnumerable<TaskDto> tasks)
        {
            return tasks.Where(t => t.ResponsibleRoleId.HasValue).Select(t => new LookupDto<Guid>
            {
                Id = t.ResponsibleRoleId.Value,
                Name = t.ResponsibleRoleName
            })
            .Distinct()
            .ToList();
        }

        public bool IsSubmitInvoiceTaskDone
        {
            get
            {
                if (Tasks == null)
                {
                    throw new Exception("Tasks have not been populated from the entities.");
                }
                var task = Tasks.FirstOrDefault(srt => srt.TaskId == Orvosi.Shared.Enums.Tasks.SubmitInvoice);
                if (task == null)
                {
                    return false;
                }
                return task.TaskStatusId == TaskStatuses.Done || task.TaskStatusId == TaskStatuses.Archive;
            }
        }
    }

    public enum AppointmentStatuses
    {
        Waiting, Done, NoShow, LateCancellation, Cancelled
    }
}