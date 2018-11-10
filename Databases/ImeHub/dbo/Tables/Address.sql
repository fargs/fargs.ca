CREATE TABLE [dbo].[Address] (
    [Id]            UNIQUEIDENTIFIER                            NOT NULL,
    [SysStartTime]  DATETIME2 (0) GENERATED ALWAYS AS ROW START CONSTRAINT [DF_SysStart] DEFAULT (sysutcdatetime()) NOT NULL,
    [SysEndTime]    DATETIME2 (0) GENERATED ALWAYS AS ROW END   CONSTRAINT [DF_SysEnd] DEFAULT (CONVERT([datetime2](0),'9999-12-31 23:59:59')) NOT NULL,
    [PhysicianId]   UNIQUEIDENTIFIER                            NULL,
    [CompanyId]     UNIQUEIDENTIFIER                            NULL,
    [AddressTypeID] TINYINT                                     NOT NULL,
    [Name]          NVARCHAR (256)                              NULL,
    [Attention]     NVARCHAR (255)                              NULL,
    [Address1]      NVARCHAR (255)                              NOT NULL,
    [Address2]      NVARCHAR (255)                              NULL,
    [CityId]        UNIQUEIDENTIFIER                            NOT NULL,
    [PostalCode]    NVARCHAR (50)                               NOT NULL,
    [TimeZoneId]    SMALLINT                                    NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Address_AddressType] FOREIGN KEY ([AddressTypeID]) REFERENCES [dbo].[AddressType] ([Id]),
    CONSTRAINT [FK_Address_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_Address_Physician] FOREIGN KEY ([PhysicianId]) REFERENCES [dbo].[Physician] ([Id]),
    CONSTRAINT [FK_Address_TimeZone] FOREIGN KEY ([TimeZoneId]) REFERENCES [dbo].[TimeZone] ([Id]),
    PERIOD FOR SYSTEM_TIME ([SysStartTime], [SysEndTime])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo].[AddressHistory], DATA_CONSISTENCY_CHECK=ON));

