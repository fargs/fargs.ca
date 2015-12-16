using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

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
        public User User { get; set; }
        public List<PhysicianCompany> Companies { get; set; }
    }
}