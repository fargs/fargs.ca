CREATE TABLE [dbo].[Country] (
    [Id]               SMALLINT       NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [ISO3DigitCountry] SMALLINT       NOT NULL,
    [ISO2CountryCode]  NVARCHAR (2)   NOT NULL,
    [ISO3CountryCode]  NVARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

