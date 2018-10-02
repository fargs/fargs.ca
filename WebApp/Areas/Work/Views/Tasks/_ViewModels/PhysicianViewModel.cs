using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.Tasks
{
    public class PhysicianViewModel : LookupViewModel<Guid>
    {
        public PhysicianViewModel(PhysicianDto physician)
        {
            this.Id = physician.Id;
            this.Name = physician.DisplayName;
            this.Code = physician.Initials;
            this.ColorCode = physician.ColorCode;
            this.TeamMembers = physician.TeamMembers.Select(LookupViewModel<Guid>.FromPersonDto);
        }
        public IEnumerable<LookupViewModel<Guid>> TeamMembers { get; set; }
    }
}