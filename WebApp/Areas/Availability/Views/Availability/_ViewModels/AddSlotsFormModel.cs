using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;
using Enums = Orvosi.Shared.Enums;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AddSlotsFormModel
    {
        public AddSlotsFormModel()
        {
        }
        public Guid AvailableDayId { get; set; }
        public short StartHour { get; set; }
        public short StartMinute { get; set; }
        public short Duration { get; set; }
        public byte Repeat { get; set; }
    }
}