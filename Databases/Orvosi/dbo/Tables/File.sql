CREATE TABLE [dbo].[File] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [ExternalFileId] NVARCHAR (128)   NULL,
    [Name]           NVARCHAR (256)   NULL,
    [DoctorId]       UNIQUEIDENTIFIER NULL,
    [CompanyId]      SMALLINT         NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_File_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (100)   CONSTRAINT [DF_File_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([Id] ASC)
);

