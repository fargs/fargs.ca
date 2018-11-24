using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class AcceptOwnershipFormModel
    {
        public AcceptOwnershipFormModel()
        {
        }
        public AcceptOwnershipFormModel(Guid ownerId)
        {
            OwnerId = ownerId;
        }
        [Required]
        public Guid OwnerId { get; set; }
    }
}