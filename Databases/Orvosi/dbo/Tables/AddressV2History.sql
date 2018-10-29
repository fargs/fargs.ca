CREATE TABLE [dbo].[AddressV2History] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [CompanyId]     UNIQUEIDENTIFIER NULL,
    [PhysicianId]   UNIQUEIDENTIFIER NULL,
    [AddressTypeID] TINYINT          NOT NULL,
    [Name]          NVARCHAR (256)   NULL,
    [Attention]     NVARCHAR (255)   NULL,
    [Address1]      NVARCHAR (255)   NOT NULL,
    [Address2]      NVARCHAR (255)   NULL,
    [CityId]        SMALLINT         NOT NULL,
    [PostalCode]    NVARCHAR (50)    NOT NULL,
    [CountryID]     SMALLINT         NOT NULL,
    [ProvinceID]    SMALLINT         NOT NULL,
    [TimeZoneId]    SMALLINT         NOT NULL,
    [SysStartTime]  DATETIME2 (0)    NOT NULL,
    [SysEndTime]    DATETIME2 (0)    NOT NULL
);


GO
CREATE CLUSTERED INDEX [ix_AddressV2History]
    ON [dbo].[AddressV2History]([SysEndTime] ASC, [SysStartTime] ASC) WITH (DATA_COMPRESSION = PAGE);

