CREATE TABLE [dbo].[Country] (
    [Id]               SMALLINT       NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [ISO3DigitCountry] SMALLINT       NOT NULL,
    [ISO2CountryCode]  NVARCHAR (2)   NOT NULL,
    [ISO3CountryCode]  NVARCHAR (3)   NOT NULL,
    [ModifiedDate]     SMALLDATETIME  CONSTRAINT [DF_Country_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]     NVARCHAR (256) CONSTRAINT [DF_Country_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Country_ISO2CountryCode] UNIQUE NONCLUSTERED ([ISO2CountryCode] ASC),
    CONSTRAINT [IX_Country_ISO3CountryCode] UNIQUE NONCLUSTERED ([ISO3CountryCode] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Country_ISO3DigitCountry]
    ON [dbo].[Country]([ISO3DigitCountry] ASC);

