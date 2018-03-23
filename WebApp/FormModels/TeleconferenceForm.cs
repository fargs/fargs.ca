using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class TeleconferenceForm
    {
        public TeleconferenceForm()
        {
        }
        public Guid? TeleconferenceId { get; set; }
        public int ServiceRequestId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public LookupViewModel<byte> Result { get; set; }
    }
}