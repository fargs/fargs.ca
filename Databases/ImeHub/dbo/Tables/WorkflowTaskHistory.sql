CREATE TABLE [dbo].[WorkflowTaskHistory] (
    [Id]                                UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]                      DATETIME2 (7)    NOT NULL,
    [SysEndTime]                        DATETIME2 (7)    NOT NULL,
    [WorkflowId]                        SMALLINT         NOT NULL,
    [Sequence]                          SMALLINT         NOT NULL,
    [Name]                              NVARCHAR (128)   NOT NULL,
    [RoleId]                            UNIQUEIDENTIFIER NULL,
    [IsBaselineDate]                    BIT              NOT NULL,
    [DueDateDurationFromBaseline]       SMALLINT         NULL,
    [EffectiveDateDurationFromBaseline] SMALLINT         NULL,
    [IsCriticalPath]                    BIT              NOT NULL,
    [IsBillable]                        BIT              NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_WorkflowTaskHistory]
    ON [dbo].[WorkflowTaskHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

