CREATE TABLE [dbo].[Organization] (
    [Id]              UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]    DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]      DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Name]            NVARCHAR (128)                              NULL,
    [Code]            NVARCHAR (10)                               NULL,
    [ColorCode]       NVARCHAR (10)                               NOT NULL,
    [OwnerId]         UNIQUEIDENTIFIER                            NOT NULL,
    [AdministratorId] UNIQUEIDENTIFIER                            NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([Id] ASC),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[OrganizationHistory], DATA_CONSISTENCY_CHECK=ON));



