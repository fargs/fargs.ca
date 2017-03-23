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

    // ServiceRequestComment
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestCommentConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequestComment>
    {
        public ServiceRequestCommentConfiguration()
            : this("dbo")
        {
        }

        public ServiceRequestCommentConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequestComment");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Comment).HasColumnName(@"Comment").IsRequired().HasColumnType("nvarchar");
            Property(x => x.PostedDate).HasColumnName(@"PostedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceRequestId).HasColumnName(@"ServiceRequestId").IsRequired().HasColumnType("int");
            Property(x => x.CommentTypeId).HasColumnName(@"CommentTypeId").IsRequired().HasColumnType("tinyint");
            Property(x => x.IsPrivate).HasColumnName(@"IsPrivate").IsRequired().HasColumnType("bit");
            Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.CreatedUser).HasColumnName(@"CreatedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);

            // Foreign keys
            HasRequired(a => a.AspNetUser).WithMany(b => b.ServiceRequestComments).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_ServiceRequestComment_AspNetUsers
            HasRequired(a => a.CommentType).WithMany(b => b.ServiceRequestComments).HasForeignKey(c => c.CommentTypeId).WillCascadeOnDelete(false); // FK_ServiceRequestComment_CommentType
            HasRequired(a => a.ServiceRequest).WithMany(b => b.ServiceRequestComments).HasForeignKey(c => c.ServiceRequestId).WillCascadeOnDelete(false); // FK_ServiceRequestComment_ServiceRequest
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
