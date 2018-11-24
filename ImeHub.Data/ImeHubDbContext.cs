// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.7
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace ImeHub.Data
{

    using System.Linq;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.2.0")]
    public class ImeHubDbContext : System.Data.Entity.DbContext, IImeHubDbContext
    {
        public System.Data.Entity.DbSet<Address> Addresses { get; set; } // Address
        public System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; } // AddressType
        public System.Data.Entity.DbSet<City> Cities { get; set; } // City
        public System.Data.Entity.DbSet<Company> Companies { get; set; } // Company
        public System.Data.Entity.DbSet<Country> Countries { get; set; } // Country
        public System.Data.Entity.DbSet<Feature> Features { get; set; } // Feature
        public System.Data.Entity.DbSet<Physician> Physicians { get; set; } // Physician
        public System.Data.Entity.DbSet<PhysicianInviteLog> PhysicianInviteLogs { get; set; } // PhysicianInviteLog
        public System.Data.Entity.DbSet<PhysicianOwner> PhysicianOwners { get; set; } // PhysicianOwner
        public System.Data.Entity.DbSet<PhysicianOwnerAcceptanceStatu> PhysicianOwnerAcceptanceStatus { get; set; } // PhysicianOwnerAcceptanceStatus
        public System.Data.Entity.DbSet<Province> Provinces { get; set; } // Province
        public System.Data.Entity.DbSet<Role> Roles { get; set; } // Role
        public System.Data.Entity.DbSet<RoleFeature> RoleFeatures { get; set; } // RoleFeature
        public System.Data.Entity.DbSet<Service> Services { get; set; } // Service
        public System.Data.Entity.DbSet<TeamMember> TeamMembers { get; set; } // TeamMember
        public System.Data.Entity.DbSet<TimeZone> TimeZones { get; set; } // TimeZone
        public System.Data.Entity.DbSet<TravelPrice> TravelPrices { get; set; } // TravelPrice
        public System.Data.Entity.DbSet<User> Users { get; set; } // User
        public System.Data.Entity.DbSet<UserClaim> UserClaims { get; set; } // UserClaim
        public System.Data.Entity.DbSet<UserLogin> UserLogins { get; set; } // UserLogin
        public System.Data.Entity.DbSet<UserRole> UserRoles { get; set; } // UserRole
        public System.Data.Entity.DbSet<Workflow> Workflows { get; set; } // Workflow
        public System.Data.Entity.DbSet<WorkflowTask> WorkflowTasks { get; set; } // WorkflowTask
        public System.Data.Entity.DbSet<WorkflowTaskDependent> WorkflowTaskDependents { get; set; } // WorkflowTaskDependent

        static ImeHubDbContext()
        {
            System.Data.Entity.Database.SetInitializer<ImeHubDbContext>(null);
        }

        public ImeHubDbContext()
            : base("Name=ImeHubDbContext")
        {
        }

        public ImeHubDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public ImeHubDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        public ImeHubDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public ImeHubDbContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as System.Data.SqlTypes.INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new AddressTypeConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new FeatureConfiguration());
            modelBuilder.Configurations.Add(new PhysicianConfiguration());
            modelBuilder.Configurations.Add(new PhysicianInviteLogConfiguration());
            modelBuilder.Configurations.Add(new PhysicianOwnerConfiguration());
            modelBuilder.Configurations.Add(new PhysicianOwnerAcceptanceStatuConfiguration());
            modelBuilder.Configurations.Add(new ProvinceConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new RoleFeatureConfiguration());
            modelBuilder.Configurations.Add(new ServiceConfiguration());
            modelBuilder.Configurations.Add(new TeamMemberConfiguration());
            modelBuilder.Configurations.Add(new TimeZoneConfiguration());
            modelBuilder.Configurations.Add(new TravelPriceConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new WorkflowConfiguration());
            modelBuilder.Configurations.Add(new WorkflowTaskConfiguration());
            modelBuilder.Configurations.Add(new WorkflowTaskDependentConfiguration());
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AddressConfiguration(schema));
            modelBuilder.Configurations.Add(new AddressTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new CityConfiguration(schema));
            modelBuilder.Configurations.Add(new CompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new CountryConfiguration(schema));
            modelBuilder.Configurations.Add(new FeatureConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianInviteLogConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianOwnerConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianOwnerAcceptanceStatuConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvinceConfiguration(schema));
            modelBuilder.Configurations.Add(new RoleConfiguration(schema));
            modelBuilder.Configurations.Add(new RoleFeatureConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceConfiguration(schema));
            modelBuilder.Configurations.Add(new TeamMemberConfiguration(schema));
            modelBuilder.Configurations.Add(new TimeZoneConfiguration(schema));
            modelBuilder.Configurations.Add(new TravelPriceConfiguration(schema));
            modelBuilder.Configurations.Add(new UserConfiguration(schema));
            modelBuilder.Configurations.Add(new UserClaimConfiguration(schema));
            modelBuilder.Configurations.Add(new UserLoginConfiguration(schema));
            modelBuilder.Configurations.Add(new UserRoleConfiguration(schema));
            modelBuilder.Configurations.Add(new WorkflowConfiguration(schema));
            modelBuilder.Configurations.Add(new WorkflowTaskConfiguration(schema));
            modelBuilder.Configurations.Add(new WorkflowTaskDependentConfiguration(schema));
            return modelBuilder;
        }
    }
}
// </auto-generated>
