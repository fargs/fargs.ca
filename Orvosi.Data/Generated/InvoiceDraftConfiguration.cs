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

    // InvoiceDraft
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceDraftConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<InvoiceDraft>
    {
        public InvoiceDraftConfiguration()
            : this("dbo")
        {
        }

        public InvoiceDraftConfiguration(string schema)
        {
            ToTable(schema + ".InvoiceDraft");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.InvoiceDate).HasColumnName(@"InvoiceDate").IsOptional().HasColumnType("datetime");
            Property(x => x.Currency).HasColumnName(@"Currency").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.PaymentTerms).HasColumnName(@"PaymentTerms").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderId).HasColumnName(@"ServiceProviderId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.CustomerId).HasColumnName(@"CustomerId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.SubTotal).HasColumnName(@"SubTotal").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.TaxRate).HasColumnName(@"TaxRate").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.TaxAmount).HasColumnName(@"TaxAmount").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.Total).HasColumnName(@"Total").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.CreatedBy).HasColumnName(@"CreatedBy").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.ModifiedBy).HasColumnName(@"ModifiedBy").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").IsRequired().HasColumnType("bit");
            Property(x => x.DeletedDate).HasColumnName(@"DeletedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.DeletedBy).HasColumnName(@"DeletedBy").IsOptional().HasColumnType("uniqueidentifier");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
