CREATE TABLE [dbo].[PhysicianInviteHistory] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]       DATETIME2 (7)    NOT NULL,
    [SysEndTime]         DATETIME2 (7)    NOT NULL,
    [PhysicianId]        UNIQUEIDENTIFIER NOT NULL,
    [ToEmail]            NVARCHAR (128)   NOT NULL,
    [ToName]             NVARCHAR (128)   NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NULL,
    [SentDate]           DATETIME         NULL,
    [LinkClickedDate]    DATETIME         NULL,
    [FromEmail]          NVARCHAR (128)   NOT NULL,
    [FromName]           NVARCHAR (128)   NOT NULL,
    [AcceptanceStatusId] TINYINT          NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_PhysicianInviteHistory]
    ON [dbo].[PhysicianInviteHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

