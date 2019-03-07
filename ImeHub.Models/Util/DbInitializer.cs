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

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.ServiceRequest.Search), Name = "Search" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.ServiceRequest.SubmitRequest), Name = "Create" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Availability.AvailabilitySection), Name = "Availability" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Team.TeamSection), Name = "Team" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Team.Create), Name = "Create" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Companies.Section), Name = "Companies" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Companies.Create), Name = "Create" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Invoices.Section), Name = "Invoices" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Invoices.Manage), Name = "Manage" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Invoices.Cancel), Name = "Cancel" });

            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Workflows.Section), Name = "Workflows" });
            db.Features.AddOrUpdate(new Feature { Id = new Guid(Features.PhysicianPortal.Workflows.Manage), Name = "Manage" });

            // SUPER ADMIN HAS ACCESS TO ALL FEATURES

            // MANAGER FEATURES

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Manager, FeatureId = new Guid(Features.UserPortal.Physicians.Manage) });

            // PHYSICIAN FEATURES
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.WorkSection) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.DaySheet) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.Tasks) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.Schedule) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Work.Additionals) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.ServiceRequest.Search) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.ServiceRequest.SubmitRequest) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Availability.AvailabilitySection) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Team.TeamSection) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Team.Create) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Companies.Section) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Companies.Create) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Invoices.Section) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Invoices.Manage) });

            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Workflows.Section) });
            db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = Enums.Role.Physician, FeatureId = new Guid(Features.PhysicianPortal.Workflows.Manage) });

            // Service Request Status
            db.ServiceRequestStatus.AddOrUpdate(new ServiceRequestStatu { Id = (byte)Enums.ServiceRequestStatus.Active, Name = "Active" });
            db.ServiceRequestStatus.AddOrUpdate(new ServiceRequestStatu { Id = (byte)Enums.ServiceRequestStatus.Closed, Name = "Closed" });
            db.ServiceRequestStatus.AddOrUpdate(new ServiceRequestStatu { Id = (byte)Enums.ServiceRequestStatus.OnHold, Name = "On Hold" });

            // ACCEPTANCE STATUS
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Accepted, Name = "Accepted" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotResponded, Name = "Not Responded" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.Rejected, Name = "Rejected" });
            db.PhysicianOwnerAcceptanceStatus.AddOrUpdate(new PhysicianOwnerAcceptanceStatu { Id = (byte)Enums.AcceptanceStatus.NotSent, Name = "Not Sent" });

            // Invite Status
            db.InviteStatus.AddOrUpdate(new InviteStatu { Id = (byte)Enums.InviteStatus.Accepted, Name = "Accepted" });
            db.InviteStatus.AddOrUpdate(new InviteStatu { Id = (byte)Enums.InviteStatus.NotResponded, Name = "Not Responded" });
            db.InviteStatus.AddOrUpdate(new InviteStatu { Id = (byte)Enums.InviteStatus.Rejected, Name = "Rejected" });
            db.InviteStatus.AddOrUpdate(new InviteStatu { Id = (byte)Enums.InviteStatus.NotSent, Name = "Not Sent" });

            // Cancellation Status
            db.CancellationStatus.AddOrUpdate(new CancellationStatu { Id = (byte)Enums.CancellationStatus.Cancellation, Name = "Cancellation" });
            db.CancellationStatus.AddOrUpdate(new CancellationStatu { Id = (byte)Enums.CancellationStatus.LateCancellation, Name = "Late Cancellation" });
            db.CancellationStatus.AddOrUpdate(new CancellationStatu { Id = (byte)Enums.CancellationStatus.NoShow, Name = "No Show" });
            db.CancellationStatus.AddOrUpdate(new CancellationStatu { Id = (byte)Enums.CancellationStatus.NotCancelled, Name = "Not Cancelled" });

            // Address Types
            db.AddressTypes.AddOrUpdate(new AddressType { Id = (byte)Enums.AddressType.CompanyAssessmentOffice, Name = "Company Assessment Office" });
            db.AddressTypes.AddOrUpdate(new AddressType { Id = (byte)Enums.AddressType.PhysicianClinic, Name = "Physician Clinic" });
            db.AddressTypes.AddOrUpdate(new AddressType { Id = (byte)Enums.AddressType.PrimaryOffice, Name = "Primary Office" });
            db.AddressTypes.AddOrUpdate(new AddressType { Id = (byte)Enums.AddressType.Billing, Name = "Billing" });

            // Countries
            db.Countries.AddOrUpdate(new Country { Id = 1, Name = "Canada", Iso3DigitCountry = 124, Iso2CountryCode = "CA", Iso3CountryCode = "CAN" });

            // Provinces
            db.Provinces.AddOrUpdate(new Province { Id = 1, ProvinceName = "Alberta", ProvinceCode = "AB", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 2, ProvinceName = "British Columbia", ProvinceCode = "BC", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 3, ProvinceName = "Manitoba", ProvinceCode = "MB", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 4, ProvinceName = "New Brunswick", ProvinceCode = "NB", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 5, ProvinceName = "Newfoundland and Labrador", ProvinceCode = "NL", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 6, ProvinceName = "Northwest Territories", ProvinceCode = "NT", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 7, ProvinceName = "Nova Scotia", ProvinceCode = "NS", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 8, ProvinceName = "Nunavut", ProvinceCode = "NU", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 9, ProvinceName = "Ontario", ProvinceCode = "ON", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 10, ProvinceName = "Prince Edward Island", ProvinceCode = "PE", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 11, ProvinceName = "Québec", ProvinceCode = "QB", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 12, ProvinceName = "Saskatchewan", ProvinceCode = "SK", CountryId = 1 });
            db.Provinces.AddOrUpdate(new Province { Id = 13, ProvinceName = "Yukon", ProvinceCode = "YT", CountryId = 1 });

            // Time Zones
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 1, Name = "Eastern Standard Time", Iana = "America/Toronto", Iso = "Canada/Eastern" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 2, Name = "Pacific Standard Time", Iana = "America/Vancouver", Iso = "Canada/Pacific" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 3, Name = "Atlantic Standard Time", Iana = "America/Halifax", Iso = "Canada/Atlantic" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 4, Name = "Central Standard Time", Iana = "America/Winnipeg", Iso = "Canada/Central" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 5, Name = "Central Standard Time", Iana = "America/Regina", Iso = "Canada/East-Saskatchewan" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 6, Name = "Mountain Standard Time", Iana = "America/Edmonton", Iso = "Canada/Mountain" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 7, Name = "Newfoundland and Labrador Standard Time", Iana = "America/St_Johns", Iso = "Canada/Newfoundland" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 8, Name = "Central Standard Time", Iana = "America/Regina", Iso = "Canada/Saskatchewan" });
            db.TimeZones.AddOrUpdate(new ImeHub.Data.TimeZone { Id = 9, Name = "Pacific Standard Time", Iana = "America/Whitehorse", Iso = "Canada/Yukon" });

        }
    }
}
