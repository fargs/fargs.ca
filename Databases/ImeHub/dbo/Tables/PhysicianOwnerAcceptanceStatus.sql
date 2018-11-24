CREATE TABLE [dbo].[PhysicianOwnerAcceptanceStatus] (
    [Id]        TINYINT       NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [Code]      NVARCHAR (10) NULL,
    [ColorCode] NVARCHAR (10) NULL,
    CONSTRAINT [PK_PhysicianOwnerAcceptanceStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);



