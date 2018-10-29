CREATE TABLE [dbo].[CompanyService] (
    [Id]               UNIQUEIDENTIFIER                            NOT NULL,
    [ServiceId]        UNIQUEIDENTIFIER                            NULL,
    [Price]            DECIMAL (18, 2)                             NULL,
    [SysStartTime]     DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]       DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [CompanyId]        UNIQUEIDENTIFIER                            NOT NULL,
    [Name]             NVARCHAR (250)                              NOT NULL,
    [IsTravelRequired] BIT                                         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompanyService] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyService_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[CompanyV2] ([Id]),
    CONSTRAINT [FK_CompanyService_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[ServiceV2] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[CompanyServiceHistory], DATA_CONSISTENCY_CHECK=ON));

