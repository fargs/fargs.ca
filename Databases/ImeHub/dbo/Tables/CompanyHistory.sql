CREATE TABLE [dbo].[CompanyHistory] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]               DATETIME2 (0)    NOT NULL,
    [SysEndTime]                 DATETIME2 (0)    NOT NULL,
    [Name]                       NVARCHAR (128)   NOT NULL,
    [Description]                NVARCHAR (MAX)   NULL,
    [Code]                       NVARCHAR (10)    NULL,
    [ColorCode]                  VARCHAR (10)     NULL,
    [PhysicianId]                UNIQUEIDENTIFIER NULL,
    [BillingEmail]               VARCHAR (100)    NULL,
    [ReportsEmail]               VARCHAR (100)    NULL,
    [PhoneNumber]                VARCHAR (100)    NULL,
    [ParentId]                   UNIQUEIDENTIFIER NULL,
    [NoShowRate]                 INT              NOT NULL,
    [LateCancellationRate]       INT              NOT NULL,
    [LateCancellationPolicy]     INT              NOT NULL,
    [LateCancellationRateFormat] TINYINT          NOT NULL,
    [NoShowRateFormat]           TINYINT          NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_CompanyHistory]
    ON [dbo].[CompanyHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

