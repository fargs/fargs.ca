CREATE TABLE [dbo].[PriceCategory] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_PriceCategory_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_PriceCategory_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PriceCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

