using Orvosi.Data;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ServiceProviderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}