CREATE TABLE [dbo].[AvailableSlotHistory] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]   DATETIME2 (7)    NOT NULL,
    [SysEndTime]     DATETIME2 (7)    NOT NULL,
    [AvailableDayId] UNIQUEIDENTIFIER NOT NULL,
    [StartTime]      TIME (7)         NOT NULL,
    [EndTime]        TIME (7)         NULL,
    [Duration]       SMALLINT         NULL
);


GO
CREATE CLUSTERED INDEX [ix_AvailableSlotHistory]
    ON [dbo].[AvailableSlotHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

