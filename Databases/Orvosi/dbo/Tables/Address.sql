CREATE TABLE [dbo].[Address] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]    UNIQUEIDENTIFIER CONSTRAINT [DF_Address_ObjectGuid] DEFAULT (newid()) NOT NULL,
    [OwnerGuid]     UNIQUEIDENTIFIER NULL,
    [AddressTypeID] TINYINT          NOT NULL,
    [Name]          NVARCHAR (256)   NULL,
    [Attention]     NVARCHAR (255)   NULL,
    [Address1]      NVARCHAR (255)   NOT NULL,
    [Address2]      NVARCHAR (255)   NULL,
    [City]          NVARCHAR (50)    NOT NULL,
    [CityId]        SMALLINT         NULL,
    [PostalCode]    NVARCHAR (50)    NULL,
    [CountryID]     SMALLINT         CONSTRAINT [DF_Address_CountryID] DEFAULT ((124)) NOT NULL,
    [ProvinceID]    SMALLINT         NULL,
    [LocationId]    SMALLINT         NULL,
    [ModifiedDate]  SMALLDATETIME    CONSTRAINT [DF_Address_ModifiedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedUser]  NVARCHAR (256)   CONSTRAINT [DF_Address_ModifiedUser] DEFAULT (suser_name()) NOT NULL,
    [TimeZoneId]    SMALLINT         NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Address_AddressType] FOREIGN KEY ([AddressTypeID]) REFERENCES [dbo].[AddressType] ([Id]),
    CONSTRAINT [FK_Address_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id]),
    CONSTRAINT [FK_Address_Countries] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_Address_Provinces] FOREIGN KEY ([ProvinceID]) REFERENCES [dbo].[Province] ([Id]),
    CONSTRAINT [FK_Address_TimeZone] FOREIGN KEY ([TimeZoneId]) REFERENCES [dbo].[TimeZone] ([Id])
);





















