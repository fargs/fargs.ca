﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OrvosiEntities : DbContext
    {
        public OrvosiEntities()
            : base("name=OrvosiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CompanyAssessorPackage> CompanyAssessorPackages { get; set; }
        public virtual DbSet<PhysicianAssessorPackage> PhysicianAssessorPackages { get; set; }
        public virtual DbSet<PhysicianDocument> PhysicianDocuments { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<PhysicianCompany> PhysicianCompanies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<SpecialRequest> SpecialRequests { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<ServicePortfolio> ServicePortfolios { get; set; }
        public virtual DbSet<ServiceCatalogue> ServiceCatalogues { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LookupItem> LookupItems { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
    
        [DbFunction("OrvosiEntities", "fn_Weekdays")]
        public virtual IQueryable<fn_Weekdays_Result> fn_Weekdays(Nullable<System.DateTime> startDate)
        {
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fn_Weekdays_Result>("[OrvosiEntities].[fn_Weekdays](@StartDate)", startDateParameter);
        }
    
        [DbFunction("OrvosiEntities", "fn_Timeframe")]
        public virtual IQueryable<fn_Timeframe_Result> fn_Timeframe(string datePart, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate)
        {
            var datePartParameter = datePart != null ?
                new ObjectParameter("DatePart", datePart) :
                new ObjectParameter("DatePart", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fn_Timeframe_Result>("[OrvosiEntities].[fn_Timeframe](@DatePart, @StartDate, @EndDate)", datePartParameter, startDateParameter, endDateParameter);
        }
    
        public virtual ObjectResult<GetPhysicianGoogleAccount_Result> GetPhysicianGoogleAccount(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPhysicianGoogleAccount_Result>("GetPhysicianGoogleAccount", idParameter);
        }
    
        public virtual int SpecialRequest_Insert(string physicianId, Nullable<int> serviceId, string timeframe, string additionalNotes, string modifiedUserName, string modifiedUserId)
        {
            var physicianIdParameter = physicianId != null ?
                new ObjectParameter("PhysicianId", physicianId) :
                new ObjectParameter("PhysicianId", typeof(string));
    
            var serviceIdParameter = serviceId.HasValue ?
                new ObjectParameter("ServiceId", serviceId) :
                new ObjectParameter("ServiceId", typeof(int));
    
            var timeframeParameter = timeframe != null ?
                new ObjectParameter("Timeframe", timeframe) :
                new ObjectParameter("Timeframe", typeof(string));
    
            var additionalNotesParameter = additionalNotes != null ?
                new ObjectParameter("AdditionalNotes", additionalNotes) :
                new ObjectParameter("AdditionalNotes", typeof(string));
    
            var modifiedUserNameParameter = modifiedUserName != null ?
                new ObjectParameter("ModifiedUserName", modifiedUserName) :
                new ObjectParameter("ModifiedUserName", typeof(string));
    
            var modifiedUserIdParameter = modifiedUserId != null ?
                new ObjectParameter("ModifiedUserId", modifiedUserId) :
                new ObjectParameter("ModifiedUserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpecialRequest_Insert", physicianIdParameter, serviceIdParameter, timeframeParameter, additionalNotesParameter, modifiedUserNameParameter, modifiedUserIdParameter);
        }
    
        public virtual int SpecialRequest_Update(Nullable<short> id, string physicianId, Nullable<int> serviceId, string timeframe, string additionalNotes, Nullable<System.DateTime> modifiedDate, string modifiedUserName, string modifiedUserId)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(short));
    
            var physicianIdParameter = physicianId != null ?
                new ObjectParameter("PhysicianId", physicianId) :
                new ObjectParameter("PhysicianId", typeof(string));
    
            var serviceIdParameter = serviceId.HasValue ?
                new ObjectParameter("ServiceId", serviceId) :
                new ObjectParameter("ServiceId", typeof(int));
    
            var timeframeParameter = timeframe != null ?
                new ObjectParameter("Timeframe", timeframe) :
                new ObjectParameter("Timeframe", typeof(string));
    
            var additionalNotesParameter = additionalNotes != null ?
                new ObjectParameter("AdditionalNotes", additionalNotes) :
                new ObjectParameter("AdditionalNotes", typeof(string));
    
            var modifiedDateParameter = modifiedDate.HasValue ?
                new ObjectParameter("ModifiedDate", modifiedDate) :
                new ObjectParameter("ModifiedDate", typeof(System.DateTime));
    
            var modifiedUserNameParameter = modifiedUserName != null ?
                new ObjectParameter("ModifiedUserName", modifiedUserName) :
                new ObjectParameter("ModifiedUserName", typeof(string));
    
            var modifiedUserIdParameter = modifiedUserId != null ?
                new ObjectParameter("ModifiedUserId", modifiedUserId) :
                new ObjectParameter("ModifiedUserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpecialRequest_Update", idParameter, physicianIdParameter, serviceIdParameter, timeframeParameter, additionalNotesParameter, modifiedDateParameter, modifiedUserNameParameter, modifiedUserIdParameter);
        }
    
        public virtual int AspNetUsers_Update(string id, string email, Nullable<bool> emailConfirmed, string phoneNumber, Nullable<bool> phoneNumberConfirmed, Nullable<bool> twoFactorEnabled, Nullable<System.DateTime> lockoutEndDateUtc, Nullable<bool> lockoutEnabled, Nullable<int> accessFailedCount, string userName, string title, string firstName, string lastName, string employeeId, Nullable<short> companyId, string companyName, string modifiedUser, Nullable<System.DateTime> lastActivationDate, Nullable<bool> isTestRecord)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var emailConfirmedParameter = emailConfirmed.HasValue ?
                new ObjectParameter("EmailConfirmed", emailConfirmed) :
                new ObjectParameter("EmailConfirmed", typeof(bool));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            var phoneNumberConfirmedParameter = phoneNumberConfirmed.HasValue ?
                new ObjectParameter("PhoneNumberConfirmed", phoneNumberConfirmed) :
                new ObjectParameter("PhoneNumberConfirmed", typeof(bool));
    
            var twoFactorEnabledParameter = twoFactorEnabled.HasValue ?
                new ObjectParameter("TwoFactorEnabled", twoFactorEnabled) :
                new ObjectParameter("TwoFactorEnabled", typeof(bool));
    
            var lockoutEndDateUtcParameter = lockoutEndDateUtc.HasValue ?
                new ObjectParameter("LockoutEndDateUtc", lockoutEndDateUtc) :
                new ObjectParameter("LockoutEndDateUtc", typeof(System.DateTime));
    
            var lockoutEnabledParameter = lockoutEnabled.HasValue ?
                new ObjectParameter("LockoutEnabled", lockoutEnabled) :
                new ObjectParameter("LockoutEnabled", typeof(bool));
    
            var accessFailedCountParameter = accessFailedCount.HasValue ?
                new ObjectParameter("AccessFailedCount", accessFailedCount) :
                new ObjectParameter("AccessFailedCount", typeof(int));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(string));
    
            var companyIdParameter = companyId.HasValue ?
                new ObjectParameter("CompanyId", companyId) :
                new ObjectParameter("CompanyId", typeof(short));
    
            var companyNameParameter = companyName != null ?
                new ObjectParameter("CompanyName", companyName) :
                new ObjectParameter("CompanyName", typeof(string));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            var lastActivationDateParameter = lastActivationDate.HasValue ?
                new ObjectParameter("LastActivationDate", lastActivationDate) :
                new ObjectParameter("LastActivationDate", typeof(System.DateTime));
    
            var isTestRecordParameter = isTestRecord.HasValue ?
                new ObjectParameter("IsTestRecord", isTestRecord) :
                new ObjectParameter("IsTestRecord", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNetUsers_Update", idParameter, emailParameter, emailConfirmedParameter, phoneNumberParameter, phoneNumberConfirmedParameter, twoFactorEnabledParameter, lockoutEndDateUtcParameter, lockoutEnabledParameter, accessFailedCountParameter, userNameParameter, titleParameter, firstNameParameter, lastNameParameter, employeeIdParameter, companyIdParameter, companyNameParameter, modifiedUserParameter, lastActivationDateParameter, isTestRecordParameter);
        }
    
        public virtual int Account_Update(string id, string email, Nullable<bool> emailConfirmed, string phoneNumber, Nullable<bool> phoneNumberConfirmed, Nullable<bool> twoFactorEnabled, Nullable<System.DateTime> lockoutEndDateUtc, Nullable<bool> lockoutEnabled, Nullable<int> accessFailedCount, string userName, Nullable<short> companyId, string modifiedUser, Nullable<System.DateTime> lastActivationDate)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var emailConfirmedParameter = emailConfirmed.HasValue ?
                new ObjectParameter("EmailConfirmed", emailConfirmed) :
                new ObjectParameter("EmailConfirmed", typeof(bool));
    
            var phoneNumberParameter = phoneNumber != null ?
                new ObjectParameter("PhoneNumber", phoneNumber) :
                new ObjectParameter("PhoneNumber", typeof(string));
    
            var phoneNumberConfirmedParameter = phoneNumberConfirmed.HasValue ?
                new ObjectParameter("PhoneNumberConfirmed", phoneNumberConfirmed) :
                new ObjectParameter("PhoneNumberConfirmed", typeof(bool));
    
            var twoFactorEnabledParameter = twoFactorEnabled.HasValue ?
                new ObjectParameter("TwoFactorEnabled", twoFactorEnabled) :
                new ObjectParameter("TwoFactorEnabled", typeof(bool));
    
            var lockoutEndDateUtcParameter = lockoutEndDateUtc.HasValue ?
                new ObjectParameter("LockoutEndDateUtc", lockoutEndDateUtc) :
                new ObjectParameter("LockoutEndDateUtc", typeof(System.DateTime));
    
            var lockoutEnabledParameter = lockoutEnabled.HasValue ?
                new ObjectParameter("LockoutEnabled", lockoutEnabled) :
                new ObjectParameter("LockoutEnabled", typeof(bool));
    
            var accessFailedCountParameter = accessFailedCount.HasValue ?
                new ObjectParameter("AccessFailedCount", accessFailedCount) :
                new ObjectParameter("AccessFailedCount", typeof(int));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var companyIdParameter = companyId.HasValue ?
                new ObjectParameter("CompanyId", companyId) :
                new ObjectParameter("CompanyId", typeof(short));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            var lastActivationDateParameter = lastActivationDate.HasValue ?
                new ObjectParameter("LastActivationDate", lastActivationDate) :
                new ObjectParameter("LastActivationDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Account_Update", idParameter, emailParameter, emailConfirmedParameter, phoneNumberParameter, phoneNumberConfirmedParameter, twoFactorEnabledParameter, lockoutEndDateUtcParameter, lockoutEnabledParameter, accessFailedCountParameter, userNameParameter, companyIdParameter, modifiedUserParameter, lastActivationDateParameter);
        }
    
        public virtual int Profile_Update(string id, string title, string firstName, string lastName, string employeeId, string modifiedUser, Nullable<bool> isTestRecord)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(string));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            var isTestRecordParameter = isTestRecord.HasValue ?
                new ObjectParameter("IsTestRecord", isTestRecord) :
                new ObjectParameter("IsTestRecord", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Profile_Update", idParameter, titleParameter, firstNameParameter, lastNameParameter, employeeIdParameter, modifiedUserParameter, isTestRecordParameter);
        }
    
        public virtual int Service_Insert(string name, string description, string code, Nullable<decimal> price, Nullable<short> serviceCategoryId, Nullable<short> servicePortfolioId, string modifiedUser)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var serviceCategoryIdParameter = serviceCategoryId.HasValue ?
                new ObjectParameter("ServiceCategoryId", serviceCategoryId) :
                new ObjectParameter("ServiceCategoryId", typeof(short));
    
            var servicePortfolioIdParameter = servicePortfolioId.HasValue ?
                new ObjectParameter("ServicePortfolioId", servicePortfolioId) :
                new ObjectParameter("ServicePortfolioId", typeof(short));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Service_Insert", nameParameter, descriptionParameter, codeParameter, priceParameter, serviceCategoryIdParameter, servicePortfolioIdParameter, modifiedUserParameter);
        }
    
        public virtual int Service_Update(Nullable<short> id, Nullable<System.Guid> objectGuid, string name, string description, string code, Nullable<decimal> price, Nullable<short> serviceCategoryId, Nullable<short> servicePortfolioId, string modifiedUser)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(short));
    
            var objectGuidParameter = objectGuid.HasValue ?
                new ObjectParameter("ObjectGuid", objectGuid) :
                new ObjectParameter("ObjectGuid", typeof(System.Guid));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var serviceCategoryIdParameter = serviceCategoryId.HasValue ?
                new ObjectParameter("ServiceCategoryId", serviceCategoryId) :
                new ObjectParameter("ServiceCategoryId", typeof(short));
    
            var servicePortfolioIdParameter = servicePortfolioId.HasValue ?
                new ObjectParameter("ServicePortfolioId", servicePortfolioId) :
                new ObjectParameter("ServicePortfolioId", typeof(short));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Service_Update", idParameter, objectGuidParameter, nameParameter, descriptionParameter, codeParameter, priceParameter, serviceCategoryIdParameter, servicePortfolioIdParameter, modifiedUserParameter);
        }
    
        public virtual int Company_Insert(string name, string code, Nullable<bool> isParent, Nullable<int> parentId, string logoCssClass, string masterBookingPageByPhysician, string masterBookingPageByTime, string masterBookingPageTeleconference, string modifiedUser)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var isParentParameter = isParent.HasValue ?
                new ObjectParameter("IsParent", isParent) :
                new ObjectParameter("IsParent", typeof(bool));
    
            var parentIdParameter = parentId.HasValue ?
                new ObjectParameter("ParentId", parentId) :
                new ObjectParameter("ParentId", typeof(int));
    
            var logoCssClassParameter = logoCssClass != null ?
                new ObjectParameter("LogoCssClass", logoCssClass) :
                new ObjectParameter("LogoCssClass", typeof(string));
    
            var masterBookingPageByPhysicianParameter = masterBookingPageByPhysician != null ?
                new ObjectParameter("MasterBookingPageByPhysician", masterBookingPageByPhysician) :
                new ObjectParameter("MasterBookingPageByPhysician", typeof(string));
    
            var masterBookingPageByTimeParameter = masterBookingPageByTime != null ?
                new ObjectParameter("MasterBookingPageByTime", masterBookingPageByTime) :
                new ObjectParameter("MasterBookingPageByTime", typeof(string));
    
            var masterBookingPageTeleconferenceParameter = masterBookingPageTeleconference != null ?
                new ObjectParameter("MasterBookingPageTeleconference", masterBookingPageTeleconference) :
                new ObjectParameter("MasterBookingPageTeleconference", typeof(string));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Company_Insert", nameParameter, codeParameter, isParentParameter, parentIdParameter, logoCssClassParameter, masterBookingPageByPhysicianParameter, masterBookingPageByTimeParameter, masterBookingPageTeleconferenceParameter, modifiedUserParameter);
        }
    
        public virtual int Company_Update(Nullable<short> id, Nullable<System.Guid> objectGuid, string name, string code, Nullable<bool> isParent, Nullable<int> parentId, string logoCssClass, string masterBookingPageByPhysician, string masterBookingPageByTime, string masterBookingPageTeleconference, string modifiedUser)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(short));
    
            var objectGuidParameter = objectGuid.HasValue ?
                new ObjectParameter("ObjectGuid", objectGuid) :
                new ObjectParameter("ObjectGuid", typeof(System.Guid));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var isParentParameter = isParent.HasValue ?
                new ObjectParameter("IsParent", isParent) :
                new ObjectParameter("IsParent", typeof(bool));
    
            var parentIdParameter = parentId.HasValue ?
                new ObjectParameter("ParentId", parentId) :
                new ObjectParameter("ParentId", typeof(int));
    
            var logoCssClassParameter = logoCssClass != null ?
                new ObjectParameter("LogoCssClass", logoCssClass) :
                new ObjectParameter("LogoCssClass", typeof(string));
    
            var masterBookingPageByPhysicianParameter = masterBookingPageByPhysician != null ?
                new ObjectParameter("MasterBookingPageByPhysician", masterBookingPageByPhysician) :
                new ObjectParameter("MasterBookingPageByPhysician", typeof(string));
    
            var masterBookingPageByTimeParameter = masterBookingPageByTime != null ?
                new ObjectParameter("MasterBookingPageByTime", masterBookingPageByTime) :
                new ObjectParameter("MasterBookingPageByTime", typeof(string));
    
            var masterBookingPageTeleconferenceParameter = masterBookingPageTeleconference != null ?
                new ObjectParameter("MasterBookingPageTeleconference", masterBookingPageTeleconference) :
                new ObjectParameter("MasterBookingPageTeleconference", typeof(string));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Company_Update", idParameter, objectGuidParameter, nameParameter, codeParameter, isParentParameter, parentIdParameter, logoCssClassParameter, masterBookingPageByPhysicianParameter, masterBookingPageByTimeParameter, masterBookingPageTeleconferenceParameter, modifiedUserParameter);
        }
    
        public virtual int ServiceRequest_Delete(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ServiceRequest_Delete", idParameter);
        }
    
        public virtual int ServiceRequest_Insert(string companyReferenceId, Nullable<short> serviceCatalogueId, Nullable<long> harvestProjectId, string title, string body, Nullable<System.DateTime> requestedDate, Nullable<System.Guid> requestedBy, Nullable<System.DateTime> cancelledDate, Nullable<System.Guid> assignedTo, Nullable<byte> statusId, Nullable<System.DateTime> dueDate, Nullable<System.TimeSpan> startTime, Nullable<System.TimeSpan> endTime, Nullable<decimal> price, string modifiedUser)
        {
            var companyReferenceIdParameter = companyReferenceId != null ?
                new ObjectParameter("CompanyReferenceId", companyReferenceId) :
                new ObjectParameter("CompanyReferenceId", typeof(string));
    
            var serviceCatalogueIdParameter = serviceCatalogueId.HasValue ?
                new ObjectParameter("ServiceCatalogueId", serviceCatalogueId) :
                new ObjectParameter("ServiceCatalogueId", typeof(short));
    
            var harvestProjectIdParameter = harvestProjectId.HasValue ?
                new ObjectParameter("HarvestProjectId", harvestProjectId) :
                new ObjectParameter("HarvestProjectId", typeof(long));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            var bodyParameter = body != null ?
                new ObjectParameter("Body", body) :
                new ObjectParameter("Body", typeof(string));
    
            var requestedDateParameter = requestedDate.HasValue ?
                new ObjectParameter("RequestedDate", requestedDate) :
                new ObjectParameter("RequestedDate", typeof(System.DateTime));
    
            var requestedByParameter = requestedBy.HasValue ?
                new ObjectParameter("RequestedBy", requestedBy) :
                new ObjectParameter("RequestedBy", typeof(System.Guid));
    
            var cancelledDateParameter = cancelledDate.HasValue ?
                new ObjectParameter("CancelledDate", cancelledDate) :
                new ObjectParameter("CancelledDate", typeof(System.DateTime));
    
            var assignedToParameter = assignedTo.HasValue ?
                new ObjectParameter("AssignedTo", assignedTo) :
                new ObjectParameter("AssignedTo", typeof(System.Guid));
    
            var statusIdParameter = statusId.HasValue ?
                new ObjectParameter("StatusId", statusId) :
                new ObjectParameter("StatusId", typeof(byte));
    
            var dueDateParameter = dueDate.HasValue ?
                new ObjectParameter("DueDate", dueDate) :
                new ObjectParameter("DueDate", typeof(System.DateTime));
    
            var startTimeParameter = startTime.HasValue ?
                new ObjectParameter("StartTime", startTime) :
                new ObjectParameter("StartTime", typeof(System.TimeSpan));
    
            var endTimeParameter = endTime.HasValue ?
                new ObjectParameter("EndTime", endTime) :
                new ObjectParameter("EndTime", typeof(System.TimeSpan));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ServiceRequest_Insert", companyReferenceIdParameter, serviceCatalogueIdParameter, harvestProjectIdParameter, titleParameter, bodyParameter, requestedDateParameter, requestedByParameter, cancelledDateParameter, assignedToParameter, statusIdParameter, dueDateParameter, startTimeParameter, endTimeParameter, priceParameter, modifiedUserParameter);
        }
    
        public virtual int ServiceRequest_Update(Nullable<int> id, Nullable<System.Guid> objectGuid, string companyReferenceId, Nullable<short> serviceCatalogueId, Nullable<long> harvestProjectId, string title, string body, Nullable<System.DateTime> requestedDate, Nullable<System.Guid> requestedBy, Nullable<System.DateTime> cancelledDate, Nullable<System.Guid> assignedTo, Nullable<byte> statusId, Nullable<System.DateTime> dueDate, Nullable<System.TimeSpan> startTime, Nullable<System.TimeSpan> endTime, Nullable<decimal> price, string modifiedUser)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var objectGuidParameter = objectGuid.HasValue ?
                new ObjectParameter("ObjectGuid", objectGuid) :
                new ObjectParameter("ObjectGuid", typeof(System.Guid));
    
            var companyReferenceIdParameter = companyReferenceId != null ?
                new ObjectParameter("CompanyReferenceId", companyReferenceId) :
                new ObjectParameter("CompanyReferenceId", typeof(string));
    
            var serviceCatalogueIdParameter = serviceCatalogueId.HasValue ?
                new ObjectParameter("ServiceCatalogueId", serviceCatalogueId) :
                new ObjectParameter("ServiceCatalogueId", typeof(short));
    
            var harvestProjectIdParameter = harvestProjectId.HasValue ?
                new ObjectParameter("HarvestProjectId", harvestProjectId) :
                new ObjectParameter("HarvestProjectId", typeof(long));
    
            var titleParameter = title != null ?
                new ObjectParameter("Title", title) :
                new ObjectParameter("Title", typeof(string));
    
            var bodyParameter = body != null ?
                new ObjectParameter("Body", body) :
                new ObjectParameter("Body", typeof(string));
    
            var requestedDateParameter = requestedDate.HasValue ?
                new ObjectParameter("RequestedDate", requestedDate) :
                new ObjectParameter("RequestedDate", typeof(System.DateTime));
    
            var requestedByParameter = requestedBy.HasValue ?
                new ObjectParameter("RequestedBy", requestedBy) :
                new ObjectParameter("RequestedBy", typeof(System.Guid));
    
            var cancelledDateParameter = cancelledDate.HasValue ?
                new ObjectParameter("CancelledDate", cancelledDate) :
                new ObjectParameter("CancelledDate", typeof(System.DateTime));
    
            var assignedToParameter = assignedTo.HasValue ?
                new ObjectParameter("AssignedTo", assignedTo) :
                new ObjectParameter("AssignedTo", typeof(System.Guid));
    
            var statusIdParameter = statusId.HasValue ?
                new ObjectParameter("StatusId", statusId) :
                new ObjectParameter("StatusId", typeof(byte));
    
            var dueDateParameter = dueDate.HasValue ?
                new ObjectParameter("DueDate", dueDate) :
                new ObjectParameter("DueDate", typeof(System.DateTime));
    
            var startTimeParameter = startTime.HasValue ?
                new ObjectParameter("StartTime", startTime) :
                new ObjectParameter("StartTime", typeof(System.TimeSpan));
    
            var endTimeParameter = endTime.HasValue ?
                new ObjectParameter("EndTime", endTime) :
                new ObjectParameter("EndTime", typeof(System.TimeSpan));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var modifiedUserParameter = modifiedUser != null ?
                new ObjectParameter("ModifiedUser", modifiedUser) :
                new ObjectParameter("ModifiedUser", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ServiceRequest_Update", idParameter, objectGuidParameter, companyReferenceIdParameter, serviceCatalogueIdParameter, harvestProjectIdParameter, titleParameter, bodyParameter, requestedDateParameter, requestedByParameter, cancelledDateParameter, assignedToParameter, statusIdParameter, dueDateParameter, startTimeParameter, endTimeParameter, priceParameter, modifiedUserParameter);
        }
    
        public virtual ObjectResult<Location_Select_PhysicianAndCompany_Result> Location_Select_PhysicianAndCompany(string physicianId, Nullable<short> companyId)
        {
            var physicianIdParameter = physicianId != null ?
                new ObjectParameter("PhysicianId", physicianId) :
                new ObjectParameter("PhysicianId", typeof(string));
    
            var companyIdParameter = companyId.HasValue ?
                new ObjectParameter("CompanyId", companyId) :
                new ObjectParameter("CompanyId", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Location_Select_PhysicianAndCompany_Result>("Location_Select_PhysicianAndCompany", physicianIdParameter, companyIdParameter);
        }
    }
}
