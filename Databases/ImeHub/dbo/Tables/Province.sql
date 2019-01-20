CREATE TABLE [dbo].[Province] (
    [Id]             SMALLINT       NOT NULL,
    [CountryID]      SMALLINT       NOT NULL,
    [ProvinceName]   NVARCHAR (100) NULL,
    [ProvinceCode]   NVARCHAR (50)  NOT NULL,
    [ProvinceTypeID] TINYINT        NULL,
    CONSTRAINT [PK_Province] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Province_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([Id])
);



