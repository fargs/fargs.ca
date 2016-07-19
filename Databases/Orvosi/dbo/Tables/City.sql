CREATE TABLE [dbo].[City] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NULL,
    [Code]         NVARCHAR (3)   NULL,
    [ProvinceId]   SMALLINT       NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_City_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (128) CONSTRAINT [DF_City_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_City_Province] FOREIGN KEY ([ProvinceId]) REFERENCES [dbo].[Province] ([Id])
);

