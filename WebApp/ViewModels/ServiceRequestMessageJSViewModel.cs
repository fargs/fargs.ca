using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels
{
    public class ServiceRequestMessageJSViewModel
    {
        public NameValueCollection QueryString { get; set; }
        public int[] ServiceRequestIds { get; set; }
    }
}