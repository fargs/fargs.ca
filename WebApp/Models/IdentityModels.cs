using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using Orvosi.Shared.Enums;
using System.Web.Mvc;
using ImeHub.Data;
using ImeHub.Models;
using WebApp.Library.Extensions;
using LinqKit;
using WebApp.Views.Shared;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }
    public class ApplicationUserClaim : IdentityUserClaim<Guid> { }
    public class ApplicationUserRole : IdentityUserRole<Guid> { }
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole> { }
 
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim(nameof(DisplayName), this.DisplayName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            userIdentity.AddClaim(new Claim(nameof(Initials), this.Initials));
            userIdentity.AddClaim(new Claim(nameof(ColorCode), this.ColorCode));
            userIdentity.AddClaim(new Claim(ClaimTypes.Sid, this.Id.ToString()));
            userIdentity.AddClaim(new Claim(nameof(RoleId), this.RoleId.ToString()));
            userIdentity.AddClaim(new Claim(nameof(IsAppTester), this.IsAppTester.ToString()));
            userIdentity.AddClaim(new Claim(nameof(this.PhysicianId), this.PhysicianId.ToString()));

            
            using (var db = new ImeHubDbContext())
            {
                // set the physician
                var role = db.Roles
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.Id == this.RoleId)
                    .Select(LookupModel<Guid>.FromRole)
                    .AsEnumerable()
                    .Select(LookupViewModel<Guid>.FromLookupModel);

                userIdentity.AddClaim(new Claim("Role", JsonConvert.SerializeObject(role)));


                // set the physician
                var physician = db.Physicians
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.Id == this.PhysicianId)
                    .Select(LookupModel<Guid>.FromPhysician)
                    .AsEnumerable()
                    .Select(LookupViewModel<Guid>.FromLookupModel);

                userIdentity.AddClaim(new Claim("Physician", JsonConvert.SerializeObject(physician)));

                // get features permissions
                if (RoleId == AspNetRoles.SuperAdmin) // Features list is used to hide/show elements in the views so the entire list is needed.
                    Features = db.Features.Select(srf => srf.Id).ToArray();
                else
                    Features = db.RoleFeatures.Where(srf => srf.RoleId == RoleId).Select(srf => srf.FeatureId).ToArray();

                userIdentity.AddClaim(new Claim("Features", Features.ToJson()));

            }

            return userIdentity;
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }
        public DateTime? LastActivationDate { get; set; }
        public bool IsTestRecord { get; set; }
        public bool IsAppTester { get; set; }
        public Guid? PhysicianId { get; set; }
        public Guid RoleId { get; set; }

        public string DisplayName
        {
            get
            {
                string s = string.Empty;
                if (string.IsNullOrEmpty(this.Title))
                {
                    s = string.Format("{0} {1}", this.FirstName, this.LastName);
                }
                else
                {
                    s = string.Format("{0} {1} {2}", this.Title, this.FirstName, this.LastName);
                }
                return s;
            }
        }
        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
            }
        }
        public Guid[] Features { get; set; }
        public Guid[] Physicians { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("ImeHubDbContext")
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim");
        }
    }
}