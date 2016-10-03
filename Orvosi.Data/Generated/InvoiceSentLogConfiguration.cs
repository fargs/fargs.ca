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

    // InvoiceSentLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceSentLogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<InvoiceSentLog>
    {
        public InvoiceSentLogConfiguration()
            : this("dbo")
        {
        }

        public InvoiceSentLogConfiguration(string schema)
        {
            ToTable(schema + ".InvoiceSentLog");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.InvoiceId).HasColumnName(@"InvoiceId").IsRequired().HasColumnType("int");
            Property(x => x.EmailTo).HasColumnName(@"EmailTo").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.SentDate).HasColumnName(@"SentDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);

            // Foreign keys
            HasRequired(a => a.Invoice).WithMany(b => b.InvoiceSentLogs).HasForeignKey(c => c.InvoiceId); // FK_InvoiceSentLog_Invoice
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
