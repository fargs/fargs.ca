CREATE TABLE [dbo].[ServiceV2] (
    [Id]                 UNIQUEIDENTIFIER                            NOT NULL,
    [Name]               NVARCHAR (128)                              NOT NULL,
    [Description]        NVARCHAR (MAX)                              NULL,
    [Code]               NVARCHAR (10)                               NULL,
    [Price]              DECIMAL (18, 2)                             NOT NULL,
    [IsLocationRequired] BIT                                         DEFAULT ((0)) NOT NULL,
    [ColorCode]          VARCHAR (10)                                NULL,
    [PhysicianId]        UNIQUEIDENTIFIER                            NULL,
    [SysStartTime]       DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_ServiceV2_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]         DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_ServiceV2_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [IsTravelRequired]   BIT                                         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_V2_Service] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceV2_AspNetUsers] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[ServiceV2History], DATA_CONSISTENCY_CHECK=ON));

