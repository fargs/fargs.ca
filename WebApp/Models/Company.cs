using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoCssClass { get; set; }
        public string MasterBookingPageByTime { get; set; }
        public string MasterBookingPageByPhysician { get; set; }
    }
}