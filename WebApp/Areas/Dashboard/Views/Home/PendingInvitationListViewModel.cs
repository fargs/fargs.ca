//using LinqKit;
//using Orvosi.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Web;
//using WebApp.Library.Extensions;
//using WebApp.Models;
//using WebApp.Views.Shared;
//using ImeHub.Data;
//using Enums = ImeHub.Models.Enums;


//namespace WebApp.Areas.Dashboard.Views.Home
//{
//    public class PendingInvitationListViewModel : ViewModelBase
//    {
//        public PendingInvitationListViewModel(ImeHub.Models.UserModel model)
//        {
//            List = model.Invites
//                .Where(i => i.AcceptanceStatus.Id == (byte)Enums.AcceptanceStatus.NotResponded)
//                .Select(i => new PhysicianInviteViewModel(i));
//        }
//        public IEnumerable<PhysicianInviteViewModel> List { get; set; }
//    }
//}