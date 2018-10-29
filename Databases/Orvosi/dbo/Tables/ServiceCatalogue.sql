CREATE TABLE [dbo].[ServiceCatalogue] (
    [Id]                   SMALLINT         IDENTITY (1, 1) NOT NULL,
    [PhysicianId]          UNIQUEIDENTIFIER NULL,
    [ServiceId]            SMALLINT         NULL,
    [CompanyId]            SMALLINT         NULL,
    [LocationId]           SMALLINT         NULL,
    [Price]                DECIMAL (18, 2)  NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_ServiceCatalogue_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]         NVARCHAR (100)   CONSTRAINT [DF_ServiceCatalogue_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [NoShowRate]           DECIMAL (18, 2)  NULL,
    [LateCancellationRate] DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_ServiceCatalogue] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceCatalogue_City] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_ServiceCatalogue_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_ServiceCatalogue_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id])
);


GO
ALTER TABLE [dbo].[ServiceCatalogue] NOCHECK CONSTRAINT [FK_ServiceCatalogue_City];


GO
ALTER TABLE [dbo].[ServiceCatalogue] NOCHECK CONSTRAINT [FK_ServiceCatalogue_Company];


GO
ALTER TABLE [dbo].[ServiceCatalogue] NOCHECK CONSTRAINT [FK_ServiceCatalogue_Service];











