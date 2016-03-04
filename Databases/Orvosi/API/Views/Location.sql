






CREATE VIEW [API].[Location]
AS
SELECT 
	 a.[Id]
	,a.[ObjectGuid]
	,a.OwnerGuid
	,e.EntityId
	,EntityDisplayName = e.DisplayName
	,e.EntityType
	,a.[AddressTypeID]
	,AddressTypeName = at.Name
	,a.[Name]
	,a.[Attention]
	,a.[Address1]
	,a.[Address2]
	,a.[City]
	,a.[PostalCode]
	,a.[CountryID]
	,a.[ProvinceID]
	,a.[LocationId]
	,a.[ModifiedDate]
	,a.[ModifiedUser]
	,CountryName = c.Name
	,CountryCode = c.ISO3CountryCode
	,ProvinceName = p.ProvinceName
	,ProvinceCode = p.ProvinceCode
	,LocationName = l.[Text]
	,LocationShortName = l.ShortText
FROM [dbo].[Address] a
LEFT JOIN dbo.[AddressType] at ON a.AddressTypeId = at.Id
LEFT JOIN API.Entity e ON a.OwnerGuid = e.EntityGuid
LEFT JOIN dbo.Country c ON a.CountryId = c.Id
LEFT JOIN dbo.Province p ON a.ProvinceId = p.Id
LEFT JOIN dbo.LookupItem l ON a.LocationId = l.Id