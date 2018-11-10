using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class AcceptOwnershipFormModel
    {
        public AcceptOwnershipFormModel()
        {
        }
        public AcceptOwnershipFormModel(Guid inviteId)
        {
            InviteId = inviteId;
        }
        [Required]
        public Guid InviteId { get; set; }
    }
}