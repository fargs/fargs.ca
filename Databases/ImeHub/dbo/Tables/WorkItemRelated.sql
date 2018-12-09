CREATE TABLE [dbo].[WorkItemRelated] (
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [ParentId]     UNIQUEIDENTIFIER                            NOT NULL,
    [ChildId]      UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_WorkItemRelated] PRIMARY KEY CLUSTERED ([ParentId] ASC, [ChildId] ASC),
    CONSTRAINT [FK_WorkItemRelated_Dependent] FOREIGN KEY ([ChildId]) REFERENCES [dbo].[WorkItem] ([Id]),
    CONSTRAINT [FK_WorkItemRelated_WorkItem] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[WorkItem] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[WorkItemRelatedHistory], DATA_CONSISTENCY_CHECK=ON));

