CREATE TABLE [dbo].[AvailableDayResourceHistory] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]   DATETIME2 (7)    NOT NULL,
    [SysEndTime]     DATETIME2 (7)    NOT NULL,
    [AvailableDayId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_AvailableDayResourceHistory]
    ON [dbo].[AvailableDayResourceHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

