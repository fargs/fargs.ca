using Fargs.Portal.Data.Invoices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fargs.Portal.Data.Companies;

namespace Fargs.Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyRole> CompanyRoles { get; set; }
        public DbSet<CompanyAccess> CompanyAccesses { get; set; }
        public DbSet<CompanyUserInvitation> CompanyUserInvitations { get; set; }
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

            modelBuilder.Entity<Company>().ToTable(nameof(Company));
            modelBuilder.Entity<CompanyRole>().ToTable(nameof(CompanyRole));
            modelBuilder.Entity<CompanyAccess>().ToTable(nameof(CompanyAccess));
            modelBuilder.Entity<CompanyUserInvitation>().ToTable(nameof(CompanyUserInvitation));
            modelBuilder.Entity<Invoice>(builder =>
            {
                builder.ToTable(nameof(Invoice));
                builder.Property(i => i.Discount).HasPrecision(10, 2);
                builder.Property(i => i.TaxRateHst).HasPrecision(10, 2);
                builder.Property(i => i.Hst).HasPrecision(10, 2);
                builder.Property(i => i.SubTotal).HasPrecision(10, 2);
                builder.Property(i => i.Total).HasPrecision(10, 2);
            });
            modelBuilder.Entity<InvoiceDetail>(builder =>
            {
                builder.ToTable(nameof(InvoiceDetail));
                builder.Property(i => i.Discount).HasPrecision(10, 2);
                builder.Property(i => i.Amount).HasPrecision(10, 2);
                builder.Property(i => i.Rate).HasPrecision(10, 2);
                builder.Property(i => i.Total).HasPrecision(10, 2);
            });
            modelBuilder.Entity<InvoiceDownloadLink>().ToTable(nameof(InvoiceDownloadLink));
        }
    }
}
