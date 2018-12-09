using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Physicians.Views.Invite
{
    public class RegisterUserFormModel
    {
        public RegisterUserFormModel()
        {
        }
        public RegisterUserFormModel(string email, Guid physicianId)
        {
            Email = email;
            PhysicianId = physicianId;
        }
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Guid PhysicianId { get; set; }
    }
}