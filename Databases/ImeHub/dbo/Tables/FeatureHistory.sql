CREATE TABLE [dbo].[FeatureHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (0)    NOT NULL,
    [SysEndTime]   DATETIME2 (0)    NOT NULL,
    [Name]         NVARCHAR (255)   NOT NULL,
    [ParentID]     UNIQUEIDENTIFIER NULL,
    [IsActive]     BIT              NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_FeatureHistory]
    ON [dbo].[FeatureHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

