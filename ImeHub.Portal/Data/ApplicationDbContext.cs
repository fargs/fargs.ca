using ImeHub.Portal.Data.Invoices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ImeHub.Portal.Data.Company;

namespace ImeHub.Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CompanyAccess> CompanyAccesses { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDownloadLink> InvoiceDownloadLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var identitySchema = "Identity";
            modelBuilder.Entity<ApplicationUser>().ToTable("User", identitySchema);
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole", identitySchema);
            modelBuilder.Entity<ApplicationRole>().ToTable("Role", identitySchema);
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin", identitySchema);
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim", identitySchema);
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("RoleClaim", identitySchema);
            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserToken", identitySchema);

            modelBuilder.Entity<CompanyAccess>().ToTable(nameof(CompanyAccess));
            modelBuilder.Entity<Invoice>().ToTable(nameof(Invoice));
            modelBuilder.Entity<InvoiceDownloadLink>().ToTable(nameof(InvoiceDownloadLink));
        }
    }
}
