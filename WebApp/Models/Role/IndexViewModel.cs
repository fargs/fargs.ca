using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Role
{
    public class IndexViewModel
    {
        public List<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> Roles { get; set; }
    }
}