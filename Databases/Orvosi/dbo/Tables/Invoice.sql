CREATE TABLE [dbo].[Invoice] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [ServiceRequestId]    INT              NULL,
    [InvoiceNumber]       NVARCHAR (128)   NOT NULL,
    [InvoiceDate]         DATETIME         NOT NULL,
    [Currency]            NVARCHAR (128)   NULL,
    [Terms]               NVARCHAR (128)   NULL,
    [DueDate]             DATETIME         NULL,
    [CompanyGuid]         UNIQUEIDENTIFIER NULL,
    [CompanyName]         NVARCHAR (128)   NOT NULL,
    [CompanyLogoCssClass] NVARCHAR (128)   NULL,
    [Email]               NVARCHAR (128)   NULL,
    [PhoneNumber]         NVARCHAR (128)   NULL,
    [Address1]            NVARCHAR (128)   NULL,
    [Address2]            NVARCHAR (128)   NULL,
    [Address3]            NVARCHAR (128)   NULL,
    [BillToGuid]          UNIQUEIDENTIFIER NULL,
    [BillToName]          NVARCHAR (128)   NOT NULL,
    [BillToAddress1]      NVARCHAR (128)   NULL,
    [BillToAddress2]      NVARCHAR (128)   NULL,
    [BillToAddress3]      NVARCHAR (128)   NULL,
    [BillToEmail]         NVARCHAR (128)   NOT NULL,
    [SubTotal]            DECIMAL (10, 2)  NULL,
    [TaxRateHst]          DECIMAL (10, 2)  NULL,
    [Discount]            DECIMAL (10, 2)  NULL,
    [Total]               DECIMAL (10, 2)  NULL,
    [PaymentReceivedDate] DATETIME         NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_Invoice_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]        NVARCHAR (100)   CONSTRAINT [DF_Invoice_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);



