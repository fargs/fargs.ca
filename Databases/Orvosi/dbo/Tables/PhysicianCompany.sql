CREATE TABLE [dbo].[PhysicianCompany] (
    [Id]           SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL,
    [CompanyId]    SMALLINT         NOT NULL,
    [StatusId]     SMALLINT         CONSTRAINT [DF__Physician__Relat__22751F6C] DEFAULT ((1)) NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_PhysicianCompany_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser] NVARCHAR (100)   CONSTRAINT [DF_PhysicianCompany_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_PhysicianCompany] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhysicianCompany_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_PhysicianCompany_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_PhysicianCompany_PhysicianCompanyStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[PhysicianCompanyStatus] ([Id])
);



