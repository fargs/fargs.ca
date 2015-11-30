using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ListViewModel
    {
        public List<User> Users { get; set; }
    }

    public class DetailViewModel
    {
        public User User { get; set; }
        public SelectList Companies { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public short RoleId { get; set; }
        public string RoleName { get; set; }
        public Nullable<short> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameSubmitted { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}