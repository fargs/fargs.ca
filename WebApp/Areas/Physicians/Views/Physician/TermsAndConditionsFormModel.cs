using System;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class TermsAndConditionsFormModel
    {
        public TermsAndConditionsFormModel()
        {
        }
        public TermsAndConditionsFormModel(Guid physicianId)
        {
            PhysicianId = physicianId;
        }

        public Guid PhysicianId { get; set; }
        public bool IsAccepted { get; set; } = false;
    }
}