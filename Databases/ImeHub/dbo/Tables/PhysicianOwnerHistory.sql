CREATE TABLE [dbo].[PhysicianOwnerHistory] (
    [PhysicianId]                 UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]                DATETIME2 (7)    NOT NULL,
    [SysEndTime]                  DATETIME2 (7)    NOT NULL,
    [Email]                       NVARCHAR (128)   NOT NULL,
    [Name]                        NVARCHAR (128)   NOT NULL,
    [UserId]                      UNIQUEIDENTIFIER NULL,
    [AcceptanceStatusId]          TINYINT          NOT NULL,
    [AcceptanceStatusChangedDate] DATETIME         NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_PhysicianOwnerHistory]
    ON [dbo].[PhysicianOwnerHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

