using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Service
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short ServiceCategoryId { get; set; }
        public short ServicePortfolioId { get; set; }
    }
}