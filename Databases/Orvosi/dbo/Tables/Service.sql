CREATE TABLE [dbo].[Service] (
    [Id]                 SMALLINT         IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Service_ObjectGuid] DEFAULT (newid()) NULL,
    [Name]               NVARCHAR (128)   NOT NULL,
    [Description]        NVARCHAR (MAX)   NULL,
    [Code]               NVARCHAR (10)    NULL,
    [Price]              DECIMAL (18, 2)  NULL,
    [ServiceCategoryId]  SMALLINT         NULL,
    [ServicePortfolioId] SMALLINT         NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_Service_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]       NVARCHAR (100)   CONSTRAINT [DF_Service_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [IsLocationRequired] BIT              CONSTRAINT [DF_Service_IsLocationRequired] DEFAULT ((1)) NOT NULL,
    [ColorCode]          VARCHAR (10)     NULL,
    [PhysicianId]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Service_ServiceCategory] FOREIGN KEY ([ServiceCategoryId]) REFERENCES [dbo].[ServiceCategory] ([Id])
);













