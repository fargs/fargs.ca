using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class IndexViewModel : ContactViewModel
    {
        public ListViewModel List { get; set; }
        
        public IndexViewModel(UserModel model) : base(model)
        {
            List = new ListViewModel(model);
        }
    }
}