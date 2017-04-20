using Orvosi.Data;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BillingEmail { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}