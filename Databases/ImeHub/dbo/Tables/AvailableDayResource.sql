CREATE TABLE [dbo].[AvailableDayResource] (
    [Id]             UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]     DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [AvailableDayId] UNIQUEIDENTIFIER                            NOT NULL,
    [UserId]         UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_AvailableDayResource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDayResource_AvailableDay] FOREIGN KEY ([AvailableDayId]) REFERENCES [dbo].[AvailableDay] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_AvailableDayResource_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[AvailableDayResourceHistory], DATA_CONSISTENCY_CHECK=ON));

