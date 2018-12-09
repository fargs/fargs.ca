CREATE TABLE [dbo].[WorkItemRelatedHistory] (
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [ParentId]     UNIQUEIDENTIFIER NOT NULL,
    [ChildId]      UNIQUEIDENTIFIER NOT NULL
);

