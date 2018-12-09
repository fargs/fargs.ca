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

            var role_superadmin = new Role { Id = Enums.Role.SuperAdmin, Name = Enums.Role.SuperAdminName };
            db.Roles.AddOrUpdate(role_superadmin);

            var role_manager = new Role { Id = Enums.Role.Manager, Name = Enums.Role.ManagerName };
            db.Roles.AddOrUpdate(role_manager);

            var role_physician = new Role { Id = Enums.Role.Physician, Name = Enums.Role.PhysicianName };
            db.Roles.AddOrUpdate(role_physician);

            var role_physician_admin = new Role { Id = Enums.Role.CaseCoordinator, Name = Enums.Role.CaseCoordinatorName };
            db.Roles.AddOrUpdate(role_physician_admin);

            // FEATURES

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.UserPortal.Features.Manage), Name = "Feature Management" });
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

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Manager, FeatureId = new Guid(Features.UserPortal.Physicians.Manage) });

            // PHYSICIAN FEATURES
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.WorkSection) });


            // ACCEPTANCE STATUS
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Accepted, Name = "Accepted" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotResponded, Name = "Not Responded" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Rejected, Name = "Rejected" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotSent, Name = "Not Sent" });
        }
    }
}
