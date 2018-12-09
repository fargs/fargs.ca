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
    public class FakeImeHubDbContext : IImeHubDbContext
    {
        public System.Data.Entity.DbSet<Address> Addresses { get; set; }
        public System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; }
        public System.Data.Entity.DbSet<City> Cities { get; set; }
        public System.Data.Entity.DbSet<Company> Companies { get; set; }
        public System.Data.Entity.DbSet<Country> Countries { get; set; }
        public System.Data.Entity.DbSet<Feature> Features { get; set; }
        public System.Data.Entity.DbSet<Physician> Physicians { get; set; }
        public System.Data.Entity.DbSet<PhysicianInviteLog> PhysicianInviteLogs { get; set; }
        public System.Data.Entity.DbSet<PhysicianOwner> PhysicianOwners { get; set; }
        public System.Data.Entity.DbSet<PhysicianOwnerAcceptanceStatu> PhysicianOwnerAcceptanceStatus { get; set; }
        public System.Data.Entity.DbSet<Province> Provinces { get; set; }
        public System.Data.Entity.DbSet<Role> Roles { get; set; }
        public System.Data.Entity.DbSet<RoleFeature> RoleFeatures { get; set; }
        public System.Data.Entity.DbSet<Service> Services { get; set; }
        public System.Data.Entity.DbSet<TeamMember> TeamMembers { get; set; }
        public System.Data.Entity.DbSet<TimeZone> TimeZones { get; set; }
        public System.Data.Entity.DbSet<TravelPrice> TravelPrices { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<UserClaim> UserClaims { get; set; }
        public System.Data.Entity.DbSet<UserLogin> UserLogins { get; set; }
        public System.Data.Entity.DbSet<UserRole> UserRoles { get; set; }
        public System.Data.Entity.DbSet<UserSetupWorkflow> UserSetupWorkflows { get; set; }
        public System.Data.Entity.DbSet<UserSetupWorkItem> UserSetupWorkItems { get; set; }
        public System.Data.Entity.DbSet<Workflow> Workflows { get; set; }
        public System.Data.Entity.DbSet<WorkItem> WorkItems { get; set; }
        public System.Data.Entity.DbSet<WorkItemRelated> WorkItemRelateds { get; set; }

        public FakeImeHubDbContext()
        {
            _changeTracker = null;
            _configuration = null;
            _database = null;

            Addresses = new FakeDbSet<Address>("Id");
            AddressTypes = new FakeDbSet<AddressType>("Id");
            Cities = new FakeDbSet<City>("Id");
            Companies = new FakeDbSet<Company>("Id");
            Countries = new FakeDbSet<Country>("Id");
            Features = new FakeDbSet<Feature>("Id");
            Physicians = new FakeDbSet<Physician>("Id");
            PhysicianInviteLogs = new FakeDbSet<PhysicianInviteLog>("Id");
            PhysicianOwners = new FakeDbSet<PhysicianOwner>("PhysicianId");
            PhysicianOwnerAcceptanceStatus = new FakeDbSet<PhysicianOwnerAcceptanceStatu>("Id");
            Provinces = new FakeDbSet<Province>("Id");
            Roles = new FakeDbSet<Role>("Id");
            RoleFeatures = new FakeDbSet<RoleFeature>("RoleId", "FeatureId");
            Services = new FakeDbSet<Service>("Id");
            TeamMembers = new FakeDbSet<TeamMember>("Id");
            TimeZones = new FakeDbSet<TimeZone>("Id");
            TravelPrices = new FakeDbSet<TravelPrice>("Id");
            Users = new FakeDbSet<User>("Id");
            UserClaims = new FakeDbSet<UserClaim>("Id");
            UserLogins = new FakeDbSet<UserLogin>("LoginProvider", "ProviderKey", "UserId");
            UserRoles = new FakeDbSet<UserRole>("UserId", "RoleId");
            UserSetupWorkflows = new FakeDbSet<UserSetupWorkflow>("Id");
            UserSetupWorkItems = new FakeDbSet<UserSetupWorkItem>("Id");
            Workflows = new FakeDbSet<Workflow>("Id");
            WorkItems = new FakeDbSet<WorkItem>("Id");
            WorkItemRelateds = new FakeDbSet<WorkItemRelated>("ParentId", "ChildId");
        }

        public int SaveChangesCount { get; private set; }
        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1);
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return System.Threading.Tasks.Task<int>.Factory.StartNew(() => 1, cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private System.Data.Entity.Infrastructure.DbChangeTracker _changeTracker;
        public System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get { return _changeTracker; } }
        private System.Data.Entity.Infrastructure.DbContextConfiguration _configuration;
        public System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get { return _configuration; } }
        private System.Data.Entity.Database _database;
        public System.Data.Entity.Database Database { get { return _database; } }
        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            throw new System.NotImplementedException();
        }
        public System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors()
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet Set(System.Type entityType)
        {
            throw new System.NotImplementedException();
        }
        public System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new System.NotImplementedException();
        }
        public override string ToString()
        {
            throw new System.NotImplementedException();
        }

    }
}
// </auto-generated>
