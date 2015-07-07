CREATE TABLE [dbo].[Service] (
    [Id]                 SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (128) NOT NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [Code]               NCHAR (10)     NULL,
    [IsAddOn]            BIT            CONSTRAINT [DF_Service_IsAddOn] DEFAULT ((0)) NULL,
    [ServiceCategoryId]  SMALLINT       NULL,
    [ServicePortfolioId] SMALLINT       NULL,
    [ModifiedDate]       DATETIME       CONSTRAINT [DF_Service_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100) CONSTRAINT [DF_Service_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Id] ASC)
);

