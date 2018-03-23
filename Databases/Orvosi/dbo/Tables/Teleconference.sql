CREATE TABLE [dbo].[Teleconference] (
    [Id]                           UNIQUEIDENTIFIER                            NOT NULL,
    [AppointmentDate]              DATETIME                                    NOT NULL,
    [StartTime]                    TIME (7)                                    NULL,
    [SysStartTime]                 DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]                   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [ServiceRequestId]             INT                                         NOT NULL,
    [LastModifiedBy]               NVARCHAR (50)                               NOT NULL,
    [TeleconferenceResultId]       TINYINT                                     NULL,
    [TeleconferenceResultSentDate] DATETIME                                    NULL,
    CONSTRAINT [PK_Teleconference] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Teleconference_ServiceRequest] FOREIGN KEY ([ServiceRequestId]) REFERENCES [dbo].[ServiceRequest] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Teleconference_TeleconferenceResult] FOREIGN KEY ([TeleconferenceResultId]) REFERENCES [dbo].[TeleconferenceResult] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TeleconferenceHistory], DATA_CONSISTENCY_CHECK=ON));

