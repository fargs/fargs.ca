CREATE TABLE [dbo].[TeamRoleHistory] (
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (256)   NOT NULL,
    [Code]         NVARCHAR (10)    NULL,
    [ColorCode]    NVARCHAR (50)    NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_TeamRoleHistory]
    ON [dbo].[TeamRoleHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

