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
    public partial class ServiceRequestConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequest>
    {
        public ServiceRequestConfiguration()
            : this("dbo")
        {
        }

        public ServiceRequestConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequest");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ObjectGuid).HasColumnName(@"ObjectGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.CompanyReferenceId).HasColumnName(@"CompanyReferenceId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ClaimantName).HasColumnName(@"ClaimantName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceCatalogueId).HasColumnName(@"ServiceCatalogueId").IsOptional().HasColumnType("smallint");
            Property(x => x.HarvestProjectId).HasColumnName(@"HarvestProjectId").IsOptional().HasColumnType("bigint");
            Property(x => x.Title).HasColumnName(@"Title").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Body).HasColumnName(@"Body").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AddressId).HasColumnName(@"AddressId").IsOptional().HasColumnType("int");
            Property(x => x.RequestedDate).HasColumnName(@"RequestedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.RequestedBy).HasColumnName(@"RequestedBy").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.CancelledDate).HasColumnName(@"CancelledDate").IsOptional().HasColumnType("datetime");
            Property(x => x.AvailableSlotId).HasColumnName(@"AvailableSlotId").IsOptional().HasColumnType("smallint");
            Property(x => x.AppointmentDate).HasColumnName(@"AppointmentDate").IsOptional().HasColumnType("date");
            Property(x => x.StartTime).HasColumnName(@"StartTime").IsOptional().HasColumnType("time");
            Property(x => x.EndTime).HasColumnName(@"EndTime").IsOptional().HasColumnType("time");
            Property(x => x.DueDate).HasColumnName(@"DueDate").IsOptional().HasColumnType("date");
            Property(x => x.CaseCoordinatorId).HasColumnName(@"CaseCoordinatorId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.IntakeAssistantId).HasColumnName(@"IntakeAssistantId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.DocumentReviewerId).HasColumnName(@"DocumentReviewerId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.Price).HasColumnName(@"Price").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Notes).HasColumnName(@"Notes").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.InvoiceItemId).HasColumnName(@"InvoiceItemId").IsOptional().HasColumnType("int");
            Property(x => x.DocumentFolderLink).HasColumnName(@"DocumentFolderLink").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsOptional().HasColumnType("smallint");
            Property(x => x.IsNoShow).HasColumnName(@"IsNoShow").IsRequired().HasColumnType("bit");
            Property(x => x.IsLateCancellation).HasColumnName(@"IsLateCancellation").IsRequired().HasColumnType("bit");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.NoShowRate).HasColumnName(@"NoShowRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.LateCancellationRate).HasColumnName(@"LateCancellationRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsOptional().HasColumnType("smallint");
            Property(x => x.LocationId).HasColumnName(@"LocationId").IsOptional().HasColumnType("int");
            Property(x => x.ServiceCataloguePrice).HasColumnName(@"ServiceCataloguePrice").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.BoxCaseFolderId).HasColumnName(@"BoxCaseFolderId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.IntakeAssistantBoxCollaborationId).HasColumnName(@"IntakeAssistantBoxCollaborationId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DocumentReviewerBoxCollaborationId).HasColumnName(@"DocumentReviewerBoxCollaborationId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);

            // Foreign keys
            HasOptional(a => a.Address).WithMany(b => b.ServiceRequests).HasForeignKey(c => c.AddressId).WillCascadeOnDelete(false); // FK_ServiceRequest_Address
            HasOptional(a => a.AvailableSlot).WithMany(b => b.ServiceRequests).HasForeignKey(c => c.AvailableSlotId).WillCascadeOnDelete(false); // FK_ServiceRequest_AvailableSlot
            HasOptional(a => a.CaseCoordinator).WithMany(b => b.CaseCoordinator).HasForeignKey(c => c.CaseCoordinatorId).WillCascadeOnDelete(false); // FK_ServiceRequest_CaseCoordinator
            HasOptional(a => a.Company).WithMany(b => b.ServiceRequests).HasForeignKey(c => c.CompanyId).WillCascadeOnDelete(false); // FK_ServiceRequest_Company
            HasOptional(a => a.DocumentReviewer).WithMany(b => b.DocumentReviewer).HasForeignKey(c => c.DocumentReviewerId).WillCascadeOnDelete(false); // FK_ServiceRequest_DocumentReviewer
            HasOptional(a => a.IntakeAssistant).WithMany(b => b.IntakeAssistant).HasForeignKey(c => c.IntakeAssistantId).WillCascadeOnDelete(false); // FK_ServiceRequest_IntakeAssistant
            HasOptional(a => a.Service).WithMany(b => b.ServiceRequests).HasForeignKey(c => c.ServiceId).WillCascadeOnDelete(false); // FK_ServiceRequest_Service
            HasRequired(a => a.Physician).WithMany(b => b.ServiceRequests).HasForeignKey(c => c.PhysicianId).WillCascadeOnDelete(false); // FK_ServiceRequest_Physician
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
