CREATE TABLE [dbo].[TeamMemberHistory] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]       DATETIME2 (7)    NOT NULL,
    [SysEndTime]         DATETIME2 (7)    NOT NULL,
    [PhysicianId]        UNIQUEIDENTIFIER NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [RoleId]             UNIQUEIDENTIFIER NOT NULL,
    [TeamMemberInviteId] UNIQUEIDENTIFIER NULL
);




GO
CREATE CLUSTERED INDEX [ix_TeamMemberHistory]
    ON [dbo].[TeamMemberHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

