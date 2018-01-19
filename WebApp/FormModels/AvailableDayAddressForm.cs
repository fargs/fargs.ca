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
    public class AvailableDayAddressForm
    {
        [Required]
        public short AvailableDayId { get; set; }
        public int? AddressId { get; set; }
        public Guid PhysicianId { get; set; }
    }
}