CREATE TABLE [dbo].[EntityType] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_EntityType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_EntityType_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_EntityType] PRIMARY KEY CLUSTERED ([Id] ASC)
);





