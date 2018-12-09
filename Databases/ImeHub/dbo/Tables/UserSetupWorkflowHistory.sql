CREATE TABLE [dbo].[UserSetupWorkflowHistory] (
    [SysStartTime]      DATETIME2 (7)    NOT NULL,
    [SysEndTime]        DATETIME2 (7)    NOT NULL,
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (128)   NOT NULL,
    [WorkflowId]        UNIQUEIDENTIFIER NULL,
    [StatusId]          TINYINT          NOT NULL,
    [StatusChangedById] UNIQUEIDENTIFIER NOT NULL,
    [StatusChangedDate] DATETIME         NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_UserSetupWorkflowHistory]
    ON [dbo].[UserSetupWorkflowHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

