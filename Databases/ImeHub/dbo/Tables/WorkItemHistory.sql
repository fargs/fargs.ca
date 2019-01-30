CREATE TABLE [dbo].[WorkItemHistory] (
    [Id]                                UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]                      DATETIME2 (7)    NOT NULL,
    [SysEndTime]                        DATETIME2 (7)    NOT NULL,
    [Sequence]                          SMALLINT         NOT NULL,
    [Name]                              NVARCHAR (128)   NOT NULL,
    [IsBaselineDate]                    BIT              NOT NULL,
    [DueDateDurationFromBaseline]       SMALLINT         NULL,
    [EffectiveDateDurationFromBaseline] SMALLINT         NULL,
    [IsCriticalPath]                    BIT              NOT NULL,
    [IsBillable]                        BIT              NOT NULL,
    [WorkflowId]                        UNIQUEIDENTIFIER NOT NULL,
    [TeamRoleId]                        UNIQUEIDENTIFIER NOT NULL
);





