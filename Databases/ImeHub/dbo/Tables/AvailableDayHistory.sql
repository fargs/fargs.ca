CREATE TABLE [dbo].[AvailableDayHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime] DATETIME2 (7)    NOT NULL,
    [SysEndTime]   DATETIME2 (7)    NOT NULL,
    [PhysicianId]  UNIQUEIDENTIFIER NOT NULL,
    [Day]          DATE             NOT NULL,
    [CompanyId]    UNIQUEIDENTIFIER NULL,
    [AddressId]    UNIQUEIDENTIFIER NULL,
    [CityId]       UNIQUEIDENTIFIER NULL
);


GO
CREATE CLUSTERED INDEX [ix_AvailableDayHistory]
    ON [dbo].[AvailableDayHistory]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

