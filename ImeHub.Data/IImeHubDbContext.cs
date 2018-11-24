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

    public interface IImeHubDbContext : System.IDisposable
    {
        System.Data.Entity.DbSet<Address> Addresses { get; set; } // Address
        System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; } // AddressType
        System.Data.Entity.DbSet<City> Cities { get; set; } // City
        System.Data.Entity.DbSet<Company> Companies { get; set; } // Company
        System.Data.Entity.DbSet<Country> Countries { get; set; } // Country
        System.Data.Entity.DbSet<Feature> Features { get; set; } // Feature
        System.Data.Entity.DbSet<Physician> Physicians { get; set; } // Physician
        System.Data.Entity.DbSet<PhysicianInviteLog> PhysicianInviteLogs { get; set; } // PhysicianInviteLog
        System.Data.Entity.DbSet<PhysicianOwner> PhysicianOwners { get; set; } // PhysicianOwner
        System.Data.Entity.DbSet<PhysicianOwnerAcceptanceStatu> PhysicianOwnerAcceptanceStatus { get; set; } // PhysicianOwnerAcceptanceStatus
        System.Data.Entity.DbSet<Province> Provinces { get; set; } // Province
        System.Data.Entity.DbSet<Role> Roles { get; set; } // Role
        System.Data.Entity.DbSet<RoleFeature> RoleFeatures { get; set; } // RoleFeature
        System.Data.Entity.DbSet<Service> Services { get; set; } // Service
        System.Data.Entity.DbSet<TeamMember> TeamMembers { get; set; } // TeamMember
        System.Data.Entity.DbSet<TimeZone> TimeZones { get; set; } // TimeZone
        System.Data.Entity.DbSet<TravelPrice> TravelPrices { get; set; } // TravelPrice
        System.Data.Entity.DbSet<User> Users { get; set; } // User
        System.Data.Entity.DbSet<UserClaim> UserClaims { get; set; } // UserClaim
        System.Data.Entity.DbSet<UserLogin> UserLogins { get; set; } // UserLogin
        System.Data.Entity.DbSet<UserRole> UserRoles { get; set; } // UserRole
        System.Data.Entity.DbSet<Workflow> Workflows { get; set; } // Workflow
        System.Data.Entity.DbSet<WorkflowTask> WorkflowTasks { get; set; } // WorkflowTask
        System.Data.Entity.DbSet<WorkflowTaskDependent> WorkflowTaskDependents { get; set; } // WorkflowTaskDependent

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);
        System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get; }
        System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get; }
        System.Data.Entity.Database Database { get; }
        System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity);
        System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors();
        System.Data.Entity.DbSet Set(System.Type entityType);
        System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();
    }

}
// </auto-generated>
