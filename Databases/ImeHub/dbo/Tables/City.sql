CREATE TABLE [dbo].[City] (
    [Id]           UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime] DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    [SysEndTime]   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    [Name]         NVARCHAR (128)                              NULL,
    [Code]         NVARCHAR (3)                                NULL,
    [ProvinceId]   SMALLINT                                    NOT NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_City_Province] FOREIGN KEY ([ProvinceId]) REFERENCES [dbo].[Province] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[CityHistory], DATA_CONSISTENCY_CHECK=ON));

