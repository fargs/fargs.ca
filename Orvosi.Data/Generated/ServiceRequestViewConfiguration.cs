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

    // ServiceRequest
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestViewConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequestView>
    {
        public ServiceRequestViewConfiguration()
            : this("API")
        {
        }

        public ServiceRequestViewConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequest");
            HasKey(x => new { x.Id, x.ObjectGuid, x.IsNoShow, x.IsLateCancellation, x.ModifiedDate, x.ModifiedUser, x.ServiceName, x.PhysicianId, x.PhysicianUserName });

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int");
            Property(x => x.ObjectGuid).HasColumnName(@"ObjectGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.CompanyReferenceId).HasColumnName(@"CompanyReferenceId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ClaimantName).HasColumnName(@"ClaimantName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceCatalogueId).HasColumnName(@"ServiceCatalogueId").IsOptional().HasColumnType("smallint");
            Property(x => x.Body).HasColumnName(@"Body").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AddressId).HasColumnName(@"AddressId").IsOptional().HasColumnType("int");
            Property(x => x.RequestedDate).HasColumnName(@"RequestedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.RequestedBy).HasColumnName(@"RequestedBy").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.CancelledDate).HasColumnName(@"CancelledDate").IsOptional().HasColumnType("datetime");
            Property(x => x.AvailableSlotId).HasColumnName(@"AvailableSlotId").IsOptional().HasColumnType("smallint");
            Property(x => x.DueDate).HasColumnName(@"DueDate").IsOptional().HasColumnType("date");
            Property(x => x.CaseCoordinatorId).HasColumnName(@"CaseCoordinatorId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.IntakeAssistantId).HasColumnName(@"IntakeAssistantId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.DocumentReviewerId).HasColumnName(@"DocumentReviewerId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceRequestPrice).HasColumnName(@"ServiceRequestPrice").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Notes).HasColumnName(@"Notes").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsOptional().HasColumnType("smallint");
            Property(x => x.IsNoShow).HasColumnName(@"IsNoShow").IsRequired().HasColumnType("bit");
            Property(x => x.IsLateCancellation).HasColumnName(@"IsLateCancellation").IsRequired().HasColumnType("bit");
            Property(x => x.NoShowRate).HasColumnName(@"NoShowRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.LateCancellationRate).HasColumnName(@"LateCancellationRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsOptional().HasColumnType("smallint");
            Property(x => x.AppointmentDate).HasColumnName(@"AppointmentDate").IsOptional().HasColumnType("date");
            Property(x => x.StartTime).HasColumnName(@"StartTime").IsOptional().HasColumnType("time");
            Property(x => x.Duration).HasColumnName(@"Duration").IsOptional().HasColumnType("int");
            Property(x => x.EndTime).HasColumnName(@"EndTime").IsOptional().HasColumnType("time");
            Property(x => x.Title).HasColumnName(@"Title").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.ServiceRequestStatusId).HasColumnName(@"ServiceRequestStatusId").IsOptional().HasColumnType("tinyint");
            Property(x => x.ServiceRequestStatusText).HasColumnName(@"ServiceRequestStatusText").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceStatusId).HasColumnName(@"ServiceStatusId").IsOptional().HasColumnType("tinyint");
            Property(x => x.ServiceStatusText).HasColumnName(@"ServiceStatusText").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceName).HasColumnName(@"ServiceName").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceCode).HasColumnName(@"ServiceCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ServicePrice).HasColumnName(@"ServicePrice").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ServiceCategoryId).HasColumnName(@"ServiceCategoryId").IsOptional().HasColumnType("smallint");
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.PhysicianDisplayName).HasColumnName(@"PhysicianDisplayName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.PhysicianUserName).HasColumnName(@"PhysicianUserName").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.PhysicianInitials).HasColumnName(@"PhysicianInitials").IsOptional().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.PhysicianColorCode).HasColumnName(@"PhysicianColorCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CompanyGuid).HasColumnName(@"CompanyGuid").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.CompanyName).HasColumnName(@"CompanyName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ParentCompanyName).HasColumnName(@"ParentCompanyName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceCataloguePrice).HasColumnName(@"ServiceCataloguePrice").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.EffectivePrice).HasColumnName(@"EffectivePrice").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.RequestedByName).HasColumnName(@"RequestedByName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.AddressName).HasColumnName(@"AddressName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.AddressTypeId).HasColumnName(@"AddressTypeId").IsOptional().HasColumnType("tinyint");
            Property(x => x.AddressTypeName).HasColumnName(@"AddressTypeName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Address1).HasColumnName(@"Address1").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Address2).HasColumnName(@"Address2").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.City).HasColumnName(@"City").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.PostalCode).HasColumnName(@"PostalCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ProvinceId).HasColumnName(@"ProvinceId").IsOptional().HasColumnType("smallint");
            Property(x => x.ProvinceName).HasColumnName(@"ProvinceName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CountryId).HasColumnName(@"CountryId").IsOptional().HasColumnType("smallint");
            Property(x => x.CountryName).HasColumnName(@"CountryName").IsOptional().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.LocationId).HasColumnName(@"LocationId").IsOptional().HasColumnType("smallint");
            Property(x => x.CaseCoordinatorName).HasColumnName(@"CaseCoordinatorName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.CaseCoordinatorInitials).HasColumnName(@"CaseCoordinatorInitials").IsOptional().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.CaseCoordinatorColorCode).HasColumnName(@"CaseCoordinatorColorCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CaseCoordinatorUserName).HasColumnName(@"CaseCoordinatorUserName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.IntakeAssistantName).HasColumnName(@"IntakeAssistantName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.IntakeAssistantInitials).HasColumnName(@"IntakeAssistantInitials").IsOptional().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.IntakeAssistantColorCode).HasColumnName(@"IntakeAssistantColorCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.IntakeAssistantUserName).HasColumnName(@"IntakeAssistantUserName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.IntakeAssistantBoxCollaborationId).HasColumnName(@"IntakeAssistantBoxCollaborationId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.IntakeAssistantBoxUserId).HasColumnName(@"IntakeAssistantBoxUserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DocumentReviewerName).HasColumnName(@"DocumentReviewerName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.DocumentReviewerInitials).HasColumnName(@"DocumentReviewerInitials").IsOptional().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.DocumentReviewerColorCode).HasColumnName(@"DocumentReviewerColorCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DocumentReviewerUserName).HasColumnName(@"DocumentReviewerUserName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.DocumentReviewerBoxCollaborationId).HasColumnName(@"DocumentReviewerBoxCollaborationId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DocumentReviewerBoxUserId).HasColumnName(@"DocumentReviewerBoxUserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CalendarEventTitle).HasColumnName(@"CalendarEventTitle").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.TotalTasks).HasColumnName(@"TotalTasks").IsOptional().HasColumnType("int");
            Property(x => x.ClosedTasks).HasColumnName(@"ClosedTasks").IsOptional().HasColumnType("int");
            Property(x => x.OpenTasks).HasColumnName(@"OpenTasks").IsOptional().HasColumnType("int");
            Property(x => x.NextTaskId).HasColumnName(@"NextTaskId").IsOptional().HasColumnType("int");
            Property(x => x.NextTaskName).HasColumnName(@"NextTaskName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.NextTaskAssignedTo).HasColumnName(@"NextTaskAssignedTo").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.NextTaskAssignedtoName).HasColumnName(@"NextTaskAssignedtoName").IsOptional().HasColumnType("nvarchar").HasMaxLength(210);
            Property(x => x.BoxCaseFolderId).HasColumnName(@"BoxCaseFolderId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.BoxPhysicianFolderId).HasColumnName(@"BoxPhysicianFolderId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceCategoryName).HasColumnName(@"ServiceCategoryName").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.DocumentFolderLink).HasColumnName(@"DocumentFolderLink").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.ServicePortfolioName).HasColumnName(@"ServicePortfolioName").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.LocationName).HasColumnName(@"LocationName").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
