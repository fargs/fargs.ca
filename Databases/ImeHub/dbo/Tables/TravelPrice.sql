CREATE TABLE [dbo].[TravelPrice] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [ServiceId]    UNIQUEIDENTIFIER                            NOT NULL,
    [Price]        DECIMAL (18, 2)                             NOT NULL,
    [CityId]       UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_TravelPrice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TravelPrice_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_TravelPrice_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TravelPriceHistory], DATA_CONSISTENCY_CHECK=ON));

