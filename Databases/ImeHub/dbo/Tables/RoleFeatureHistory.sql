CREATE TABLE [dbo].[RoleFeatureHistory] (
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [RoleId]       UNIQUEIDENTIFIER NOT NULL,
    [FeatureId]    UNIQUEIDENTIFIER NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_RoleFeatureHistory]
    ON [dbo].[RoleFeatureHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

