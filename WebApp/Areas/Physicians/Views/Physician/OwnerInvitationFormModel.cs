using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class OwnerInvitationFormModel
    {
        public OwnerInvitationFormModel()
        {
        }

        public OwnerInvitationFormModel(Guid physicianId, IIdentity identity)
        {
            PhysicianId = physicianId;
            FromEmail = identity.GetEmail();
            FromName = identity.GetDisplayName();
        }

        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}