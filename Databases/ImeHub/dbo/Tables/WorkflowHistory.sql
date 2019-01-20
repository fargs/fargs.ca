CREATE TABLE [dbo].[WorkflowHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [Name]         NVARCHAR (128)   NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL
);




GO
CREATE CLUSTERED INDEX [ix_WorkflowHistory]
    ON [dbo].[WorkflowHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

