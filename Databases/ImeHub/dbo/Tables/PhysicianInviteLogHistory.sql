CREATE TABLE [dbo].[PhysicianInviteLogHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL,
    [To]           NVARCHAR (128)   NOT NULL,
    [Cc]           NVARCHAR (128)   NULL,
    [Bcc]          NVARCHAR (128)   NULL,
    [Subject]      NVARCHAR (128)   NOT NULL,
    [Body]         NVARCHAR (MAX)   NOT NULL
);

