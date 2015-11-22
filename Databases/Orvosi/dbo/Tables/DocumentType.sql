CREATE TABLE [dbo].[DocumentType] (
    [Id]           TINYINT        IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [ModifiedDate] DATETIME       CONSTRAINT [DF_DocumentType_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100) CONSTRAINT [DF_DocumentType_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

