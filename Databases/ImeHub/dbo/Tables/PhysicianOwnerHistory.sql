CREATE TABLE [dbo].[PhysicianOwnerHistory] (
    [PhysicianId]                 UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]                DATETIME2 (7)    NOT NULL,
    [SysEndTime]                  DATETIME2 (7)    NOT NULL,
    [Email]                       NVARCHAR (128)   NOT NULL,
    [UserId]                      UNIQUEIDENTIFIER NULL,
    [AcceptanceStatusId]          TINYINT          NOT NULL,
    [AcceptanceStatusChangedDate] DATETIME         NOT NULL,
    [Title]                       NVARCHAR (10)    NULL,
    [FirstName]                   NVARCHAR (128)   NULL,
    [LastName]                    NVARCHAR (128)   NULL
);




GO
CREATE CLUSTERED INDEX [ix_PhysicianOwnerHistory]
    ON [dbo].[PhysicianOwnerHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

