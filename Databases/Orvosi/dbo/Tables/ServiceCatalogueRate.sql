CREATE TABLE [dbo].[ServiceCatalogueRate] (
    [Id]                     INT              IDENTITY (1, 1) NOT NULL,
    [ServiceProviderGuid]    UNIQUEIDENTIFIER NULL,
    [CustomerGuid]           UNIQUEIDENTIFIER NULL,
    [NoShowRate]             DECIMAL (18, 2)  NULL,
    [LateCancellationRate]   DECIMAL (18, 2)  NULL,
    [ModifiedDate]           DATETIME         CONSTRAINT [DF_ServiceCatalogueRate_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]           NVARCHAR (100)   CONSTRAINT [DF_ServiceCatalogueRate_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [LateCancellationPolicy] INT              NULL,
    CONSTRAINT [PK_ServiceCatalogueRate] PRIMARY KEY CLUSTERED ([Id] ASC)
);


