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
using Orvosi.Data;
using WebApp.Library.Extensions;

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

            var roleId = Roles.First().RoleId;
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", this.DisplayName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            userIdentity.AddClaim(new Claim("Initials", this.Initials));
            userIdentity.AddClaim(new Claim(ClaimTypes.Sid, this.Id.ToString()));
            userIdentity.AddClaim(new Claim("RoleId", roleId.ToString()));
            userIdentity.AddClaim(new Claim("Roles", string.Join("|", Roles.Select(r => r.RoleId))));
            userIdentity.AddClaim(new Claim("IsAppTester", this.IsAppTester.ToString()));

            // ASP.NET Identity automatically creates ClaimTypes.Role for all the associated Roles. We only want to have one role at a time. Delete all except for the default.
            var roles = userIdentity.FindAll(i => i.Type == ClaimTypes.Role).Where(r => r.Value != defaultRole);
            foreach (var role in roles)
            {
                userIdentity.RemoveClaim(role);
            }

            using (var db = new OrvosiDbContext())
            {
                if (roleId == AspNetRoles.SuperAdmin) // Features list is used to hide/show elements in the views so the entire list is needed.
                    Features = db.Features.Select(srf => srf.Id).ToArray();
                else
                    Features = db.AspNetRolesFeatures.Where(srf => srf.AspNetRolesId == roleId).Select(srf => srf.FeatureId).ToArray();

                userIdentity.GetClaimsIdentity().AddClaim(new Claim("Features", Features.ToJson()));


                IQueryable<AspNetUser> userSelectListQuery;
                // set the physicians to the cookie
                if (this.IsAppTester)
                {
                    userSelectListQuery = db.AspNetUsers
                        .Where(u => u.AspNetUserRoles.Any(r => r.RoleId == AspNetRoles.Physician));
                }
                else
                {
                    userSelectListQuery = db.Collaborators
                        .Join(db.AspNetUsers,
                            c => c.UserId,
                            u => u.Id,
                            (c, u) => new { c.User, CollaboratorUserId = c.CollaboratorUserId })
                        .Where(u => u.CollaboratorUserId == this.Id)
                        .Select(u => u.User);
                }


                userIdentity.GetClaimsIdentity().AddClaim(new Claim("Physicians", userSelectListQuery
                    .Select(u => u.Id)
                    .ToArray().ToJson()));

            }

            return userIdentity;
        }
        

        public Nullable<short> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }
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
        public short[] Features { get; set; }
        public Guid[] Physicians { get; set; }
        public bool IsAppTester { get; set; }
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