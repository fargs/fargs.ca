CREATE TABLE [dbo].[UserSetupWorkItem] (
    [SysStartTime]          DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]            DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Id]                    UNIQUEIDENTIFIER                            NOT NULL,
    [CreatedFromWorkItemId] UNIQUEIDENTIFIER                            NULL,
    [Name]                  NVARCHAR (128)                              NOT NULL,
    [Sequence]              SMALLINT                                    NOT NULL,
    [UserSetupWorkflowId]   UNIQUEIDENTIFIER                            NOT NULL,
    [ResponsibleRoleId]     UNIQUEIDENTIFIER                            NULL,
    [StatusId]              TINYINT                                     NOT NULL,
    [StatusChangedById]     UNIQUEIDENTIFIER                            NOT NULL,
    [StatusChangedDate]     DATETIME                                    NOT NULL,
    [AssignedToId]          UNIQUEIDENTIFIER                            NULL,
    [AssignedToChangedById] UNIQUEIDENTIFIER                            NULL,
    [AssignedToChangedDate] DATETIME                                    NULL,
    [DueDate]               DATETIME                                    NULL,
    CONSTRAINT [PK_UserSetupWorkItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSetupWorkItem_StatusChangedBy_User] FOREIGN KEY ([StatusChangedById]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_UserSetupWorkItem_UserSetupWorkflow] FOREIGN KEY ([UserSetupWorkflowId]) REFERENCES [dbo].[UserSetupWorkflow] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[UserSetupWorkItemHistory], DATA_CONSISTENCY_CHECK=ON));



