CREATE TABLE [dbo].[WorkItem] (
    [Id]                                UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]                      DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]                        DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Sequence]                          SMALLINT                                    NOT NULL,
    [Name]                              NVARCHAR (128)                              NOT NULL,
    [RoleId]                            UNIQUEIDENTIFIER                            NULL,
    [IsBaselineDate]                    BIT                                         CONSTRAINT [DF_WorkItem_IsBaselineDate] DEFAULT ((0)) NOT NULL,
    [DueDateDurationFromBaseline]       SMALLINT                                    NULL,
    [EffectiveDateDurationFromBaseline] SMALLINT                                    NULL,
    [IsCriticalPath]                    BIT                                         CONSTRAINT [DF_WorkItem_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [IsBillable]                        BIT                                         CONSTRAINT [DF_WorkItem_IsBillable] DEFAULT ((0)) NOT NULL,
    [WorkflowId]                        UNIQUEIDENTIFIER                            NOT NULL,
    CONSTRAINT [PK_WorkItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkItem_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflow] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[WorkItemHistory], DATA_CONSISTENCY_CHECK=ON));



