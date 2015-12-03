CREATE TABLE [dbo].[Organization] (
    [Id]           SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]   UNIQUEIDENTIFIER CONSTRAINT [DF_Organization_ObjectGuid] DEFAULT (newid()) NULL,
    [Name]         NVARCHAR (128)   NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Organization_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)   CONSTRAINT [DF_Organization_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([Id] ASC)
);

