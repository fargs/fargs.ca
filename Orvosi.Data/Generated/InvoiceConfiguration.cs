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

    // Invoice
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
            : this("dbo")
        {
        }

        public InvoiceConfiguration(string schema)
        {
            ToTable(schema + ".Invoice");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ObjectGuid).HasColumnName(@"ObjectGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.InvoiceNumber).HasColumnName(@"InvoiceNumber").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.InvoiceDate).HasColumnName(@"InvoiceDate").IsRequired().HasColumnType("datetime");
            Property(x => x.Currency).HasColumnName(@"Currency").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Terms).HasColumnName(@"Terms").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.DueDate).HasColumnName(@"DueDate").IsOptional().HasColumnType("datetime");
            Property(x => x.ServiceProviderGuid).HasColumnName(@"ServiceProviderGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceProviderName).HasColumnName(@"ServiceProviderName").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderEntityType).HasColumnName(@"ServiceProviderEntityType").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ServiceProviderLogoCssClass).HasColumnName(@"ServiceProviderLogoCssClass").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderEmail).HasColumnName(@"ServiceProviderEmail").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderPhoneNumber).HasColumnName(@"ServiceProviderPhoneNumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderAddress1).HasColumnName(@"ServiceProviderAddress1").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderAddress2).HasColumnName(@"ServiceProviderAddress2").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderCity).HasColumnName(@"ServiceProviderCity").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderPostalCode).HasColumnName(@"ServiceProviderPostalCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderProvince).HasColumnName(@"ServiceProviderProvince").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ServiceProviderCountry).HasColumnName(@"ServiceProviderCountry").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerGuid).HasColumnName(@"CustomerGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.CustomerName).HasColumnName(@"CustomerName").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerEntityType).HasColumnName(@"CustomerEntityType").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CustomerAddress1).HasColumnName(@"CustomerAddress1").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerAddress2).HasColumnName(@"CustomerAddress2").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerCity).HasColumnName(@"CustomerCity").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerPostalCode).HasColumnName(@"CustomerPostalCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerProvince).HasColumnName(@"CustomerProvince").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerCountry).HasColumnName(@"CustomerCountry").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerEmail).HasColumnName(@"CustomerEmail").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.SubTotal).HasColumnName(@"SubTotal").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.TaxRateHst).HasColumnName(@"TaxRateHst").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.Discount).HasColumnName(@"Discount").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.Total).HasColumnName(@"Total").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            Property(x => x.SentDate).HasColumnName(@"SentDate").IsOptional().HasColumnType("datetime");
            Property(x => x.DownloadDate).HasColumnName(@"DownloadDate").IsOptional().HasColumnType("datetime");
            Property(x => x.PaymentReceivedDate).HasColumnName(@"PaymentReceivedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Hst).HasColumnName(@"Hst").IsOptional().HasColumnType("decimal").HasPrecision(10,2);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
