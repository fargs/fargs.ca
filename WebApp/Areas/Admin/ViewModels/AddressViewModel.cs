using System.Web.Mvc;
using Orvosi.Data;
using System;

namespace WebApp.Areas.Admin.ViewModels
{
    public class AddressViewModel
    {
        public Address Address { get; set; }
        public string Owner { get; set; }
    }
    public class OwnerViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}