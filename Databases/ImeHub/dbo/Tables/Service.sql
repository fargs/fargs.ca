CREATE TABLE [dbo].[Service] (
    [Id]               UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]     DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_Service_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]       DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_Service_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [CompanyId]        UNIQUEIDENTIFIER                            NOT NULL,
    [Name]             NVARCHAR (128)                              NOT NULL,
    [Description]      NVARCHAR (2000)                             NULL,
    [Code]             NVARCHAR (10)                               NULL,
    [ColorCode]        VARCHAR (10)                                NULL,
    [Price]            DECIMAL (18, 2)                             NOT NULL,
    [IsTravelRequired] BIT                                         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Service_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE,
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[ServiceHistory], DATA_CONSISTENCY_CHECK=ON));

