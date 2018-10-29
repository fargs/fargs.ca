CREATE TABLE [dbo].[TravelPriceHistory] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]     DATETIME2 (7)    NOT NULL,
    [SysEndTime]       DATETIME2 (7)    NOT NULL,
    [CompanyServiceId] UNIQUEIDENTIFIER NOT NULL,
    [Price]            DECIMAL (18, 2)  NOT NULL,
    [CityId]           SMALLINT         NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_TravelPriceHistory]
    ON [dbo].[TravelPriceHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

