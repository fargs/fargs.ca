using System;
using System.Security.Principal;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(ListViewModel list, IIdentity identity, DateTime now) : base(identity, now)
        {
            List = list;
        }
        public ListViewModel List { get; set; }
    }
}