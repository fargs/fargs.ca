CREATE TABLE [dbo].[UserSetupWorkItemHistory] (
    [SysStartTime]          DATETIME2 (7)    NOT NULL,
    [SysEndTime]            DATETIME2 (7)    NOT NULL,
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [CreatedFromWorkItemId] UNIQUEIDENTIFIER NULL,
    [Name]                  NVARCHAR (128)   NOT NULL,
    [Sequence]              SMALLINT         NOT NULL,
    [UserSetupWorkflowId]   UNIQUEIDENTIFIER NOT NULL,
    [ResponsibleRoleId]     UNIQUEIDENTIFIER NULL,
    [StatusId]              TINYINT          NOT NULL,
    [StatusChangedById]     UNIQUEIDENTIFIER NOT NULL,
    [StatusChangedDate]     DATETIME         NOT NULL,
    [AssignedToId]          UNIQUEIDENTIFIER NULL,
    [AssignedToChangedById] UNIQUEIDENTIFIER NULL,
    [AssignedToChangedDate] DATETIME         NULL,
    [DueDate]               DATETIME         NULL
);


GO
CREATE CLUSTERED INDEX [ix_UserSetupWorkItemHistory]
    ON [dbo].[UserSetupWorkItemHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

