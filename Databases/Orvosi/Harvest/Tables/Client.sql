CREATE TABLE [Harvest].[Client] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [UpdatedAt]      DATETIME       NOT NULL,
    [CreatedAt]      DATETIME       NOT NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [HighriseId]     BIGINT         NULL,
    [CacheVersion]   BIGINT         NOT NULL,
    [Currency]       INT            NULL,
    [Active]         BIT            NOT NULL,
    [CurrencySymbol] NVARCHAR (MAX) NULL,
    [Details]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Harvest.Client] PRIMARY KEY CLUSTERED ([Id] ASC)
);

