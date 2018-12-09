using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Physicians.Views.Invite
{
    public class AcceptOwnerInviteFormModel
    {
        public Guid PhysicianId { get; set; }
        public string Email { get; set; }
    }
}