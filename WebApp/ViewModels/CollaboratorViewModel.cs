using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.CollaboratorViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Role> Roles { get; set; }
        public int Total { get; set; }
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Orvosi.Shared.Model.Person> People { get; set; }
    }
}