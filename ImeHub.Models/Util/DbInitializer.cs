using ImeHub.Data;
using System;
using System.Data.Entity.Migrations;
using Features = ImeHub.Models.Enums.Features;

namespace ImeHub.Models.Database
{
    public class DbInitializer
    {
        private IImeHubDbContext db;
        public DbInitializer(IImeHubDbContext db)
        {
            this.db = db;
        }
        public void SeedDatabase()
        {
            // ROLES

            var ROLE_SUPERADMIN = new Guid("7fab67dd-286b-492f-865a-0cb0ce1261ce");
            var role_superadmin = new Role { Id = ROLE_SUPERADMIN, Name = "Super Admin" };
            db.Roles.AddOrUpdate(role_superadmin);

            var ROLE_MANAGER = new Guid("ffb72d29-ed21-4a71-87aa-d1f7c21d7f26");
            var role_manager = new Role { Id = ROLE_MANAGER, Name = "Manager" };
            db.Roles.AddOrUpdate(role_manager);

            var ROLE_PHYSICIAN = new Guid("234df31c-69a1-4186-9815-1cf37233d448");
            var role_physician = new Role { Id = ROLE_PHYSICIAN, Name = "Physician" };
            db.Roles.AddOrUpdate(role_physician);

            var ROLE_PHYSICIAN_ADMIN = new Guid("d27a6fba-e218-4e7d-8dc8-c60fab892e85");
            var role_physician_admin = new Role { Id = ROLE_PHYSICIAN_ADMIN, Name = "Admin" };
            db.Roles.AddOrUpdate(role_physician_admin);

            var ROLE_PHYSICIAN_ASSISTANT = new Guid("2065ff56-0931-49ff-9461-e56ed022c640");
            var role_physician_assistant = new Role { Id = ROLE_PHYSICIAN_ASSISTANT, Name = "Assistant" };
            db.Roles.AddOrUpdate(role_physician_assistant);


            // FEATURES

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.UserPortal.Users.Manage), Name = "User Management" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.UserPortal.Roles.Manage), Name = "Role Management" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.UserPortal.Physicians.Manage), Name = "Physician Management" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Work.WorkSection), Name = "Work" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Work.DaySheet), Name = "DaySheet" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Work.Tasks), Name = "Tasks" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Work.Schedule), Name = "Schedule" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Work.Additionals), Name = "Additionals" });

            // SUPER ADMIN HAS ACCESS TO ALL FEATURES

            // MANAGER FEATURES

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = ROLE_MANAGER, FeatureId = new Guid(Features.UserPortal.Physicians.Manage) });

            // PHYSICIAN FEATURES
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = ROLE_PHYSICIAN, FeatureId = new Guid(Features.PhysicianPortal.Work.WorkSection) });


            // ACCEPTANCE STATUS
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Accepted, Name = "Accepted" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotResponded, Name = "Not Responded" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Rejected, Name = "Rejected" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotSent, Name = "Not Sent" });

            db.SaveChanges();
        }
    }
}
