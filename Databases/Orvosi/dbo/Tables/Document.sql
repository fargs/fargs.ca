CREATE TABLE [dbo].[Document] (
    [Id]             SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]     UNIQUEIDENTIFIER NOT NULL,
    [DocumentTypeId] TINYINT          NULL,
    [Path]           NVARCHAR (2000)  NULL,
    [Extension]      NCHAR (10)       NULL,
    [Name]           NVARCHAR (256)   NULL,
    [CompanyId]      INT              NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_Document_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (100)   CONSTRAINT [DF_Document_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Document_DocumentType] FOREIGN KEY ([DocumentTypeId]) REFERENCES [dbo].[DocumentType] ([Id])
);

