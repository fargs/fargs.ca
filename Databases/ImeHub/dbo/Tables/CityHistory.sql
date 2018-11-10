CREATE TABLE [dbo].[CityHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [Name]         NVARCHAR (128)   NULL,
    [Code]         NVARCHAR (3)     NULL,
    [ProvinceId]   SMALLINT         NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_CityHistory]
    ON [dbo].[CityHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

