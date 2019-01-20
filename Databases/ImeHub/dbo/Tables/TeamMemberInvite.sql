CREATE TABLE [dbo].[TeamMemberInvite] (
    [Id]                      UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]            DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]              DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [To]                      NVARCHAR (128)                              NOT NULL,
    [Cc]                      NVARCHAR (128)                              NULL,
    [Bcc]                     NVARCHAR (128)                              NULL,
    [Subject]                 NVARCHAR (128)                              NOT NULL,
    [Body]                    NVARCHAR (MAX)                              NOT NULL,
    [InviteStatusId]          TINYINT                                     DEFAULT ((4)) NOT NULL,
    [InviteStatusChangedDate] DATETIME                                    NOT NULL,
    [InviteStatusChangedBy]   UNIQUEIDENTIFIER                            NOT NULL,
    [Title]                   NVARCHAR (128)                              NULL,
    [FirstName]               NVARCHAR (128)                              NULL,
    [LastName]                NVARCHAR (128)                              NULL,
    [RoleId]                  UNIQUEIDENTIFIER                            NOT NULL,
    [PhysicianId]             UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_TeamMemberInvite] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeamMemberInvite_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TeamMemberInvite_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_TeamMemberInviteStatus_InviteStatus] FOREIGN KEY ([InviteStatusId]) REFERENCES [dbo].[InviteStatus] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[TeamMemberInviteHistory], DATA_CONSISTENCY_CHECK=ON));



