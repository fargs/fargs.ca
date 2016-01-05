﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace WebApp.Areas.Staff.ViewModels.Home
{
    public class IndexViewModel
    {
        public List<ServiceRequest> Today { get; set; }
        public List<ServiceRequest> Upcoming { get; set; }
    }
}