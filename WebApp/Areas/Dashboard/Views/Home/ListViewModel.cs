using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using ImeHub.Data;
using System.Security.Principal;
using System;
using System.Linq;
using LinqKit;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(ImeHub.Models.UserModel model)
        {
            AsTeamMember = model.AsTeamMember
                .Select(LookupViewModel<Guid>.FromLookupModel);

            AsManager = model.AsManager
                .Select(LookupViewModel<Guid>.FromLookupModel);

            AsOwner = model.AsOwner
                .Select(LookupViewModel<Guid>.FromLookupModel);
        }
        public IEnumerable<LookupViewModel<Guid>> AsTeamMember { get; set; }
        public IEnumerable<LookupViewModel<Guid>> AsManager { get; set; }
        public IEnumerable<LookupViewModel<Guid>> AsOwner { get; set; }
    }
}