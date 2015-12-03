CREATE TABLE [dbo].[Case] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [ExternalCaseId] NVARCHAR (128)   NULL,
    [Name]           NVARCHAR (256)   NULL,
    [PhysicianId]    UNIQUEIDENTIFIER NULL,
    [CompanyId]      SMALLINT         NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_Case_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]   NVARCHAR (100)   CONSTRAINT [DF_Case_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED ([Id] ASC)
);

