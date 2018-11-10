using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.Admin.Views.Home
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(IIdentity identity, DateTime now) : base(identity, now)
        {
        }
    }
}