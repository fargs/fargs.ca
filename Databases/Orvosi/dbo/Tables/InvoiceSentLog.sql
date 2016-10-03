CREATE TABLE [dbo].[InvoiceSentLog] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [InvoiceId]    INT            NOT NULL,
    [EmailTo]      NVARCHAR (128) NOT NULL,
    [SentDate]     DATETIME       NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_InvoiceSentLog_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_InvoiceSentLog_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_InvoiceSentLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvoiceSentLog_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

