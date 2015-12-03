using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Admin.ViewModels.CompanyViewModels
{
    public class IndexViewModel
    {
        public List<Company> Companies { get; set; }
        public List<User> CompanyContacts { get; set; }
    }
}