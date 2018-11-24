using System;
using Enums = ImeHub.Models.Enums;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class PhysicianInviteViewModel
    {
        public PhysicianInviteViewModel(ImeHub.Models.PhysicianModel physician)
        {
            PhysicianId = physician.Id;
            To = physician.Owner.Email;
            Physician = new ReadOnlyViewModel(physician);
        }

        public Guid PhysicianId { get; set; }
        public ReadOnlyViewModel Physician { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}