CREATE TABLE [dbo].[WorkflowTask] (
    [Id]                                UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]                      DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]                        DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [WorkflowId]                        SMALLINT                                    NOT NULL,
    [Sequence]                          SMALLINT                                    NOT NULL,
    [Name]                              NVARCHAR (128)                              NOT NULL,
    [RoleId]                            UNIQUEIDENTIFIER                            NULL,
    [IsBaselineDate]                    BIT                                         CONSTRAINT [DF_WorkflowTask_IsBaselineDate] DEFAULT ((0)) NOT NULL,
    [DueDateDurationFromBaseline]       SMALLINT                                    NULL,
    [EffectiveDateDurationFromBaseline] SMALLINT                                    NULL,
    [IsCriticalPath]                    BIT                                         CONSTRAINT [DF_WorkflowTask_IsCriticalPath] DEFAULT ((0)) NOT NULL,
    [IsBillable]                        BIT                                         CONSTRAINT [DF_WorkflowTask_IsBillable] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkflowTask] PRIMARY KEY CLUSTERED ([Id] ASC),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[WorkflowTaskHistory], DATA_CONSISTENCY_CHECK=ON));

