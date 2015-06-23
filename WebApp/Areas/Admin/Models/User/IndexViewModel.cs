using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Admin.Models.User
{
    public class IndexViewModel
    {
        public List<ApplicationUser> Users { get; set; }
    }
}