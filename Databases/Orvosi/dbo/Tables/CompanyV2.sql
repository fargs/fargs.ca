CREATE TABLE [dbo].[CompanyV2] (
    [Id]                         UNIQUEIDENTIFIER                            NOT NULL,
    [Name]                       NVARCHAR (128)                              NOT NULL,
    [Description]                NVARCHAR (MAX)                              NULL,
    [Code]                       NVARCHAR (10)                               NULL,
    [ColorCode]                  VARCHAR (10)                                NULL,
    [PhysicianId]                UNIQUEIDENTIFIER                            NULL,
    [SysStartTime]               DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_CompanyV2_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]                 DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_CompanyV2_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [BillingEmail]               VARCHAR (100)                               NULL,
    [ReportsEmail]               VARCHAR (100)                               NULL,
    [PhoneNumber]                VARCHAR (100)                               NULL,
    [ParentId]                   UNIQUEIDENTIFIER                            NULL,
    [NoShowRate]                 INT                                         NOT NULL,
    [LateCancellationRate]       INT                                         NOT NULL,
    [LateCancellationPolicy]     INT                                         NOT NULL,
    [LateCancellationRateFormat] TINYINT                                     NOT NULL,
    [NoShowRateFormat]           TINYINT                                     NOT NULL,
    CONSTRAINT [PK_V2_Company] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyV2_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[CompanyV2] ([Id]),
    CONSTRAINT [FK_CompanyV2_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[CompanyV2History], DATA_CONSISTENCY_CHECK=ON));

