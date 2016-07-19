using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.Role
{
    public class AssignUsersViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public List<User> Users { get; set; }
    }

    public class User
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAssigned { get; set; }
    }
}