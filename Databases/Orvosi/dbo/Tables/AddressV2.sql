CREATE TABLE [dbo].[AddressV2] (
    [Id]            UNIQUEIDENTIFIER                                   NOT NULL,
    [CompanyId]     UNIQUEIDENTIFIER                                   NULL,
    [PhysicianId]   UNIQUEIDENTIFIER                                   NULL,
    [AddressTypeID] TINYINT                                            NOT NULL,
    [Name]          NVARCHAR (256)                                     NULL,
    [Attention]     NVARCHAR (255)                                     NULL,
    [Address1]      NVARCHAR (255)                                     NOT NULL,
    [Address2]      NVARCHAR (255)                                     NULL,
    [CityId]        SMALLINT                                           NOT NULL,
    [PostalCode]    NVARCHAR (50)                                      NOT NULL,
    [CountryID]     SMALLINT                                           CONSTRAINT [DF_AddressV2_CountryID] DEFAULT ((124)) NOT NULL,
    [ProvinceID]    SMALLINT                                           NOT NULL,
    [TimeZoneId]    SMALLINT                                           NOT NULL,
    [SysStartTime]  DATETIME2 (0) GENERATED ALWAYS AS ROW START HIDDEN CONSTRAINT [DF_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]    DATETIME2 (0) GENERATED ALWAYS AS ROW END HIDDEN   CONSTRAINT [DF_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    CONSTRAINT [PK_AddressV2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AddressV2_AddressType] FOREIGN KEY ([AddressTypeID]) REFERENCES [dbo].[AddressType] ([Id]),
    CONSTRAINT [FK_AddressV2_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_AddressV2_CompanyV2] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[CompanyV2] ([Id]),
    CONSTRAINT [FK_AddressV2_Countries] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_AddressV2_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_AddressV2_Provinces] FOREIGN KEY ([ProvinceID]) REFERENCES [dbo].[Province] ([Id]),
    CONSTRAINT [FK_AddressV2_TimeZone] FOREIGN KEY ([TimeZoneId]) REFERENCES [dbo].[TimeZone] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[AddressV2History], DATA_CONSISTENCY_CHECK=ON));

