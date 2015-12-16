CREATE TABLE [dbo].[PhysicianLicense] (
    [Id]               SMALLINT        IDENTITY (1, 1) NOT NULL,
    [PhysicianId]      NVARCHAR (128)  NOT NULL,
    [CollegeName]      NVARCHAR (128)  NULL,
    [LongName]         NVARCHAR (2000) NULL,
    [ExpiryDate]       DATE            NULL,
    [MemberName]       NVARCHAR (256)  NULL,
    [CertificateClass] NVARCHAR (128)  NULL,
    [IsPrimary]        BIT             CONSTRAINT [DF__Physician__IsPri__70099B30] DEFAULT ((0)) NULL,
    [Preference]       TINYINT         NULL,
    [DocumentId]       SMALLINT        NULL,
    [ModifiedDate]     DATETIME        CONSTRAINT [DF_PhysicianLicense_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]     NVARCHAR (100)  CONSTRAINT [DF_PhysicianLicense_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianLicense] PRIMARY KEY CLUSTERED ([Id] ASC)
);

