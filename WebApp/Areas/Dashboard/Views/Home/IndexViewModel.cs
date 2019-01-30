using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Dashboard.Views.Home
{
    public class IndexViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public ListViewModel List { get; set; }

        public IndexViewModel(ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var user = db.Users.AsNoTracking()
                .AsExpandable()
                .Where(u => u.Id == LoggedInUserId)
                .Select(UserModel.FromUser)
                .Single();

            
            SetProperties(user);
        }
        public IndexViewModel(UserModel user)
        {
            SetProperties(user);
        }

        private void SetProperties(UserModel user)
        {
            User = new UserViewModel
            {
                Id = user.Id,
                EmailProvider = user.EmailProvider,
                EmailProviderCredential = user.EmailProviderCredential,
                IsEmailProviderSet = user.IsEmailProviderSet
            };
            List = new ListViewModel(user);
        }
        
        public class UserViewModel
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string EmailProvider { get; set; }
            public string EmailProviderCredential { get; set; }
            public bool IsEmailProviderSet { get; set; }
        }
    }
}