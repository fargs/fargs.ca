using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", this.DisplayName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            userIdentity.AddClaim(new Claim(ClaimTypes.Sid, this.Id));

            return userIdentity;
        }

        public Nullable<short> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Nullable<DateTime> LastActivationDate { get; set; }
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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
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

        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCatalogue> ServiceCatalogue { get; set; }
        public DbSet<SpecialRequest> SpecialRequests { get; set; }
    }
}