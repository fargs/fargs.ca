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

    // ServiceRequestMessage
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestMessageConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequestMessage>
    {
        public ServiceRequestMessageConfiguration()
            : this("dbo")
        {
        }

        public ServiceRequestMessageConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequestMessage");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Message).HasColumnName(@"Message").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.PostedDate).HasColumnName(@"PostedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceRequestId).HasColumnName(@"ServiceRequestId").IsRequired().HasColumnType("int");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);

            // Foreign keys
            HasRequired(a => a.AspNetUser).WithMany(b => b.ServiceRequestMessages).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_ServiceRequestMessage_AspNetUsers
            HasRequired(a => a.ServiceRequest).WithMany(b => b.ServiceRequestMessages).HasForeignKey(c => c.ServiceRequestId).WillCascadeOnDelete(false); // FK_ServiceRequestMessage_ServiceRequest
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>