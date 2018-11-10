CREATE TABLE [dbo].[Role] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (0) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Name]         NVARCHAR (256)                              NOT NULL,
    [Code]         NVARCHAR (10)                               NULL,
    [ColorCode]    NVARCHAR (50)                               NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[RoleHistory], DATA_CONSISTENCY_CHECK=ON));

