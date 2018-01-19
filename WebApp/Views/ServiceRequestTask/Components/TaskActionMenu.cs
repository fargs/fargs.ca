using System;
using WebApp.ViewDataModels;
using WebApp.ViewModels;

namespace WebApp.Views.ServiceRequestTask.Components
{
    public class TaskActionMenu
    {
        public ViewTarget ViewTarget { get; set; }
        public long Id { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public int ServiceRequestId { get; set; }
        public short TaskStatusId { get; set; }
        public bool IsAppointment { get; set; } = false;
        public DateTime? DueDate { get; set; }
    }

}