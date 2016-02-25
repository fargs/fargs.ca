CREATE TABLE [dbo].[ServiceCatalogue] (
    [Id]                   SMALLINT        IDENTITY (1, 1) NOT NULL,
    [PhysicianId]          NVARCHAR (128)  NULL,
    [ServiceId]            SMALLINT        NULL,
    [CompanyId]            SMALLINT        NULL,
    [LocationId]           SMALLINT        NULL,
    [Price]                DECIMAL (18, 2) NULL,
    [ModifiedDate]         DATETIME        CONSTRAINT [DF_ServiceCatalogue_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]         NVARCHAR (100)  CONSTRAINT [DF_ServiceCatalogue_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [NoShowRate]           DECIMAL (18, 2) NULL,
    [LateCancellationRate] DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_ServiceCatalogue] PRIMARY KEY CLUSTERED ([Id] ASC)
);







