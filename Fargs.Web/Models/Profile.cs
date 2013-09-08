using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fargs.Web.Models
{
    public class Profile
    {
        public string Title { get; set; }
        public List<Job> Jobs { get; set; }
    }
}