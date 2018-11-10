CREATE TABLE [dbo].[WorkflowTaskDependentHistory] (
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [ParentId]     UNIQUEIDENTIFIER NOT NULL,
    [ChildId]      UNIQUEIDENTIFIER NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_WorkflowTaskDependentHistory]
    ON [dbo].[WorkflowTaskDependentHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

