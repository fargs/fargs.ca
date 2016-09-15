using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ListViewModel
    {
        public List<AspNetUser> Users { get; set; }
    }

    public class DetailViewModel
    {
        public AspNetUser User { get; set; }
        public SelectList Companies { get; set; }
    }

    public class AccountViewModel
    {
        public Account Account { get; set; }
        public SelectList Companies { get; set; }
    }

    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
    }

    public class CompaniesViewModel
    {
        public AspNetUser User { get; set; }
        public List<PhysicianCompany> Companies { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}