// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace Orvosi.Data
{


    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class OrvosiDbContext : System.Data.Entity.DbContext, IOrvosiDbContext
    {
        public System.Data.Entity.DbSet<Address> Addresses { get; set; } // Address
        public System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; } // AddressType
        public System.Data.Entity.DbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        public System.Data.Entity.DbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        public System.Data.Entity.DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        public System.Data.Entity.DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        public System.Data.Entity.DbSet<AspNetUserRole> AspNetUserRoles { get; set; } // AspNetUserRoles
        public System.Data.Entity.DbSet<AvailableDay> AvailableDays { get; set; } // AvailableDay
        public System.Data.Entity.DbSet<AvailableSlot> AvailableSlots { get; set; } // AvailableSlot
        public System.Data.Entity.DbSet<Company> Companies { get; set; } // Company
        public System.Data.Entity.DbSet<Country> Countries { get; set; } // Country
        public System.Data.Entity.DbSet<Document> Documents { get; set; } // Document
        public System.Data.Entity.DbSet<DocumentTemplate> DocumentTemplates { get; set; } // DocumentTemplate
        public System.Data.Entity.DbSet<DocumentType> DocumentTypes { get; set; } // DocumentType
        public System.Data.Entity.DbSet<Invoice> Invoices { get; set; } // Invoice
        public System.Data.Entity.DbSet<InvoiceDetail> InvoiceDetails { get; set; } // InvoiceDetail
        public System.Data.Entity.DbSet<Lookup> Lookups { get; set; } // Lookup
        public System.Data.Entity.DbSet<LookupItem> LookupItems { get; set; } // LookupItem
        public System.Data.Entity.DbSet<Organization> Organizations { get; set; } // Organization
        public System.Data.Entity.DbSet<Person> People { get; set; } // Person
        public System.Data.Entity.DbSet<Physician> Physicians { get; set; } // Physician
        public System.Data.Entity.DbSet<PhysicianCompany> PhysicianCompanies { get; set; } // PhysicianCompany
        public System.Data.Entity.DbSet<PhysicianInsurance> PhysicianInsurances { get; set; } // PhysicianInsurance
        public System.Data.Entity.DbSet<PhysicianLicense> PhysicianLicenses { get; set; } // PhysicianLicense
        public System.Data.Entity.DbSet<PhysicianLocation> PhysicianLocations { get; set; } // PhysicianLocation
        public System.Data.Entity.DbSet<Price> Prices { get; set; } // Price
        public System.Data.Entity.DbSet<Province> Provinces { get; set; } // Province
        public System.Data.Entity.DbSet<RefactorLog> RefactorLogs { get; set; } // __RefactorLog
        public System.Data.Entity.DbSet<RoleCategory> RoleCategories { get; set; } // RoleCategory
        public System.Data.Entity.DbSet<Service> Services { get; set; } // Service
        public System.Data.Entity.DbSet<ServiceCatalogue> ServiceCatalogues { get; set; } // ServiceCatalogue
        public System.Data.Entity.DbSet<ServiceCatalogueRate> ServiceCatalogueRates { get; set; } // ServiceCatalogueRate
        public System.Data.Entity.DbSet<ServiceCategory> ServiceCategories { get; set; } // ServiceCategory
        public System.Data.Entity.DbSet<ServicePortfolio> ServicePortfolios { get; set; } // ServicePortfolio
        public System.Data.Entity.DbSet<ServiceRequest> ServiceRequests { get; set; } // ServiceRequest
        public System.Data.Entity.DbSet<ServiceRequestBoxCollaboration> ServiceRequestBoxCollaborations { get; set; } // ServiceRequestBoxCollaboration
        public System.Data.Entity.DbSet<ServiceRequestTask> ServiceRequestTasks { get; set; } // ServiceRequestTask
        public System.Data.Entity.DbSet<ServiceRequestTemplate> ServiceRequestTemplates { get; set; } // ServiceRequestTemplate
        public System.Data.Entity.DbSet<ServiceRequestTemplateTask> ServiceRequestTemplateTasks { get; set; } // ServiceRequestTemplateTask
        public System.Data.Entity.DbSet<Task> Tasks { get; set; } // Task
        public System.Data.Entity.DbSet<Time> Times { get; set; } // Time

        static OrvosiDbContext()
        {
            System.Data.Entity.Database.SetInitializer<OrvosiDbContext>(null);
        }

        public OrvosiDbContext()
            : base("Name=OrvosiDbContext")
        {
            InitializePartial();
        }

        public OrvosiDbContext(string connectionString)
            : base(connectionString)
        {
            InitializePartial();
        }

        public OrvosiDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
            InitializePartial();
        }

        public OrvosiDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializePartial();
        }

        public OrvosiDbContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializePartial();
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
            modelBuilder.Configurations.Add(new AspNetRoleConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserClaimConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserLoginConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserRoleConfiguration());
            modelBuilder.Configurations.Add(new AvailableDayConfiguration());
            modelBuilder.Configurations.Add(new AvailableSlotConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new DocumentConfiguration());
            modelBuilder.Configurations.Add(new DocumentTemplateConfiguration());
            modelBuilder.Configurations.Add(new DocumentTypeConfiguration());
            modelBuilder.Configurations.Add(new InvoiceConfiguration());
            modelBuilder.Configurations.Add(new InvoiceDetailConfiguration());
            modelBuilder.Configurations.Add(new LookupConfiguration());
            modelBuilder.Configurations.Add(new LookupItemConfiguration());
            modelBuilder.Configurations.Add(new OrganizationConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new PhysicianConfiguration());
            modelBuilder.Configurations.Add(new PhysicianCompanyConfiguration());
            modelBuilder.Configurations.Add(new PhysicianInsuranceConfiguration());
            modelBuilder.Configurations.Add(new PhysicianLicenseConfiguration());
            modelBuilder.Configurations.Add(new PhysicianLocationConfiguration());
            modelBuilder.Configurations.Add(new PriceConfiguration());
            modelBuilder.Configurations.Add(new ProvinceConfiguration());
            modelBuilder.Configurations.Add(new RefactorLogConfiguration());
            modelBuilder.Configurations.Add(new RoleCategoryConfiguration());
            modelBuilder.Configurations.Add(new ServiceConfiguration());
            modelBuilder.Configurations.Add(new ServiceCatalogueConfiguration());
            modelBuilder.Configurations.Add(new ServiceCatalogueRateConfiguration());
            modelBuilder.Configurations.Add(new ServiceCategoryConfiguration());
            modelBuilder.Configurations.Add(new ServicePortfolioConfiguration());
            modelBuilder.Configurations.Add(new ServiceRequestConfiguration());
            modelBuilder.Configurations.Add(new ServiceRequestBoxCollaborationConfiguration());
            modelBuilder.Configurations.Add(new ServiceRequestTaskConfiguration());
            modelBuilder.Configurations.Add(new ServiceRequestTemplateConfiguration());
            modelBuilder.Configurations.Add(new ServiceRequestTemplateTaskConfiguration());
            modelBuilder.Configurations.Add(new TaskConfiguration());
            modelBuilder.Configurations.Add(new TimeConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AddressConfiguration(schema));
            modelBuilder.Configurations.Add(new AddressTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetRoleConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserClaimConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserLoginConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserRoleConfiguration(schema));
            modelBuilder.Configurations.Add(new AvailableDayConfiguration(schema));
            modelBuilder.Configurations.Add(new AvailableSlotConfiguration(schema));
            modelBuilder.Configurations.Add(new CompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new CountryConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentTemplateConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new InvoiceConfiguration(schema));
            modelBuilder.Configurations.Add(new InvoiceDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new LookupConfiguration(schema));
            modelBuilder.Configurations.Add(new LookupItemConfiguration(schema));
            modelBuilder.Configurations.Add(new OrganizationConfiguration(schema));
            modelBuilder.Configurations.Add(new PersonConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianCompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianInsuranceConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianLicenseConfiguration(schema));
            modelBuilder.Configurations.Add(new PhysicianLocationConfiguration(schema));
            modelBuilder.Configurations.Add(new PriceConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvinceConfiguration(schema));
            modelBuilder.Configurations.Add(new RefactorLogConfiguration(schema));
            modelBuilder.Configurations.Add(new RoleCategoryConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceCatalogueConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceCatalogueRateConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceCategoryConfiguration(schema));
            modelBuilder.Configurations.Add(new ServicePortfolioConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceRequestConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceRequestBoxCollaborationConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceRequestTaskConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceRequestTemplateConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceRequestTemplateTaskConfiguration(schema));
            modelBuilder.Configurations.Add(new TaskConfiguration(schema));
            modelBuilder.Configurations.Add(new TimeConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(System.Data.Entity.DbModelBuilder modelBuilder);
    }
}
// </auto-generated>
