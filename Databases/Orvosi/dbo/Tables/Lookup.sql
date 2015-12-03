CREATE TABLE [dbo].[Lookup] (
    [Id]           SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_Lookup_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_Lookup_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

