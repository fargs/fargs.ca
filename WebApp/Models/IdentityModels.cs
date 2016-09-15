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
            // cache the default role
            var defaultRole = userIdentity.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", this.DisplayName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            userIdentity.AddClaim(new Claim(ClaimTypes.Sid, this.Id.ToString()));
            userIdentity.AddClaim(new Claim("RoleId", Roles.First().RoleId.ToString()));
            userIdentity.AddClaim(new Claim("Roles", string.Join("|", Roles.Select(r => r.RoleId))));

            // ASP.NET Identity automatically creates ClaimTypes.Role for all the associated Roles. We only want to have one role at a time. Delete all except for the default.
            var roles = userIdentity.FindAll(i => i.Type == ClaimTypes.Role).Where(r => r.Value != defaultRole);
            foreach (var role in roles)
            {
                userIdentity.RemoveClaim(role);
            }
            return userIdentity;
        }

        public Nullable<short> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime? LastActivationDate { get; set; }
        public bool IsTestRecord { get; set; }
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
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
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
        }
    }
}