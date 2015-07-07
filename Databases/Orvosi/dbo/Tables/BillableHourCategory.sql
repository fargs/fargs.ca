CREATE TABLE [dbo].[BillableHourCategory] (
    [Id]           SMALLINT        IDENTITY (1, 1) NOT NULL,
    [Hours]        DECIMAL (18, 2) NULL,
    [Code]         NVARCHAR (128)  NOT NULL,
    [ModifiedDate] DATETIME        CONSTRAINT [DF_BillableHourCategory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)  CONSTRAINT [DF_BillableHourCategory_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_BillableHourCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

