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
        }

        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}