CREATE TABLE [dbo].[InvoiceDetail] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [InvoiceId]           INT              NOT NULL,
    [ServiceRequestId]    INT              NULL,
    [Description]         NVARCHAR (256)   NOT NULL,
    [Quantity]            SMALLINT         NULL,
    [Rate]                DECIMAL (10, 2)  NULL,
    [Total]               DECIMAL (10, 2)  NULL,
    [Discount]            DECIMAL (10, 2)  NULL,
    [DiscountDescription] NVARCHAR (256)   NULL,
    [Amount]              DECIMAL (10, 2)  NULL,
    [AdditionalNotes]     NVARCHAR (1000)  NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_InvoiceDetail_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]        NVARCHAR (100)   CONSTRAINT [DF_InvoiceDetail_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [IsDeleted]           BIT              CONSTRAINT [DF_InvoiceDetail_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DeletedDate]         DATETIME         NULL,
    [DeletedBy]           UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_InvoiceDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvoiceDetail_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_InvoiceDetail_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id])
);







