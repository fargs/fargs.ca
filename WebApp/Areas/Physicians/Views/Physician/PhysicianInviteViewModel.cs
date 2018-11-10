using System;
using Enums = ImeHub.Models.Enums;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class PhysicianInviteViewModel
    {
        public PhysicianInviteViewModel(ImeHub.Models.PhysicianModel.PhysicianInviteModel invite)
        {
            Id = invite.Id;
            PhysicianId = invite.PhysicianId;
            ToEmail = invite.ToEmail;
            ToName = invite.ToName;
            FromEmail = invite.FromEmail;
            FromName = invite.FromName;
            PhysicianName = invite.Physician.Name;
            FromEmail = invite.FromEmail;
            FromName = invite.FromName;
            AcceptanceStatus = invite.AcceptanceStatus.Name;
            SentDate = invite.SentDate.HasValue ? invite.SentDate.ToOrvosiDateTimeFormat() : "Not Sent";
        }

        public Guid Id { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public Guid PhysicianId { get; set; }
        public string PhysicianName { get; set; }
        public string AcceptanceStatus { get; set; }
        public string SentDate { get; set; }
    }
}