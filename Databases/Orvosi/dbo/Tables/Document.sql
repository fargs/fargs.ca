CREATE TABLE [dbo].[Document] (
    [Id]                 SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Document_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [OwnedByObjectGuid]  UNIQUEIDENTIFIER NULL,
    [DocumentTemplateId] SMALLINT         NULL,
    [EffectiveDate]      DATETIME         NULL,
    [ExpiryDate]         DATETIME         NULL,
    [Path]               NVARCHAR (2000)  NULL,
    [Extension]          NVARCHAR (50)    NULL,
    [Name]               NVARCHAR (256)   NULL,
    [Content]            VARBINARY (MAX)  NULL,
    [ContentType]        NVARCHAR (50)    NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_Document_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   CONSTRAINT [DF_Document_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Document_DocumentTemplate] FOREIGN KEY ([DocumentTemplateId]) REFERENCES [dbo].[DocumentTemplate] ([Id])
);





