using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(ListViewModel list, ReadOnlyViewModel readOnly, IIdentity identity, DateTime now) : base(identity, now)
        {
            List = list;
            ReadOnly = readOnly;    
        }
        public ListViewModel List { get; set; }
        public ReadOnlyViewModel ReadOnly { get; set; }
    }
}