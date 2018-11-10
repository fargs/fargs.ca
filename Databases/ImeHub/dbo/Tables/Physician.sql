CREATE TABLE [dbo].[Physician] (
    [Id]               UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]     DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_Physician_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]       DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_Physician_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [CompanyName]      NVARCHAR (250)                              NULL,
    [Code]             NVARCHAR (10)                               NULL,
    [ColorCode]        NVARCHAR (10)                               NULL,
    [OwnerId]          UNIQUEIDENTIFIER                            NULL,
    [ManagerId]        UNIQUEIDENTIFIER                            NOT NULL,
    [PrimaryAddressId] SMALLINT                                    NULL,
    [Designations]     NVARCHAR (128)                              NULL,
    CONSTRAINT [PK_Physician] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Physician_Manager] FOREIGN KEY ([ManagerId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Physician_Owner] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[PhysicianHistory], DATA_CONSISTENCY_CHECK=ON));

