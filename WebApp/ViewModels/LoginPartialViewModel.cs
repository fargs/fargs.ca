using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Orvosi.Data;
using System.Security.Principal;
using WebApp.Library.Extensions;

namespace WebApp.ViewModels
{
    public class LoginPartialViewModel
    {
        public LoginPartialViewModel()
        {
        }

        public bool IsAuthenticated { get; set; }
        public string UserDisplayName { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; }
        public Guid RoleId { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}