CREATE TABLE [dbo].[Invoice] (
    [Id]                          INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]                  UNIQUEIDENTIFIER CONSTRAINT [DF_Invoice_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [InvoiceNumber]               NVARCHAR (128)   NOT NULL,
    [InvoiceDate]                 DATETIME         NOT NULL,
    [Currency]                    NVARCHAR (128)   NULL,
    [Terms]                       NVARCHAR (128)   NULL,
    [DueDate]                     DATETIME         NULL,
    [ServiceProviderGuid]         UNIQUEIDENTIFIER NOT NULL,
    [ServiceProviderName]         NVARCHAR (128)   NOT NULL,
    [ServiceProviderEntityType]   NVARCHAR (50)    NOT NULL,
    [ServiceProviderLogoCssClass] NVARCHAR (128)   NULL,
    [ServiceProviderEmail]        NVARCHAR (128)   NULL,
    [ServiceProviderPhoneNumber]  NVARCHAR (128)   NULL,
    [ServiceProviderAddress1]     NVARCHAR (128)   NULL,
    [ServiceProviderAddress2]     NVARCHAR (128)   NULL,
    [ServiceProviderCity]         NVARCHAR (128)   NULL,
    [ServiceProviderPostalCode]   NVARCHAR (128)   NULL,
    [ServiceProviderProvince]     NVARCHAR (128)   NULL,
    [ServiceProviderCountry]      NVARCHAR (128)   NULL,
    [CustomerGuid]                UNIQUEIDENTIFIER NOT NULL,
    [CustomerName]                NVARCHAR (128)   NOT NULL,
    [CustomerEntityType]          NVARCHAR (50)    NOT NULL,
    [CustomerAddress1]            NVARCHAR (128)   NULL,
    [CustomerAddress2]            NVARCHAR (128)   NULL,
    [CustomerCity]                NVARCHAR (128)   NULL,
    [CustomerPostalCode]          NVARCHAR (128)   NULL,
    [CustomerProvince]            NVARCHAR (128)   NULL,
    [CustomerCountry]             NVARCHAR (128)   NULL,
    [CustomerEmail]               NVARCHAR (128)   NOT NULL,
    [SubTotal]                    DECIMAL (10, 2)  NULL,
    [TaxRateHst]                  DECIMAL (10, 2)  NULL,
    [Discount]                    DECIMAL (10, 2)  NULL,
    [Total]                       DECIMAL (10, 2)  NULL,
    [SentDate]                    DATETIME         NULL,
    [DownloadDate]                DATETIME         NULL,
    [PaymentReceivedDate]         DATETIME         NULL,
    [ModifiedDate]                DATETIME         CONSTRAINT [DF_Invoice_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]                NVARCHAR (100)   CONSTRAINT [DF_Invoice_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [Hst]                         DECIMAL (10, 2)  NULL,
    [IsDeleted]                   BIT              CONSTRAINT [DF_Invoice_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedDate]                 DATETIME         NULL,
    [DeletedBy]                   UNIQUEIDENTIFIER NULL,
    [ServiceProviderHstNumber]    NVARCHAR (50)    NULL,
    [BoxFileId]                   NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);













