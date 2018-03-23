CREATE TABLE [dbo].[TeleconferenceHistory] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [AppointmentDate]              DATETIME         NOT NULL,
    [StartTime]                    TIME (7)         NULL,
    [SysStartTime]                 DATETIME2 (7)    NOT NULL,
    [SysEndTime]                   DATETIME2 (7)    NOT NULL,
    [ServiceRequestId]             INT              NOT NULL,
    [LastModifiedBy]               NVARCHAR (50)    NOT NULL,
    [TeleconferenceResultId]       TINYINT          NULL,
    [TeleconferenceResultSentDate] DATETIME         NULL
);


GO
CREATE CLUSTERED INDEX [ix_TeleconferenceHistory]
    ON [dbo].[TeleconferenceHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

