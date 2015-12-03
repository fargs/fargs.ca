CREATE TABLE [dbo].[PhysicianCompany] (
    [Id]                   SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]          UNIQUEIDENTIFIER NOT NULL,
    [CompanyId]            SMALLINT         NOT NULL,
    [RelationshipStatusId] TINYINT          DEFAULT ((1)) NOT NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_PhysicianCompany_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]         NVARCHAR (100)   CONSTRAINT [DF_PhysicianCompany_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianCompany] PRIMARY KEY CLUSTERED ([Id] ASC)
);

