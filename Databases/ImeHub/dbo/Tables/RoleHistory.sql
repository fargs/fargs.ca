CREATE TABLE [dbo].[RoleHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [Name]         NVARCHAR (256)   NOT NULL,
    [Code]         NVARCHAR (10)    NULL,
    [ColorCode]    NVARCHAR (50)    NULL
);


GO
CREATE CLUSTERED INDEX [ix_RoleHistory]
    ON [dbo].[RoleHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

