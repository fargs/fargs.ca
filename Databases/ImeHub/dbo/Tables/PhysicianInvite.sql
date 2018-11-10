CREATE TABLE [dbo].[PhysicianInvite] (
    [Id]                 UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]       DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]         DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [PhysicianId]        UNIQUEIDENTIFIER                            NOT NULL,
    [ToEmail]            NVARCHAR (128)                              NOT NULL,
    [ToName]             NVARCHAR (128)                              NOT NULL,
    [UserId]             UNIQUEIDENTIFIER                            NULL,
    [SentDate]           DATETIME                                    NULL,
    [LinkClickedDate]    DATETIME                                    NULL,
    [FromEmail]          NVARCHAR (128)                              NOT NULL,
    [FromName]           NVARCHAR (128)                              NOT NULL,
    [AcceptanceStatusId] TINYINT                                     NOT NULL,
    CONSTRAINT [PK_PhysicianInvite] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhysicianInvite_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_PhysicianInvite_PhysicianInviteAcceptanceStatus] FOREIGN KEY ([AcceptanceStatusId]) REFERENCES [dbo].[PhysicianInviteAcceptanceStatus] ([Id]),
    CONSTRAINT [FK_PhysicianInvite_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[PhysicianInviteHistory], DATA_CONSISTENCY_CHECK=ON));

