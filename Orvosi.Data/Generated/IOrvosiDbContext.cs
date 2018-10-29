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

    public interface IOrvosiDbContext : System.IDisposable
    {
        System.Data.Entity.DbSet<Account> Accounts { get; set; } // Account
        System.Data.Entity.DbSet<Address> Addresses { get; set; } // Address
        System.Data.Entity.DbSet<AddressType> AddressTypes { get; set; } // AddressType
        System.Data.Entity.DbSet<AddressV2> AddressV2 { get; set; } // AddressV2
        System.Data.Entity.DbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        System.Data.Entity.DbSet<AspNetRolesFeature> AspNetRolesFeatures { get; set; } // AspNetRolesFeature
        System.Data.Entity.DbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        System.Data.Entity.DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        System.Data.Entity.DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        System.Data.Entity.DbSet<AspNetUserRole> AspNetUserRoles { get; set; } // AspNetUserRoles
        System.Data.Entity.DbSet<AspNetUserView> AspNetUserViews { get; set; } // User
        System.Data.Entity.DbSet<AvailableDay> AvailableDays { get; set; } // AvailableDay
        System.Data.Entity.DbSet<AvailableDayResource> AvailableDayResources { get; set; } // AvailableDayResource
        System.Data.Entity.DbSet<AvailableSlot> AvailableSlots { get; set; } // AvailableSlot
        System.Data.Entity.DbSet<AvailableSlotView> AvailableSlotViews { get; set; } // AvailableSlot
        System.Data.Entity.DbSet<BillableEntity> BillableEntities { get; set; } // BillableEntity
        System.Data.Entity.DbSet<City> Cities { get; set; } // City
        System.Data.Entity.DbSet<Collaborator> Collaborators { get; set; } // Collaborator
        System.Data.Entity.DbSet<CommentType> CommentTypes { get; set; } // CommentType
        System.Data.Entity.DbSet<Company> Companies { get; set; } // Company
        System.Data.Entity.DbSet<CompanyService> CompanyServices { get; set; } // CompanyService
        System.Data.Entity.DbSet<CompanyV2> CompanyV2 { get; set; } // CompanyV2
        System.Data.Entity.DbSet<Country> Countries { get; set; } // Country
        System.Data.Entity.DbSet<Document> Documents { get; set; } // Document
        System.Data.Entity.DbSet<DocumentTemplate> DocumentTemplates { get; set; } // DocumentTemplate
        System.Data.Entity.DbSet<DocumentType> DocumentTypes { get; set; } // DocumentType
        System.Data.Entity.DbSet<Feature> Features { get; set; } // Feature
        System.Data.Entity.DbSet<Invoice> Invoices { get; set; } // Invoice
        System.Data.Entity.DbSet<InvoiceDetail> InvoiceDetails { get; set; } // InvoiceDetail
        System.Data.Entity.DbSet<InvoiceSentLog> InvoiceSentLogs { get; set; } // InvoiceSentLog
        System.Data.Entity.DbSet<LocationArea> LocationAreas { get; set; } // LocationArea
        System.Data.Entity.DbSet<LocationView> LocationViews { get; set; } // Location
        System.Data.Entity.DbSet<Lookup> Lookups { get; set; } // Lookup
        System.Data.Entity.DbSet<LookupItem> LookupItems { get; set; } // LookupItem
        System.Data.Entity.DbSet<MedicolegalType> MedicolegalTypes { get; set; } // MedicolegalType
        System.Data.Entity.DbSet<Organization> Organizations { get; set; } // Organization
        System.Data.Entity.DbSet<OTask> OTasks { get; set; } // Task
        System.Data.Entity.DbSet<Person> People { get; set; } // Person
        System.Data.Entity.DbSet<Physician> Physicians { get; set; } // Physician
        System.Data.Entity.DbSet<PhysicianCompany> PhysicianCompanies { get; set; } // PhysicianCompany
        System.Data.Entity.DbSet<PhysicianCompanyStatu> PhysicianCompanyStatus { get; set; } // PhysicianCompanyStatus
        System.Data.Entity.DbSet<PhysicianCompanyView> PhysicianCompanyViews { get; set; } // PhysicianCompany
        System.Data.Entity.DbSet<PhysicianInsurance> PhysicianInsurances { get; set; } // PhysicianInsurance
        System.Data.Entity.DbSet<PhysicianLicense> PhysicianLicenses { get; set; } // PhysicianLicense
        System.Data.Entity.DbSet<PhysicianLocation> PhysicianLocations { get; set; } // PhysicianLocation
        System.Data.Entity.DbSet<PhysicianLocationArea> PhysicianLocationAreas { get; set; } // PhysicianLocationArea
        System.Data.Entity.DbSet<PhysicianService> PhysicianServices { get; set; } // PhysicianService
        System.Data.Entity.DbSet<PhysicianServiceRequestTemplate> PhysicianServiceRequestTemplates { get; set; } // Physician_ServiceRequestTemplate
        System.Data.Entity.DbSet<PhysicianSpeciality> PhysicianSpecialities { get; set; } // PhysicianSpeciality
        System.Data.Entity.DbSet<Price> Prices { get; set; } // Price
        System.Data.Entity.DbSet<Profile> Profiles { get; set; } // Profile
        System.Data.Entity.DbSet<Province> Provinces { get; set; } // Province
        System.Data.Entity.DbSet<Receipt> Receipts { get; set; } // Receipt
        System.Data.Entity.DbSet<RefactorLog> RefactorLogs { get; set; } // __RefactorLog
        System.Data.Entity.DbSet<RoleCategory> RoleCategories { get; set; } // RoleCategory
        System.Data.Entity.DbSet<Service> Services { get; set; } // Service
        System.Data.Entity.DbSet<ServiceCatalogue> ServiceCatalogues { get; set; } // ServiceCatalogue
        System.Data.Entity.DbSet<ServiceCatalogueRate> ServiceCatalogueRates { get; set; } // ServiceCatalogueRate
        System.Data.Entity.DbSet<ServiceCategory> ServiceCategories { get; set; } // ServiceCategory
        System.Data.Entity.DbSet<ServicePortfolio> ServicePortfolios { get; set; } // ServicePortfolio
        System.Data.Entity.DbSet<ServiceRequest> ServiceRequests { get; set; } // ServiceRequest
        System.Data.Entity.DbSet<ServiceRequestBoxCollaboration> ServiceRequestBoxCollaborations { get; set; } // ServiceRequestBoxCollaboration
        System.Data.Entity.DbSet<ServiceRequestComment> ServiceRequestComments { get; set; } // ServiceRequestComment
        System.Data.Entity.DbSet<ServiceRequestCommentAccess> ServiceRequestCommentAccesses { get; set; } // ServiceRequestCommentAccess
        System.Data.Entity.DbSet<ServiceRequestMessage> ServiceRequestMessages { get; set; } // ServiceRequestMessage
        System.Data.Entity.DbSet<ServiceRequestResource> ServiceRequestResources { get; set; } // ServiceRequestResource
        System.Data.Entity.DbSet<ServiceRequestStatu> ServiceRequestStatus { get; set; } // ServiceRequestStatus
        System.Data.Entity.DbSet<ServiceRequestTask> ServiceRequestTasks { get; set; } // ServiceRequestTask
        System.Data.Entity.DbSet<ServiceRequestTemplate> ServiceRequestTemplates { get; set; } // ServiceRequestTemplate
        System.Data.Entity.DbSet<ServiceRequestTemplateTask> ServiceRequestTemplateTasks { get; set; } // ServiceRequestTemplateTask
        System.Data.Entity.DbSet<ServiceRequestView> ServiceRequestViews { get; set; } // ServiceRequest
        System.Data.Entity.DbSet<ServiceV2> ServiceV2 { get; set; } // ServiceV2
        System.Data.Entity.DbSet<TaskPhase> TaskPhases { get; set; } // TaskPhase
        System.Data.Entity.DbSet<TaskStatu> TaskStatus { get; set; } // TaskStatus
        System.Data.Entity.DbSet<TeamMember> TeamMembers { get; set; } // TeamMember
        System.Data.Entity.DbSet<Teleconference> Teleconferences { get; set; } // Teleconference
        System.Data.Entity.DbSet<TeleconferenceResult> TeleconferenceResults { get; set; } // TeleconferenceResult
        System.Data.Entity.DbSet<Time> Times { get; set; } // Time
        System.Data.Entity.DbSet<TimeZone> TimeZones { get; set; } // TimeZone
        System.Data.Entity.DbSet<TravelPrice> TravelPrices { get; set; } // TravelPrice
        System.Data.Entity.DbSet<UserInbox> UserInboxes { get; set; } // UserInbox

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);
        
        // Stored Procedures
        System.Collections.Generic.List<GetAssignedServiceRequestsReturnModel> GetAssignedServiceRequests(System.Guid? assignedTo, System.DateTime? now, bool? showClosed, int? serviceRequestId);
        System.Collections.Generic.List<GetAssignedServiceRequestsReturnModel> GetAssignedServiceRequests(System.Guid? assignedTo, System.DateTime? now, bool? showClosed, int? serviceRequestId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetAssignedServiceRequestsReturnModel>> GetAssignedServiceRequestsAsync(System.Guid? assignedTo, System.DateTime? now, bool? showClosed, int? serviceRequestId);

        System.Collections.Generic.List<GetBoxTokensReturnModel> GetBoxTokens(System.Guid? userId);
        System.Collections.Generic.List<GetBoxTokensReturnModel> GetBoxTokens(System.Guid? userId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetBoxTokensReturnModel>> GetBoxTokensAsync(System.Guid? userId);

        System.Collections.Generic.List<GetCompanyProvinceReturnModel> GetCompanyProvince(int? companyId);
        System.Collections.Generic.List<GetCompanyProvinceReturnModel> GetCompanyProvince(int? companyId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetCompanyProvinceReturnModel>> GetCompanyProvinceAsync(int? companyId);

        System.Collections.Generic.List<GetNextInvoiceNumberReturnModel> GetNextInvoiceNumber();
        System.Collections.Generic.List<GetNextInvoiceNumberReturnModel> GetNextInvoiceNumber(out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetNextInvoiceNumberReturnModel>> GetNextInvoiceNumberAsync();

        System.Collections.Generic.List<GetServiceCatalogueReturnModel> GetServiceCatalogue(System.Guid? physicianId);
        System.Collections.Generic.List<GetServiceCatalogueReturnModel> GetServiceCatalogue(System.Guid? physicianId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetServiceCatalogueReturnModel>> GetServiceCatalogueAsync(System.Guid? physicianId);

        System.Collections.Generic.List<GetServiceCatalogueReturnModel> GetServiceCatalogueForCompany(System.Guid? physicianId, short? companyId);
        System.Collections.Generic.List<GetServiceCatalogueReturnModel> GetServiceCatalogueForCompany(System.Guid? physicianId, short? companyId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetServiceCatalogueReturnModel>> GetServiceCatalogueForCompanyAsync(System.Guid? physicianId, short? companyId);

        int GetServiceCatalogueMatrix(string physicianId, short? companyId);
        // GetServiceCatalogueMatrixAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

        System.Collections.Generic.List<GetServiceCatalogueRateReturnModel> GetServiceCatalogueRate(System.Guid? serviceProviderGuid, System.Guid? customerGuid);
        System.Collections.Generic.List<GetServiceCatalogueRateReturnModel> GetServiceCatalogueRate(System.Guid? serviceProviderGuid, System.Guid? customerGuid, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetServiceCatalogueRateReturnModel>> GetServiceCatalogueRateAsync(System.Guid? serviceProviderGuid, System.Guid? customerGuid);

        System.Collections.Generic.List<GetServiceRequestReturnModel> GetServiceRequest(int? serviceRequestId, System.DateTime? now);
        System.Collections.Generic.List<GetServiceRequestReturnModel> GetServiceRequest(int? serviceRequestId, System.DateTime? now, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetServiceRequestReturnModel>> GetServiceRequestAsync(int? serviceRequestId, System.DateTime? now);

        System.Collections.Generic.List<GetServiceRequestResourcesReturnModel> GetServiceRequestResources(int? serviceRequestId);
        System.Collections.Generic.List<GetServiceRequestResourcesReturnModel> GetServiceRequestResources(int? serviceRequestId, out int procResult);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GetServiceRequestResourcesReturnModel>> GetServiceRequestResourcesAsync(int? serviceRequestId);

        int GetServiceRequestTasks(System.DateTime? now, string serviceRequestIds);
        // GetServiceRequestTasksAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

        int SaveBoxTokens(string accessToken, string refreshToken, System.Guid? userId);
        // SaveBoxTokensAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

        int ToggleCancellation(int? id, System.DateTime? cancelledDate, bool? isLateCancellation, string notes);
        // ToggleCancellationAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

        int ToggleNoShow(int? id);
        // ToggleNoShowAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

    }

}
// </auto-generated>
