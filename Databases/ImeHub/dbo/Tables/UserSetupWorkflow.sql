CREATE TABLE [dbo].[UserSetupWorkflow] (
    [SysStartTime]      DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]        DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Id]                UNIQUEIDENTIFIER                            NOT NULL,
    [Name]              NVARCHAR (128)                              NOT NULL,
    [WorkflowId]        UNIQUEIDENTIFIER                            NULL,
    [StatusId]          TINYINT                                     NOT NULL,
    [StatusChangedById] UNIQUEIDENTIFIER                            NOT NULL,
    [StatusChangedDate] DATETIME                                    NOT NULL,
    CONSTRAINT [PK_UserSetupWorkflow] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSetupWorkflow_StatusChangedBy_User] FOREIGN KEY ([StatusChangedById]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_UserSetupWorkflow_User] FOREIGN KEY ([Id]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[UserSetupWorkflowHistory], DATA_CONSISTENCY_CHECK=ON));



