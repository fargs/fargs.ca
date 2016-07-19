using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.Role
{
    public class IndexViewModel
    {
        public List<Role> Roles { get; set; }
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
    }
}