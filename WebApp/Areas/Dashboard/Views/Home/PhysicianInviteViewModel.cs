using System;
using WebApp.Library.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class PhysicianInviteViewModel
    {
        public PhysicianInviteViewModel()
        {
        }
        public PhysicianInviteViewModel(ImeHub.Models.UserModel.PhysicianInviteModel invite)
        {
            Id = invite.Id;
            PhysicianId = invite.PhysicianId;
            ToEmail = invite.ToEmail;
            ToName = invite.ToName;
            FromEmail = invite.FromEmail;
            FromName = invite.FromName;
            PhysicianName = invite.Physician.Name;
            AcceptanceStatus = invite.AcceptanceStatus.Name;
        }

        public Guid Id { get; private set; }
        public Guid PhysicianId { get; private set; }
        public string ToEmail { get; private set; }
        public string ToName { get; private set; }
        public string FromEmail { get; private set; }
        public string FromName { get; private set; }
        public string PhysicianName { get; private set; }
        public string AcceptanceStatus { get; private set; }
    }
}