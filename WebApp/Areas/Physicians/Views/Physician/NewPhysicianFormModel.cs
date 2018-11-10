using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class NewPhysicianFormModel
    {
        
        [Required]
        public string CompanyName { get; set; }
    }
}