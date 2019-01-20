CREATE TABLE [dbo].[TeamMemberInviteHistory] (
    [Id]                      UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]            DATETIME2 (7)    NOT NULL,
    [SysEndTime]              DATETIME2 (7)    NOT NULL,
    [To]                      NVARCHAR (128)   NOT NULL,
    [Cc]                      NVARCHAR (128)   NULL,
    [Bcc]                     NVARCHAR (128)   NULL,
    [Subject]                 NVARCHAR (128)   NOT NULL,
    [Body]                    NVARCHAR (MAX)   NOT NULL,
    [InviteStatusId]          TINYINT          NOT NULL,
    [InviteStatusChangedDate] DATETIME         NOT NULL,
    [InviteStatusChangedBy]   UNIQUEIDENTIFIER NOT NULL,
    [Title]                   NVARCHAR (128)   NULL,
    [FirstName]               NVARCHAR (128)   NULL,
    [LastName]                NVARCHAR (128)   NULL,
    [RoleId]                  UNIQUEIDENTIFIER NOT NULL,
    [PhysicianId]             UNIQUEIDENTIFIER NOT NULL
);




GO
CREATE CLUSTERED INDEX [ix_TeamMemberInviteHistory]
    ON [dbo].[TeamMemberInviteHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

