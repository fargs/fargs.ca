CREATE TABLE [dbo].[UserRoleHistory] (
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [RoleId]       UNIQUEIDENTIFIER NOT NULL
);

