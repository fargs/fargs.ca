CREATE TABLE [dbo].[PhysicianHistory] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]     DATETIME2 (0)    NOT NULL,
    [SysEndTime]       DATETIME2 (0)    NOT NULL,
    [CompanyName]      NVARCHAR (250)   NULL,
    [Code]             NVARCHAR (10)    NULL,
    [ColorCode]        NVARCHAR (10)    NULL,
    [OwnerId]          UNIQUEIDENTIFIER NULL,
    [ManagerId]        UNIQUEIDENTIFIER NOT NULL,
    [PrimaryAddressId] SMALLINT         NULL,
    [Designations]     NVARCHAR (128)   NULL
);


GO
CREATE CLUSTERED INDEX [ix_PhysicianHistory]
    ON [dbo].[PhysicianHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

