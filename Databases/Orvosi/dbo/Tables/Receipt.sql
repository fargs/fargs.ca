CREATE TABLE [dbo].[Receipt] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [InvoiceId]    INT              NOT NULL,
    [ReceivedDate] DATETIME         NOT NULL,
    [IsPaidInFull] BIT              CONSTRAINT [DF_Receipt_IsPaidInFull] DEFAULT ((1)) NOT NULL,
    [Amount]       DECIMAL (18, 2)  NOT NULL,
    [CreatedDate]  DATETIME         CONSTRAINT [DF_Receipt_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedUser]  NVARCHAR (50)    NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Receipt_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_Receipt_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Receipt_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id])
);

