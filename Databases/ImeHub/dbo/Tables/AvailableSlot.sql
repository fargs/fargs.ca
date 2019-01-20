CREATE TABLE [dbo].[AvailableSlot] (
    [Id]             UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]     DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [AvailableDayId] UNIQUEIDENTIFIER                            NOT NULL,
    [StartTime]      TIME (7)                                    NOT NULL,
    [EndTime]        TIME (7)                                    NULL,
    [Duration]       SMALLINT                                    NULL,
    CONSTRAINT [PK_AvailableSlot] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AvailableDay_AvailableSlot] FOREIGN KEY ([AvailableDayId]) REFERENCES [dbo].[AvailableDay] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[AvailableSlotHistory], DATA_CONSISTENCY_CHECK=ON));

