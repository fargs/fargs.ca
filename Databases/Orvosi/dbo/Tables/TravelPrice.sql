CREATE TABLE [dbo].[TravelPrice] (
    [Id]               UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]     DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]       DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [CompanyServiceId] UNIQUEIDENTIFIER                            NOT NULL,
    [Price]            DECIMAL (18, 2)                             NOT NULL,
    [CityId]           SMALLINT                                    NOT NULL,
    CONSTRAINT [PK_TravelPrice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TravelPrice_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_TravelPrice_CompanyService] FOREIGN KEY ([CompanyServiceId]) REFERENCES [dbo].[CompanyService] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TravelPriceHistory], DATA_CONSISTENCY_CHECK=ON));

