CREATE TABLE [dbo].[Price] (
    [Id]              SMALLINT        IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]      NVARCHAR (128)  NOT NULL,
    [PriceCategoryId] SMALLINT        NULL,
    [Price]           DECIMAL (18, 2) NULL,
    [ModifiedDate]    DATETIME        CONSTRAINT [DF_Price_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]    NVARCHAR (100)  CONSTRAINT [DF_Price_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED ([Id] ASC)
);

