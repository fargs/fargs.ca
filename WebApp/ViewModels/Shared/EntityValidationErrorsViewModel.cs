using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.Shared
{
    public class EntityValidationErrorsViewModel
    {
        public string Id { get; set; }
        public DbEntityValidationException  Exception { get; set; }
    }
}