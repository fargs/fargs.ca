CREATE TABLE [dbo].[AddressHistory] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [SysStartTime]  DATETIME2 (0)    NOT NULL,
    [SysEndTime]    DATETIME2 (0)    NOT NULL,
    [PhysicianId]   UNIQUEIDENTIFIER NULL,
    [CompanyId]     UNIQUEIDENTIFIER NULL,
    [AddressTypeID] TINYINT          NOT NULL,
    [Name]          NVARCHAR (256)   NULL,
    [Attention]     NVARCHAR (255)   NULL,
    [Address1]      NVARCHAR (255)   NOT NULL,
    [Address2]      NVARCHAR (255)   NULL,
    [CityId]        UNIQUEIDENTIFIER NOT NULL,
    [PostalCode]    NVARCHAR (50)    NOT NULL,
    [TimeZoneId]    SMALLINT         NOT NULL
);

