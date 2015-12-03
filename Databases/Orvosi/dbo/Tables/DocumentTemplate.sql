CREATE TABLE [dbo].[DocumentTemplate] (
    [Id]                SMALLINT         IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (128)   NOT NULL,
    [OwnedByObjectGuid] UNIQUEIDENTIFIER NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_DocumentTemplate_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]      NVARCHAR (100)   CONSTRAINT [DF_DocumentTemplate_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_DocumentTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

