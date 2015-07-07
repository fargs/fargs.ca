CREATE TABLE [dbo].[ServicePortfolio] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_ServicePortfolio_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_ServicePortfolio_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_ServicePortfolio] PRIMARY KEY CLUSTERED ([Id] ASC)
);

