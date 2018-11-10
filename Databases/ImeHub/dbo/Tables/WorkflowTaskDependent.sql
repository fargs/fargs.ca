CREATE TABLE [dbo].[WorkflowTaskDependent] (
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [ParentId]     UNIQUEIDENTIFIER                            NOT NULL,
    [ChildId]      UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_WorkflowTaskDependent] PRIMARY KEY CLUSTERED ([ParentId] ASC, [ChildId] ASC),
    CONSTRAINT [FK_WorkflowTaskDependent_Dependent] FOREIGN KEY ([ChildId]) REFERENCES [dbo].[WorkflowTask] ([Id]),
    CONSTRAINT [FK_WorkflowTaskDependent_WorkflowTask] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[WorkflowTask] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[WorkflowTaskDependentHistory], DATA_CONSISTENCY_CHECK=ON));

