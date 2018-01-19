using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class AvailableDayResourceForm
    {
        [Required]
        public short AvailableDayId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}