CREATE TABLE [dbo].[Address] (
    [AddressID]     INT              IDENTITY (1, 1) NOT NULL,
    [ObjectGuid]    UNIQUEIDENTIFIER NOT NULL,
    [AddressTypeID] TINYINT          NOT NULL,
    [Attention]     NVARCHAR (255)   NULL,
    [Address1]      NVARCHAR (255)   NOT NULL,
    [Address2]      NVARCHAR (255)   NULL,
    [City]          NVARCHAR (50)    NOT NULL,
    [PostalCode]    NVARCHAR (50)    NULL,
    [CountryID]     SMALLINT         NOT NULL,
    [ProvinceID]    SMALLINT         NULL,
    [ContactEmail]  NVARCHAR (255)   NULL,
    [ContactPhone]  NVARCHAR (255)   NULL,
    [CreatedDate]   SMALLDATETIME    CONSTRAINT [DF_Address_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedUser]   NVARCHAR (256)   NOT NULL,
    [ModifiedDate]  SMALLDATETIME    NOT NULL,
    [ModifiedUser]  NVARCHAR (256)   NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressID] ASC),
    CONSTRAINT [FK_Address_AddressType] FOREIGN KEY ([AddressTypeID]) REFERENCES [dbo].[AddressType] ([AddressTypeID]),
    CONSTRAINT [FK_Address_Countries] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([CountryID]),
    CONSTRAINT [FK_Address_Provinces] FOREIGN KEY ([ProvinceID]) REFERENCES [dbo].[Province] ([ProvinceID]),
    CONSTRAINT [UK_Address_ObjectGuid] UNIQUE NONCLUSTERED ([ObjectGuid] ASC)
);



