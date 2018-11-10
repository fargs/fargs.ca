CREATE TABLE [dbo].[Company] (
    [Id]                         UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]               DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_Company_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]                 DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_Company_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [Name]                       NVARCHAR (128)                              NOT NULL,
    [Description]                NVARCHAR (MAX)                              NULL,
    [Code]                       NVARCHAR (10)                               NULL,
    [ColorCode]                  VARCHAR (10)                                NULL,
    [PhysicianId]                UNIQUEIDENTIFIER                            NULL,
    [BillingEmail]               VARCHAR (100)                               NULL,
    [ReportsEmail]               VARCHAR (100)                               NULL,
    [PhoneNumber]                VARCHAR (100)                               NULL,
    [ParentId]                   UNIQUEIDENTIFIER                            NULL,
    [NoShowRate]                 INT                                         NOT NULL,
    [LateCancellationRate]       INT                                         NOT NULL,
    [LateCancellationPolicy]     INT                                         NOT NULL,
    [LateCancellationRateFormat] TINYINT                                     NOT NULL,
    [NoShowRateFormat]           TINYINT                                     NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Company_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_Company_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[CompanyHistory], DATA_CONSISTENCY_CHECK=ON));

