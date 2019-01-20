CREATE TABLE [dbo].[TeamMember] (
    [Id]                 UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]       DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]         DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [PhysicianId]        UNIQUEIDENTIFIER                            NOT NULL,
    [UserId]             UNIQUEIDENTIFIER                            NOT NULL,
    [RoleId]             UNIQUEIDENTIFIER                            NOT NULL,
    [TeamMemberInviteId] UNIQUEIDENTIFIER                            NULL,
    CONSTRAINT [PK_TeamMember] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeamMember_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_TeamMember_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[TeamRole] ([Id]),
    CONSTRAINT [FK_TeamMember_TeamMemberInvite] FOREIGN KEY ([TeamMemberInviteId]) REFERENCES [dbo].[TeamMemberInvite] ([Id]),
    CONSTRAINT [FK_TeamMember_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TeamMemberHistory], DATA_CONSISTENCY_CHECK=ON));



