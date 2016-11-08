using System;
using System.Collections.Generic;

namespace Orvosi.Shared.Accounting
{
    public interface IServiceRequest
    {
        int Id { get; set; }
        string ClaimantName { get; set; }
        DateTime? DueDate { get; set; }
        DateTime? AppointmentDate { get; set; }
        TimeSpan? StartTime { get; set; }
        DateTime Now { get; set; }
        DateTime? CancelledDate { get; set; }
        bool IsLateCancellation { get; set; }
        bool IsNoShow { get; set; }
        bool IsClosed { get; set; }
        decimal? ServiceCataloguePrice { get; set; }
        decimal? NoShowRate { get; set; }
        decimal? LateCancellationRate { get; set; }
        string Notes { get; set; }
        Guid PhysicianId { get; set; }
        //Service Service { get; set; }
        //Company Company { get; set; }
        //IEnumerable<ServiceRequestTask> ServiceRequestTasks { get; set; }
        //Address Address { get; set; }
        byte? ServiceStatusId { get; }
        bool IsCancelled { get; }
        bool? IsAppointmentComplete { get; }
    }
}