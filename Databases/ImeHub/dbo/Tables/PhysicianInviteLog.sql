CREATE TABLE [dbo].[PhysicianInviteLog] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER                            NOT NULL,
    [To]           NVARCHAR (128)                              NOT NULL,
    [Cc]           NVARCHAR (128)                              NULL,
    [Bcc]          NVARCHAR (128)                              NULL,
    [Subject]      NVARCHAR (128)                              NOT NULL,
    [Body]         NVARCHAR (MAX)                              NOT NULL,
    CONSTRAINT [PK_PhysicianInviteLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhysicianInviteLog_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[PhysicianInviteLogHistory], DATA_CONSISTENCY_CHECK=ON));

